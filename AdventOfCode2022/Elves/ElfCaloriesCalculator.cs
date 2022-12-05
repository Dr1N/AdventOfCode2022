namespace AdventOfCode2022.Elves;

public static class ElfCaloriesCalculator
{
    private const string CaloriesPath = @"Data/Calories.txt";

    public static List<int> Calculate()
    {
        try
        {
            var data = File.ReadAllLines(CaloriesPath);
            var calories = new List<int>();
            var currentCalories = 0;
            foreach (var line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    calories.Add(currentCalories);
                    currentCalories = 0;
                    continue;
                }
                currentCalories += int.Parse(line);
            }

            return calories;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}