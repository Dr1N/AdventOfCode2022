using System.Diagnostics;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day12Solver : ISolver
{
    private const int Start = -1;
    private const int End = -2;
    
    private Vertex[,] map;

    public Day12Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        InitializeMap(data.ToList());
    }

    public string PartOne()
    {
        PrintMap(map);
        return string.Empty;
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private void InitializeMap(IReadOnlyList<string> data)
    {
        var width = data[0].Length;
        var height = data.Count;
        map = new Vertex[height, width];
        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                if (data[row][col] != 'S' && data[row][col] != 'E')
                {
                    map[row, col] = new Vertex(row, col, data[row][col] - 'a');
                }
                else
                {
                    map[row, col] = new Vertex(row, col, data[row][col] == 'S' ? Start : End);
                }
            }
        }
    }

    private static void PrintMap(Vertex[,] map)
    {
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                switch (map[i,j].Value)
                {
                    case Start:
                        Debug.Write('S');
                        break;
                    case End:
                        Debug.Write('E');
                        break;
                    default:
                        Debug.Write((char)(map[i,j].Value + 'a'));
                        break;
                }
            }
            Debug.WriteLine(string.Empty);
        }
    }
    
    private record Vertex(int X, int Y, int Value);
}
