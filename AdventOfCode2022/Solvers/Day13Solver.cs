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
            Debug.WriteLine($"Message {++cnt}:");
            Debug.WriteLine(message.GetAsStringLine());
            Debug.WriteLine($"IsValid: {message.IsValid()}");
            Debug.WriteLine(new string('=', 50));
        }
        
        return string.Empty;
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private record Message(Package Left, Package Right)
    {
        public bool IsValid()
        {
            // Check length
            if (Left.Tokens().Count() > Right.Tokens().Count())
            {
                return false;
            }
            
            // Left is short
            var leftTokens = Left.Tokens().ToList();
            var rightTokens = Right.Tokens().ToList();
            var cnt = 0;
            var result = true;
            while (true)
            {
                if (cnt == leftTokens.Count) break;
                
                var leftToken = leftTokens[cnt];
                var rightToken = rightTokens[cnt];
                if (leftToken is IntegerToken ilt && rightToken is IntegerToken irt)
                {
                    result = result && ilt.Value <= irt.Value;
                }
                else if (leftToken is ListToken llt && rightToken is ListToken rlt)
                {
                    
                }
                else if (leftToken is IntegerToken it1 && rightToken is ListToken rlt1)
                {
                    
                }
                else if (leftToken is ListToken llt1 && rightToken is IntegerToken rt1)
                {
                    
                }
                cnt++;
            }
            
            return result;
        }

        public string GetAsStringLine()
        {
            var leftTokens = Left.Tokens().ToList();
            var rightTokens = Right.Tokens().ToList();
            return string.Join(" | ", leftTokens) + " vs " + string.Join(" | ", rightTokens);
        }
        
        public override string ToString() => string.Join(Environment.NewLine, Left, Right);
    }
    
    private record Package(string Value)
    {
        public IEnumerable<Token> Tokens() => TokenParser.GetTokens(Value);

        public override string ToString() => Value;
    }

    private static class TokenParser
    {
        private const char Separator = ',';
        public const char ListStartChar = '[';
        public const char ListEndChar = ']';
        
        public static IEnumerable<Token> GetTokens(string value)
        {
            // Remove end and start braces
            var trimed = value[1..^1];
            var packagePointer = 0;
    
            // Empty List
            if (trimed.Length == 0) yield return new ListToken(value);
            
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
                    yield return new IntegerToken(token);
                    
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
                    yield return new ListToken(token);
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

    private class IntegerToken : Token, IComparable<IntegerToken>
    {
        public int Value { get; }
        
        public IntegerToken(string value) : base(value)
        {
            if (!int.TryParse(value, out var intVal))
            {
                throw new ArgumentException(null, nameof(value));
            }

            Value = intVal;
        }

        public int CompareTo(IntegerToken other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Value.CompareTo(other.Value);
        }
    }
    
    private class ListToken : Token, IComparable<ListToken>
    {
        public ListToken(string value) : base(value)
        {
            if (!value.StartsWith(TokenParser.ListStartChar)
                || !value.EndsWith(TokenParser.ListEndChar))
            {
                throw new ArgumentException(null, nameof(value));
            }
        }

        public int CompareTo(ListToken other)
        {
            throw new NotImplementedException();
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
