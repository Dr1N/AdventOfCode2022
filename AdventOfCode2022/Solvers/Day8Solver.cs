using System.Text;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day8Solver : ISolver
{
    private readonly IEnumerable<string> data;
    
    private readonly int[,] forest;
    
    private int ForestWidth => data.First().Length;

    private int ForestHeight => data.Count();
    
    public Day8Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
        this.forest = new int[ForestWidth, ForestHeight];
        InitializeForest();
    }

    public string PartOne()
    {
        throw new NotImplementedException();
    }

    public string PartTwo()
    {
        throw new NotImplementedException();
    }

    private void InitializeForest()
    {
        var row = 0;
        var col = 0;
        foreach (var line in data)
        {
            foreach (var symbol in line)
            {
                forest[row, col] = symbol - '0';
                col++;
            }
            col = 0;
            row++;
        }
    }

    private static string CreateMapFromArray(int[,] data)
    {
        var sb = new StringBuilder();
        for (var row = 0; row < data.GetLength(0); row++)
        {
            for (var col = 0; col < data.GetLength(1); col++)
            {
                sb.Append(data[row, col]);
            }
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}
