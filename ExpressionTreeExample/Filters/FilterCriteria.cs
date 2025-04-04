namespace ExpressionTreeExample.Filters;

public record FilterCriteria(string Field, object Value, OperatorType Operator);

public enum OperatorType
{
    Equals,
    LessThan,
    GreaterThan
}