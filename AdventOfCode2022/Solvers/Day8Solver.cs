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
        forest = new int[ForestWidth, ForestHeight];
        InitializeForest();
    }

    public string PartOne()
    {
        var visibilityMap = CreateVisibilityMap(forest);
        var result = visibilityMap.Cast<int>().Count(e => e == 1); // 1 - is visible
        
        return result.ToString();
    }

    public string PartTwo()
    {
        var scenicMap = CreateScenicMap(forest);
        var result = scenicMap.Cast<int>().Max();

        return result.ToString();
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
    
    private static int[,] CreateVisibilityMap(int[,] data)
    {
        var result = new int[data.GetLength(0), data.GetLength(1)];
        InitializeArray(result, 1);
        for (var row = 1; row < data.GetLength(0) - 1; row++)
        {
            for (var col = 1; col < data.GetLength(1) - 1; col++)
            {
                result[row, col] = GetTreeVisibility(row, col, data);
            }
        }
        
        return result;
    }

    private static int GetTreeVisibility(int row, int col, int[,] data)
    {
        var treeRow = GetRowFromMatrix(data, row);
        var treeCol = GetColumnFromMatrix(data, col);
        
        // Right Side -> Element
       var fromRightSideVisible 
           = data[row,col] > GetMaxElementFromSubArray(treeRow, col + 1, treeRow.Length - 1);
        
        // Left Side -> Element
        var fromLeftSideVisible 
            = data[row,col] > GetMaxElementFromSubArray(treeRow, 0, col - 1);
        
        // Bottom Side -> Element
        var fromBottomSideVisible 
            = data[row,col] > GetMaxElementFromSubArray(treeCol, row + 1, treeCol.Length - 1);
        
        // Top Side -> Element
        var fromTopSideVisible
            = data[row,col] > GetMaxElementFromSubArray(treeCol, 0, row - 1);
        
        var visible = fromRightSideVisible
            || fromLeftSideVisible
            || fromLeftSideVisible
            || fromBottomSideVisible
            || fromTopSideVisible;

        return visible ? 1 : 0;
    }

    private static int GetMaxElementFromSubArray(IReadOnlyList<int> array, int startIndex, int endIndex)
        => Enumerable.Range(startIndex, endIndex - startIndex + 1)
            .Select(index => array[index])
            .Max();
    
    private static int[,] CreateScenicMap(int[,] data)
    {
        var result = new int[data.GetLength(0), data.GetLength(1)];
        InitializeArray(result, 0);
        for (var row = 0; row < data.GetLength(0); row++)
        {
            for (var col = 0; col < data.GetLength(1); col++)
            {
                result[row, col] = GetScenicScore(row, col, data);
            }
        }
        
        return result;
    }
    
    private static int GetScenicScore(int row, int col, int[,] data)
    {
        var treeRow = GetRowFromMatrix(data, row);
        var treeCol = GetColumnFromMatrix(data, col);
        
        // Left (Up) <- Element -> Right (Bottom)
        var horizontalScores = CalculateScenicInArray(treeRow, col);
        var verticalScores = CalculateScenicInArray(treeCol, row);

        return horizontalScores * verticalScores;
    }

    private static int CalculateScenicInArray(IReadOnlyList<int> array, int elementIndex)
    {
        bool CalculateScores(int nextIndex, int current, ref int scores)
        {
            var nextSiblingTree = array[nextIndex];
            if (current >= nextSiblingTree)
            {
                scores++;
            }

            return current <= nextSiblingTree;
        }

        var currentElement = array[elementIndex];
        var isLeftElement = elementIndex == 0;
        var isRightElement = elementIndex == array.Count - 1;
        
        // Element -> Right (Down)
        var rightScores = 0;
        if (!isRightElement)
        {
            for (var i = elementIndex + 1; i < array.Count; i++)
            {
                if (CalculateScores(i, currentElement, ref rightScores)) break;
            }
        }
        
        // Element -> Left (Up)
        var leftScores = 0;
        if (!isLeftElement)
        {
            for (var i = elementIndex - 1; i >= 0; i--)
            {
                if (CalculateScores(i, currentElement, ref leftScores)) break;
            }
        }
        
        return rightScores * leftScores;
    }
    
    private static void InitializeArray(int[,] array, int value)
    {
        for (var row = 0; row < array.GetLength(0); row++)
        {
            for (var col = 0; col < array.GetLength(1); col++)
            {
                array[row, col] = value;
            }
        }
    }

    private static int[] GetRowFromMatrix(int[,] data, int rowNumber)
    {
        return Enumerable.Range(0, data.GetLength(1))
            .Select(x => data[rowNumber, x])
            .ToArray();
    }
    
    private static int[] GetColumnFromMatrix(int[,] data, int columnNumber)
    {
        return Enumerable.Range(0, data.GetLength(0))
            .Select(x => data[x, columnNumber])
            .ToArray();
    }
}
