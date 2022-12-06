using Day00;

using static Day00.ReadInputs;


Read(x => x).ToConsole("Hello, Day 05");

var deck = new Deck();
deck.Shuffle().ToConsole();

var game = new SolitarePop();
while (game.Step())
{
    Console.WriteLine();
};

public class SolitarePop
{
    private readonly Deck deck;
    private readonly List<Card> hand;

    public SolitarePop()
    {
        deck = new Deck();
        hand = new List<Card>();
    }

    public bool Step()
    {
        try
        {
            for (var deal = hand.Count; deal < 4; deal++)
            {
                var card = deck.Deal(hand);
                Console.WriteLine($"dealt {card} to player");
            }

            Console.WriteLine($"Hand has {Decks.ToString(hand.ToArray())}");
            if (hand[^1].Rank == hand[^4].Rank || hand[^1].Suit == hand[^4].Suit)
            {
                Console.WriteLine($"Found match between {hand[^1]} and {hand[^4]}");

                Console.WriteLine($"Removed {Decks.ToString(hand.ToArray()[^3..^1])}");
                hand.RemoveRange(hand.Count - 3, 2);
                Console.WriteLine($"Hand has {Decks.ToString(hand.ToArray())}, deck has {deck.Count} cards left.");
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
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
}
