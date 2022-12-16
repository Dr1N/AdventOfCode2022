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
        foreach (var movment in data)
        {
            simmulation.Move(movment);
        }
        
        return string.Empty;
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private record struct Point(int X, int Y);
    
    private class Simmulation
    {
        private readonly List<Point> headPath = new() { new Point(0, 0) };
        private readonly List<Point> tailPath = new() { new Point(0, 0) };

        public void Move(string command)
        {
            var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var direction = parts[0];
            var length = int.Parse(parts[1]);

            for (var i = 0; i < length; i++)
            {
                switch (direction)
                {
                    case "L":
                        break;
                    case "R":
                        break;
                    case "U":
                        break;
                    case "D":
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
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
