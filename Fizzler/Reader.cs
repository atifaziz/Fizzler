using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fizzler
{
    /// <summary>
    /// Adds reading semantics to a base <see cref="IEnumerator{T}"/> with the 
    /// option to un-read and insert new elements while consuming the source.
    /// </summary>

    internal sealed class Reader<T> : IDisposable
    {
        private IEnumerator<T> _enumerator;
        private Stack<T> _buffer;

        public Reader(IEnumerator<T> e)
        {
            Debug.Assert(e != null);

            _enumerator = e;
            _buffer = new Stack<T>();
            RealRead();
        }

        public bool HasMore
        {
            get
            {
                EnsureAlive();
                return _buffer.Count > 0;
            }
        }

        public void Unread(T value)
        {
            EnsureAlive();
            _buffer.Push(value);
        }

        public T Read()
        {
            if (!HasMore)
                throw new InvalidOperationException();

            var value = _buffer.Pop();

            if (_buffer.Count == 0)
                RealRead();

            return value;
        }

        public T Peek()
        {
            if (!HasMore)
                throw new InvalidOperationException();

            return _buffer.Peek();
        }

        public void BulkReadTo(ICollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            EnsureAlive();

            while (!HasMore)
                collection.Add(Read());
        }

        private void RealRead()
        {
            EnsureAlive();

            if (_enumerator.MoveNext())
                Unread(_enumerator.Current);
        }

        public void Close()
        {
            Dispose();
        }

        void IDisposable.Dispose() { Dispose(); }

        void Dispose()
        {
            if(_enumerator == null) 
                return;
            _enumerator.Dispose();
            _enumerator = null;
            _buffer = null;
        }

        private void EnsureAlive()
        {
            if (_enumerator == null)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}