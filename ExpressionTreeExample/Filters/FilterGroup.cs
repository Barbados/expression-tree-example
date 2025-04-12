using ExpressionTreeExample.Filters.Enums;

namespace ExpressionTreeExample.Filters;

public class FilterGroup
{
    public IEnumerable<FilterCondition> Conditions { get; set; }
    public LogicalOperator LogicalOperator { get; set; } = LogicalOperator.None;
}