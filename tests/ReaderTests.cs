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
            Assert.That(() => new Reader<int>((IEnumerator<int>)null!),
                        Throws.ArgumentNullException("e"));
        }

        [Test]
        public void NullEnumerableInitialization()
        {
            Assert.That(() => new Reader<int>((IEnumerable<int>)null!),
                        Throws.ArgumentNullException("e"));
        }

        [Test]
        public void HasMoreWhenEmpty()
        {
            using var reader = new Reader<int>([]);
            Assert.That(reader.HasMore, Is.False);
        }

        [Test]
        public void HasMoreWhenNotEmpty()
        {
            using var reader = new Reader<int>(new int[1]);
            Assert.That(reader.HasMore, Is.True);
        }

        [Test]
        public void ReadEmpty()
        {
            using var reader = new Reader<int>([]);
            Assert.That(reader.Read, Throws.InvalidOperationException);
        }

        [Test]
        public void Unreading()
        {
            using var reader = new Reader<int>([78, 910]);
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
            using var reader = new Reader<int>([]);
            Assert.That(reader.Peek, Throws.InvalidOperationException);
        }

        [Test]
        public void PeekNonEmpty()
        {
            using var reader = new Reader<int>([12, 34, 56]);
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
            using var reader = new Reader<int>([12, 34, 56]);
            using var e = reader.GetEnumerator();
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
            using var reader = new Reader<int>([12, 34, 56]);
            var e = ((IEnumerable)reader).GetEnumerator();
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
            using var e = new TestEnumerator<object>();
            Assert.That(e.Disposed, Is.False);
            new Reader<object>(e).Close();
            Assert.That(e.Disposed, Is.True);
        }

        [Test]
        public void DisposeDisposes()
        {
            using var e = new TestEnumerator<object>();
            Assert.That(e.Disposed, Is.False);
            ((IDisposable)new Reader<object>(e)).Dispose();
            Assert.That(e.Disposed, Is.True);
        }

        [Test]
        public void DisposeDisposesOnce()
        {
            using var e = new TestEnumerator<object>();
            Assert.That(e.Disposed, Is.False);
            IDisposable disposable = new Reader<object>(e);
            disposable.Dispose();
            Assert.That(e.DisposeCallCount, Is.EqualTo(1));
            disposable.Dispose();
            Assert.That(e.DisposeCallCount, Is.EqualTo(1));
        }

        [Test]
        public void HasMoreDisposed()
        {
            Assert.That(() => _ = CreateDisposedReader<int>().HasMore,
                        Throws.ObjectDisposedException(typeof(Reader<>).Name));
        }

        [Test]
        public void ReadDisposed()
        {
            Assert.That(() => _ = CreateDisposedReader<int>().Read(),
                        Throws.ObjectDisposedException(typeof(Reader<>).Name));
        }

        [Test]
        public void UnreadDisposed()
        {
            Assert.That(() => CreateDisposedReader<int>().Unread(42),
                        Throws.ObjectDisposedException(typeof(Reader<>).Name));
        }

        [Test]
        public void PeekDisposed()
        {
            Assert.That(() => CreateDisposedReader<int>().Peek(),
                        Throws.ObjectDisposedException(typeof(Reader<>).Name));
        }

        [Test]
        public void EnumerateDisposed()
        {
            Assert.That(() => CreateDisposedReader<int>().GetEnumerator(),
                        Throws.ObjectDisposedException(typeof(Reader<>).Name));
        }

        static Reader<T> CreateDisposedReader<T>()
        {
            var reader = new Reader<T>([]);
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
            object? IEnumerator.Current => Current;
        }
    }
}
