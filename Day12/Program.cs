using static Day00.ReadInputs;

var grid =
    new Grid<char>(
        rows: Read()!,
        factory: row => row.Select(x => x));

var start = grid.Nodes().First(x => x == 'S');
start.SetValue('a');

var end = grid.Nodes().First(x => x == 'E');
end.SetValue('z');

foreach (var node in grid)
{
    foreach (var near in grid.Neighbors(node, withDiagonals: false))
    {
        if (node.Value + 1 >= near.Value)
        {
            node.AddNeighbor(near);
        }
    }
}

grid.FillDistances(from: end);
DrawGrid();
Console.WriteLine($"Part One: Distance to start is {start.Distance}");

var cursor = Console.GetCursorPosition();
while (start != end)
{
    Console.SetCursorPosition(start.X, start.Y);
    Console.Write("█");
    start = start.Neighbors.OrderBy(x => x.Distance).First();
}
Console.SetCursorPosition(cursor.Left, cursor.Top);

var part2 = grid.Nodes()
    .Where(x => x.Value == 'a')
    .OrderBy(x => x.Distance)
    .Select(x => x.Distance)
    .Take(5);

Console.WriteLine($"Part Two: {string.Join(", ", part2)}");

void DrawGrid()
{
    grid.Render(drawCell: (node, draw) =>
    {
        Console.ForegroundColor = node.Distance switch
        {
            < 50 => ConsoleColor.Blue,
            < 100 => ConsoleColor.DarkBlue,
            < 150 => ConsoleColor.DarkCyan,
            < 200 => ConsoleColor.Cyan,
            < 250 => ConsoleColor.Magenta,
            < 300 => ConsoleColor.DarkMagenta,
            < 350 => ConsoleColor.DarkRed,
            < 400 => ConsoleColor.Red,
            < 450 => ConsoleColor.DarkGreen,
            < 500 => ConsoleColor.Green,
            < 550 => ConsoleColor.Yellow,
            < 600 => ConsoleColor.DarkYellow,
            int.MaxValue => ConsoleColor.Black,
            _ => ConsoleColor.White
        };

        draw(node.Distance.ToString().Last().ToString());

        Console.ResetColor();
    }, draw: default);
}
