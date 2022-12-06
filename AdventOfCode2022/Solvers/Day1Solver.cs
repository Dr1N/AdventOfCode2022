using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day1Solver : ISolver
{
    private readonly IEnumerable<string> data;
    private IEnumerable<int> calories = null!;

    private IEnumerable<int> ElvesCalories => calories ??= CalculateElvesCalories();

    public Day1Solver(IEnumerable<string> data) =>
        this.data = data ?? throw new ArgumentNullException(nameof(data));

    public string PartOne() => ElvesCalories
        .Max()
        .ToString();

    public string PartTwo() => ElvesCalories
        .OrderByDescending(e => e)
        .Take(3)
        .Sum()
        .ToString();
   
    private IEnumerable<int> CalculateElvesCalories()
    {
        var elvesCaloriesList = new List<int>();
        var currentElveCalories = 0;
        foreach (var line in data)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                elvesCaloriesList.Add(currentElveCalories);
                currentElveCalories = 0;
                continue;
            }
            currentElveCalories += int.Parse(line);
        }

        return elvesCaloriesList;
    }
}
