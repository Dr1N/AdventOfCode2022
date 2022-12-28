using System.Diagnostics;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day14Solver : ISolver
{
    private const StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
    private readonly IEnumerable<string> data;

    public Day14Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        var map = BuildMap(data);
        var maxX = map.Select(e => e.X).Max();
        var minX = map.Select(e => e.X).Min();
        var maxY = map.Select(e => e.Y).Max();
        var minY = map.Select(e => e.Y).Min();
        
        Debug.WriteLine($"{minX}...{maxX} : {minY} ... {maxY}");
        
        return string.Empty;
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private static List<Point> BuildMap(IEnumerable<string> stringMap)
        => (from line in stringMap
                from point in line.Split("->", options) 
                select point.Split(',', options) 
                into coords 
            select new Point(int.Parse(coords[0]), int.Parse(coords[1])))
            .ToList();
    private record struct Point(int X, int Y);
}
