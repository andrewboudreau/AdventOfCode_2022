using Day00;
using static Day00.ReadInputs;

Read(x => x.Split(',').Split('-').Chunk(4)
    .Select(x => new
    {
        Line1 = new Line(x.ElementAt(0), x.ElementAt(1)),
        Line2 = new Line(x.ElementAt(2), x.ElementAt(3))
    }).First())
    .Aggregate((0, 0), (sum, pair) =>
    {
        // Part One
        var fullyContains = false;
        if (pair.Line1.MinX == pair.Line2.MinX || pair.Line1.MaxX == pair.Line2.MaxX)
        {
            fullyContains = true;
        }
        else if (pair.Line1.MinX < pair.Line2.MinX)
        {
            fullyContains = pair.Line1.MaxX >= pair.Line2.MaxX;
        }
        else
        {
            fullyContains = pair.Line2.MaxX >= pair.Line1.MaxX;
        }

        // Part Two
        var overlaps = fullyContains;
        if (!overlaps)
        {
            if (pair.Line1.MinX <= pair.Line2.MinX && pair.Line1.MaxX >= pair.Line2.MinX)
            {
                overlaps = true;
            }
            else if (pair.Line1.MinX >= pair.Line2.MinX && pair.Line2.MaxX >= pair.Line1.MinX)
            {
                overlaps = true;
            }
        }

        if (sum.Item2 < 5)
        {
            RenderLine(pair.Line1, overlaps);
            RenderLine(pair.Line2, overlaps);
            Console.WriteLine();
        }

        return (fullyContains ? sum.Item1 + 1 : sum.Item1, overlaps ? sum.Item2 + 1 : sum.Item2);
    })
    .ToConsole("Day 04: (Part1, Part2)");



static void RenderLine(Line line, bool fullyContains)
{
    var foreground = Console.ForegroundColor;
    var n = 100;
    for (var i = 0; i < n; i++)
    {
        if (i % 10 == 0)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        if (i == line.MinX || i == line.MaxX)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        if (i >= line.MinX && i <= line.MaxX)
        {
            Console.Write(
                i % 10 == 0 ?
                i.ToString().First() :
                i.ToString().Last());
        }
        else
        {
            Console.Write('.');
        }

        Console.ForegroundColor = foreground;
    }

    if (fullyContains) Console.Write($"   |{line.X1}-{line.X2}");
    else Console.Write($"    {line.X1}-{line.X2}");
    Console.WriteLine();
}


//935 too high
// 844 too low