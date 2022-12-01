using Day00;
using static Day00.ReadInputs;

Read()
    .Aggregate(
        new List<List<int>>() { new() },
        (acc, calories) =>
        {
            if (string.IsNullOrEmpty(calories)) 
                acc.Add(new());
            else
                acc[^1].Add(int.Parse(calories));

            return acc;
        }, 
        acc => acc.Select(x => x.Sum()).OrderByDescending(x => x))
    .ToConsole(totals => new[]
    {
        $"Elf with the max calories is {totals.First()}",
        $"Top 3 Elves with the max calories are {totals.First()}, {totals.Second()}, {totals.Third()}",
        $"Top 3 Elves with the max calories sum is {totals.Take(3).Sum()}"
    });
