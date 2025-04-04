using System.Linq.Expressions;

namespace ExpressionTreeExample.Filters;

public static class DynamicSorter
{
    public static IQueryable<T>? ApplySorting<T>(this IQueryable<T>? query, 
        string sortBy, 
        bool descending = false)
    {
        if (string.IsNullOrEmpty(sortBy))
            return query;

        var param = Expression.Parameter(typeof(T), "p");
        var property = Expression.Property(param, sortBy);
        var lambda = Expression.Lambda(property, param);
        
        var methodName = descending ? "OrderByDescending" : "OrderBy";
        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type);

        return (IQueryable<T>)method.Invoke(null, [query, lambda])!;
    }
}