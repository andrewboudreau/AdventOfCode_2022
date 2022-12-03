using Day00;
using static Day00.ReadInputs;


Read(x => x!)
    .Aggregate(0, (sum, sack) =>
        sum += sack[..(sack.Length / 2)]
            .Intersect(sack[(sack.Length / 2)..])
            .Sum(x => Value(x)))
    .ToConsole("Part One");


Read(x => x!)
    .Chunk(3)
    .Aggregate(0, (sum, batch) =>
        sum += batch.First()
            .Intersect(batch.Second())
            .Intersect(batch.Third())
            .Sum(x => Value(x)))
    .ToConsole("Part Two");


static int Value(char id)
{
    if (char.IsLower(id))
    {
        return id - 'a' + 1;
    }

    return id - 'A' + 27;
}
