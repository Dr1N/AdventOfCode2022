namespace AdventOfCode2022.Elves;

// A,X - Rock
// B,Y - Paper
// C,Z - Scissors

public static class RockPaperScissors
{
    private const string CaloriesPath = @"Data/Strategy.txt";

    private static readonly Dictionary<string, int> GameScores = new()
    {
        { "A X", 1 + 3 },
        { "A Y", 2 + 6 },
        { "A Z", 3 + 0 },
        
        { "B X", 1 + 0 },
        { "B Y", 2 + 3 },
        { "B Z", 3 + 6 },
        
        { "C X", 1 + 6 },
        { "C Y", 2 + 0 },
        { "C Z", 3 + 3 },
    };
    
    private static readonly Dictionary<string, string> StrategyReplacement = new()
    {
        { "A X", "A Z" },
        { "B X", "B X" },
        { "C X", "C Y" },
        
        { "A Y", "A X" },
        { "B Y", "B Y" },
        { "C Y", "C Z" },
        
        { "A Z", "A Y" },
        { "B Z", "B Z" },
        { "C Z", "C X" },
    };

    public static int Calculate() =>
        File.ReadAllLines(CaloriesPath)
            .Sum(game => GameScores[game]);
    
    public static int CalculateSecond() =>
        File.ReadAllLines(CaloriesPath)
                .Select(game => StrategyReplacement[game])
                .Select(replaced => GameScores[replaced])
                .Sum();
}