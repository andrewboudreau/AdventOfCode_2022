using System.Diagnostics.CodeAnalysis;

public class Node<T> : IEqualityComparer<T>, IEquatable<T>
{
    private readonly List<Node<T>> neighbors;
    public Node(T value)
    {
        X = 0;
        Y = 0;
        Value = value;
        neighbors = new List<Node<T>>();
    }

    public Node(int x, int y, T value)
    {
        X = x;
        Y = y;
        Value = value;
        neighbors = new List<Node<T>>();
    }
    public Node((int x, int y, T value) node)
        : this(node.x, node.y, node.value)
    {
    }

    public int X { get; private set; }
    public int Y { get; private set; }

    public T Value { get; private set; }

    public int Distance { get; private set; }

    public int Visited { get; private set; }

    public bool IsVisited => Visited > 0;

    public IEnumerable<Node<T>> Neighbors => neighbors;

    public int Visit() => Visited += 1;

    public void ResetVisited()
    {
        Visited = 0;
    }

    public void ResetDistance()
    {
        Distance = int.MaxValue;
    }

    public int SetDistance(int distance)
    {
        Visit();
        Distance = distance;
        return Distance;
    }

    public void FillDistances(int distance = 0)
    {
        SetDistance(distance);

        foreach (var neighbor in neighbors)
        {
            if (!neighbor.IsVisited)
            {
                neighbor.FillDistances(distance + 1);
            }
            else if (neighbor.Distance > distance + 1)
            {
                neighbor.FillDistances(distance + 1);
            }

            if (neighbor.Visited > 5)
            {
                return;
            }
        }
    }

    public IEnumerable<Node<T>> ShortestPathTo(Node<T> end)
    {
        var current = this;
        while (current != end)
        {
            var next = current.Neighbors
                .OrderBy(x => x.Distance)
                .First(x => !x.IsVisited);

            next.Visit();

            yield return next;
            current = next;
        }
    }

    public T SetValue(T value)
            => Value = value;

    public T SetValue(Func<T, T> setter)
        => SetValue(setter(Value));

    public void SetPosition((int X, int Y) position)
    {
        X = position.X;
        Y = position.Y;
    }

    public void AddNeighbor(Node<T> neighbor)
        => neighbors.Add(neighbor);

    public void Deconstruct(out int x, out int y, out T value)
    {
        x = X;
        y = Y;
        value = Value;
    }

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    public override string ToString() => $"{X},{Y} {Value}";

    public bool Equals(T? x, T? y)
        => (x == null && y == null) || x!.Equals(y);

    public int GetHashCode([DisallowNull] T obj)
        => GetHashCode(obj);

    public bool Equals(T? other)
        => Equals(this, other);

    public static implicit operator T(Node<T> node) => node.Value;
}
