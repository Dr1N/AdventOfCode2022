using System.Text;
using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day4Solver : ISolver
{
    private const char PairSeparator = ',';
    private const char DiapasonSeparator = '-';

    private readonly IEnumerable<string> data;

    public Day4Solver(IEnumerable<string> data)
        => this.data = data ?? throw new ArgumentNullException(nameof(data));

    public string PartOne()
        => Calculate((d1, d2) => d1.Include(d2)).ToString();
    
    public string PartTwo()
        => Calculate((d1, d2) => d1.Intersection(d2)).ToString();

    private int Calculate(Func<Diapason, Diapason, bool> func)
    {
        var result = 0;
        foreach (var pairs in data)
        {
            var (diapason1, diapason2) = Diapason.MakeDiapasonPair(pairs);
            if (func(diapason1, diapason2))
            {
                result++;
            }
        }

        return result;
    }
    
    private readonly struct Diapason
    {
        private int Start { get; }

        private int End { get; }
        
        private int Length => End - Start;

        public static (Diapason, Diapason)MakeDiapasonPair(string str)
        {
            var pairValues = str.Split(PairSeparator);

            return (new Diapason(pairValues[0]), new Diapason(pairValues[1]));
        }

        private Diapason(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException("Invalid input string (empty)", nameof(str));
            }
            
            var values = str.Split(DiapasonSeparator);

            if (values.Length != 2)
            {
                throw new ArgumentException($"Invalid input string: {str}", nameof(str));
            }

            var startParsed = int.TryParse(values[0], out var start);
            var endParsed =int.TryParse(values[1], out var end);
            
            if (!(startParsed && endParsed))
            {
                throw new ArgumentException($"Invalid input string: {str}", nameof(str));
            }
            
            Start = start;
            End = end;
        }

        public bool Include(Diapason other)
        {
            if (this >= other)
            {
                return Start <= other.Start && End >= other.End;
            }
            
            return other.Start <= Start && other.End >= End;
        }

        public bool Intersection(Diapason other)
        {
            if (this >= other)
            {
                return other.Start >= Start && other.Start <= End
                       || other.End >= Start && other.End <= End;
            }

            return Start >= other.Start && Start <= other.End
                   || End >= other.Start && End <= other.End;
        }

        public static bool operator <=(Diapason self, Diapason other) => self.Length <= other.Length;
      
        public static bool operator >=(Diapason self, Diapason other) => !(self <= other);
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 1; i <= End; i++)
            {
                if (i < Start || i > End)
                {
                    var iLen = i.ToString().Length;
                    sb.Append(new string('.', iLen + 1));
                }
                else
                {
                    sb.Append($"{i} ");
                }
            }

            return sb.ToString();
        }
    }
}
