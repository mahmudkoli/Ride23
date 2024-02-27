using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Metadata;

namespace Ride23.Framework.Persistence.NoSQL;
public class CassandraTypeMapper
{
    // Dictionary mapping .NET types to Cassandra CQL types
    private static readonly Dictionary<Type, string> TypeMappings = new Dictionary<Type, string>
    {
        { typeof(bool), "boolean" },
        { typeof(byte), "tinyint" },
        { typeof(sbyte), "tinyint" },
        { typeof(short), "smallint" },
        { typeof(ushort), "smallint" },
        { typeof(int), "int" },
        { typeof(uint), "int" },
        { typeof(long), "bigint" },
        { typeof(ulong), "bigint" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
        { typeof(char), "text" },
        { typeof(string), "text" },
        { typeof(Guid), "uuid" },
        { typeof(DateTime), "timestamp" },
        { typeof(byte[]), "blob" }
        // Add more mappings as needed
    };

    private static string GetCqlType(Type propertyType)
    {
        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            // If the property is a nullable type, unwrap the underlying type
            propertyType = Nullable.GetUnderlyingType(propertyType);
        }

        // Check if the property type is in the dictionary of type mappings
        if (TypeMappings.TryGetValue(propertyType, out string cqlType))
        {
            return cqlType;
        }

        throw new NotSupportedException($"Type {propertyType.Name} is not supported in Cassandra.");
    }

    private static bool IsValidProperty(Type propertyType)
    {
        // Get the underlying type if the property is nullable
        Type underlyingType = Nullable.GetUnderlyingType(propertyType);
        if (underlyingType != null)
        {
            propertyType = underlyingType;
        }

        // Check if the property type is in the dictionary of type mappings
        return TypeMappings.ContainsKey(propertyType);
    }

    private static string GetColumnDefinition(PropertyInfo property)
    {
        var columnName = property.Name.ToLower();
        var propertyType = property.PropertyType;

        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            propertyType = Nullable.GetUnderlyingType(propertyType);
        }

        var columnType = GetCqlType(propertyType);
        var primaryKey = IsPrimaryKeyProperty(property) ? " PRIMARY KEY" : "";

        return $"{columnName} {columnType} {primaryKey}";
    }

    private static bool IsPrimaryKeyProperty(PropertyInfo property)
    {
        return Attribute.IsDefined(property, typeof(KeyAttribute)) || property.Name.ToLower() == "id";
    }

    private static IEnumerable<PropertyInfo> FilterProperties(Type entityType, bool excludeId = false)
    {
        return entityType.GetProperties().Where(p => (!excludeId || p.Name.ToLower() != "id") && IsValidProperty(p.PropertyType));
    }

    public static IEnumerable<PropertyInfo> GetProperties<TEntity>() where TEntity : class
    {
        return typeof(TEntity).GetProperties().Where(property => IsValidProperty(property.PropertyType));
    }

    public static string GetTableName<TEntity>() where TEntity : class
    {
        string tableName = typeof(TEntity).Name.ToLower();
        if (tableName.EndsWith("s") || tableName.EndsWith("ch") || tableName.EndsWith("sh") || tableName.EndsWith("x") || tableName.EndsWith("z"))
        {
            return tableName + "es";
        }
        else if (tableName.EndsWith("y") && tableName.Length > 1 && !IsVowel(tableName[tableName.Length - 2]))
        {
            return tableName.Remove(tableName.Length - 1) + "ies";
        }
        else
        {
            return tableName + "s";
        }
    }

    public static IEnumerable<string> GetTableColumnsDefinition<TEntity>() where TEntity : class
    {
        return FilterProperties(typeof(TEntity)).Select(GetColumnDefinition);
    }
    
    public static IEnumerable<string> GetTableColumns<TEntity>(bool excludeId = false) where TEntity : class
    {
        return FilterProperties(typeof(TEntity), excludeId).Select(p => p.Name.ToLower());
    }
    
    public static IEnumerable<object?> GetTableValues<TEntity>(TEntity entity, bool excludeId = false) where TEntity : class
    {
        return FilterProperties(typeof(TEntity), excludeId).Select(p => p.GetValue(entity));
    }

    private static bool IsVowel(char c)
    {
        return "aeiouAEIOU".IndexOf(c) != -1;
    }
}
