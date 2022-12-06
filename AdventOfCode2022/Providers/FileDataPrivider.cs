using AdventOfCode2022.Helpers;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Providers;

/// <inheritdoc />
public class FileDataProvider : IDataProvider
{
    private readonly string basePath;
    
    public FileDataProvider(string basePath)
    {
        if (string.IsNullOrWhiteSpace(basePath))
        {
            throw new ArgumentException($"{nameof(basePath)} can't be empty");
        }

        if (!Directory.Exists(basePath))
        {
            throw new ArgumentException($"{basePath} not exists");
        }
        
        this.basePath = basePath;
    }
    
    public IEnumerable<string> GetData(int day)
    {
        DayHelper.CheckDay(day);

        var fileName = $"day_{day}.txt";
        var filePath = Path.Combine(basePath, fileName);
        if (!File.Exists(filePath))
        {
            throw new ArgumentException($"File {fileName} not exists in {basePath} directory");
        }

        return File.ReadAllLines(filePath);
    }
}
