using Core.Utilities.Ensures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class QuerableExtensions
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            Type? type = typeof(TEntity);
            PropertyInfo? property = type.GetProperties()
                             .FirstOrDefault(p => p.Name.ToLower().Equals( orderByProperty.ToLower()));
            Ensure.That(property, orderByProperty).IsNotNull();
            ParameterExpression? parameter = Expression.Parameter(type, "p");
            MemberExpression? propertyAccess = Expression.MakeMemberAccess(parameter, property!);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property!.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
