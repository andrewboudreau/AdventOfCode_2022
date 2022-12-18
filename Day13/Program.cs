using Day00;

using System.Collections;
using System.Text;

using static Day00.ReadInputs;

var pairs = ReadRecords(rows => new Packets(rows[0], rows[1]));

var index = 1;
var orderedMessageIndexes = new List<int>();



// 4170 is too low

foreach (var pair in pairs)
{
    var ordered = pair.IsOrdered();
    Console.WriteLine($"Pair {index} is {(ordered ? "Ordered" : "NOT Ordered")}");
    Console.WriteLine();

    if (ordered) orderedMessageIndexes.Add(index);
    index++;
}

Console.WriteLine($"Part One, the sum of the correctly ordered packet indexes is {orderedMessageIndexes.Sum(index => index)}");

public class Packets
{
    public Packets(string lhs, string rhs)
    {
        Left = Receive(lhs);
        Right = Receive(rhs);
    }

    public ArrayList Left { get; }

    public ArrayList Right { get; }

    private ArrayList Receive(string packet)
    {
        bool isOne = false;
        Stack<ArrayList> stack = new();
        stack.Push(new ArrayList());

        foreach (var token in packet)
        {
            if (isOne)
            {
                isOne = false;
                stack.Peek().Add(token == '0' ? 10 : 1);
                //Console.WriteLine($"Add {(token == '0' ? 10 : 1)}");
            }
            else if (char.IsNumber(token))
            {
                if (token == '1')
                {
                    isOne = true;
                    continue;
                }
                else
                {
                    stack.Peek().Add((int)char.GetNumericValue(token));
                    //Console.WriteLine("Add " + token);
                }
            }

            if (token == '[')
            {
                // Console.WriteLine($"Push ArrayList");
                var next = new ArrayList();
                stack.Peek().Add(next);
                stack.Push(next);
            }
            else if (token == ']')
            {
                //Console.WriteLine($"Pop ArrayList");
                stack.Pop();
            }
        }

        return stack.Pop();
    }

    public bool IsOrdered()
    {
        var result = Compare(Left, Right);
        if (result == 0)
        {
            throw new InvalidOperationException("result should never be zero");
        }
        
        return result == -1;
    }

    public StringBuilder Print(object? obj, StringBuilder? builder = default)
    {
        ArgumentNullException.ThrowIfNull(obj);

        builder ??= new StringBuilder();
        if (obj is ArrayList items)
        {
            builder.Append("[");
            for (var i = 0; i < items.Count; i++)
            {
                Print(items[i], builder);
                if (i < items.Count - 1)
                {
                    builder.Append(",");
                }
            }
            builder.Append("]");
            return builder;
        }

        builder.Append((int)obj);
        return builder;
    }

    public int Compare(object? left, object? right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        //Console.WriteLine($"left={left.GetType().Name} right={right.GetType().Name}");
        if (left is IEnumerable leftEnumerable && right is IEnumerable rightEnumerable)
        {
            Console.WriteLine($"Compare {Print(left)} vs {Print(right)}");
            bool leftHasItems = false;
            bool rightHasItems = false;

            var leftIter = leftEnumerable.GetEnumerator();
            var rightIter = rightEnumerable.GetEnumerator();

            while (
                (leftHasItems = leftIter.MoveNext()) &&
                (rightHasItems = rightIter.MoveNext()))
            {
                var result = Compare(leftIter.Current, rightIter.Current);
                if (result != 0)
                {
                    Console.WriteLine((result == -1) ? "Left side is lower value" : "Right side is lower value" );
                    return result;
                }
            }

            if (!leftHasItems && !rightHasItems)
            {
                Console.WriteLine("Both are empty!");
                return 0;
            }
            
            if (leftHasItems)
            {
                var result = Compare(leftIter.Current, rightIter.Current);
                if (result != 0)
                {
                    Console.WriteLine((result == -1) ? "Left side is lower value" : "Right side is lower value");
                    return result;
                }
                Console.WriteLine($"Left side still has items, right is empty");
            }
            else
            {
                var result = Compare(leftIter.Current, rightIter.Current);
                if (result != 0)
                {
                    Console.WriteLine((result == -1) ? "Left side is lower value" : "Right side is lower value");
                    return result;
                }
                Console.WriteLine($"Right side still has items, left is empty");
            }

            return leftHasItems ? 1 : -1;
        }
        else if (left is int leftNumber && right is int rightNumber)
        {
            Console.WriteLine($"Compare {leftNumber} vs {rightNumber} is {leftNumber.CompareTo(rightNumber)}");
            return leftNumber.CompareTo(rightNumber);
        }
        else if (left is IEnumerable enumerable && right is int number)
        {
            Console.WriteLine($"Compare {Print(left)} vs {number}");
            return Compare(enumerable, new ArrayList(new[] { number }));
        }
        else
        {
            Console.WriteLine($"Compare {(int)left} vs {Print(right)}");
            return Compare(new ArrayList(new[] { (int)left }), (IEnumerable)right);
        }
    }
}
