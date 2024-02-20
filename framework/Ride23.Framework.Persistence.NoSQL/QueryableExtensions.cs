using Ride23.Framework.Core.Pagination;
using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ride23.Framework.Persistence.NoSQL
{
    public static class QueryableExtensions
    {
        public static async Task<PagedList<T>> ApplyPagingAsync<T>(this IQueryable<T> collection, ISession session, Guid pagingState, int pageSize)
        {
            if (pageSize <= 0) pageSize = 10;

            var statement = new SimpleStatement("SELECT * FROM your_table WHERE token(your_partition_key) > ? LIMIT ?", pagingState, pageSize);
            statement.SetPageSize(pageSize);

            var resultSet = await session.ExecuteAsync(statement);
            var pagedResult = new PagedList<T>(resultSet.Select(row => MapRowToDocument<T>(row)), 1,1,1);
            return pagedResult;
        }

        private static T MapRowToDocument<T>(Row row)
        {
            // Implement logic to map Cassandra row to your document type
            // Example:
            // var document = new T();
            // document.Property1 = row.GetValue<string>("column_name1");
            // document.Property2 = row.GetValue<int>("column_name2");
            // return document;
            return default; // Placeholder, replace with actual implementation
        }
    }
}
