using System.Collections;

namespace Day00
{
    public abstract class EnumerableTuples<T> : IEnumerable<T>
    {
        protected readonly List<T> values = new();

        public virtual void Add(string row)
        {
            values.Add(Parse(row));
        }

        public abstract T Parse(string row);

        public IEnumerator<T> GetEnumerator()
            => values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();
    }
}
