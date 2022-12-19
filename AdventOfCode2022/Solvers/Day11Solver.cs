using System.Text;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day11Solver : ISolver
{
    private const int WorryDividerOne = 3;
    private const int RoundsOne = 20;
    private const int RoundsTwo = 10_000;
    
    private readonly IEnumerable<string> data;

    public Day11Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        var monkeys = InitializeMonkey(data);
        var simulator = new JungleSimulator(monkeys, RoundsOne);
        simulator.Simulate(e => e / WorryDividerOne);
        
        return simulator.GetAnswer().ToString();
    }

    public string PartTwo()
    {
        var monkeys = InitializeMonkey(data);
        var simulator = new JungleSimulator(monkeys, RoundsTwo);
        var lcm = monkeys.Select(e => e.Divider).Aggregate(1, (x, y) => x * y);
        simulator.Simulate(e => e % lcm);
        
        return simulator.GetAnswer().ToString();
    }

    /// <summary>
    /// Create Monkeys from input string
    /// </summary>
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

    /// <summary>
    /// Rounds simulator (monkey manager)
    /// </summary>
    private sealed class JungleSimulator
    {
        private readonly int rounds;
        
        private readonly List<Monkey> monkeys;

        public JungleSimulator(List<Monkey> monkeys, int rounds)
        {
            this.rounds = rounds;
            this.monkeys = monkeys;
        } 

        public void Simulate(Func<long, long> worryOperation)
        {
            for (var i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    while (true)
                    {
                        var thr = monkey.ThrowThing(worryOperation);
                        if (thr == Throw.Empty) break;

                        var targetMonkey = monkeys[thr.MonkeyIndex];
                        targetMonkey.CatchThing(thr.NewValue);
                    }
                }
            }
        }

        public long GetAnswer() => monkeys
            .OrderByDescending(e => e.Inspects)
            .Take(2)
            .Select(e => e.Inspects)
            .Aggregate(1L, (x,y) => x * y);
    }
    
    /// <summary>
    /// Monkey
    /// </summary>
    private sealed class Monkey
    {
        public int Inspects { get; private set; }

        public int Divider { get; private set; }

        private readonly Queue<long> things = new();

        private Func<long, long> monkeyOperation;

        private int trueMonkeyIndex;

        private int falseMonkeyIndex;

        public Monkey(string description)
        {
            var parts = description.Split(Environment.NewLine);
            InitThings(parts[1].Split(":", StringSplitOptions.RemoveEmptyEntries)[1]);
            InitMonkeyOperation(parts[2].Split(":", StringSplitOptions.RemoveEmptyEntries)[1]);
            InitDivider(parts[3].Split(":", StringSplitOptions.RemoveEmptyEntries)[1]);
            InitMonkeyIndexes(new[]
            {
                parts[4].Split(":", StringSplitOptions.RemoveEmptyEntries)[1],
                parts[5].Split(":", StringSplitOptions.RemoveEmptyEntries)[1],
            });
        }

        public Throw ThrowThing(Func<long, long> worryOperation)
        {
            if (things.Count == 0) return Throw.Empty;
            
            Inspects++;
            var oldValue = things.Dequeue();
            var newValue = worryOperation(monkeyOperation(oldValue));
            
            return newValue % Divider == 0
                ? new Throw(newValue, trueMonkeyIndex)
                : new Throw(newValue, falseMonkeyIndex);
        }

        public void CatchThing(long thing) => things.Enqueue(thing);
        
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
        
        private void InitMonkeyOperation(string input)
        {
            var parts = input.Split('=', StringSplitOptions.RemoveEmptyEntries);
            var calcOperation = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];
            var argument = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last();

            if (int.TryParse(argument, out var arg))
            {
                monkeyOperation = calcOperation switch
                {
                    "+" => old => old + arg,
                    "*" => old => old * arg,
                    _ => throw new InvalidOperationException()
                };
            }
            else if (argument == "old")
            {
                monkeyOperation = calcOperation switch
                {
                    "+" => old => old + old,
                    "*" => old => old * old,
                    _ => throw new InvalidOperationException()
                };
            }
        }

        private void InitDivider(string input) =>
            Divider = int.Parse(input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        
        private void InitMonkeyIndexes(IReadOnlyList<string> input)
        {
            trueMonkeyIndex = int.Parse(input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
            falseMonkeyIndex = int.Parse(input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        }
    }

    /// <summary>
    /// Represents the throw of thing
    /// </summary>
    /// <param name="NewValue">Thing new value</param>
    /// <param name="MonkeyIndex">Target monkey index</param>
    private sealed record Throw(long NewValue, int MonkeyIndex)
    {
        public static Throw Empty => default;
    }
}
