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

    public Day7Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        const long sizeTheshhold = 100_000;

        var directoriesSizes = CalculateDirectoriesSizes(data);
        
        var result = directoriesSizes.Values
            .Where(e => e < sizeTheshhold)
            .Sum();
        
        return result.ToString();
    }

    public string PartTwo()
    {
        const int totalSpase = 70_000_000;
        const int needSpase = 30_000_000;
        
        var directoriesSizes = CalculateDirectoriesSizes(data);
        var totalFileSizes = directoriesSizes["/"];
        var currentFreeSpace = totalSpase - totalFileSizes;
        var neededSpace = needSpase - currentFreeSpace;

        var result = directoriesSizes.Values
            .Where(e => e >= neededSpace)
            .Min();
        
        return result.ToString();
    }

    private Dictionary<string, long> CalculateDirectoriesSizes(IEnumerable<string> consoleInputOutput)
    {
        var currentPathDirectories = new Stack<string>();
        var result = new Dictionary<string, long>() {{"/", 0}};
        foreach (var line in consoleInputOutput)
        {
            // Change current dir
            if (line.StartsWith(ChangeDirectoryCommand))
            {
                ChangeDirectory(line, currentPathDirectories);
                continue;
            }

            // Show dir content - skip
            if (line == ListCommand) continue;
            
            // Add directory to dictionary
            if (line.StartsWith(DirectoryListPrefix))
            {
                var dir = line.Split(' ')[1];
                var separator = CurrentDirectory(currentPathDirectories).EndsWith('/') 
                    ? string.Empty
                    : new string(DirectorySeparator, 1);
                result.Add($"{CurrentDirectory(currentPathDirectories)}{separator}{dir}", 0);
                continue;
            }
           
            // Calculate file sizes in current directory
            var fileSize = long.Parse(line.Split(' ')[0]);
            
            // Add file size for directory
            result[CurrentDirectory(currentPathDirectories)] += fileSize;
        }

        return CalculateSubfolders(result);
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
    
    private static void ChangeDirectory(string line, Stack<string> currentDirectories)
    {
        var commandArgument = line.Split(' ')[2];
        if (commandArgument == UpCommandArgument)
        {
            currentDirectories.Pop();
        }
        else
        {
            currentDirectories.Push(commandArgument);
        }
    }
    
    private static string CurrentDirectory(IReadOnlyCollection<string> currentDirectories)
    {
        if (currentDirectories.Count == 0)
        {
            return string.Empty;
        }

        var directoriesList = new List<string>(currentDirectories);
        directoriesList.Reverse();
        if (directoriesList.Count == 1)
        {
            return directoriesList[0];
        }
        
        var path = string.Join(DirectorySeparator, directoriesList)[1..];
                
        return path;
    }
}
