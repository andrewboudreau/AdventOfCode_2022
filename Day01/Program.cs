using static Day00.ReadInputs;

Console.WriteLine(
    ReadInts()
        .Aggregate(
            seed: (prev: int.MaxValue, increases: 0),
            func: (acc, next) =>
                (next, (next > acc.prev)
                    ? acc.increases + 1
                    : acc.increases)));


var prev = new int[3] { int.MaxValue, int.MaxValue, int.MaxValue, };

Console.WriteLine(ReadInts().Aggregate(
    seed: (prev, increases: 0),
    func: (acc, next) =>
    {
        acc.increases +=
            next + acc.prev[2] + acc.prev[1] > acc.prev[0] + acc.prev[1] + acc.prev[2]
                ? 1 : 0;

        acc.prev[0] = acc.prev[1];
        acc.prev[1] = acc.prev[2];
        acc.prev[2] = next;
        return acc;
    }).increases - 1);