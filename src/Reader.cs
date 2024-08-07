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

namespace Fizzler
{
    #region Imports

    using System;
    using System.Collections;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Adds reading semantics to a base <see cref="IEnumerator{T}"/> with the
    /// option to un-read and insert new elements while consuming the source.
    /// </summary>
    public sealed class Reader<T> : IDisposable, IEnumerable<T>
    {
        IEnumerator<T>? enumerator;
        Stack<T>? buffer;

        /// <summary>
        /// Initialize a new <see cref="Reader{T}"/> with a base
        /// <see cref="IEnumerable{T}"/> object.
        /// </summary>
        public Reader(IEnumerable<T> e) :
            this(e is { } some ? some.GetEnumerator() : throw new ArgumentNullException(nameof(e))) { }

        /// <summary>
        /// Initialize a new <see cref="Reader{T}"/> with a base
        /// <see cref="IEnumerator{T}"/> object.
        /// </summary>
        public Reader(IEnumerator<T> e)
        {
            this.enumerator = e ?? throw new ArgumentNullException(nameof(e));
            this.buffer = new Stack<T>();
            RealRead();
        }

        ObjectDisposedException ObjectDisposedException() => new(GetType().Name);

        /// <summary>
        /// Indicates whether there is, at least, one value waiting to be read or not.
        /// </summary>
        public bool HasMore => this.buffer is { Count: var count }
                             ? count > 0
                             : throw ObjectDisposedException();

        /// <summary>
        /// Pushes back a new value that will be returned on the next read.
        /// </summary>
        public void Unread(T value)
        {
            if (this.buffer is not { } someBuffer)
                throw ObjectDisposedException();
            someBuffer.Push(value);
        }

        /// <summary>
        /// Reads and returns the next value.
        /// </summary>
        public T Read()
        {
            switch (this.buffer)
            {
                case null:
                    throw ObjectDisposedException();
                case { Count: 0 }:
                    throw new InvalidOperationException();
                case var someBuffer:
                    var value = someBuffer.Pop();
                    if (someBuffer.Count == 0)
                        RealRead();
                    return value;
            }
        }

        /// <summary>
        /// Peeks the next value waiting to be read.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if there is no value waiting to be read.
        /// </exception>
        public T Peek() => this.buffer switch
        {
            null => throw ObjectDisposedException(),
            { Count: 0 } => throw new InvalidOperationException(),
            var buffer => buffer.Peek()
        };

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the remaining
        /// values to be read.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return this.enumerator is not null ? Iterator(this) : throw ObjectDisposedException();

            static IEnumerator<T> Iterator(Reader<T> reader)
            {
                while (reader.HasMore)
                    yield return reader.Read();
            }
        }

        void RealRead()
        {
            if (this.enumerator is not { } someEnumerator)
                throw ObjectDisposedException();

            if (someEnumerator.MoveNext())
                Unread(someEnumerator.Current);
        }

        /// <summary>
        /// Disposes the enumerator used to initialize this object
        /// if that enumerator supports <see cref="IDisposable"/>.
        /// </summary>
        public void Close() => Dispose();

#pragma warning disable CA1063 // Implement IDisposable correctly (false negative)
        void IDisposable.Dispose() => Dispose();
#pragma warning restore CA1063 // Implement IDisposable correctly

        void Dispose()
        {
            if(this.enumerator == null)
                return;
            this.enumerator.Dispose();
            this.enumerator = null;
            this.buffer = null;
        }
    }
}
