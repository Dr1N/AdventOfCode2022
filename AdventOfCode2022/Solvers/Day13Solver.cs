using System.Diagnostics;
using System.Text;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day13Solver : ISolver
{
    private readonly List<Message> messages = new();

    public Day13Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        Initialize(data);
    }

    public string PartOne()
    {
        var cnt = 0;
        foreach (var message in messages)
        {
            cnt++;
            var package1Tokens = message.Package1.Tokens().ToList();
            var package2Tokens = message.Package2.Tokens().ToList();
            Debug.WriteLine($"Message {cnt}:");
            Debug.WriteLine(string.Join(" | ", package1Tokens) + " vs " + string.Join(" | ", package2Tokens));
            Debug.WriteLine(new string('=', 50));
        }
        
        return string.Empty;
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private record Message(Package Package1, Package Package2)
    {
        public bool IsValid()
        {
            return false;
        }

        public override string ToString() => string.Join(Environment.NewLine, Package1, Package2);
    }
    
    private record Package(string Value)
    {
        public IEnumerable<string> Tokens() => TokenParser.GetTokens(Value);

        public override string ToString() => Value;
    }

    private static class TokenParser
    {
        private const char Separator = ',';
        public const char ListStartChar = '[';
        public const char ListEndChar = ']';
        
        public static IEnumerable<string> GetTokens(string value)
        {
            // Remove end and start braces
            var trimed = value[1..^1];
            var packagePointer = 0;
    
            // Empty List
            if (trimed.Length == 0) yield return value;
            
            while (true)
            {
                // End of package
                if (packagePointer >= trimed.Length) yield break;
                
                var currentPointerValue = trimed[packagePointer];
                
                // Skip separator
                if (currentPointerValue == Separator)
                {
                    packagePointer++;
                    continue;
                }
                
                // Integer value
                if (currentPointerValue != ListStartChar)
                {
                    // Find separator
                    var separatorIndex = trimed.IndexOf(Separator, packagePointer);
                    var substringLength = separatorIndex != -1
                        ? separatorIndex - packagePointer
                        : trimed.Length - packagePointer;  // Last token
                    
                    // From pointer to comma
                    var token = trimed.Substring(packagePointer, substringLength);
                    yield return token;
                    
                    // End of package
                    if (separatorIndex == -1) yield break;
                    
                    packagePointer = separatorIndex + 1;
                }
                // List value
                else if (currentPointerValue == ListStartChar)
                {
                    var stack = new Stack<char>();
                    stack.Push(currentPointerValue);
                    var startList = packagePointer;
                    while (true)
                    {
                        packagePointer++;
                        currentPointerValue = trimed[packagePointer];
                        switch (currentPointerValue)
                        {
                            case ListStartChar:
                                stack.Push(currentPointerValue);
                                break;
                            case ListEndChar:
                                stack.Pop();
                                break;
                        }
                        
                        // End of list
                        if (stack.Count == 0) break;
                    }

                    packagePointer++;
                    var listLenght = packagePointer - startList;
                    var token = trimed.Substring(startList, listLenght);
                    yield return token;
                }
            }
        }
    }
    
    private class Token
    {
        private readonly string value;

        protected Token(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            this.value = value;
        }

        public override string ToString() => value;
    }

    private class IntegerToken : Token
    {
        public int Value { get; }
        
        protected IntegerToken(string value) : base(value)
        {
            if (!int.TryParse(value, out var intVal))
            {
                throw new ArgumentException(null, nameof(value));
            }

            Value = intVal;
        }
    }
    
    private class ListToken : Token
    {
        public ListToken(string value) : base(value)
        {
            if (!value.StartsWith(TokenParser.ListStartChar)
                || !value.EndsWith(TokenParser.ListEndChar))
            {
                throw new ArgumentException(null, nameof(value));
            }
        }
    }
    
    #region Helpers
    
    private void Initialize(IEnumerable<string> data)
    {
        var lines = new List<string>(2);
        foreach (var line in data)
        {
            lines.Add(line);
            if (!string.IsNullOrWhiteSpace(line)) continue;
            messages.Add(new Message(new Package(lines[0]), new Package(lines[1])));
            lines.Clear();
        }

        if (lines.Count == 2)
        {
            messages.Add(new Message(new Package(lines[0]), new Package(lines[1])));
        }
    }
    
    private string GetSignalsString()
    {
        var sb = new StringBuilder();
        foreach (var (package1, package2) in messages)
        {
            sb.AppendLine(package1.ToString());
            sb.AppendLine(package2.ToString());
            sb.AppendLine();
        }

        return sb.ToString();
    }
    
    #endregion
}
