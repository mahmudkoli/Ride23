using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;
using Ride23.Framework.Core.Database;
using Ride23.Framework.Core.Domain;
using Ride23.Framework.Core.Services;

namespace Ride23.Framework.Persistence.NoSQL
{
    public class CassandraRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class, IBaseEntity<TId>, new()
    {
        private readonly ICassandraDbContext _context;
        private readonly ISession _session;
        private readonly IDateTimeService _dateTimeProvider;
        private readonly IMapper _mapper;
        private readonly string _tableName;

        public CassandraRepository(ICassandraDbContext context, IDateTimeService dateTimeProvider)
        {
            _context = context;
            _session = _context.Session;
            _dateTimeProvider = dateTimeProvider;
            _mapper = new Mapper(_session);
            _tableName = CassandraTypeMapper.GetTableName<TEntity>();
            if (!CheckTableExists()) CreateTable();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var query = BuildSelectQuery(predicate);
            var result = await _mapper.FirstOrDefaultAsync<TEntity>(query);
            return result != null;
        }

        public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var query = BuildSelectQuery(predicate) + " ALLOW FILTERING";
            var result = await _mapper.FetchAsync<TEntity>(query);
            return result.ToList();
        }

        public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var query = BuildSelectQuery(predicate);
            var result = await _mapper.FirstOrDefaultAsync<TEntity>(query);
            return result;
        }

        public async Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            var query = $"SELECT * FROM {_tableName} WHERE id = ?";
            var result = await _mapper.FirstOrDefaultAsync<TEntity>(query, id);
            return result;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var query = $"SELECT * FROM {_tableName}";
            var result = await _mapper.FetchAsync<TEntity>(query);
            return result.ToList().AsReadOnly();
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var columnNames = CassandraTypeMapper.GetTableColumns<TEntity>();
            var columnNamesStr = string.Join(", ", columnNames);
            var placeholders = string.Join(", ", columnNames.Select(_ => "?"));
            var insertQuery = $"INSERT INTO {_tableName} ({columnNamesStr}) VALUES ({placeholders})";
            var statement = new SimpleStatement(insertQuery, CassandraTypeMapper.GetTableValues<TEntity>(entity).ToArray());
            await _session.ExecuteAsync(statement);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var columnNames = CassandraTypeMapper.GetTableColumns<TEntity>(true);
            var setColumnAssignments = string.Join(", ", columnNames.Select(col => $"{col} = ?"));
            var updateQuery = $"UPDATE {_tableName} SET {setColumnAssignments} WHERE id = ?";
            var columnValues = CassandraTypeMapper.GetTableValues<TEntity>(entity, true);
            var statement = new SimpleStatement(updateQuery, columnValues.Append(entity.Id).ToArray());
            await _session.ExecuteAsync(statement);
        }

        public async Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var ids = entities.Select(x => x.Id);
            var inClause = string.Join(",", Enumerable.Range(0, ids.Count()).Select(i => "?"));
            var query = $"DELETE FROM {_tableName} WHERE id IN ({inClause})";
            var statement = new SimpleStatement(query, ids.Cast<object>().ToArray());
            await _session.ExecuteAsync(statement);
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var entities = await FindAsync(predicate, cancellationToken);
            await DeleteRangeAsync(entities, cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DeleteByIdAsync(entity.Id, cancellationToken);
        }

        public async Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            var query = $"DELETE FROM {_tableName} WHERE id = ?";
            var statement = new SimpleStatement(query, id);
            await _session.ExecuteAsync(statement);
        }

        public void Dispose()
        {
            // No need to dispose Cassandra session
        }

        public TEntity MapRowToEntity(Row row)
        {
            var entity = new TEntity();
            var properties = CassandraTypeMapper.GetProperties<TEntity>();

            foreach (var property in properties)
            {
                var columnName = property.Name.ToLower();
                if (!row.IsNull(columnName))
                {
                    var columnValue = row.GetValue<object>(columnName);
                    property.SetValue(entity, Convert.ChangeType(columnValue, property.PropertyType));
                }
            }

            return entity;
        }

        public string BuildSelectQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return $"SELECT * FROM {_tableName} WHERE {BuildWhereClause(predicate.Body)}";
        }

        private string BuildWhereClause(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    return BuildBinaryExpression((BinaryExpression)expression, "=");
                case ExpressionType.LessThan:
                    return BuildBinaryExpression((BinaryExpression)expression, "<");
                case ExpressionType.GreaterThan:
                    return BuildBinaryExpression((BinaryExpression)expression, ">");
                case ExpressionType.LessThanOrEqual:
                    return BuildBinaryExpression((BinaryExpression)expression, "<=");
                case ExpressionType.GreaterThanOrEqual:
                    return BuildBinaryExpression((BinaryExpression)expression, ">=");
                case ExpressionType.AndAlso:
                    var andExpression = (BinaryExpression)expression;
                    var left = BuildWhereClause(andExpression.Left);
                    var right = BuildWhereClause(andExpression.Right);
                    return $"{left} AND {right}";
                // Add cases for other types of expressions as needed
                default:
                    throw new NotSupportedException($"Expression type {expression.NodeType} is not supported.");
            }
        }

        private string BuildBinaryExpression(BinaryExpression expression, string op)
        {
            var leftOperand = ((MemberExpression)expression.Left).Member.Name.ToLower();
            var rightOperand = GetConstantValue(expression.Right);
            return $"{leftOperand} {op} {rightOperand}";
        }

        private object GetConstantValue(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    return ((ConstantExpression)expression).Value;
                case ExpressionType.Convert:
                    var unaryExpression = (UnaryExpression)expression;
                    return GetConstantValue(unaryExpression.Operand);
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)expression;
                    var instance = memberExpression.Expression != null
                        ? Evaluate(memberExpression.Expression) // Recursively evaluate the instance expression
                        : null;
                    if (memberExpression.Member is FieldInfo field)
                    {
                        return field.GetValue(instance);
                    }
                    else if (memberExpression.Member is PropertyInfo property)
                    {
                        return property.GetValue(instance);
                    }
                    throw new NotSupportedException($"Expression type {expression.NodeType} is not supported.");
                default:
                    throw new NotSupportedException($"Expression type {expression.NodeType} is not supported.");
            }
        }

        private object Evaluate(Expression expression)
        {
            var lambda = Expression.Lambda(expression);
            var compiledLambda = lambda.Compile();
            return compiledLambda.DynamicInvoke();
        }

        private bool CheckTableExists()
        {
            var query = $"SELECT * FROM system_schema.tables WHERE keyspace_name = '{_session.Keyspace}' AND table_name = '{_tableName}'";
            var result = _session.Execute(query);
            return !result.IsExhausted();
        }

        private void CreateTable()
        {
            var columns = string.Join(", ", CassandraTypeMapper.GetTableColumnsDefinition<TEntity>());
            var query = $"CREATE TABLE IF NOT EXISTS {_tableName} ({columns})";
            _session.Execute(query);
        }
    }
}
