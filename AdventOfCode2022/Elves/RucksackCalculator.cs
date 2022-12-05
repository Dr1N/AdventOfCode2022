namespace AdventOfCode2022.Elves;

public static class RucksackCalculator
{
    private const string Rucksack = @"Data/Rucksack.txt";
    private const string ItemsCodes = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly Dictionary<char, int> Priors = 
        ItemsCodes.ToDictionary(e => e, e => ItemsCodes.IndexOf(e) + 1);

    public static int Calculate()
    {
        var data = File.ReadAllLines(Rucksack);
        var result = 0;
        foreach (var rucksack in data)
        {
            var compartemtOneSet = new HashSet<char>(rucksack[..(rucksack.Length / 2)]);
            var compartemtTwoSet = new HashSet<char>(rucksack[(rucksack.Length / 2)..]);
            compartemtOneSet.IntersectWith(compartemtTwoSet);
            result += compartemtOneSet.Select(e => Priors[e]).Sum();
        }
        
        return result;
    }

    public static int CalculateGroups()
    {
        var data = File.ReadAllLines(Rucksack);
        var result = 0;
        foreach (var group in data.Chunk(3))
        {
            var set1 = new HashSet<char>(group[0]);
            var set2 = new HashSet<char>(group[1]);
            var set3 = new HashSet<char>(group[2]);
            
            set1.IntersectWith(set2);
            set1.IntersectWith(set3);

            result += Priors[set1.First()];
        }

        return result;
    }
}