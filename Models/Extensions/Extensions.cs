using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookApi.Models.Extensions
{
  public static class Extensions
  {
    public static IOrderedQueryable<T> OrderByMember<T>(this IQueryable<T> source, string memberPath, bool descending)
    {
      ParameterExpression parameter = Expression.Parameter(typeof(T), "item");

      Expression member = memberPath.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);

      LambdaExpression keySelector = Expression.Lambda(member, parameter);

      MethodCallExpression methodCall = Expression.Call(
          typeof(Queryable), descending ? "OrderByDescending" : "OrderBy",
          new[] { parameter.Type, member.Type },
          source.Expression, Expression.Quote(keySelector));

      return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
    }
  }
}