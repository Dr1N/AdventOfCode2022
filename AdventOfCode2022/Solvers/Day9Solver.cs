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
        var simmulation = new Simmulation();
        foreach (var command in data)
        {
            simmulation.Move(new Vector(command));
        }
        
        return string.Empty;
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
    
    private record struct Point(int X, int Y);

    private readonly record struct Vector(Direction Direction, int Length)
    {
        public Vector(string command) : this()
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentNullException(nameof(command));
            }
            
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

        public IEnumerable<Point> GetPathCoords(Point startPoint)
        {
            var result = new List<Point>();
            for (var i = 0; i < Length; i++)
            {
                var (startX, startY) = startPoint;
                switch (Direction)
                {
                    case Direction.Left:
                        result.Add(new Point(startX - 1, startY));
                        break;
                    case Direction.Right:
                        result.Add(new Point(startX + 1, startY));
                        break;
                    case Direction.Up:
                        result.Add(new Point(startX, startY - 1));
                        break;
                    case Direction.Down:
                        result.Add(new Point(startX, startY + 1));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            return result;
        }
    }
    
    private class Simmulation
    {
        private readonly List<Point> headPath = new() { new Point(0, 0) };
        private readonly List<Point> tailPath = new() { new Point(0, 0) };

        public void Move(Vector vector)
        {
            var head = headPath.Last();
            var path = vector.GetPathCoords(head);
            headPath.AddRange(path);
        }

        private static double Distance(Point p1, Point p2)
            => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        
        public override string ToString()
        {
            var head = string.Join("->", headPath);
            var tail = string.Join("->", headPath);
            
            return $"Head: {head}{Environment.NewLine}{tail}";
        }
    }
}

