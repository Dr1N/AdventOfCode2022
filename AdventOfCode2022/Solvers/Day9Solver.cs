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
        Console.BufferWidth = 400;
        Console.BufferHeight = 400;
        Console.Clear();
        
        var simmulation = new Simulator();
        var move = 0;
        foreach (var command in data)
        {
           //Console.Title = (++move).ToString();
            simmulation.Move(new Simulator.Vector(command));
            //Console.Clear();
           //simmulation.PrintConsole();
        }

        var headCount = simmulation.headPath;
        var tailCount = simmulation.tailPath;
        var result = simmulation.tailPath.Distinct().Count();
        
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
        
        public readonly List<Point> headPath = new() { new Point(0, 0) };
        public readonly List<Point> tailPath = new() { new Point(0, 0) };

        private const int OffsetX = 100;
        private const int OffsetY = 100;

        public void Move(Vector vector)
        {
            var head = headPath.Last();
            var tail = tailPath.Last();
            var (hPath, tPath) = vector.GetPathCoords(head, tail);
            headPath.AddRange(hPath);
            tailPath.AddRange(tPath);
        }

        public void PrintConsole()
        {
            foreach (var (x,y) in headPath)
            {
                Console.CursorLeft = x + OffsetX;
                Console.CursorTop = y + OffsetY;
                Console.Write("#");
            }
            
            foreach (var (x,y) in tailPath)
            {
                Console.CursorLeft = x + OffsetX;
                Console.CursorTop = y + OffsetY;
                Console.Write("~");
            }

            var head = headPath.Last();
            PrintSymbol(head, 'H', ConsoleColor.Green);
            
            var tail = tailPath.Last();
            PrintSymbol(tail, 'T', ConsoleColor.Red);
        }

        private void PrintSymbol(Point point, char sym, ConsoleColor consoleColor)
        {
            Console.CursorLeft = point.X + OffsetX;
            Console.CursorTop = point.Y + OffsetY;
            Console.ForegroundColor = consoleColor;
            Console.Write(sym);
            Console.ResetColor();
        }
    }
}

