using System.Text.Json.Nodes;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

/// Thank you bro (https://github.com/encse), you are genius :)
/// https://github.com/encse/adventofcode/blob/master/2022/Day13/Solution.cs
public class Day13Solver : ISolver
{
    private readonly IEnumerable<string> data;
    private string DataString => string.Join(Environment.NewLine, data);
    
    public Day13Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        this.data = data;
    }

    public string PartOne()
    {
        var result = GetPackets(DataString)
            .Chunk(2)
            .Select((pair, index)
                => Compare(pair[0], pair[1]) < 0 ? index + 1 : 0)
            .Sum();
        
        return result.ToString();
    }

    public string PartTwo()
    {
        var divider = GetPackets($"[[2]]{Environment.NewLine}[[6]]").ToList();
        var packets = GetPackets(DataString).Concat(divider).ToList();
        packets.Sort(Compare);
        var result = (packets.IndexOf(divider[0]) + 1) * (packets.IndexOf(divider[1]) + 1);
        
        return result.ToString();
    }

    private static IEnumerable<JsonNode> GetPackets(string input) => input
        .Split(Environment.NewLine)
        .Where(e => !string.IsNullOrEmpty(e))
        .Select(e => JsonNode.Parse(e));

    private static int Compare(JsonNode left, JsonNode right)
    {
        if (left is JsonValue && right is JsonValue) {
            return (int)left - (int)right;
        }
        
        var leftArray = left as JsonArray ?? new JsonArray((int)left);
        var rightArray = right as JsonArray ?? new JsonArray((int)right);
        
        return leftArray.Zip(rightArray)
            .Select(pair => Compare(pair.First, pair.Second))
            .FirstOrDefault(e => e != 0, leftArray.Count - rightArray.Count);
    }
}
