# AdventOfCode_2022
C# solutions for the 2022 Advent of Code.

## Day00: How to use the tools
```csharp
// The Read extensions streams data form the console input 
//  or a given file contents to make parsing inputs really easy.
public static IEnumerable<string?> Read()
{
    var args = Environment.GetCommandLineArgs();
    var inputFile = args.Length > 1 ? args[1] : string.Empty;
    if (!string.IsNullOrEmpty(inputFile))
    {
        foreach (var line in File.ReadAllLines(inputFile))
        {
            yield return line;
        }
    }
    else
    {
        while (true)
            yield return Console.ReadLine();
    }

    // Run the example
    AdventOfCode_2022\Day01> dotnet run problem.txt
```

## Day01: An exercise in LINQ
```csharp
// How to use the Read extensions.
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
```

## Day02: Running a rock, paper, scissor, game engine by replaying a list of matches.
```csharp
Read()
    .Aggregate(
        new RockPaperScissor(),
        (game, match) => game.PartOneStrategy(match))
    .ToConsole("Part One");

Read()
    .Aggregate(
        new RockPaperScissor(),
        (game, match) => game.PartTwoStrategy(match))
    .ToConsole("Part Two");
```