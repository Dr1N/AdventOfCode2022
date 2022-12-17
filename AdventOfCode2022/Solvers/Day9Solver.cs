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
        var simulator = new RopeSimulator(2);
        foreach (var command in data)
        {
            simulator.Move(new Vector(command));
        }
        
        return simulator.LastPartPointCount.ToString();
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private class RopeSimulator
    {
        private static readonly double BreakDistance = Math.Sqrt(2.0);
        private readonly List<List<Point>> ropeCoords;
        
        public int LastPartPointCount => ropeCoords
            .Last()
            .Distinct()
            .Count();

        public RopeSimulator(int ropeParts)
        {
            ropeCoords = new List<List<Point>>();
            for (var i = 0; i < ropeParts; i++)
            {
                ropeCoords.Add(new List<Point> {new(0, 0)});
            }
        }
        
        public void Move(Vector vector)
        {
            for (var i = 0; i < ropeCoords.Count - 1; i++)
            {
                // First and second coords of rope parts
                var first = ropeCoords[i];
                var second = ropeCoords[i + 1];
                
                // Current coordinates of rope parts
                var firstPosition = first.Last();
                var secondPosition = second.Last();
                
                // Path parts after movement by vector
                var (firstPathCoords, secondPathCoords)
                    = GetPathCoords(vector, firstPosition, secondPosition);
                
                // Adding rope parts coordinates
                first.AddRange(firstPathCoords);
                second.AddRange(secondPathCoords);
            }
        }

        private static (IEnumerable<Point>, IEnumerable<Point>) GetPathCoords(
            Vector vector,
            Point basePoint,
            Point nextPoint)
        {
            var basePointPath = new List<Point>();
            var nextPointPath = new List<Point>();
            
            // Moving
            for (var i = 0; i < vector.Length; i++)
            {
                var nextBasePointPosition = vector.Direction switch
                {
                    Direction.Left => new Point(basePoint.X - 1, basePoint.Y),
                    Direction.Right => new Point(basePoint.X + 1, basePoint.Y),
                    Direction.Up => new Point(basePoint.X, basePoint.Y - 1),
                    Direction.Down => new Point(basePoint.X, basePoint.Y + 1),
                    _ => throw new ArgumentOutOfRangeException()
                };
                basePointPath.Add(nextBasePointPosition);
                basePoint = nextBasePointPosition;
                
                // Moving next part if needed
                var currentDistance = Distance(nextPoint, nextBasePointPosition);
                var isNeedMoveNextPoint = currentDistance > BreakDistance;
                if (isNeedMoveNextPoint)
                {
                    nextPoint = GetNearestPoint(nextPoint, nextBasePointPosition);
                    nextPointPath.Add(nextPoint);
                }
                else
                {
                    nextPointPath.Add(nextPoint);
                }
            }
            
            return (basePointPath, nextPointPath);
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
                .Select(e => Distance(e, dst))
                .ToList();
            var index = distances.IndexOf(distances.Min());
            
            return candidates[index];
        }
        
        private static double Distance(Point p1, Point p2)
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
