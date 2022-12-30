using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day14Solver : ISolver
{
    private const StringSplitOptions Options =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
    
    private readonly IEnumerable<string> data;

    public Day14Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        var rocks = BuildRockMap(data);
        var sands = new HashSet<Point>();
        var isEnd = false;
        
        // Simulation
        while (true)
        {
            // Create new sand
            var currentSandPosition = new Point(500, 0);
            
            // Move new sand to rest or outside
            while (true)
            {
                var nextSandPosition = NextPosition(rocks, sands, currentSandPosition);
                if (nextSandPosition == Point.Rest)
                {
                    sands.Add(currentSandPosition);
                    break;
                }
        
                if (nextSandPosition == Point.Infinity)
                {
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
        var rocks = BuildRockMapWithFloor(data);
        var sands = new HashSet<Point>();
        
        // Simulation
        while (true)
        {
            // Create new sand
            var currentSandPosition = new Point(500, 0);
            
            // Coords for block sand generator (500-0)
            var block = new HashSet<Point>
            {
                new(499, 1),
                new(500, 1),
                new (501, 1),
            };

            // Generator blocked - end of work
            if (block.All(e => sands.Contains(e)))
            {
                // Last sand
                sands.Add(new Point(500,0));
                break;
            }
            
            // Move new sand to rest
            while (true)
            {
                var nextSandPosition = NextPositionWithFloor(rocks, sands, currentSandPosition);
                if (nextSandPosition == Point.Rest)
                {
                    sands.Add(currentSandPosition);
                    break;
                }
                currentSandPosition = nextSandPosition;
            }
        }
        
        return sands.Count.ToString();
    }

    private static RockMap BuildRockMap(IEnumerable<string> stringMap)
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
                // Vertical
                if (start.X == end.X) 
                {
                    var (minY, maxY) = MinMax(start.Y,  end.Y);
                    Enumerable
                        .Range(minY, maxY - minY + 1)
                        .ToList()
                        .ForEach(e => results.Add(new Point(start.X, e)));
                }
                // Horizontal
                else if (start.Y == end.Y) 
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
        
        return new RockMap(results);
    }
    
    private static RockMap BuildRockMapWithFloor(IEnumerable<string> stringMap)
    {
        var result = BuildRockMap(stringMap);
        
        // Max sand positions from start point (triangle - 90-45-45)
        var height = result.MaxY + 2;
        var minX = result.MinX;
        var maxX = result.MaxX;
        for (var i = minX - height; i < maxX + height; i++)
        {
            result.Rock.Add(new Point(i, height));
        }
        
        return result;
    }
    
   private static Point NextPosition(
        RockMap rock,
        ICollection<Point> sands,
        Point current)
    {
        var posiblePositions = GeneratePossiblePositions(current);

        // Can't move - sand in rest
        var blockedPositionCount = posiblePositions.Count(rock.Rock.Contains) + posiblePositions.Count(sands.Contains);
        if (blockedPositionCount == posiblePositions.Count)
            return Point.Rest;
        
        // Next Position (first from possible positions list)
        foreach (var next in posiblePositions)
        {
            // Position is blocked go to next position
            if (rock.Rock.Contains(next) || sands.Contains(next)) continue;
            
            // Next position outside
            if (next.X < rock.MinX || rock.MaxX < next.X || next.Y > rock.MaxY)
                return Point.Infinity;
            
            return next;
        }
        
        // WTF? Matrix has you
        throw new InvalidOperationException("Broken!");
    }

   private static Point NextPositionWithFloor(
        RockMap rock,
        ICollection<Point> sands,
        Point current)
    {
        // Possible next positions coordinates
        var posiblePositions = GeneratePossiblePositions(current);
        
        // Can't move - sand in rest
        var blockedPositionCount = posiblePositions.Count(rock.Rock.Contains) + posiblePositions.Count(sands.Contains);
        if (blockedPositionCount == posiblePositions.Count)
            return Point.Rest;
        
        // Next possible position
        foreach (var next in posiblePositions)
        {
            // Position is blocked go to next position
            if (rock.Rock.Contains(next) || sands.Contains(next)) continue;
            
            return next;
        }
        
        // WTF? Matrix has you
        throw new InvalidOperationException("Broken!");
    }
    
   private static List<Point> GeneratePossiblePositions(Point current)
   {
       var p1 = new Point(current.X, current.Y + 1); // Down
       var p2 = new Point(current.X - 1, current.Y + 1); // Left - Down
       var p3 = new Point(current.X + 1, current.Y + 1); // Right - Down
       
       return new List<Point> { p1, p2, p3 };
   }
   
    private readonly record struct Point(int X, int Y)
    {
        public static readonly Point Rest = new(-1, -1);
        public static readonly Point Infinity = new(-2, -2);
        
        public static Point Parse(string point)
        {
            var coords = point.Split(",", Options);

            return new Point(int.Parse(coords[0]), int.Parse(coords[1]));
        }
    }

    private readonly struct RockMap
    {
        public ISet<Point> Rock { get; }
        public int MinX { get; }
        public int MaxX { get; }
        public int MaxY { get; }
        
        public RockMap(ISet<Point> rock)
        {
            Rock = rock;
            MinX = rock.Select(e => e.X).Min();
            MaxX = rock.Select(e => e.X).Max();
            MaxY = rock.Select(e => e.Y).Max();
        }
    }
}
