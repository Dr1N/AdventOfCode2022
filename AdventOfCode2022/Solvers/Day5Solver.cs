using System.Text;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

/*
                [B] [L]     [J]    
            [B] [Q] [R]     [D] [T]
            [G] [H] [H] [M] [N] [F]
        [J] [N] [D] [F] [J] [H] [B]
    [Q] [F] [W] [S] [V] [N] [F] [N]
[W] [N] [H] [M] [L] [B] [R] [T] [Q]
[L] [T] [C] [R] [R] [J] [W] [Z] [L]
[S] [J] [S] [T] [T] [M] [D] [B] [H]
 1   2   3   4   5   6   7   8   9
 */

public class Day5Solver : ISolver
{
    private readonly IEnumerable<string> data;
    
    public Day5Solver(IEnumerable<string> data) 
        => this.data = data ?? throw new ArgumentNullException(nameof(data));
   
    public string PartOne()
    {
        var supply = new Supply();
        
        return DoWork(supply, supply.Use9000Crane);
    }

    public string PartTwo()
    {
        var supply = new Supply();

        return DoWork(supply, supply.Use9001Crane);
    }

    private string DoWork(Supply supply, Action<Supply.Operation> action)
    {
        foreach (var command in data)
        {
            action(new Supply.Operation(command));
        }

        var topCrates = supply.GetTops();
        var result = topCrates.Select(e => e.Trim('[', ']'));

        return string.Join(string.Empty, result);
    }
    
    private readonly struct Supply
    {
        private readonly List<Stack<string>> crates = new();

        public Supply()
        {
            crates.AddRange(new []
            {
                new Stack<string>(new []{"[W]", "[L]", "[S]"}),
                new Stack<string>(new []{"[Q]", "[N]", "[T]", "[J]"}),
                new Stack<string>(new []{"[J]", "[F]", "[H]", "[C]", "[S]"}),
                new Stack<string>(new []{"[B]", "[G]", "[N]", "[W]", "[M]", "[R]", "[T]"}),
                new Stack<string>(new []{"[B]", "[Q]", "[H]", "[D]", "[S]", "[L]", "[R]", "[T]"}),
                new Stack<string>(new []{"[L]", "[R]", "[H]", "[F]", "[V]", "[B]", "[J]", "[N]"}),
                new Stack<string>(new []{"[M]", "[J]", "[N]", "[R]", "[W]", "[D]"}),
                new Stack<string>(new []{"[J]", "[D]", "[N]", "[H]", "[F]", "[T]", "[Z]", "[B]"}),
                new Stack<string>(new []{"[T]", "[F]", "[B]", "[N]", "[Q]", "[L]", "[H]"}),
            });
            
            // Fix for incoming data
            ReverseStacks();
        }

        internal void Use9000Crane(Operation operation)
        {
            var source = crates[operation.From - 1];
            var destination = crates[operation.To - 1];
            for (var i = 0; i < operation.Quantity; i++)
            {
                destination.Push(source.Pop());
            }
        }

        public void Use9001Crane(Operation operation)
        {
            var source = crates[operation.From - 1];
            var destination = crates[operation.To - 1];
            var crane = new Stack<string>();
            
            for (var i = 0; i < operation.Quantity; i++)
            {
                crane.Push(source.Pop());
            }

            var craneCapacity = crane.Count;
            for (var i = 0; i < craneCapacity; i++)
            {
                destination.Push(crane.Pop());
            }
        }
        
        public IEnumerable<string> GetTops()
        {
            var result = new List<string>(crates.Count);
            result.AddRange(crates.Select(stack => stack.Peek()));

            return result;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            var cnt = 0;
            foreach (var list in crates.Select(stack => new List<string>(stack)))
            {
                list.Reverse();
                sb.Append($" {++cnt} ");
                sb.Append(string.Join(" ", list));
                sb.AppendLine();
            }
            
            return sb.ToString();
        }
        
        private void ReverseStacks()
        {
            foreach (var stack in crates)
            {
                var list = new List<string>(stack);
                stack.Clear();
                list.ForEach(e => stack.Push(e));
            }
        }
        
        public readonly struct Operation
        {
            public Operation(string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new ArgumentException($"Input string is empty");
                }
                
                var parts = str.Split(' ');

                if (parts.Length != 6)
                {
                    throw new ArgumentException($"Input string invalid format: {str}");
                }
                
                var qntSuccess = int.TryParse(parts[1], out var quantity);
                var fromSuccess = int.TryParse(parts[3], out var from);
                var toSuccess = int.TryParse(parts[5], out var to);

                if (!qntSuccess || !fromSuccess || !toSuccess )
                {
                    throw new ArgumentException($"Input string invalid values: {str}");
                }
                
                Quantity = quantity;
                From = from;
                To = to;
            }
        
            public int From { get; }
        
            public int To { get; }
        
            public int Quantity { get; }

            public override string ToString()
            {
                return $"move {Quantity} from {From} to {To}";
            }
        }
    }
}
