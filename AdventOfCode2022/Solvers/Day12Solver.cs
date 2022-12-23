using AdventOfCode2022.Interfaces;

namespace AdventOfCode2022.Solvers;

public class Day12Solver : ISolver
{
    private Vertex[,] map;
    private Vertex start;
    private Vertex end;
    
    public Day12Solver(IEnumerable<string> data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        InitializeMap(data.ToList());
    }

    public string PartOne()
    {
        var result = Bfs();
        
        return result.Select(e => e.Distance).Max().ToString();
    }

    public string PartTwo()
    {
        return string.Empty;
    }

    private IEnumerable<Vertex> Bfs()
    {
        var queue = new Queue<Vertex>();
        var visited = new HashSet<Vertex>();
        
        queue.Enqueue(start);
        
        while (queue.Count > 0)
        {
            var currentVertex = queue.Dequeue();
            visited.Add(currentVertex);
            if (currentVertex == end)
            {
                return visited;
            }
            
            var neighbors = GetNeighbors(currentVertex);
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor) && !queue.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return visited;
    }

    // Get Neighbors for vertex
    private List<Vertex> GetNeighbors(Vertex vertex)
    {
        var result = new List<Vertex>(4);
        
        // Check values different, we can step if different not more 1
        bool CheckValues(Vertex current, Vertex next)
            => current.Height + 1 >= next.Height;
        
        // Left vertex
        if (vertex.X - 1 >= 0 && CheckValues(vertex,map[vertex.X - 1, vertex.Y]))
        {
            var next = map[vertex.X - 1, vertex.Y] with { Distance = vertex.Distance + 1, X = vertex.X - 1 };
            result.Add(next);
        }
        
        // Right vertex
        if (vertex.X + 1 < map.GetLength(0) && CheckValues(vertex,map[vertex.X + 1, vertex.Y]))
        {
            var next = map[vertex.X + 1, vertex.Y] with { Distance = vertex.Distance + 1, X = vertex.X + 1 };
            result.Add(next);
        }
        
        // Up vertex
        if (vertex.Y - 1 >= 0 && CheckValues(vertex,map[vertex.X, vertex.Y - 1]))
        {
            var next = map[vertex.X, vertex.Y - 1] with { Distance = vertex.Distance + 1, Y = vertex.Y - 1 };
            result.Add(next);
        }
        
        // Down vertex
        if (vertex.Y + 1 < map.GetLength(1) && CheckValues(vertex,map[vertex.X, vertex.Y + 1]))
        {
            var next = map[vertex.X, vertex.Y + 1] with { Distance = vertex.Distance + 1, Y = vertex.Y + 1};
            result.Add(next);
        }

        return result;
    }
    
    // Parse map
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
                    map[row, col] = new Vertex(row, col, 0, data[row][col]);
                }
                else if (data[row][col] == 'S')
                {
                    map[row, col] = new Vertex(row, col, 0, 'a');
                    start = map[row, col];
                }
                else if (data[row][col] == 'E')     
                {
                    map[row, col] = new Vertex(row, col, 0, 'z');
                    end = map[row, col];
                }
            }
        }
    }

    private record Vertex(int X, int Y, int Distance, char Height)
    {
        public virtual bool Equals(Vertex other)
        {
            if (other == null) return false;

            return other.X == X && other.Y == Y;
        }

        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}
