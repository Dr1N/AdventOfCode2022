namespace AdventOfCode2022.Interfaces;

public interface IDataProvider
{
    /// <summary>
    /// Get data as string collection
    /// </summary>
    /// <param name="day">Task day [1,25]</param>
    /// <returns>Collection of input data</returns>
    IEnumerable<string> GetData(int day);
}
