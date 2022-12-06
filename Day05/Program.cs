using Day00;

using System.Collections;
using System.Text;

using static Day00.ReadInputs;

Read()
    .SolveWith<CraneSystem>()
    .ToConsole("Part One");

var crane9001 = new CraneSystem(true);
Read()
    .SolveWith(crane9001)
    .ToConsole("Part Two");

/// <summary>
/// The solution for day 5. 
/// A crane system which shuffles stacks of items per a given set of intial condition and instructions.
/// </summary>
public class CraneSystem : ISolve
{
    private readonly List<List<char>> builder = new();
    private readonly List<Stack<char>> stacks = new();
    private readonly Plans plans = new();

    private readonly bool isVersion9001;
    private bool initialized;
    private bool alreadyBuilt;

    public CraneSystem()
        : this(false)
    {
    }

    public CraneSystem(bool isVersion9001 = false)
    {
        this.isVersion9001 = isVersion9001;
    }

    public CraneSystem Load(string? row)
    {
        if (row is null || row.Length == 0)
            return this;

        else if (row.StartsWith('[') || row.StartsWith("  "))
            return AddStacksRow(row);

        else if (row.StartsWith(" 1"))
            return BuildStacks();

        else if (row.StartsWith('m'))
            return AddPlan(row);

        throw new InvalidOperationException($"Cannot load input '{row}'");
    }

    public CraneSystem AddStacksRow(string row)
    {
        if (alreadyBuilt)
        {
            throw new InvalidOperationException($"Cannot add stack rows after stacks are already built.");
        }

        var columns = (row.Length + 1) / 4;
        if (!initialized)
        {
            // Lazily create the 2nd dimensions since here know how many columns the input defines.
            initialized = true;

            // Create an empty stack for each column.
            builder.AddRange(
                Enumerable.Range(0, columns).Select(x => new List<char>()));
        }

        // parse the stack value (every 4 spaces) that have an entry for the given row of data.
        for (var column = 0; column < columns; column++)
        {
            var crate = row[column * 4 + 1];
            if (!char.IsWhiteSpace(crate))
            {
                builder[column].Add(crate);
            }
        }

        return this;
    }

    public CraneSystem BuildStacks()
    {
        if (!initialized)
        {
            throw new InvalidOperationException($"{nameof(CraneSystem)} must have stacks configured using {nameof(AddStacksRow)} before it can be built.");
        }
        if (alreadyBuilt)
        {
            throw new InvalidOperationException($"{nameof(CraneSystem)} cannot be built more than once.");
        }

        foreach (var stack in builder)
        {
            stack.Reverse();
            stacks.Add(new Stack<char>(stack));
        }

        alreadyBuilt = true;
        return this;
    }

    public CraneSystem AddPlan(string plan)
    {
        if (!initialized && !alreadyBuilt)
        {
            throw new InvalidOperationException($"Cannot add plan until stacks have been configured and built.");
        }

        plans.Add(plan);
        return this;
    }

    public CraneSystem Solve()
    {
        return isVersion9001 ?
            Version9001() :
            Version9000();
    }

    /// <summary>
    /// Solves part one by popping and pushing queue items one at a time N times from one stack to another.
    /// </summary>
    /// <returns>The active crane system</returns>
    public CraneSystem Version9000()
    {
        foreach (var (Count, From, To) in plans)
        {
            for (var i = 0; i < Count; i++)
            {
                stacks[To].Push(
                    stacks[From].Pop());
            }
        }

        return this;
    }

    /// <summary>
    /// Solves part two by slicing a non-reversed group of N items from one stack to another.
    /// </summary>
    /// <returns>The active crane system</returns>
    public CraneSystem Version9001()
    {
        foreach (var (Count, From, To) in plans)
        {
            var temp = new Stack<char>(Count);
            for (var i = 0; i < Count; i++)
            {
                temp.Push(stacks[From].Pop());
            }
            for (var i = 0; i < Count; i++)
            {
                stacks[To].Push(temp.Pop());
            }
        }

        return this;
    }

    /// <summary>
    /// Returns the top item from each stack.
    /// </summary>
    /// <returns>A string of all the top items</returns>
    /// <remarks>We assume all stacks have atleast one item.</remarks>
    public string TopRow()
    {
        var sb = new StringBuilder();
        foreach (var stack in stacks)
        {
            sb.Append(stack.Peek());
        }

        sb.AppendLine();
        return sb.ToString();
    }

    /// <summary>
    /// Destructive rendering of the queue.
    /// </summary>
    /// <returns>A string built by popping the queue'd items.</returns>
    public override string ToString()
    {
        var buffer = new StringBuilder();
        buffer.AppendLine($"TopRow: {TopRow()}");

        while (stacks.Any(x => x.Any()))
        {
            foreach (var column in stacks)
            {
                if (column.Any())
                {
                    buffer.Append($"[{column.Pop()}] ");
                }
                else
                {
                    buffer.Append("    ");
                }
            }

            buffer.AppendLine();
        }

        return buffer.ToString();
    }

    ISolve ISolve.Load(string? data) => Load(data);

    ISolve ISolve.Solve() => Solve();
}

public class Plans : IEnumerable<(int Count, int From, int To)>
{
    private readonly List<(int, int, int)> plans = new();

    public void Add(string plan)
    {
        var parts = plan.Split(' ');
        plans.Add((int.Parse(parts[1]), int.Parse(parts[3]) - 1, int.Parse(parts[5]) - 1));
    }

    public IEnumerator<(int Count, int From, int To)> GetEnumerator()
        => plans.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
