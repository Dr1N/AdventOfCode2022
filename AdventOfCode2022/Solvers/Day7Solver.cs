using System.Text;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day7Solver : ISolver
{
    private const char DirectorySeparator = '/';
    private const string ChangeDirectoryCommand = "$ cd";
    private const string ListCommand = "$ ls";
    private const string UpCommandArgument = "..";
    private const string DirectoryListPrefix = "dir";
    
    private readonly IEnumerable<string> data;
    private readonly Stack<string> currentPathDirectories = new();

    public Day7Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        const long sizeTheshhold = 100_000;
        var directoriesSizes = CalculateDirectoriesSizes(data);
        var withSubfolders = CalculateSubfolders(directoriesSizes);
        
        var result = withSubfolders.Values
            .Where(e => e < sizeTheshhold)
            .Sum();
        
        return result.ToString();
    }

    public string PartTwo()
    {
        throw new NotImplementedException();
    }

    private Dictionary<string, long> CalculateDirectoriesSizes(IEnumerable<string> consoleInputOutput)
    {
        var result = new Dictionary<string, long>() {{"/", 0}};
        foreach (var line in consoleInputOutput)
        {
            // Change dir
            if (line.StartsWith(ChangeDirectoryCommand))
            {
                ChangeDirectory(line);
                continue;
            }

            // Show dir content - skip
            if (line == ListCommand) continue;
            
            // Add directory to dictionary
            if (line.StartsWith(DirectoryListPrefix))
            {
                var dir = line.Split(' ')[1];
                var separator = CurrentDirectory().EndsWith('/') 
                    ? string.Empty
                    : new string(DirectorySeparator, 1);
                result.Add(CurrentDirectory() + separator + dir, 0);
                continue;
            };
           
            // Calculate file sizes in current directory
            var fileSize = long.Parse(line.Split(' ')[0]);
            
            // Save file sizes for directory
            result[CurrentDirectory()] += fileSize;
        }

        return result;
    }

    private static Dictionary<string, long> CalculateSubfolders(Dictionary<string, long> directoriesSizes)
    {
        var sorted = directoriesSizes
            .OrderBy(e => e.Key)
            .ThenBy(e => e.Key.Length)
            .ToDictionary(e => e.Key, v => v.Value);
        
        foreach (var (target, _) in sorted)
        {
            foreach (var (inner, size) in sorted)
            {
                if (inner == target) continue;
                
                if (inner.StartsWith(target))
                {
                    sorted[target] += size;
                }
            }
        }
        
        return sorted;
    }
    
    private void ChangeDirectory(string line)
    {
        var commandArgument = line.Split(' ')[2];
        if (commandArgument == UpCommandArgument)
        {
            currentPathDirectories.Pop();
        }
        else
        {
            currentPathDirectories.Push(commandArgument);
        }
    }
    
    private string CurrentDirectory()
    {
        if (currentPathDirectories.Count == 0)
        {
            return string.Empty;
        }

        var currentDirectories = new List<string>(currentPathDirectories);
        currentDirectories.Reverse();
        if (currentDirectories.Count == 1)
        {
            return currentDirectories[0];
        }
        
        var path = string.Join(DirectorySeparator, currentDirectories)[1..];
                
        return path;
    }

    private static string GetDirectorySizes(Dictionary<string,long> directoriesSizes)
    {
        if (directoriesSizes.Count == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        foreach (var (dir, size) in directoriesSizes)
        {
            sb.AppendLine($"{dir} : {size}");
        }

        return sb.ToString();
    }
}
