using ExpressionTreeExample.Filters.Enums;

namespace ExpressionTreeExample.Filters;

public record FilterCondition(string Field, object Value, OperatorType Operator, LogicalOperator LogicalOperator = LogicalOperator.None);