using System.Text;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day10Solver : ISolver
{
    private const int DisplayHeight = 6;
    private const int DisplayWidth = 40;
    
    private readonly IReadOnlyList<ICommand> program;
    
    public Day10Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        program = data
            .Select(CommandFactory.Create)
            .ToList();
    }

    public string PartOne()
    {
        var processor = new LoggingProcessor();
        processor.Execute(program);
        
        var result = 0;
        foreach (var cnt in new[] { 20, 60, 100, 140, 180, 220 })
        {
            var register = processor.Log[cnt - 2]; // wtf
            var strength = cnt * register;
            result += strength;
        }
        
        return result.ToString();
    }

    public string PartTwo()
    {
        var processor = new CrtProcessor();
        processor.Execute(program);
        
        var output = processor.Output;
        var sb = new StringBuilder(Environment.NewLine);
        for (var line = 0; line < DisplayHeight; line++) {
            sb.AppendLine(output.Substring(line * DisplayWidth, DisplayWidth - 1));
        }
        
        return sb.ToString();
    }

    private static class CommandFactory
    {
        public static ICommand Create(string command)
        {
            if (command == "noop") return new NopeCommand();
            
            if (command.StartsWith("addx"))
                return new AddxCommand(int.Parse(command.Split(' ')[1]));
            
            throw new InvalidOperationException();
        }
    }
    
    /// <summary>
    /// Log register state
    /// </summary>
    private class LoggingProcessor : IProcessor
    {
        private int RegisterX { get; set; } = 1;

        public List<int> Log { get; } = new();

        public void Execute(IEnumerable<ICommand> program)
        {
            foreach (var command in program)
            {
                switch (command)
                {
                    case NopeCommand:
                        Log.Add(RegisterX); // Tact 1 (Save Register)
                        break;
                    case AddxCommand addx:
                        Log.Add(RegisterX); // Tact 1 (Save Register - begin changing Register)
                        RegisterX += addx.Argument;
                        Log.Add(RegisterX); // Tact 2 (new Register value)
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Generate CRT output line
    /// </summary>
    private class CrtProcessor : IProcessor
    {
        public string Output => outputBuilder.ToString();

        private int RegisterX { get; set; } = 1;

        private int Cycles { get; set; }

        private readonly StringBuilder outputBuilder = new();
        
        public void Execute(IEnumerable<ICommand> program)
        {
            foreach (var command in program)
            {
                switch (command)
                {
                    case NopeCommand:
                        Cycles++;
                        AddOutputValue();
                        break;
                    case AddxCommand addx:
                    {
                        Cycles++;
                        AddOutputValue();
                        Cycles++;
                        AddOutputValue();
                        RegisterX += addx.Argument;
                    }
                    break;
                }
            }
        }

        private void AddOutputValue()
        {
            var delta = Cycles % 40 - RegisterX;
            outputBuilder.Append(delta is >= 0 and <= 2 ? "█" : ".");
        }
    }
    
    private readonly struct AddxCommand : ICommand
    {
        public int Argument { get; }
        
        public AddxCommand(int argument) => Argument = argument;
        
        public override string ToString() => $"addx {Argument}";
    }

    private readonly struct NopeCommand : ICommand
    {
        public NopeCommand()
        {
        }
        
        public override string ToString() => "nope";
    }

    private interface ICommand
    {
    }
    
    private interface IProcessor
    {
        void Execute(IEnumerable<ICommand> program);
    }
}
