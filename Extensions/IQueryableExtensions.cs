using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vega.Data.Models;
using Vega.Interfaces;

namespace Vega.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject queryObj, Dictionary<string, Expression<Func<T, object>>> columnsMapping)
        {
            if (String.IsNullOrWhiteSpace(queryObj.SortBy) || !columnsMapping.ContainsKey(queryObj.SortBy))
                return query;

            if (queryObj.IsSortAscending)
                query = query.OrderBy(columnsMapping[queryObj.SortBy]);
            else
                query = query.OrderByDescending(columnsMapping[queryObj.SortBy]);
                
            return query;
        }
    }
}