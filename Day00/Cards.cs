using System.Text;

namespace Day00
{
    public record Card(Rank Rank, Suit Suit) : IComparable<Card>
    {
        public int CompareTo(Card? other)
        {
            ArgumentNullException.ThrowIfNull(other);

            if (Rank.Value == other.Rank.Value)
                return 0;
            else if (Rank.Value < other.Rank.Value)
                return -1;
            else
                return 1;
        }

        public override string ToString() => $"{Rank.Icon}{Suit.Solid}";
    }

    public static class Cards
    {
        public static IEnumerable<Card> All()
        {
            foreach (var suit in Suits.All())
                foreach (var rank in Ranks.All())
                    yield return new Card(rank, suit);
        }

        public static Deck Shuffle(Deck deck)
        {
            deck.Sort((left, right) => Random.Shared.Next(-1, 2));
            return deck;
        }

        public static Deck FisherYates(Deck deck)
        {
            /*
             * To shuffle an array a of n elements (indices 0..n-1):
             *  for i from n - 1 downto 1 do
             *      j = random integer with 0 <= j <= i
             *          exchange a[j] and a[i]
             */
            for (var i = deck.Count - 1; i > 0; i--)
            {
                var j = Random.Shared.Next(0, i + 1);
                (deck[j], deck[i]) = (deck[i], deck[j]);
            }

            return deck;
        }
    }

    public class Deck : List<Card>
    {
        public Deck(bool empty = false)
            : base(empty ? Enumerable.Empty<Card>() : Cards.All())
        {
        }

        public Deck()
            : this(false)
        {
        }

        public Deck Shuffle()
            => Cards.Shuffle(this);

        public Deck FisherYates()
            => Cards.FisherYates(this);

        public Card Deal(Deck destination)
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The deck is empty.");
            }

            Card top = this[0];
            if (!Remove(top))
            {
                throw new InvalidOperationException($"Deck has cards but remove failed top card {top}.");
            }

            destination.Accept(top, this);
            return top;
        }

        public Card Deal(List<Card>? destination = default)
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The deck is empty.");
            }

            Card top = this[0];
            if (!Remove(top))
            {
                throw new InvalidOperationException($"Deck has cards but remove failed top card {top}.");
            }

            destination?.Add(top);
            return top;
        }

        public void Accept(Card card, Deck from)
        {
            Add(card);
        }

        public override string ToString()
            => Decks.ToString(this);
    }

    public record Rank(string Name, int Value, string Icon)
    {
        static Rank()
        {
            Ace = new();
            Two = new();
            Three = new();
            Four = new();
            Five = new();
            Six = new();
            Seven = new();
            Eight = new();
            Nine = new();
            Ten = new();
            Jack = new();
            Queen = new();
            King = new();
        }

        public static Ace Ace { get; }
        public static Two Two { get; }
        public static Three Three { get; }
        public static Four Four { get; }
        public static Five Five { get; }
        public static Six Six { get; }
        public static Seven Seven { get; }
        public static Eight Eight { get; }
        public static Nine Nine { get; }
        public static Ten Ten { get; }
        public static Jack Jack { get; }
        public static Queen Queen { get; }
        public static King King { get; }

        public override string ToString() => Icon;

        public string ToConstantLengthString() => Icon.Length == 1 ? " " + Icon : Icon;
    }

    public record Ace() : Rank(nameof(Ace), 1, "A");
    public record Two() : Rank(nameof(Two), 2, "2");
    public record Three() : Rank(nameof(Three), 3, "3");
    public record Four() : Rank(nameof(Four), 4, "4");
    public record Five() : Rank(nameof(Five), 5, "5");
    public record Six() : Rank(nameof(Six), 6, "6");
    public record Seven() : Rank(nameof(Seven), 7, "7");
    public record Eight() : Rank(nameof(Eight), 8, "8");
    public record Nine() : Rank(nameof(Nine), 9, "9");
    public record Ten() : Rank(nameof(Ten), 10, "10");
    public record Jack() : Rank(nameof(Jack), 11, "J");
    public record Queen() : Rank(nameof(Queen), 12, "Q");
    public record King() : Rank(nameof(King), 13, "K");

    public static class Ranks
    {
        public static IEnumerable<Rank> All()
        {
            yield return Rank.Ace;
            yield return Rank.Two;
            yield return Rank.Three;
            yield return Rank.Four;
            yield return Rank.Five;
            yield return Rank.Six;
            yield return Rank.Seven;
            yield return Rank.Eight;
            yield return Rank.Nine;
            yield return Rank.Ten;
            yield return Rank.Jack;
            yield return Rank.Queen;
            yield return Rank.King;
        }
    }

    public record Suit(string Name, char Outline, char Solid)
    {
        static Suit()
        {
            Clubs = new();
            Diamonds = new();
            Hearts = new();
            Spades = new();
        }

        public static Club Clubs { get; }
        public static Diamond Diamonds { get; }
        public static Heart Hearts { get; }
        public static Spade Spades { get; }

        public record Heart() : Suit(nameof(Heart), '♡', '♥');

        public record Spade() : Suit(nameof(Spade), '♤', '♠');

        public record Club() : Suit(nameof(Club), '♧', '♣');

        public record Diamond() : Suit(nameof(Diamond), '♢', '♦');
    };

    public static class Suits
    {
        public static IEnumerable<Suit> All()
        {
            yield return Suit.Clubs;
            yield return Suit.Diamonds;
            yield return Suit.Hearts;
            yield return Suit.Spades;
        }
    }

    public static class Decks
    {
        public static string ToString(this List<Card> source)
            => ToString(source.ToArray());

        public static string ToString(this Card[] source, bool disableMultiline = false)
        {
            int size = 13;
            bool multiline = source.Length > size && !disableMultiline;

            var sb = new StringBuilder();
            if (multiline)
            {
                sb.AppendLine();
            }

            foreach (var chunk in source.Chunk(size))
            {
                foreach (var card in chunk)
                {
                    if (multiline && card.Rank.Value != 10)
                    {
                        sb.AppendFormat(" {0}", card);
                    }
                    else
                    {
                        sb.AppendFormat("{0}", card);
                    }

                    if (card != chunk[^1] || disableMultiline)
                    {
                        sb.Append(' ');
                    }
                }

                if (multiline)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }
}