using Day00;

using System.Diagnostics;
using System.Linq.Expressions;
using System.Numerics;
using System.Threading.Channels;

using static Day00.ReadInputs;
using static Day00.StringParseExtensions;

internal class ProgramChannels
{
    private static async Task Main(string[] args)
    {
        const int decayDivisor_partOne = 3;
        const int decayDivisor_partTwo = 1;
        BigInteger decayDivisor = decayDivisor_partTwo;

        BigInteger modu = ReadRecords(rows => BigInteger.Parse(ParseInt(rows[3]).ToString()))
            .Aggregate((BigInteger)1, (acc, x) => acc *= x);

        var myChannel = Channel.CreateUnbounded<int>();
        Console.WriteLine("Go");
        Stopwatch stopwatch = Stopwatch.StartNew();
        long previous = 0;

        _ = Task.Factory.StartNew(async () =>
        {
            var writer = myChannel.Writer;
            foreach (var i in Enumerable.Range(0, 100_000_000))
            {
                await writer.WriteAsync(i);
            }

            myChannel.Writer.Complete();
        });

        await foreach (var item in myChannel.Reader.ReadAllAsync())
        {
            if (item % 10_000_000 == 0)
            {
                Console.WriteLine($"{item:#,###}");
                Console.WriteLine($"Took {stopwatch.ElapsedMilliseconds - previous:#,###}ms");
                previous = stopwatch.ElapsedMilliseconds;
            }
        }

        await foreach (var item in myChannel.Reader.ReadAllAsync())
        {
            if (item % 10_000_000 == 0)
            {
                Console.WriteLine($"{item:#,###}");
                Console.WriteLine($"Took {stopwatch.ElapsedMilliseconds - previous:#,###}ms");
                previous = stopwatch.ElapsedMilliseconds;
            }
        }
    }
}

//var monkeys = ReadRecords(rows =>
//{
//    // parse 'Starting Items'
//    var items = ParseInts(rows[1], skipUntil: ':', thenSplitOn: ',').Select(x => (BigInteger)x);

//    // parse 'Operation'
//    var ops = ParseParts(rows[2], skipUntil: '=', thenSplitOn: ' ');
//    modu *= BigInteger.Parse(ParseInt(rows[3]).ToString());

//    ParameterExpression p1 = Expression.Parameter(typeof(BigInteger));
//    Expression p2 = BigInteger.TryParse(ops[2], out var constant) ?
//        Expression.Constant(constant, typeof(BigInteger)) :
//        p1;

//    var operation = ops[1] == "*" ?
//        Expression.Lambda<Func<BigInteger, BigInteger>>(
//            Expression.Modulo(
//                Expression.Multiply(p1, p2),
//                Expression.Constant(modu, typeof(BigInteger))), p1) :

//        Expression.Lambda<Func<BigInteger, BigInteger>>(
//            Expression.Modulo(
//                Expression.Add(p1, p2),
//                Expression.Constant(modu, typeof(BigInteger))), p1);

//    // parse 'Test'
//    Func<BigInteger, int> passTo =
//        input =>
//            input % ParseInt(rows[3]) == 0 ?
//            ParseInt(rows[4]) :
//            ParseInt(rows[5]);

//    return new Monkey<BigInteger>(items, operation.Compile(), passTo);
//}).ToList();

//for (var round = 0; round < 10_000; round++)
//{
//    foreach (var (Monkey, Index) in monkeys.Select((monkey, index) => (monkey, index)))
//    {
//        Monkey.ProcessItems(monkeys);
//    }

//    //foreach (var (Monkey, Index) in monkeys.Select((monkey, index) => (monkey, index)))
//    //{
//    //    Console.WriteLine($"After Round {round}, the monkeys are holding");
//    //    Console.WriteLine($"Monkey {Index}: {string.Join(", ", Monkey.Items)}");
//    //}

//    if ((round + 1) % 1000 == 0)
//    {
//        var topInspectors = monkeys.Select(x => x.Inspections).OrderByDescending(x => x).Take(4);
//        Console.WriteLine($"Inspection Counts {string.Join(",", topInspectors)}");
//        Console.WriteLine($"Product of the top two inspectors is {(long)topInspectors.First() * topInspectors.Second()}");
//        Console.WriteLine();
//    }
//}

//public class Monkey<T>
//{
//    private readonly Queue<T> items;
//    private readonly Func<T, T> operation;
//    private readonly Func<T, int> passTo;
//    private int inspections = 0;

//    public Monkey(IEnumerable<T> items, Func<T, T> operation, Func<T, int> passTo)
//    {
//        this.items = new Queue<T>(items);
//        this.operation = operation;
//        this.passTo = passTo;
//    }

//    public IEnumerable<T> Items => items;

//    public int Inspections => inspections;

//    public void ProcessItems(List<Monkey<T>> monkeys)
//    {
//        while (items.Count > 0)
//        {
//            inspections++;

//            var item = items.Dequeue();
//            item = operation(item);
//            monkeys[passTo(item)].Receive(item);
//        }
//    }

//    public void Receive(T item)
//        => items.Enqueue(item);
//}