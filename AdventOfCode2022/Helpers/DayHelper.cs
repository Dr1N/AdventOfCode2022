namespace AdventOfCode2022.Helpers;

public static class DayHelper
{
    private const int MinDay = 1;
    private const int MaxDay = 25;

    public static void CheckDay(int day)
    {
        if (!IsValid(day))
        {
            throw new ArgumentException(
                $"Invalid day. {nameof(day)} must be greater than 0 and less or equal than {MaxDay}");
        }
    }

    public static bool IsValid(int day) => day is not (< MinDay or > MaxDay);
}
