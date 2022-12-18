using Day00;

using System.Security.Cryptography.X509Certificates;

using static Day00.ReadInputs;

const char WALL = '#';
const char EMPTY = '.';
const char SOURCE = '+';
const char ROCK = '@';

var simulation = ReadTo(rows =>
{
    var walls = rows
        .Select(row => row!.Split("->").Zip(row.Split("->").Skip(1)))
        .SelectMany(
            lines => lines,
            (lines, line) =>
                new Line(
                    line.First.Split(",", StringSplitOptions.TrimEntries)
                    .Concat(
                        line.Second.Split(",", StringSplitOptions.TrimEntries))
                    .Select(int.Parse).ToArray()))
        .SelectMany(x => x.Path())
        .Distinct()
        .Select(node => (node.X, node.Y, WALL));


    var grid = new SimulationGrid<char>(walls);
    grid.Create(500, 0, SOURCE);

    return grid;
});


for (var i = 0; i < 2000; i++)
{
    var f = 0;
    simulation.Step((sim, node, step) =>
    {
        if (node.Value == WALL)
        {
            return;
        }
        if (node.Value == ROCK)
        {
            if (sim.TryMove(node, out (int X, int Y) destination))
            {
                //Console.WriteLine($"{f++} {step}: rock to dest {destination.X},{destination.Y}");
                node.SetPosition((destination.X, destination.Y));
            }
        }

        if (node.Value == SOURCE)
        {
            sim.Create(node.X, node.Y + 1, ROCK);
        }
    });
    
    if (i % 100 == 0)
    {
        Console.Clear();
        simulation.Render(
            drawCell: (node, draw)
                => draw(node?.Value.ToString() ?? "."));
    }
}


//var lines = walls.SelectMany(x => x, (section, line) => line);

//((int X, int Y) TopLeft, (int X, int Y) BottomRight) bounding =
//    (
//        (lines.Min(x => x.MinX) - 4, lines.Min(y => y.MinY)),
//        (lines.Max(x => x.MaxX) + 4, lines.Max(y => y.MaxY) + 4)
//    );

//Console.WriteLine($"bounding box is {bounding}");
//var min = Math.Max(bounding.BottomRight.X - bounding.TopLeft.X, bounding.BottomRight.Y);
//Console.WriteLine("min is " + min);


//var grid = new SimulationGrid<char>();

//grid[500, 0]!.SetValue('+');

//foreach (var wall in walls)
//{
//    foreach (var section in wall)
//    {
//        foreach (var (X, Y) in section.Path())
//        {
//            if (grid[X, Y] is Node<char> node)
//            {
//                node.SetValue('#');
//            }
//            else
//            {
//                throw new InvalidOperationException($"Could not find node at {X},{Y}");
//            }
//        }
//    }
//}



