using System.Linq.Expressions;

namespace ExpressionTreeExample.Filters;

public class FilterCriterionBuilder
{
    public static Expression<Func<T, bool>>? BuildCondition<T>(IEnumerable<FilterCriteria> filters)
        where T : class
    {
        Expression<Func<T, bool>>? filterExpression = null;
        var param = Expression.Parameter(typeof(T), "p");

        foreach (var (filterPropertyName, filterPropertyValue, @operator) in filters)
        {
            Expression property = Expression.Property(param, filterPropertyName);
            Expression? condition = @operator switch
            {
                OperatorType.GreaterThan => Expression.GreaterThan(property, Expression.Constant(filterPropertyValue)),
                OperatorType.LessThan => Expression.LessThan(property, Expression.Constant(filterPropertyValue)),
                OperatorType.Equals => Expression.Equal(property, Expression.Constant(filterPropertyValue)),
                _ => null
            };

            filterExpression = Expression.Lambda<Func<T, bool>>(condition!, param);
        }

        return filterExpression;
    }
}