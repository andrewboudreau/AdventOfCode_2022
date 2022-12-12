namespace Day00
{
    public static class StringParseExtensions
    {
        /// <summary>
        /// Find the first integer in a string.
        /// </summary>
        /// <param name="source">the string to parse, should contain an integer sounded by the split char.</param>
        /// <param name="thenSplitOn">what to split the string upon, defaults to ' ' a space.</param>
        /// <returns>The first parse-able integer in the source.</returns>
        public static int ParseInt(string source, char thenSplitOn = ' ')
        {
            int parsed = 0;
            _ = source
                .Split(thenSplitOn, StringSplitOptions.TrimEntries)
                .First(x => int.TryParse(x, out parsed));

            return parsed;
        }

        /// <summary>
        /// Find the first integer in a string.
        /// </summary>
        /// <param name="source">the string to parse, should contain an integer sounded by the split char.</param>
        /// <param name="skipUntil">Starts parsing integers after the first instance of this char.</param>
        /// <param name="thenSplitOn">what to split the string upon, defaults to ',' a comma.</param>
        /// <returns>The set of parse-able integer in the source.</returns>
        public static IEnumerable<int> ParseInts(string source, char skipUntil = ':', char thenSplitOn = ',')
        {
            return new string(source.SkipWhile(x => x != skipUntil).Skip(2).ToArray())
                .Split(thenSplitOn, StringSplitOptions.TrimEntries)
                .Select(x => int.Parse(x));
        }

        /// <summary>
        /// Finds the set of parts in the string.
        /// </summary>
        /// <param name="source">The string to parse.</param>
        /// <param name="skipUntil">Starts parsing integers after the first instance of this char.</param>
        /// <param name="thenSplitOn">The character to split the string upon.</param>
        /// <returns></returns>
        public static string[] ParseParts(string source, char skipUntil, char thenSplitOn)
        {
            return new string(source.SkipWhile(x => x != skipUntil).Skip(2).ToArray())
                .Split(thenSplitOn, StringSplitOptions.TrimEntries);
        }
    }
}
