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
        simulator.Simulate();
        
        return simulator.GetAnswer().ToString();
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

        if (monkeyDescription.Length != 0)
        {
            result.Add(new Monkey(monkeyDescription.ToString()));
        }
        
        return result;
    }

    private class JungleSimulator
    {
        private const int Rounds = 20;
        
        private readonly List<Monkey> monkeys;

        public JungleSimulator(List<Monkey> monkeys) => this.monkeys = monkeys;

        public void Simulate()
        {
            for (var i = 0; i < Rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    while (true)
                    {
                        var thr = monkey.ThrowThing();
                        if (thr is null) break;

                        var targetMonkey = monkeys[thr.Monkey];
                        targetMonkey.CatchThing(thr.Thing);
                    }
                }
            }
        }

        public int GetAnswer() => monkeys
            .OrderByDescending(e => e.Inspects)
            .Take(2)
            .Select(e => e.Inspects)
            .Aggregate(1, (x,y) => x * y);
    }
    
    private class Monkey
    {
        public int Inspects { get; private set; }
        
        private const int WorryDivider = 3;
        
        private readonly Queue<int> things = new();

        private Func<int, int> operation;

        private int divider;

        private int trueMonkeyIndex;

        private int falseMonkeyIndex;

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

        public Throw ThrowThing()
        {
            if (!things.Any())
            {
                return null;
            }

            Inspects++;
            var thing = things.Dequeue();
            var value = operation(thing);
            var boredValue = value / WorryDivider;
            
            return boredValue % divider == 0
                ? new Throw(boredValue, trueMonkeyIndex)
                : new Throw(boredValue, falseMonkeyIndex);
        }

        public void CatchThing(int thing) => things.Enqueue(thing);
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Inspects: {Inspects}");
            sb.AppendLine($"Things: {string.Join(", ", things)}");
            
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
            trueMonkeyIndex = int.Parse(input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
            falseMonkeyIndex = int.Parse(input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        }
    }

    private record Throw(int Thing, int Monkey);
}
