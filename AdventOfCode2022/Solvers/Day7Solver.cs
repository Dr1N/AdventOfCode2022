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
    private readonly Stack<string> currentPathDictionaries = new();

    public Day7Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        var sizes = CalculateDirectoriesSizes(data);
        var sorted = CalculateSubfolders(sizes);
        
        Console.WriteLine(GetDirectorySizes(sorted));
        
        return string.Empty;
    }

    public string PartTwo()
    {
        throw new NotImplementedException();
    }

    private Dictionary<string, long> CalculateDirectoriesSizes(IEnumerable<string> consoleInputOutput)
    {
        var result = new Dictionary<string, long>();
        var isList = false;
        foreach (var line in consoleInputOutput)
        {
            // Change dir
            if (line.StartsWith(ChangeDirectoryCommand))
            {
                isList = false;
                ChangeDirectory(line);
                continue;
            }

            // Show dir content
            if (line == ListCommand)
            {
                isList = true;
                continue;
            }
            
            // Skip directory
            if (!isList || line.StartsWith(DirectoryListPrefix)) continue;
           
            // Calculate file sizes in current directory
            var fileSize = long.Parse(line.Split(' ')[0]);
            
            // Save file sizes for directory
            var currentDir = CurrentDirectory();
            if (result.ContainsKey(currentDir))
            {
                result[currentDir] += fileSize;
            }
            else
            {
                result.Add(currentDir, fileSize);
            }
        }

        return result;
    }

    private static Dictionary<string, long> CalculateSubfolders(Dictionary<string, long> directoriesSizes)
    {
        var sorter = directoriesSizes
            .OrderBy(e => e.Key)
            .ThenBy(e => e.Key.Length)
            .ToDictionary(e => e.Key, v => v.Value);

        return sorter;
    }
    
    private void ChangeDirectory(string line)
    {
        var commandArgument = line.Split(' ')[2];
        if (commandArgument == UpCommandArgument)
        {
            currentPathDictionaries.Pop();
        }
        else
        {
            currentPathDictionaries.Push(commandArgument);
        }
    }
    
    private string CurrentDirectory()
    {
        if (currentPathDictionaries.Count == 0)
        {
            return string.Empty;
        }

        var currentDirectories = new List<string>(currentPathDictionaries);
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
