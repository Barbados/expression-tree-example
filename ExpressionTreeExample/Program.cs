// See https://aka.ms/new-console-template for more information

using ExpressionTreeExample.Filters;
using ExpressionTreeExample.Filters.Enums;

var records = new List<Person>
{
    new("Pedro", 45),
    new("Perdo", 43),
    new("Olga", 21),
    new(Name:"Albert", Age:43)
};

var filters = new List<FilterCondition>
{
    new("Age", 25, OperatorType.GreaterThan),
    new("Name", "ber", OperatorType.Contains, LogicalOperator.And)
};

var filterGroups = new List<FilterGroup>
{
    new()
    {
        Conditions = filters
    }
};

var filterExpression = FilterCriterionBuilder.BuildCondition<Person>(filterGroups);

var personsQuery = records.AsQueryable()
    .Where(filterExpression!)
    .ApplySorting("Name");
foreach (var record in personsQuery!)
{
    Console.WriteLine($"Person name: {record.Name}");
}

Console.WriteLine("Hello, World!");

internal record Person(string Name, int Age);