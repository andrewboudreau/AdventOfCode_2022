using System.Collections;

namespace Day00
{
    public abstract class EnumerableTuples<T> : IEnumerable<T>
    {
        protected readonly List<T> values = new();

        public abstract IEnumerable<T> Add(string row);

        public IEnumerator<T> GetEnumerator()
            => values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();
    }
}
