// See https://aka.ms/new-console-template for more information

using ExpressionTreeExample.Filters;

var records = new List<Person>
{
    new("Pedro", 45),
    new("Olga", 21),
    new(Name:"Albert", Age:43)
};

var filters = new List<FilterCriteria>
{
    new("Age", 25, OperatorType.GreaterThan)
};

var filterExpression = FilterCriterionBuilder.BuildCondition<Person>(filters);

var personsQuery = records.AsQueryable()
    .Where(filterExpression!)
    .ApplySorting("Name");
foreach (var record in personsQuery!)
{
    Console.WriteLine($"Person name: {record.Name}");
}

Console.WriteLine("Hello, World!");

internal record Person(string Name, int Age);