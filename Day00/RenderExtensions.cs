namespace Day00
{
    public static class RenderExtensions
    {
        public static void ToConsole<T>(this IEnumerable<T> source, Func<IEnumerable<T>, IEnumerable<string>> renderer)
        {
            foreach (var output in renderer(source))
            {
                Console.WriteLine(output);
            }
        }

        public static void ToConsole<T>(this IEnumerable<T> source, Func<IEnumerable<T>, string> renderer)
            => Console.WriteLine(renderer(source));

        public static void ToConsole<T>(this T source, Func<T, string> renderer)
            => Console.WriteLine(renderer(source));

        public static void ToConsole<T>(this T source)
            => ToConsole(source, x => x?.ToString() ?? string.Empty);

        public static void ToConsole<T>(this T source, string title)
            => ToConsole(source, x => $"{title}\r\n{x?.ToString() ?? string.Empty}\r\n");
    }
}
