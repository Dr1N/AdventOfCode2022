using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

/*
 A,X - Rock
 B,Y - Paper
 C,Z - Scissors
*/

public class Day2Solver : ISolver
{
    private readonly IReadOnlyDictionary<string, int> gameScores = new Dictionary<string, int>()
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
    
    private readonly IReadOnlyDictionary<string, string> strategyReplacement = new Dictionary<string, string>()
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

    private readonly IEnumerable<string> data;

    public Day2Solver(IEnumerable<string> data) => 
        this.data = data ?? throw new ArgumentNullException(nameof(data));
    
    public string PartOne() => 
        data.Sum(game => gameScores[game])
            .ToString();
    
    public string PartTwo() =>
        data.Select(game => strategyReplacement[game])
            .Select(replaced => gameScores[replaced])
            .Sum()
            .ToString();
}
