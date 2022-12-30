using System.Diagnostics;
using System.Text;
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
        var rocks = BuildRockMap(data);
        var sands = new List<Point>();
        
        var cnt = 0; // Iterations
        var isEnd = false;
        
        // Simulation
        while (true)
        {
            // Create new sand
            var currentSandPosition = new Point(500, 0);
            
            // Move new sand to rest
            while (true)
            {
                cnt++;
                // Visualisation
                DrawDebugState(cnt, rocks, sands, currentSandPosition);
                var nextSandPosition = NextPosition(rocks, sands, currentSandPosition);
                if (nextSandPosition == Point.Empty)
                {
                    // Add rest sand and continue
                    sands.Add(currentSandPosition);
                    break;
                }

                if (nextSandPosition == Point.Infinity)
                {
                    // End of generation - sand in infinity
                    isEnd = true;
                    break;
                }
                
                currentSandPosition = nextSandPosition;
            }

            if (isEnd) break;
        }
        
        return sands.Count.ToString();
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private static List<Point> BuildRockMap(IEnumerable<string> stringMap)
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

    private static void DrawDebugState(
        int number,
        ICollection<Point> rock,
        ICollection<Point> sands,
        Point current,
        bool inDebug = true)
    {
        if (inDebug)
        {
            Debug.WriteLine($"Step: {number} Sands: {sands.Count}");
            Debug.WriteLine(DrawMap(rock, sands, current));
            Debug.WriteLine(string.Empty);
        }
        else
        {
            Console.Clear();
            Console.WriteLine($"===== Step: {number} ======");
            Console.WriteLine(DrawMap(rock, sands, current));
            Console.WriteLine(string.Empty);
        }
    }
    
    private static string DrawMap(ICollection<Point> rock, ICollection<Point> sand, Point sandPosition)
    {
        var minX = rock.Select(e => e.X).Min();
        var maxX = rock.Select(e => e.X).Max();
        var maxY = rock.Select(e => e.Y).Max();

        var sb = new StringBuilder();
        for (var row = 0; row <= maxY; row++)
        {
            for (var col = minX; col <= maxX; col++)
            {
                var currentPoint = new Point(col, row);
                if (currentPoint == sandPosition)
                {
                    sb.Append('+'); // Active sand
                }
                else if (sand.Contains(currentPoint))
                {
                    sb.Append('o'); // Rest sand
                }
                else
                {
                    sb.Append(rock.Contains(currentPoint) ? '#' : '.'); // Space rock or empty
                }
            }

            sb.AppendLine(string.Empty);
        }

        return sb.ToString();
    }

    private static Point NextPosition(
        IEnumerable<Point> rock,
        IEnumerable<Point> sands,
        Point current)
    {
        var busyPoints = rock.Concat(sands).ToArray();
        
        // Possible next positions coordinates
        var p1 = new Point (current.X, current.Y + 1);  // Down
        var p2 = new Point (current.X - 1, current.Y + 1); // Left - Down
        var p3 = new Point (current.X + 1, current.Y + 1); // Right - Down
        var posiblePositions = new List<Point> { p1, p2, p3 };
        
        // Can't move - sand in rest
        if (posiblePositions.All(e => busyPoints.Contains(e)))
            return Point.Empty;
        
        // Next Position (first from possible positions list)
        foreach (var next in posiblePositions)
        {
            if (!busyPoints.Contains(next))
            {
                // Block borders coords for sand
                // var minX = busyPoints.Select(e => e.X).Min();
                // var maxX = busyPoints.Select(e => e.X).Max();
                // var minY = busyPoints.Select(e => e.Y).Min();
                // var maxY = busyPoints.Select(e => e.Y).Max();
                
                // Next position outside
                // if ((next.X < minX || maxX > next.X)
                //     || (next.Y < minY || next.Y > maxY))
                // {
                //     return Point.Infinity;
                // }
                
                return next;
            }
        }
        
        // WTF? Matrix has you
        throw new InvalidOperationException("Broken!");
    }
    
    private readonly record struct Point(int X, int Y)
    {
        public static readonly Point Empty = new Point(-1, -1);
        public static readonly Point Infinity = new Point(-2, -2);
        
        public static Point Parse(string point)
        {
            var coords = point.Split(",", Options);

            return new Point(int.Parse(coords[0]), int.Parse(coords[1]));
        }
    }
}
