using static Day00.ReadInputs;

var visible = 0;
var grid = new Grid<int>(ReadAsRowsOfInts());
grid.Each(tree =>
{
    if (
        // Is tree on the edge?
        tree.X == 0 ||
        tree.Y == 0 ||
        tree.X == grid.Width - 1 ||
        tree.Y == grid.Width - 1 ||

        // is tree visible from edge
        grid.UpFrom(tree).All(height => height < tree.Value) ||
        grid.LeftFrom(tree).All(height => height < tree.Value) ||
        grid.DownFrom(tree).All(height => height < tree.Value) ||
        grid.RightFrom(tree).All(height => height < tree.Value))
    {
        visible++;
    }

     Console.WriteLine($"node {tree.X}:{tree.Y} RightFrom={string.Join(",", grid.RightFrom(tree))}");
});

Console.WriteLine($"Part One: {visible} trees are visible.");


// Part Two
IEnumerable<IEnumerable<int>> source =
    grid.Rows().Skip(1).SkipLast(1)
        .Select(row => row.Skip(1).SkipLast(1)
            .Select(tree =>
                    grid.UpFrom(tree).TakeUntil(h => tree.Value <= h).Count() *
                    grid.LeftFrom(tree).TakeUntil(h => tree.Value <= h).Count() *
                    grid.DownFrom(tree).TakeUntil(h => tree.Value <= h).Count() *
                    grid.RightFrom(tree).TakeUntil(h => tree.Value <= h).Count()));

var scores = new Grid<int>(source);

var mostScenic = scores.Nodes().MaxBy(x => x.Value)
    ?? throw new NullReferenceException("");

Console.WriteLine($"Part Two: The max scenic score is {mostScenic.Value} at {mostScenic.X},{mostScenic.Y}");

if (scores.Width < 9)
{
    scores.Render(0, 2, draw: row =>
    {
        Console.WriteLine(string.Join(",", row.Select(x => x.Value.ToString())));
    });
}


public static class EnumerableExtensions
{
    public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (T item in source)
        {
            if (!predicate(item))
            {
                yield return item;
            }
            else
            {
                yield return item;
                yield break;
            }
        }
    }
}