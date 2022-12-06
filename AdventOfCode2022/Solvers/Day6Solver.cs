using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day6Solver : ISolver
{
    private const int HeaderLength = 4;
    private const int MessageLength = 14;
    
    private readonly string message;

    public Day6Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        message = data.First();
    }

    public string PartOne() => FindPosition(HeaderLength).ToString();
    
    public string PartTwo() => FindPosition(MessageLength).ToString();

    private int FindPosition(int packetLength)
    {
        var batchCharSet = new HashSet<char>();
        for (var i = 0; i < message.Length - packetLength; i++)
        {
            // Get batch
            var currentBatch = message.AsSpan(i, packetLength);
            
            // Set hash set
            batchCharSet.Clear();
            foreach (var c in currentBatch)
            {
                batchCharSet.Add(c);
            }
            
            // Check set and batch
            var isUniqueSet = currentBatch.Length == batchCharSet.Count;
            if (isUniqueSet)
            {
                return i + packetLength;
            }
        }

        return -1;
    }
}
