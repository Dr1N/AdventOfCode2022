using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day3Solver : ISolver
{
    private const string ItemsCodes = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly IReadOnlyDictionary<char, int> ItemPriorities = 
        ItemsCodes.ToDictionary(e => e, e => ItemsCodes.IndexOf(e) + 1);

    private readonly IEnumerable<string> data;

    public Day3Solver(IEnumerable<string> data)
        => this.data = data ?? throw new ArgumentNullException(nameof(data));

    public string PartOne()
    {
        var result = 0;
        foreach (var rucksack in data)
        {
            var compartemtOneSet = new HashSet<char>(rucksack[..(rucksack.Length / 2)]);
            var compartemtTwoSet = new HashSet<char>(rucksack[(rucksack.Length / 2)..]);
            compartemtOneSet.IntersectWith(compartemtTwoSet);
            result += compartemtOneSet.Select(e => ItemPriorities[e]).Sum();
        }
        
        return result.ToString();
    }

    public string PartTwo()
    {
        var result = 0;
        foreach (var group in data.Chunk(3))
        {
            var set1 = new HashSet<char>(group[0]);
            var set2 = new HashSet<char>(group[1]);
            var set3 = new HashSet<char>(group[2]);
            
            set1.IntersectWith(set2);
            set1.IntersectWith(set3);

            result += ItemPriorities[set1.First()];
        }

        return result.ToString();
    }
}
