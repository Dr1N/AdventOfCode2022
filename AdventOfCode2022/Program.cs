using AdventOfCode2022.Elves;

try
{
    var result = SupplyCalculator.Calculate();
    
    Console.WriteLine($"Supply: {result}");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ResetColor();
}
