using Day00;
using Day00.Solvers;

using System.Diagnostics;

using static Day00.ReadInputs;

var simulation = new RopeSimulation();
Read()
    .SolveWith(solveWith: simulation)
    .RenderGrid();

var step = 0;
while (simulation.Step())
{
    simulation.RenderStep();
    Thread.Sleep(150);
}

public class RopeSimulation : Solution
{
    private static readonly int knotCount = 10;
    private static readonly (int X, int Y) offset = (15,5);

    private readonly List<(int X, int Y)> movements = new() { };
    private readonly List<(int X, int Y)[]> rope = new() { CreateKnotFrame(knotCount) };

    private int currentStep = -1;

    public IEnumerable<(int X, int Y)> Movements => movements;
    public IEnumerable<(int X, int Y)[]> Rope => rope;

    public bool Step()
    {
        currentStep++;
        return currentStep < rope.Count;
    }

    public int ResetStep(int step = -1) => currentStep = step;

    public override ISolve Load(string? data)
    {
        (char Direction, int Value) = (data!.Split(" ")[0][0], int.Parse(data!.Split(" ")[1]));

        movements.Add(
            Direction switch
            {
                'U' => (0, Value),
                'L' => (-Value, 0),
                'D' => (0, -Value),
                'R' => (Value, 0),
                _ => throw new NotImplementedException(),
            });

        var previous = rope[^1][0];
        var movement = new Line(
            previous.X, previous.Y,
            previous.X + movements[^1].X, previous.Y + movements[^1].Y);

        foreach (var step in movement.Path().Skip(1))
        {
            var knots = ((int X, int Y)[])rope[^1].Clone();
            knots[0] = (step.X, step.Y);

            rope.Add(knots);

            for (var i = 1; i < knotCount; i++)
            {
                var head = knots[i - 1];
                var tail = knots[i];

                var x = head.X > tail.X ? 1 : -1;
                var y = head.Y > tail.Y ? 1 : -1;

                var distance = tail.DistanceTo(head);
                if (distance == 0 || distance == 1)
                {
                    // tail is close enough
                }
                else if (distance == 2)
                {
                    if (tail.X != head.X && tail.Y != head.Y)
                    {
                        // tail and head are diagonal.
                        // tail is close enough
                    }
                    else if (tail.X == head.X)
                    {
                        // tail needs to move in the Y direction
                        knots[i] = (tail.X, tail.Y + y);
                    }
                    else if (tail.Y == head.Y)
                    {
                        // tail needs to move in the X direction
                        knots[i] = (tail.X + x, tail.Y);
                    }
                    else
                    {
                        Console.WriteLine($"Frame {rope.Count}: Knots:{string.Join(" ", knots)}");
                        throw new UnreachableException();
                    }
                }
                else if (distance == 3 || distance == 4)
                {
                    // tails needs to move diagonal
                    knots[i] = (tail.X + x, tail.Y + y);
                }
                else
                {
                    Console.WriteLine($"Frame {rope.Count}:{string.Join(" ", knots)}");
                    throw new UnreachableException($"distance is too high {distance}");
                }

                //Console.WriteLine("distances to previous: " + string.Join(" ", rope.Skip(1).Select((x, i) => rope[i].Last().DistanceTo(rope[i + 1].Last()))));
                //currentStep = rope.Count - 1;
                //RenderStep();
            }

            //Console.WriteLine($"{rope.Count}" + string.Join(" ", knots));
        }
        return this;
    }

    public override ISolve Solve()
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"tail locations {rope.Select(x => x.Last()).ToHashSet().Count()}");
        return this;
    }

    private readonly int consolePadding = 2;

    public RopeSimulation RenderGrid()
    {
        Console.SetCursorPosition(consolePadding, consolePadding);
        for (var i = 0; i < 20; i++)
        {
            Console.SetCursorPosition(consolePadding, Console.CursorTop);
            Console.WriteLine(".".PadLeft(30, '.'));
        }

        return this;
    }

    public RopeSimulation RenderStep()
    {
        Console.CursorVisible = false;

        if (currentStep > 0)
        {
            foreach (var (X, Y) in rope[currentStep - 1])
            {
                if (consolePadding + X > 0 && consolePadding + X < Console.BufferWidth - 1)
                {
                    Console.SetCursorPosition(consolePadding + X, consolePadding + Y);
                    Console.Write('.');
                }
            }
        }

        char i = 'A';
        foreach (var (X, Y) in rope[currentStep])
        {
            if (consolePadding + X > 0 && consolePadding + X < Console.BufferWidth - 1)
            {
                Console.SetCursorPosition(consolePadding + X, consolePadding + Y);
                Console.Write(i++);
            }
        }

        return this;
    }

    private static (int X, int Y)[] CreateKnotFrame(int total, params (int X, int Y)[] others)
    {
        return others.Concat(Enumerable.Repeat((offset.X, offset.Y), total - others.Length)).ToArray();
    }
}

public static class PositionExtensions
{
    public static bool IsTouching(this (int X, int Y) lhs, (int X, int Y) rhs)
    {
        return Math.Abs(rhs.X - lhs.X) < 2 && Math.Abs(rhs.Y - lhs.Y) < 2;
    }

    public static bool IsNotTouching(this (int X, int Y) lhs, (int X, int Y) rhs)
    {
        return !IsTouching(lhs, rhs);
    }

    public static int DistanceTo(this (int X, int Y) lhs, (int X, int Y) rhs)
    {
        return Math.Abs(lhs.X - rhs.X) + Math.Abs(lhs.Y - rhs.Y);
    }
}
