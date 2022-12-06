# AdventOfCode_2022
C# solutions for the 2022 Advent of Code.
[Advent of Code 2022](https://adventofcode.com)

## Day00: How to use the Read()
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

## Day02: Rock, Paper, Scissor game engine
Given a list of matches, calculate your score.  
Read works great to read each rows of the input as a round. `Aggregate` gives an easy way to perform an operation on each round, and even provides a way to seed the starting system.

A RockPaperScissor game engine seeds the aggregate operation which performs a match evaluation on each input line.
```csharp
Read(x => x!)
    .Aggregate(
        new RockPaperScissor(),
        (game, match) => game.Evaluate(
            Shoot.From(match.Player()),
            Shoot.From(match.Opponent())))
    .ToConsole("Part One");

Read(x => x!)
    .Aggregate(
        new RockPaperScissor(),
        (game, match) =>
        {
            var them = Shoot.From(match.Opponent());
            var you = match.Player() switch
            {
                'X' => them.Wins(),
                'Y' => them.Draws(),
                'Z' => them.Loses(),
                _ => throw new NotImplementedException()
            };

            return game.Evaluate(you, them);
        })
    .ToConsole("Part Two");
```

## Day03: Linq intersections and sums.
```csharp
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
```


## Day05: A crane simulation. 
The challenge moves chars between collections. The code users `Solution` to provides 2 simple interfaces to 1) Load Data, 2) Solve a problem using the given data.

```csharp
Read()
    .SolveWith<CraneSystem>()
    .ToConsole("Part One");

var crane9001 = new CraneSystem(true);
Read()
    .SolveWith(
        prepareWith: (solver, input) => solver.Load(input),
        solveWith: crane9001)
    .ToConsole("Part Two");

```

The supporting extensions methods bring the solution together with the input stream to make the Program easier to write. 
```csharp
public static class ISolverExtensions
{
    public static TSolver SolveWith<TSolver>(this IEnumerable<string?> source, TSolver? solveWith = default)
        where TSolver : ISolve, ILoadInput, new()
            => SolveWith(source, (solver, row) => (TSolver)solver.Load(row), solveWith);

    public static TSolver SolveWith<TSolver>(this IEnumerable<string?> source, Func<TSolver, string?, TSolver> prepareWith, TSolver? solveWith = default) 
        where TSolver : ISolve, new()
    {
        solveWith ??= new TSolver();
        foreach (var row in source)
        {
            solveWith = prepareWith(solveWith, row);
        }

        solveWith.Solve();
        return solveWith;
    }
}
```