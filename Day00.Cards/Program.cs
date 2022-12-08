using Day00;
using static Day00.ReadInputs;


Read(x => x).ToConsole("Hello, Day 05");

var deck = new Deck();
deck.FisherYates().ToConsole();


int games = 0;
while (games++ < 100)
{
    var game = new OneHandedSolitare(new Deck().FisherYates());
    while (game.Step())
    {
        if (game.Win)
        {
            game.SaveDeck("win");
        }
    };
}

public class OneHandedSolitare
{
    private readonly string runId = Guid.NewGuid().ToString("N")[..5];
    private int save = 0;

    private readonly Deck deck;
    private readonly Deck hand;

    public OneHandedSolitare(Deck? deck = default)
    {
        this.deck = deck ?? new Deck();
        hand = new Deck(empty: true);
        SaveDeck();
    }
    public IEnumerable<Card> Hand => hand;

    public bool Step(int repeat)
    {
        for (int i = 0; i < repeat; i++)
            if (!Step())
                return false;

        return false;
    }

    public bool Step()
    {
        while (hand.Count < 4 && deck.Any())
        {
            var card = deck.Deal(hand);
            //Console.WriteLine($"dealt {card} to player");
        }

        if (hand.Count < 4)
        {
            return false;
        }

        Console.WriteLine($"Hand has {Decks.ToString(hand.ToArray()[..^4], true)}|{Decks.ToString(hand.ToArray()[^4..])}|");
        if (hand.Count >= 4 &&
            hand[^1].Rank == hand[^2].Rank &&
            hand[^2].Rank == hand[^3].Rank &&
            hand[^3].Rank == hand[^4].Rank)
        {
            //Console.WriteLine($"Found 4 Ranks in a row between {Decks.ToString(hand.ToArray()[^4..])}");
            hand.RemoveRange(hand.Count - 4, 4);
        }
        else if (hand.Count >= 4 &&
            hand[^1].Suit == hand[^2].Suit &&
            hand[^2].Suit == hand[^3].Suit &&
            hand[^3].Suit == hand[^4].Suit)
        {
            //Console.WriteLine($"Found 4 Suits in a row between {Decks.ToString(hand.ToArray()[^4..])}");
            hand.RemoveRange(hand.Count - 4, 4);
        }
        else if (hand[^1].Rank == hand[^4].Rank || hand[^1].Suit == hand[^4].Suit)
        {
            //Console.WriteLine($"Found bookend match, removing {Decks.ToString(hand.ToArray()[^3..^1])}");
            hand.RemoveRange(hand.Count - 3, 2);
        }
        else
        {
            if (deck.Count == 0)
            {
                return false;
            }

            _ = deck.Deal(hand);
            //Console.WriteLine($"Added Card {card}");
        }

        return true;
    }

    public bool Win => hand.Count == 0 && deck.Count == 0;

    public void SaveHand()
        => Save(hand, "hand");

    public void SaveDeck(string? name = default)
        => Save(deck, name ?? "deck");

    public void Save<T>(T data, string filename)
        => File.WriteAllText($"{filename}-{save++}-{runId}.txt", data!.ToString());
}
