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

    public int X { get; init; }
    public int Y { get; init; }
    public T Value { get; private set; }

    public IEnumerable<Node<T>> Neighbors => neighbors;

    public T SetValue(T value)
        => Value = value;

    public T SetValue(Func<T, T> setter)
        => SetValue(setter(Value));

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
