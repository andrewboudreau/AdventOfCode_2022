using System.Collections;
using System.Collections.Immutable;

public class SimulationGrid<T> : IEnumerable<Node<T>>
{
    private readonly List<Node<T>> entities;
    //private IImmutableDictionary<(int X, int Y), Node<T>> locations;
    //private Dictionary<(int X, int Y), Node<T>> buffer;

    private readonly int width;
    private readonly int height;
    private ulong step;

    public SimulationGrid(IEnumerable<(int X, int Y, T Entity)> nodes, int width = 0, int height = 0)
    {
        this.width = width;
        this.height = height;

        entities = new();
        foreach (var entity in nodes)
        {
            entities.Add(new Node<T>(entity));
        }

        //locations = entities.ToImmutableDictionary(k => (k.X, k.Y));
        //buffer = new(locations.Count);

        if (width == 0 || height == 0)
        {
            this.width = entities.Max(k => k.X);
            this.height = entities.Max(k => k.Y);
        }
    }

    public SimulationGrid(IEnumerable<string> rows, Func<string, IEnumerable<T>> factory)
       : this(rows.Select(factory))
    {
    }

    public SimulationGrid(IEnumerable<IEnumerable<T>> map)
    {
        entities = new();

        int x = 0;
        int y = 0;

        foreach (var row in map)
        {
            foreach (var value in row)
            {
                x++;
                var node = new Node<T>(x, y, value);
                entities.Add(node);
            }

            if (width == 0)
            {
                width = x;
            }
            x = 0;
            y++;
        }

        //locations = entities.ToImmutableDictionary(k => (k.X, k.Y));
    }

    public int Width => width;

    public int Height => height;

    public IEnumerable<Node<T>> Nodes() => entities;

    public IEnumerable<Node<T>> Neighbors(Node<T> position, bool withDiagonals = true)
    {
        if (withDiagonals && this[position.X + 1, position.Y - 1] is Node<T> downRight)
        {
            yield return downRight;
        }

        if (this[position.X + 1, position.Y] is Node<T> right)
        {
            yield return right;
        }

        if (this[position.X, position.Y - 1] is Node<T> down)
        {
            yield return down;
        }

        if (withDiagonals && this[position.X - 1, position.Y - 1] is Node<T> downLeft)
        {
            yield return downLeft;
        }

        if (this[position.X - 1, position.Y] is Node<T> left)
        {
            yield return left;
        }

        if (withDiagonals && this[position.X - 1, position.Y + 1] is Node<T> upLeft)
        {
            yield return upLeft;
        }

        if (this[position.X, position.Y + 1] is Node<T> up)
        {
            yield return up; ;
        }

        if (withDiagonals && this[position.X + 1, position.Y + 1] is Node<T> upRight)
        {
            yield return upRight;
        }
    }

    public (int MinX, int MinY, int MaxX, int MaxY) BoundingBox
    {
        get
        {
            int minX = int.MaxValue, minY = int.MaxValue,
                maxX = int.MinValue, maxY = int.MinValue;

            // this should just be the first and last sorted nodes.
            foreach (var node in Nodes())
            {
                if (node.X < minX) minX = node.X;
                if (node.Y < minY) minY = node.Y;
                if (node.X > maxX) maxX = node.X;
                if (node.Y > maxY) maxY = node.Y;
            }

            return (minX, minY, maxX, maxY);
        }
    }

    public Node<T>? this[int x, int y]
    {
        get
        {
            if (x < 0) return default;
            if (x > width) return default;
            if (y < 0) return default;
            if (y > height) return default;

            //if (locations.TryGetValue((x, y), out var node))
            //{
            //    return node;
            //}

            return entities.FirstOrDefault(node => node.X == x && node.Y == y);
        }
    }

    public Node<T> GetOrCreate((int X, int Y) position, Func<(int X, int Y), Node<T>> factory)
    {
        var node = this[position.X, position.Y];
        if (node is not null)
            return node;

        node = factory(position);
        entities.Add(node);

        //locations = entities.ToImmutableDictionary(k => (k.X, k.Y));
        return node;
    }
    public Node<T> Create(int x, int y, T value)
    {
        var entity = new Node<T>(x, y, value);
        entities.Add(entity);

        //locations = entities.ToImmutableDictionary(k => (k.X, k.Y));
        return entity;
    }

    public Node<T> GetOrCreate(int x, int y, T value)
    {
        //if (locations.TryGetValue((x, y), out var node))
        //{
        //    return node;
        //}
        var node = this[x, y];
        if (node is not null)
        {
            return node;
        }

        var entity = new Node<T>(x, y, value);
        entities.Add(entity);

        //locations = entities.ToImmutableDictionary(k => (k.X, k.Y));
        return entity;
    }

    public SimulationGrid<T> Each(Action<Node<T>> action)
    {
        foreach (var node in Nodes())
        {
            action(node);
        }

        return this;
    }

    public SimulationGrid<T> WhileTrue(Func<SimulationGrid<T>, bool> operation)
    {
        while (operation(this)) ;
        return this;
    }


    public void FillDistances(Node<T> from)
    {
        this.ResetVisited();
        this.ResetDistances();

        from.SetDistance(0);
        var nodes = new Queue<Node<T>>();
        nodes.Enqueue(from);

        while (nodes.TryDequeue(out var current))
        {
            foreach (var node in Nodes().Where(x => x.Neighbors.Contains(current)).Except(nodes))
            {
                if (current.Distance + 1 < node.Distance)
                {
                    nodes.Enqueue(node);
                    node.SetDistance(current.Distance + 1);
                }
            }
        }
    }

    public IEnumerable<IEnumerable<Node<T>?>> Rows(bool excludeEmpty = false)
    {
        for (int y = BoundingBox.MinY; y < BoundingBox.MaxY; y++)
        {
            yield return Enumerable.Range(BoundingBox.MinX, BoundingBox.MaxX - BoundingBox.MinX)
                .Where(x => this[x, y] != null || !excludeEmpty)
                .Select(x => this[x, y]);
        }
    }

    public IEnumerable<IEnumerable<Node<T>?>> Viewport(bool excludeEmpty = false)
    {
        for (int y = BoundingBox.MinY; y < BoundingBox.MaxY + 4; y++)
        {
            yield return Enumerable.Range(BoundingBox.MinX - 4, BoundingBox.MaxX - BoundingBox.MinX + 4)
                .Where(x => this[x, y] != null || !excludeEmpty)
                .Select(x => this[x, y]);
        }
    }

    public void Step(Action<SimulationGrid<T>, Node<T>, ulong> update)
    {
        step++;
        //buffer.Clear();

        var keys = entities.OrderByDescending(k => k.Y).ThenBy(k => k.X).ToList();
        foreach (var node in keys)
        {
            if (node is null)
                throw new NullReferenceException($"node is null at key {node.X},{node.Y}");

            update(this, node, step);
        }
    }

    public bool TryMove(Node<T> node, out (int X, int Y) destination)
    {
        if (this[node.X, node.Y + 1] is null)
        {
            destination = (node.X, node.Y + 1);
            return true;
        }
        else if (this[node.X - 1, node.Y + 1] is null)
        {
            destination = (node.X - 1, node.Y + 1);
            return true;
        }
        else if (this[node.X + 1, node.Y + 1] is null)
        {
            destination = (node.X + 1, node.Y + 1);
            return true;
        }

        destination = (node.X, node.Y);
        return false;
    }

    public IEnumerator<Node<T>> GetEnumerator()
        => entities.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
       => GetEnumerator();
}

public static class SimulationGridExtensions
{
    public static IEnumerable<T> UpFrom<T>(this SimulationGrid<T> grid, Node<T> node)
    {
        for (var i = node.Y - 1; i >= 0; i--)
        {
            yield return grid[node.X, i]!;
        }
    }

    public static IEnumerable<T> DownFrom<T>(this SimulationGrid<T> grid, Node<T> node)
    {
        for (var i = node.Y + 1; i < grid.Width; i++)
        {
            yield return grid[node.X, i]!;
        }
    }

    public static IEnumerable<T> LeftFrom<T>(this SimulationGrid<T> grid, Node<T> node)
    {
        for (var i = node.X - 1; i >= 0; i--)
        {
            yield return grid[i, node.Y]!;
        }
    }

    public static IEnumerable<T> RightFrom<T>(this SimulationGrid<T> grid, Node<T> node)
    {
        for (var i = node.X + 1; i < grid.Width; i++)
        {
            yield return grid[i, node.Y]!;
        }
    }

    public static void ResetVisited<T>(this SimulationGrid<T> grid)
        => grid.Each(node => node.ResetVisited());

    public static void ResetDistances<T>(this SimulationGrid<T> grid)
        => grid.Each(node => node.ResetDistance());

}

public static class SimulationGridRenderExtensions
{
    public static void Render<T>(this SimulationGrid<T> grid, Action<Node<T>, Action<string>> drawCell, Action<string>? draw = default)
    {
        draw ??= Console.Write;
        foreach (var row in grid.Viewport())
        {
            foreach (var node in row)
            {
                drawCell(node, draw);
            }

            draw(Environment.NewLine);
        }
    }

    public static void Render<T>(this SimulationGrid<T> grid, Dictionary<(int X, int Y), string> display, Action<string>? draw = default)
    {
        draw ??= Console.Write;
        foreach (var row in grid.Viewport())
        {
            foreach (var node in row)
            {
                if (display.TryGetValue((node.X, node.Y), out var sprite))
                {
                    draw(sprite);
                }
                else
                {
                    draw(node.Value.ToString());
                }
            }

            draw(Environment.NewLine);
        }
    }

    public static void Render<T>(this SimulationGrid<T> grid, int x = 25, int y = 2, Action<IEnumerable<Node<T>>>? draw = default, Action<int, int>? setPosition = default)
    {
        draw ??= Console.WriteLine;
        setPosition ??= Console.SetCursorPosition;
        foreach (var row in grid.Viewport())
        {
            setPosition(x, y++);
            draw(row);
        }
    }

    public static void Render<T>(this SimulationGrid<T> grid, Action<string>? draw)
    {
        draw ??= Console.WriteLine;
        foreach (var row in grid.Viewport())
        {
            draw(string.Join("", row.Select(x => x.Value)));
            //draw(string.Join("", row.Select(x => $"({x.X},{x.Y})[{x.Value}]")));
        }
    }
}