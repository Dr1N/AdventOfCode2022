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
        var simulator = new Simulator();
        foreach (var command in data)
        {
            simulator.Move(new Simulator.Vector(command));
        }

        var result = simulator.TailPath.Distinct().Count();
        
        return result.ToString();
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
    
    private class Simulator
    {
        private readonly List<Point> headPath = new() { new Point(0, 0) };
        public readonly List<Point> TailPath = new() { new Point(0, 0) };

        public void Move(Vector vector)
        {
            var head = headPath.Last();
            var tail = TailPath.Last();
            var (hPath, tPath) = vector.GetPathCoords(head, tail);
            headPath.AddRange(hPath);
            TailPath.AddRange(tPath);
        }
        
        public record struct Point(int X, int Y);

        public readonly struct Vector
        {
            private Direction Direction { get; }

            private int Length { get; }

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

            public (IEnumerable<Point>, IEnumerable<Point>) GetPathCoords(Point headPoint, Point tailPoint)
            {
                var headResult = new List<Point>();
                var tailResults = new List<Point>();
                for (var i = 0; i < Length; i++)
                {
                    var (startX, startY) = headPoint;
                    Point nextHeadPoint;
                    switch (Direction)
                    {
                        case Direction.Left:
                            nextHeadPoint = new Point(startX - 1, startY);
                            headResult.Add(nextHeadPoint);
                            break;
                        case Direction.Right:
                            nextHeadPoint = new Point(startX + 1, startY);
                            headResult.Add(nextHeadPoint);
                            break;
                        case Direction.Up:
                            nextHeadPoint = new Point(startX, startY - 1);
                            headResult.Add(nextHeadPoint);
                            break;
                        case Direction.Down:
                            nextHeadPoint = new Point(startX, startY + 1);
                            headResult.Add(nextHeadPoint);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    var dist = Distance(nextHeadPoint, tailPoint);
                    if (dist > Math.Sqrt(2))
                    {
                        tailResults.Add(headPoint);
                    }
                    
                    headPoint = nextHeadPoint;
                }
                
                return (headResult, tailResults);
            }
        
            private static double Distance(Point p1, Point p2)
                => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}
