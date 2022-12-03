using Day00;
using static Day00.ReadInputs;

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

public class RockPaperScissor
{
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

public static class RockPaperScissorParsingExtensions
{
    public static char Player(this string match) => match[2];
    public static char Opponent(this string match) => match[0];
}