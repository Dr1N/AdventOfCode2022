using AdventOfCode2022.Helpers;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

/// <inheritdoc />
public class SolverFactory : ISolverFactory
{
    private readonly IDataProvider dataProvider;

    public SolverFactory(IDataProvider dataProvider)
        => this.dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
    
    public ISolver CreateSolver(int day)
    {
        DayHelper.CheckDay(day);
        var data = GetDataFromProvider(day);
        
        return day switch
        {
            1 => new Day1Solver(data),
            2 => new Day2Solver(data),
            3 => new Day3Solver(data),
            4 => new Day4Solver(data),
            5 => new Day5Solver(data),
            6 => new Day6Solver(data),
            7 => new Day7Solver(data),
            8 => new Day8Solver(data),
            9 => new Day9Solver(data),
            10 => new Day10Solver(data),
            _ => throw new InvalidOperationException(
                $"Can't create solver for {day}",
                new NotImplementedException())
        };
    }

    private IEnumerable<string> GetDataFromProvider(int day)
    {
        try
        {
            return dataProvider.GetData(day);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Can't create solver for {day}. See inner exception", e);
        }
    }
}