namespace Day00
{
    public static class ReadInputs
    {
        public static IEnumerable<int> AsInts(string source)
            => source.Select(c => (int)char.GetNumericValue(c));

        public static IEnumerable<IEnumerable<int>> ReadAsRowsOfInts()
        => Read(AsInts).Select(x => x);

        public static IEnumerable<T> Read<T>(Func<string, T> factory) =>
            Read()
                .TakeWhile(x => !string.IsNullOrEmpty(x))
                .Select(x => factory(x!));

        public static IEnumerable<int> ReadInts() =>
            Read(x => int.Parse(x));

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
        }

        public static T ReadTo<T>(Func<IEnumerable<string?>, T> factory)
            => factory(Read());
    }
}