using AdventOfCode2022.Helpers;
using AdventOfCode2022.Interfaces;
using AdventOfCode2022.Providers;
using AdventOfCode2022.Solvers;

const string dataPath = "Data";

try
{
    IDataProvider dataProvider = new FileDataProvider(dataPath);
    ISolverFactory solverFactory = new SolverFactory(dataProvider);
    
    while (true)
    {
        try
        {
            Console.Write("Enter day (or 'exit'): ");
            var input = Console.ReadLine();
            if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (!int.TryParse(input, out var day) || !DayHelper.IsValid(day))
            {
                Console.WriteLine("Invalid Day. Day should be in [1, 25] diapason");
                continue;
            }
            var solver = solverFactory.CreateSolver(day);
            
            Console.WriteLine();
            
            Console.WriteLine($"Day: {day}");
            var one = solver.PartOne();
            Console.WriteLine($"Part one: {one}");
            var two = solver.PartTwo();
            Console.WriteLine($"Part two: {two}");
            
            Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Solving error: {e.Message}");
            Console.ResetColor();
        }
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Critical error: {ex.Message}");
    Console.ResetColor();
}

Console.WriteLine("Good buy");
