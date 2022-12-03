using Day00;

using static Day00.ReadInputs;

Read()
    .Aggregate(
        new RockPaperScissor(),
        (game, match) => game.PartOneStrategy(match!))
    .ToConsole("Part One");

Read()
    .Aggregate(
        new RockPaperScissor(),
        (game, match) => game.PartTwoStrategy(match!))
    .ToConsole("Part Two");

public class RockPaperScissor
{
    private static readonly int player = 2;
    private static readonly int opponent = 0;

    public int OpponentScore { get; private set; }

    public int Score { get; private set; }

    public int Matches { get; private set; }

    public RockPaperScissor Evaluate(Shoot you, Shoot them)
    {
        Score += you.Versus(them);
        OpponentScore += them.Versus(you);

        Matches++;
        return this;
    }

    public RockPaperScissor PartOneStrategy(string match)
        => Evaluate(Shoot.From(match[player]), Shoot.From(match[opponent]));

    public RockPaperScissor PartTwoStrategy(string match)
    {
        var them = Shoot.From(match[opponent]);
        var you = match[player] switch
        {
            'X' => them.Wins(),
            'Y' => them.Draws(),
            'Z' => them.Loses(),
            _ => throw new NotImplementedException()
        };

        return Evaluate(you, them);
    }

    public override string ToString()
        => $"After {Matches} matches, You have {Score} points and your opponent has {OpponentScore} points.";
}

/// <summary>
/// One of the options Rock, Paper, Scissor.
/// </summary>
/// <param name="Value">The value for picking this option</param>
public record Shoot(int Value)
{
    public virtual int Versus(Shoot opponent)
        => throw new NotImplementedException();

    public Shoot Wins()
        => this switch
        {
            Rock => new Scissor(),
            Paper => new Rock(),
            Scissor => new Paper(),
            _ => throw new NotImplementedException()
        };

    public Shoot Loses()
        => this switch
        {
            Paper => new Scissor(),
            Rock => new Paper(),
            Scissor => new Rock(),
            _ => throw new NotImplementedException()
        };

    /// <summary>
    /// Return the shoot which draws with the current shoot.
    /// </summary>
    /// <returns>The draw options</returns>
    public Shoot Draws() => this;

    /// <summary>
    /// Parses the shoot for the current value.
    /// </summary>
    /// <param name="x">The character coded for Rock, Paper, or Scissor.</param>
    /// <returns>The Rock, Paper, or Scissor shoot.</returns>
    /// <exception cref="NotImplementedException">Throws when an invalid value cannot be parsed.</exception>
    public static Shoot From(char x)
    {
        return char.ToUpper(x) switch
        {
            'X' => new Rock(),
            'A' => new Rock(),

            'Y' => new Paper(),
            'B' => new Paper(),

            'Z' => new Scissor(),
            'C' => new Scissor(),
            _ => throw new NotImplementedException($"{x} is an unknown shoot value.")
        };
    }
}

public record Rock() : Shoot(1)
{
    public override int Versus(Shoot opponent)
    {
        return opponent switch
        {
            Rock => 3,
            Paper => 0,
            Scissor => 6,
            _ => throw new NotImplementedException()
        } + Value;
    }
}

public record Paper() : Shoot(2)
{
    public override int Versus(Shoot opponent)
    {
        return opponent switch
        {
            Rock => 6,
            Paper => 3,
            Scissor => 0,
            _ => throw new NotImplementedException()
        } + Value;
    }
}

public record Scissor() : Shoot(3)
{
    public override int Versus(Shoot opponent)
    {
        return opponent switch
        {
            Rock => 0,
            Paper => 6,
            Scissor => 3,
            _ => throw new NotImplementedException()
        } + Value;
    }
}
