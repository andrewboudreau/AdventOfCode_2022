using System.Collections;
using System.Collections.Generic;

public class Grid<T> : IEnumerable<Node<T>>
{
    private readonly List<Node<T>> nodes;
    private readonly int width;

    public Grid(IEnumerable<string> rows, Func<string, IEnumerable<T>> factory)
       : this(rows.Select(factory))
    {
    }

    public Grid(IEnumerable<IEnumerable<T>> map)
    {
        nodes = new List<Node<T>>();
        int x = 0;
        int y = 0;

        foreach (var row in map)
        {
            foreach (var value in row)
            {
                nodes.Add(new Node<T>(x++, y, value));
            }

            if (width == 0)
            {
                width = x;
            }
            x = 0;
            y++;
        }
    }

    public Node<T>? this[int x, int y]
    {
        get
        {
            if (x < 0) return default;
            if (x >= width) return default;
            if (y < 0) return default;
            if (y >= width) return default;

            int offset = y * width + x;
            if (offset < 0 || offset >= nodes.Count) return default;
            return nodes[offset];
        }
    }

    public IEnumerable<Node<T>> Neighbors(Node<T> position, bool withDiagonals = true)
    {
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

        if (this[position.X - 1, position.Y] is Node<T> left)
        {
            yield return left;
        }

        if (this[position.X + 1, position.Y] is Node<T> right)
        {
            yield return right;
        }

        if (withDiagonals && this[position.X - 1, position.Y - 1] is Node<T> downLeft)
        {
            yield return downLeft;
        }

        if (this[position.X, position.Y - 1] is Node<T> down)
        {
            yield return down;
        }

        if (withDiagonals && this[position.X + 1, position.Y - 1] is Node<T> downRight)
        {
            yield return downRight;
        }
    }

    public IEnumerable<Node<T>> Nodes()
    {
        for (var offset = 0; offset < nodes.Count; offset++)
        {
            yield return nodes[offset];
        }
    }

    public Grid<T> WhileTrue(Func<Grid<T>, bool> operation)
    {
        while (operation(this)) ;
        return this;
    }

    public Grid<T> Each(Action<Node<T>> action)
    {
        foreach (var node in Nodes())
        {
            action(node);
        }

        return this;
    }

    public IEnumerable<IEnumerable<Node<T>>> Rows()
    {
        for (var row = 0; row < nodes.Count / width; row++)
        {
            yield return nodes.Skip(row * width).Take(width);
        }
    }

    public Grid<T> Render(int x = 25, int y = 2, Action<string>? draw = default, Action<int, int>? setPosition = default)
    {
        draw ??= Console.WriteLine;
        setPosition ??= Console.SetCursorPosition;
        foreach (var row in Rows())
        {
            setPosition(x, y++);
            draw(string.Join("", row.Select(x => x.Value)));
        }

        return this;
    }

    public Grid<T> WriteTo(Action<string>? draw = default)
    {
        draw ??= Console.WriteLine;
        foreach (var row in Rows())
        {
            draw(string.Join("", row.Select(x => x.Value)));
            //draw(string.Join("", row.Select(x => $"({x.X},{x.Y})[{x.Value}]")));
        }

        return this;
    }

    public IEnumerator<Node<T>> GetEnumerator()
        => Nodes().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}