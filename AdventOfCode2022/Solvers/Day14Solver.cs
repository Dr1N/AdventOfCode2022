using System.Diagnostics;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day14Solver : ISolver
{
    private const StringSplitOptions Options = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
    private readonly IEnumerable<string> data;

    public Day14Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        var map = BuildMap(data);
        var width = map.Select(e => e.X).Max() - map.Select(e => e.X).Min() + 1;
        var heigth = map.Select(e => e.Y).Max() - map.Select(e => e.Y).Min() + 1;
        
        Debug.WriteLine($"{width} x {heigth}");
        
        return string.Empty;
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private static List<Point> BuildMap(IEnumerable<string> stringMap)
    {
        (int Min, int Max) MinMax(int p1, int p2)
            => (Math.Min(p1, p2), Math.Max(p1, p2));
        
        var results = new HashSet<Point>();
        foreach (var line in stringMap)
        {
            var points = line.Split("->", Options);
            for (var i = 0; i < points.Length - 1; i++)
            {
                var start = Point.Parse(points[i]);
                var end = Point.Parse(points[i + 1]);
                if (start.X == end.X) // Vertical
                {
                    var (minY, maxY) = MinMax(start.Y,  end.Y);
                    Enumerable
                        .Range(minY, maxY - minY + 1)
                        .ToList()
                        .ForEach(e => results.Add(new Point(start.X, e)));
                }
                else if (start.Y == end.Y) // Horizontal
                {
                    var (minX, maxX) = MinMax(start.X, end.X);
                    Enumerable
                        .Range(minX, maxX - minX + 1)
                        .ToList()
                        .ForEach(e => results.Add(new Point(e, start.Y)));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
        
        return results.ToList();
    }

    private static string DrawMap(IEnumerable<Point> map)
    {
        return string.Empty;
    }
    
    private readonly record struct Point(int X, int Y)
    {
        public static Point Parse(string point)
        {
            var coords = point.Split(",", Options);

            return new Point(int.Parse(coords[0]), int.Parse(coords[1]));
        }
    }
}
