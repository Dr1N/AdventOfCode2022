using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day8Solver : ISolver
{
    private readonly IEnumerable<string> data;

    public Day8Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        throw new NotImplementedException();
    }

    public string PartTwo()
    {
        throw new NotImplementedException();
    }
}
