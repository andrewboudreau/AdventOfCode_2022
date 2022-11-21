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
    }
}
