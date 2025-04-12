using System.Linq.Expressions;
using ExpressionTreeExample.Filters.Enums;

namespace ExpressionTreeExample.Filters;

public class FilterCriterionBuilder
{
    public static Expression<Func<T, bool>>? BuildCondition<T>(IEnumerable<FilterCondition> filters)
        where T : class
    {
        Expression filterExpression = null;
        var param = Expression.Parameter(typeof(T), "p");

        foreach (var filter in filters)
        {
            filterExpression = BuildFilterExpression<T>(param, filter);
        }

        return Expression.Lambda<Func<T, bool>>(filterExpression!, param);;
    }

    public static Expression<Func<T, bool>>? BuildCondition<T>(List<FilterGroup> filterGroups)
        where T : class
    {
        if (filterGroups.Count == 0)
            return null;

        var param = Expression.Parameter(typeof(T), "e");
        Expression? finalExpression = null;
        
        foreach (var group in filterGroups)
        {
            Expression? groupExpression = null;
            foreach (var condition in group.Conditions)
            {
                var conditionExpression = BuildFilterExpression<T>(param, condition);
                groupExpression = ApplyLogicalOperator(groupExpression, conditionExpression, condition.LogicalOperator);
            }

            if (groupExpression != null)
            {
                finalExpression = ApplyLogicalOperator(finalExpression, groupExpression, group.LogicalOperator);
            }
        }

        return finalExpression != null ? Expression.Lambda<Func<T, bool>>(finalExpression, param) : null;
    }

    private static Expression BuildFilterExpression<T>(ParameterExpression param, FilterCondition filterCondition) 
        where T : class
    {
        Expression<Func<T, bool>> filterExpression;
        Expression property = Expression.Property(param, filterCondition.Field);
        var filterPropertyValue = filterCondition.Value;
        var constantValue = Expression.Constant(filterPropertyValue);
        Expression? condition = filterCondition.Operator switch
        {
            OperatorType.GreaterThan => Expression.GreaterThan(property, constantValue),
            OperatorType.LessThan => Expression.LessThan(property, constantValue),
            OperatorType.Equals => Expression.Equal(property, constantValue),
            OperatorType.Contains => Expression.Call(property, typeof(string).GetMethod("Contains", [typeof(string)])!, constantValue),
            _ => null
        };
        
        return condition;
    }

    private static Expression ApplyLogicalOperator(Expression? left, Expression right, LogicalOperator logicalOperator)
    {
        return left == null
            ? right
            : logicalOperator == LogicalOperator.And
                ? Expression.AndAlso(left, right)
                : Expression.OrElse(left, right);
    }
}