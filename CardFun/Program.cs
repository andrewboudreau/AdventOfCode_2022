using Day00;

using static Day00.ReadInputs;


Read(x => x).ToConsole("Hello, Day 05");

var deck = new Deck();
deck.Shuffle().ToConsole();

var game = new SolitarePop(deck);
while (game.Step())
{
    Console.WriteLine();
};

public class SolitarePop
{
    private readonly Deck deck;
    private readonly Deck hand;

    public SolitarePop(Deck? deck = default)
    {
        this.deck = deck ?? new Deck();
        hand = new Deck(empty: true);
    }

    public bool Step(int repeat)
    {
        for (int i = 0; i < repeat; i++)
            if (!Step())
                return false;

        return false;
    }

    public bool Step()
    {
        while (hand.Count < 4)
        {
            var card = deck.Deal(hand);
            Console.WriteLine($"dealt {card} to player");
        }

        Console.WriteLine($"Hand has {hand}");

        if (hand.Count >= 4 &&
            hand[^1].Rank == hand[^2].Rank &&
            hand[^2].Rank == hand[^3].Rank &&
            hand[^3].Rank == hand[^4].Rank)
        {
            Console.WriteLine($"Found 4 Ranks in a row between {Decks.ToString(hand.ToArray()[^4..^1])}");
            hand.RemoveRange(hand.Count - 5, 4);
        }
        else if (hand.Count >= 4 &&
            hand[^1].Suit == hand[^2].Suit &&
            hand[^2].Suit == hand[^3].Suit &&
            hand[^3].Suit == hand[^4].Suit)
        {
            Console.WriteLine($"Found 4 Suits in a row between {Decks.ToString(hand.ToArray()[^4..^1])}");
            hand.RemoveRange(hand.Count - 5, 4);
        }
        else if (hand[^1].Rank == hand[^4].Rank || hand[^1].Suit == hand[^4].Suit)
        {
            Console.WriteLine($"Found bookend match, removing {Decks.ToString(hand.ToArray()[^3..^1])}");
            hand.RemoveRange(hand.Count - 3, 2);
        }
        else
        {
            if (deck.Count == 0)
            {
                return false;
            }

            Console.WriteLine($"Added Card {deck.Deal(hand)}");
            return true;
        }

        return true;
    }
}
