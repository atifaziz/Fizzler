#region Copyright and License
//
// Fizzler - CSS Selector Engine for Microsoft .NET Framework
// Copyright (c) 2009 Atif Aziz, Colin Ramsay. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
// details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
#endregion

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
        [Test]
        public void NullEnumeratorInitialization()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                new Reader<int>((IEnumerator<int>)null));
            Assert.That(e.ParamName, Is.EqualTo("e"));
        }

        [Test]
        public void NullEnumerableInitialization()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                new Reader<int>((IEnumerable<int>)null));
            Assert.That(e.ParamName, Is.EqualTo("e"));
        }

        [Test]
        public void HasMoreWhenEmpty()
        {
            Assert.That(new Reader<int>(new int[0]).HasMore, Is.False);
        }

        [Test]
        public void HasMoreWhenNotEmpty()
        {
            Assert.That(new Reader<int>(new int[1]).HasMore, Is.True);
        }

        [Test]
        public void ReadEmpty()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new Reader<int>(new int[0]).Read());
        }

        [Test]
        public void Unreading()
        {
            var reader = new Reader<int>(new[] { 78, 910 });
            reader.Unread(56);
            reader.Unread(34);
            reader.Unread(12);
            Assert.That(reader.Read(), Is.EqualTo(12));
            Assert.That(reader.Read(), Is.EqualTo(34));
            Assert.That(reader.Read(), Is.EqualTo(56));
            Assert.That(reader.Read(), Is.EqualTo(78));
            Assert.That(reader.Read(), Is.EqualTo(910));
        }

        [Test]
        public void PeekEmpty()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new Reader<int>(new int[0]).Peek());
        }

        [Test]
        public void PeekNonEmpty()
        {
            var reader = new Reader<int>(new[] { 12, 34, 56 });
            Assert.That(reader.Peek(), Is.EqualTo(12));
            Assert.That(reader.Read(), Is.EqualTo(12));
            Assert.That(reader.Peek(), Is.EqualTo(34));
            Assert.That(reader.Read(), Is.EqualTo(34));
            Assert.That(reader.Peek(), Is.EqualTo(56));
            Assert.That(reader.Read(), Is.EqualTo(56));
        }

        [Test]
        public void Enumeration()
        {
            var e = new Reader<int>(new[] { 12, 34, 56 }).GetEnumerator();
            Assert.That(e.MoveNext(), Is.True);
            Assert.That(e.Current, Is.EqualTo(12));
            Assert.That(e.MoveNext(), Is.True);
            Assert.That(e.Current, Is.EqualTo(34));
            Assert.That(e.MoveNext(), Is.True);
            Assert.That(e.Current, Is.EqualTo(56));
            Assert.That(e.MoveNext(), Is.False);
        }

        [Test]
        public void EnumerationNonGeneric()
        {
            var e = ((IEnumerable) new Reader<int>(new[] { 12, 34, 56 })).GetEnumerator();
            Assert.That(e.MoveNext(), Is.True);
            Assert.That(e.Current, Is.EqualTo(12));
            Assert.That(e.MoveNext(), Is.True);
            Assert.That(e.Current, Is.EqualTo(34));
            Assert.That(e.MoveNext(), Is.True);
            Assert.That(e.Current, Is.EqualTo(56));
            Assert.That(e.MoveNext(), Is.False);
        }

        [Test]
        public void CloseDisposes()
        {
            var e = new TestEnumerator<object>();
            Assert.That(e.Disposed, Is.False);
            new Reader<object>(e).Close();
            Assert.That(e.Disposed, Is.True);
        }

        [Test]
        public void DisposeDisposes()
        {
            var e = new TestEnumerator<object>();
            Assert.That(e.Disposed, Is.False);
            ((IDisposable) new Reader<object>(e)).Dispose();
            Assert.That(e.Disposed, Is.True);
        }

        [Test]
        public void DisposeDisposesOnce()
        {
            var e = new TestEnumerator<object>();
            Assert.That(e.Disposed, Is.False);
            var disposable = ((IDisposable)new Reader<object>(e));
            disposable.Dispose();
            Assert.That(e.DisposeCallCount, Is.EqualTo(1));
            disposable.Dispose();
            Assert.That(e.DisposeCallCount, Is.EqualTo(1));
        }

        [Test]
        public void HasMoreDisposed()
        {
            var e = Assert.Throws<ObjectDisposedException>(() =>
            {
                var unused = CreateDisposedReader<int>().HasMore;
            });
            Assert.That(e.ObjectName, Is.EqualTo(typeof(Reader<>).Name));
        }

        [Test]
        public void ReadDisposed()
        {
            var e = Assert.Throws<ObjectDisposedException>(() =>
                CreateDisposedReader<int>().Read());
            Assert.That(e.ObjectName, Is.EqualTo(typeof(Reader<>).Name));
        }

        [Test]
        public void UnreadDisposed()
        {
            var e = Assert.Throws<ObjectDisposedException>(() =>
                CreateDisposedReader<int>().Unread(42));
            Assert.That(e.ObjectName, Is.EqualTo(typeof(Reader<>).Name));
        }

        [Test]
        public void PeekDisposed()
        {
            var e = Assert.Throws<ObjectDisposedException>(() =>
                CreateDisposedReader<int>().Peek());
            Assert.That(e.ObjectName, Is.EqualTo(typeof(Reader<>).Name));
        }

        [Test]
        public void EnumerateDisposed()
        {
            var e = Assert.Throws<ObjectDisposedException>(() =>
                CreateDisposedReader<int>().GetEnumerator());
            Assert.That(e.ObjectName, Is.EqualTo(typeof(Reader<>).Name));
        }

        static Reader<T> CreateDisposedReader<T>()
        {
            var reader = new Reader<T>(new T[0]);
            reader.Close();
            return reader;
        }

        sealed class TestEnumerator<T> : IEnumerator<T>
        {
            public int DisposeCallCount { get; private set; }
            public bool Disposed => DisposeCallCount > 0;

            public void Dispose() => DisposeCallCount++;
            public bool MoveNext() => false;
            public void Reset() => throw new NotImplementedException();
            public T Current => throw new NotImplementedException();
            object IEnumerator.Current => Current;
        }
    }
}
