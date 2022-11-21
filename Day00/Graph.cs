using System.Collections;
public class Graph<T> : IEnumerable<Node<T>>
{
    private readonly HashSet<Node<T>> nodes;

    public Graph(IEnumerable<(T Left, T Right)> edges)
    {
        var pairs = edges.ToList();
        nodes = new HashSet<Node<T>>(pairs.Count);
     
        var values = new HashSet<T>(pairs.Count);
        Node<T>? left = null;
        Node<T>? right = null;

        foreach ((T Left, T Right) in edges)
        {
            left = null;
            right = null;
            if (values.Add(Left))
            {
                left = new Node<T>(Left);
                nodes.Add(left);
            }

            if (values.Add(Right))
            {
                right = new Node<T>(Right);
                nodes.Add(right);
            }

            left ??= nodes.First(x => x.Value!.Equals(Left));
            right ??= nodes.First(x => x.Value!.Equals(Right));

            left.AddNeighbor(right);
            right.AddNeighbor(left);
        }
    }

    public IEnumerable<Node<T>> Nodes => nodes;

    public Graph<T> WhileTrue(Func<Graph<T>, bool> operation)
    {
        while (operation(this)) ;
        return this;
    }

    public Graph<T> Each(Action<Node<T>> action)
    {
        foreach (var node in Nodes)
        {
            action(node);
        }

        return this;
    }

    public Graph<T> Render(int x = 25, int y = 2, Action<string>? draw = default, Action<int, int>? setPosition = default)
    {
        draw ??= Console.WriteLine;
        setPosition ??= Console.SetCursorPosition;
        foreach (var node in Nodes)
        {
            setPosition(x, y++);
            draw(string.Join(" ", node.Value));
        }

        return this;
    }

    public Graph<T> WriteTo(Action<string>? draw = default)
    {
        draw ??= Console.WriteLine;
        foreach (var node in Nodes)
        {
            draw(string.Join(" ", node.Value));
        }

        return this;
    }

    public IEnumerator<Node<T>> GetEnumerator()
        => Nodes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}