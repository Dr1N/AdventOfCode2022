using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day12Solver : ISolver
{
    private readonly IEnumerable<string> data;
    private int[,] map;

    public Day12Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        InitMap(data);
    }

    public string PartOne()
    {
        return string.Empty;
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private void InitMap(IEnumerable<string> data)
    {
        
    }

    private record Point(int X, int Y);
}
