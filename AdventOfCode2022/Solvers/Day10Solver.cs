using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day10Solver : ISolver
{
    private readonly IEnumerable<string> data;

    public Day10Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        return string.Empty;
    }

    public string PartTwo()
    {
        return string.Empty;
    }
}
