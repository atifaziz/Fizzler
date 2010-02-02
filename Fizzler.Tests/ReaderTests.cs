namespace Fizzler.Tests
{
    #region Imports

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class ReaderTests
    {
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void NullEnumeratorInitialization()
        {
            new Reader<int>((IEnumerator<int>)null);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void NullEnumerableInitialization()
        {
            new Reader<int>((IEnumerable<int>)null);
        }

        [Test]
        public void HasMoreWhenEmpty()
        {
            Assert.IsFalse(new Reader<int>(new int[0]).HasMore);
        }

        [Test]
        public void HasMoreWhenNotEmpty()
        {
            Assert.IsTrue(new Reader<int>(new int[1]).HasMore);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void ReadEmpty()
        {
            new Reader<int>(new int[0]).Read();
        }

        [Test]
        public void Unreading()
        {
            var reader = new Reader<int>(new[] { 78, 910 });
            reader.Unread(56);
            reader.Unread(34);
            reader.Unread(12);
            Assert.AreEqual(12, reader.Read());
            Assert.AreEqual(34, reader.Read());
            Assert.AreEqual(56, reader.Read());
            Assert.AreEqual(78, reader.Read());
            Assert.AreEqual(910, reader.Read());
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void PeekEmpty()
        {
            new Reader<int>(new int[0]).Peek();
        }

        [Test]
        public void PeekNonEmpty()
        {
            var reader = new Reader<int>(new[] { 12, 34, 56 });
            Assert.AreEqual(12, reader.Peek());
            Assert.AreEqual(12, reader.Read());
            Assert.AreEqual(34, reader.Peek());
            Assert.AreEqual(34, reader.Read());
            Assert.AreEqual(56, reader.Peek());
            Assert.AreEqual(56, reader.Read());
        }

        [Test]
        public void Enumeration()
        {
            var e = new Reader<int>(new[] { 12, 34, 56 }).GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(12, e.Current);
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(34, e.Current);
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(56, e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        [Test]
        public void EnumerationNonGeneric()
        {
            var e = ((IEnumerable) new Reader<int>(new[] { 12, 34, 56 })).GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(12, e.Current);
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(34, e.Current);
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(56, e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        [Test]
        public void CloseDisposes()
        {
            var e = new TestEnumerator<object>();
            Assert.IsFalse(e.Disposed);
            new Reader<object>(e).Close();
            Assert.IsTrue(e.Disposed);
        }

        [Test]
        public void DisposeDisposes()
        {
            var e = new TestEnumerator<object>();
            Assert.IsFalse(e.Disposed);
            ((IDisposable) new Reader<object>(e)).Dispose();
            Assert.IsTrue(e.Disposed);
        }

        [Test]
        public void DisposeDisposesOnce()
        {
            var e = new TestEnumerator<object>();
            Assert.IsFalse(e.Disposed);
            var disposable = ((IDisposable)new Reader<object>(e));
            disposable.Dispose();
            Assert.AreEqual(1, e.DisposeCallCount);
            disposable.Dispose();
            Assert.AreEqual(1, e.DisposeCallCount);
        }

        [Test, ExpectedException(typeof(ObjectDisposedException))]
        public void HasMoreDisposed()
        {
            var unused = CreateDisposedReader<int>().HasMore;
        }

        [Test, ExpectedException(typeof(ObjectDisposedException))]
        public void ReadDisposed()
        {
            CreateDisposedReader<int>().Read();
        }

        [Test, ExpectedException(typeof(ObjectDisposedException))]
        public void UnreadDisposed()
        {
            CreateDisposedReader<int>().Unread(42);
        }

        [Test, ExpectedException(typeof(ObjectDisposedException))]
        public void PeekDisposed()
        {
            CreateDisposedReader<int>().Peek();
        }

        [Test, ExpectedException(typeof(ObjectDisposedException))]
        public void EnumerateDisposed()
        {
            CreateDisposedReader<int>().GetEnumerator();
        }

        private static Reader<T> CreateDisposedReader<T>()
        {
            var reader = new Reader<T>(new T[0]);
            reader.Close();
            return reader;
        }

        private sealed class TestEnumerator<T> : IEnumerator<T>
        {
            public int DisposeCallCount { get; set; }            
            public bool Disposed { get { return DisposeCallCount > 0; } }

            public void Dispose()
            {
                DisposeCallCount++;
            }

            public bool MoveNext()
            {
                return false;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public T Current
            {
                get { throw new NotImplementedException(); }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}