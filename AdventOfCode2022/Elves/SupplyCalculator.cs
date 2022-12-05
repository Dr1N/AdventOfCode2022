using System.Text;

namespace AdventOfCode2022.Elves;

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

public static class SupplyCalculator
{
    private const string SupplyPlan = @"Data/Supply.txt";
    private static readonly string[] Data = File.ReadAllLines(SupplyPlan);

    private class Supply
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

            ReverseStacks();
        }

        public void Execute(Operation operation)
        {
            var source = crates[operation.From - 1];
            var dest = crates[operation.To - 1];
            for (var i = 0; i < operation.Quantity; i++)
            {
                dest.Push(source.Pop());
            }
        }

        public void Execute9001(Operation operation)
        {
            var source = crates[operation.From - 1];
            var dest = crates[operation.To - 1];
            var tmp = new List<string>();
            for (var i = 0; i < operation.Quantity; i++)
            {
                tmp.Add(source.Pop());
            }

            tmp.Reverse();
            tmp.ForEach(e => dest.Push(e));
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
            foreach (var stack in crates)
            {
                var list = new List<string>(stack);
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
    }

    private readonly struct Operation
    {
        public Operation(string str)
        {
            var parts = str.Split(' ');
            Quantity = int.Parse(parts[1]);
            From = int.Parse(parts[3]);
            To = int.Parse(parts[5]);
        }
        
        public int From { get; }
        
        public int To { get; }
        
        public int Quantity { get; }

        public override string ToString()
        {
            return $"move {Quantity} from {From} to {To}";
        }
    }
    
    public static string Calculate()
    {
        var supply = new Supply();
        foreach (var command in Data)
        {
            var operation = new Operation(command);
            supply.Execute9001(operation);
        }

        var tops = supply.GetTops();
        var result = tops.Select(e => e.Trim('[', ']'));

        return string.Join(string.Empty, result);
    }
}