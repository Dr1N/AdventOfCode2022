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
        var simulator = new RopeSimulator(10);
        foreach (var command in data)
        {
            simulator.Move(new Vector(command));
        }
        
        return simulator.LastPartPointCount.ToString();
    }

    private class RopeSimulator
    {
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
                var first = ropeCoords[i];
                var second = ropeCoords[i + 1];
                var (f, s) = GetPairCoords(vector, first.Last(), second.Last());
                first.AddRange(f);
                second.AddRange(s);
            }
        }

        private static (IEnumerable<Point>, IEnumerable<Point>) GetPairCoords(Vector vector, Point first, Point second)
        {
            var firstPath = new List<Point>();
            var secondPath = new List<Point>();
            for (var i = 0; i < vector.Length; i++)
            {
                var (startX, startY) = first;
                var nextFirstPoint = vector.Direction switch
                {
                    Direction.Left => new Point(startX - 1, startY),
                    Direction.Right => new Point(startX + 1, startY),
                    Direction.Up => new Point(startX, startY - 1),
                    Direction.Down => new Point(startX, startY + 1),
                    _ => throw new ArgumentOutOfRangeException()
                };

                firstPath.Add(nextFirstPoint);
                
                var dist = Distance(nextFirstPoint, second);
                if (dist > Math.Sqrt(2))
                {
                    secondPath.Add(first);
                }
                
                first = nextFirstPoint;
            }
            
            return (firstPath, secondPath);
        }

        private static double Distance(Point p1, Point p2)
            => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
    
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
}
