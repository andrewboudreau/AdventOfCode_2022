using System.Collections.Immutable;

using static Day00.ReadInputs;

var input = Read().First().AsSpan();
var answer = 0;

for (var i = 4; i < input.Length; i++)
{
    if (
        input[i - 1] != input[i - 2] &&
        input[i - 1] != input[i - 3] &&
        input[i - 1] != input[i - 4] &&
        input[i - 2] != input[i - 3] &&
        input[i - 2] != input[i - 4] &&
        input[i - 3] != input[i - 4])
    {
        answer = i;
        break;
    }
}

Console.WriteLine($"Stopped at index {answer}");
Console.WriteLine($"{input.ToString().Substring(answer - 4, 4)}");

var partTwo = 0;
for (var end = 14; end < input.Length - 1; end++)
{
    var hash = input[(end - 14)..end].ToImmutableArray().ToImmutableHashSet();
    if (hash.Count == 14)
    {
        partTwo = end;
        break;
    }
}

Console.WriteLine($"Stopped at index {partTwo}");
Console.WriteLine($"{input.ToString().Substring(partTwo - 14, 14)}");