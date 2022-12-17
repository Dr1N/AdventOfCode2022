using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day9Solver : ISolver
{
    private readonly IEnumerable<string> data;
    
    public Day9Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        this.data = data;
    }

    public string PartOne()
    {
        var vectors = data
            .Select(e => new Vector(e))
            .ToList();
        
        var headWay = RopeSimulator.MoveHead(vectors);
        var tailWay = RopeSimulator.MoveTail(headWay);
        
        return tailWay.Distinct().Count().ToString();
    }

    public string PartTwo()
    {
        var vectors = data
            .Select(e => new Vector(e))
            .ToList();
        
        var headWay = RopeSimulator.MoveHead(vectors);
        var tailWay = RopeSimulator.MoveTail(headWay);
        
        return tailWay.Distinct().Count().ToString();
    }

    private static class RopeSimulator
    {
        private static readonly double BreakDistance = Math.Sqrt(2.0);
        
        public static IEnumerable<Point> MoveHead(IEnumerable<Vector> vectors)
        {
            var result = new List<Point>{ new (0, 0) };
            foreach (var vector in vectors)
            {
                for (var i = 0; i < vector.Length; i++)
                {
                    var lastPosition = result.Last();
                    var nextPosition = GetNextPosition(lastPosition, vector.Direction);
                    result.Add(nextPosition);
                }
            }
            
            return result;
        }

        public static IEnumerable<Point> MoveTail(IEnumerable<Point> headWay)
        {
            var result = new List<Point> { new(0, 0) };
            foreach (var point in headWay)
            {
                var lastPosition = result.Last();
                var realDistance = GetDistance(lastPosition, point);
                var isBreak = realDistance > BreakDistance;
                if (!isBreak) continue;
                var nextPoint = GetNearestPoint(lastPosition,point);
                result.Add(nextPoint);;
            }

            return result;
        }
        
        private static Point GetNextPosition(Point point, Direction direction)
        {
            return direction switch
            {
                Direction.Left => new Point(point.X - 1, point.Y),
                Direction.Right => new Point(point.X + 1, point.Y),
                Direction.Up => new Point(point.X, point.Y - 1),
                Direction.Down => new Point(point.X, point.Y + 1),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private static Point GetNearestPoint(Point src, Point dst)
        {
            // Generate candidates
            var candidates = new List<Point>(8)
            {
                new(src.X + 1, src.Y),
                new(src.X - 1, src.Y),
                new(src.X, src.Y - 1),
                new(src.X, src.Y + 1),
                new(src.X - 1, src.Y - 1),
                new(src.X + 1, src.Y + 1),
                new(src.X - 1, src.Y + 1),
                new(src.X + 1, src.Y - 1),
            };
            
            // Select next position by min distance
            var distances = candidates
                .Select(e => GetDistance(e, dst))
                .ToList();
            var index = distances.IndexOf(distances.Min());
            
            return candidates[index];
        }
        
        private static double GetDistance(Point p1, Point p2)
            => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
    
    #region Stuctures
    
    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
    
    private readonly record struct Point(int X, int Y);

    private readonly record struct Vector
    {
        public Direction Direction { get; }

        public int Length { get; }

        public Vector(string command)
        {
            var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var direction = parts[0];
            
            Direction = direction switch
            {
                "L" => Direction.Left,
                "R" => Direction.Right,
                "U" => Direction.Up,
                "D" => Direction.Down,
                _ => throw new ArgumentOutOfRangeException()
            };
            Length = int.Parse(parts[1]);
        }
    }
    
    #endregion
}
