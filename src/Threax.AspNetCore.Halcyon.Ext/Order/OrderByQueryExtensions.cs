using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public static class OrderByQueryExtensions
    {
        /// <summary>
        /// Given the name of a property on the entity and an order this will create the appropriate
        /// IQueryable to sort on that property.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="orderByProperty">The property to order by, must exist in TEntity or an exception will be thrown.</param>
        /// <param name="direction">The direction to order results by.</param>
        /// <remarks>
        /// Originally written by user Icarus
        /// https://stackoverflow.com/questions/7265186/how-do-i-specify-the-linq-orderby-argument-dynamically
        /// </remarks>
        /// <returns>A new query ordered by the given property.</returns>
        public static IQueryable<TEntity> OrderByProperty<TEntity>(this IQueryable<TEntity> source, string orderByProperty, OrderDirection direction)
        {
            string command = direction == OrderDirection.Ascending ? "OrderBy" : "OrderByDescending";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
