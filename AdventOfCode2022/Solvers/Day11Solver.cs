using System.Diagnostics;
using System.Text;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day11Solver : ISolver
{
    private readonly IEnumerable<string> data;

    public Day11Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        var monkeys = InitializeMonkey(data);
        var simulator = new JungleSimulator(monkeys);
        
        return string.Empty;
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private static List<Monkey> InitializeMonkey(IEnumerable<string> data)
    {
        var result = new List<Monkey>();
        var monkeyDescription = new StringBuilder();
        foreach (var line in data)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                result.Add(new Monkey(monkeyDescription.ToString()));
                monkeyDescription.Clear();
                continue;
            }

            monkeyDescription.AppendLine(line);
        }
        
        return result;
    }

    private class JungleSimulator
    {
        private const int Rounds = 20;
        
        private readonly List<Monkey> monkeys;

        public JungleSimulator(List<Monkey> monkeys)
        {
            this.monkeys = monkeys ?? throw new ArgumentNullException();
        }

        public void Simulate()
        {
            for (var i = 0; i < Rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    
                }
            }
        }

        public int GetAnswer()
        {
            var result = 0;
            
            return result;
        }
    }
    
    private class Monkey
    {
        public int Inspects { get; private set; }
        
        private const int WorryDivider = 3;
        
        private readonly Queue<int> things = new();

        private Func<int, int> operation;

        private int divider;

        private int trueMonkey;

        private int falseMonkey;

        public Monkey(string description)
        {
            var parts = description.Split(Environment.NewLine);
            InitThings(parts[1].Split(":", StringSplitOptions.RemoveEmptyEntries)[1]);
            InitOperation(parts[2].Split(":", StringSplitOptions.RemoveEmptyEntries)[1]);
            InitDivider(parts[3].Split(":", StringSplitOptions.RemoveEmptyEntries)[1]);
            InitMonkey(new[]
            {
                parts[4].Split(":", StringSplitOptions.RemoveEmptyEntries)[1],
                parts[5].Split(":", StringSplitOptions.RemoveEmptyEntries)[1],
            });
        }

        public Throw Throw()
        {
            return null;
        }

        public void Catch(int thing) => things.Enqueue(thing);
        
        public int GetWorryLevel()
        {
            return 0;
        }

        public int GetNextMonkey()
        {
            return 0;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Things: {string.Join(',', things)}");
            sb.AppendLine($"Operation: {operation}");
            sb.AppendLine($"Divider: {divider}");
            sb.AppendLine($"True index: {trueMonkey}");
            sb.AppendLine($"False index: {falseMonkey}");
            
            return sb.ToString();
        }
        
        private void InitThings(string input)
        {
            input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ForEach(e => this.things.Enqueue(int.Parse(e)));
        }
        
        private void InitOperation(string input)
        {
            var parts = input.Split('=', StringSplitOptions.RemoveEmptyEntries);
            var calcOperation = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];
            var argument = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last();

            if (int.TryParse(argument, out var arg))
            {
                operation = calcOperation switch
                {
                    "+" => e => e + arg,
                    "*" => e => e * arg,
                    "-" => e => e - arg,
                    "/" => e => e / arg,
                    _ => throw new InvalidOperationException()
                };
            }
            else if (argument == "old")
            {
                operation = calcOperation switch
                {
                    "+" => e => e + e,
                    "*" => e => e * e,
                    "-" => e => e - e,
                    "/" => e => e / e,
                    _ => throw new InvalidOperationException()
                };
            }
        }

        private void InitDivider(string input) =>
            divider = int.Parse(input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        
        private void InitMonkey(IReadOnlyList<string> input)
        {
            trueMonkey = int.Parse(input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
            falseMonkey = int.Parse(input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        }
    }

    private record Throw(int Thing, int Monkey);
}
