/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// EnumerableExtension
    ///
    /// <summary>
    /// Provides extended methods of the IEnumerable(T) class.
    /// </summary>
    ///
    /// <remarks>
    /// The code is derived from the following URL:
    /// https://github.com/dotnet/docs/blob/main/samples/snippets/csharp/concepts/linq/how-to-group-results-by-contiguous-keys_1.cs
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    internal static class EnumerableExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ChunkBy
        ///
        /// <summary>
        /// Groups elements into chunks that represent subsequences of
        /// contiguous keys.
        /// </summary>
        ///
        /// <param name="src">Source sequence.</param>
        /// <param name="selector">Key selector.</param>
        ///
        /// <returns>
        /// A collection of elements.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<IGrouping<TKey, TSource>> ChunkBy<TSource, TKey>(
           this IEnumerable<TSource> src,
           Func<TSource, TKey> selector
        ) => ChunkBy(src, selector, EqualityComparer<TKey>.Default);

        /* ----------------------------------------------------------------- */
        ///
        /// ChunkBy
        ///
        /// <summary>
        /// Groups elements into chunks that represent subsequences of
        /// contiguous keys.
        /// </summary>
        ///
        /// <param name="src">Source sequence.</param>
        /// <param name="selector">Key selector.</param>
        /// <param name="comparer">Key comparer.</param>
        ///
        /// <returns>
        /// A collection of elements.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<IGrouping<TKey, TSource>> ChunkBy<TSource, TKey>(
            this IEnumerable<TSource> src,
            Func<TSource, TKey> selector,
            IEqualityComparer<TKey> comparer
        ) {
            var it = src.GetEnumerator();
            if (!it.MoveNext()) yield break;

            Chunk<TKey, TSource> current = null;
            while (true)
            {
                var key = selector(it.Current);
                current = new(key, it, e => comparer.Equals(key, selector(e)));
                yield return current;
                if (current.CopyAllChunkElements()) yield break;
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Chunk
        ///
        /// <summary>
        /// A Chunk is a contiguous group of one or more source elements
        /// that have the same key. A Chunk has a key and a list of
        /// ChunkItem objects, which are copies of the elements in the source
        /// sequence.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private class Chunk<TKey, TSource> : IGrouping<TKey, TSource>
        {
            #region Constructors

            /* ----------------------------------------------------------------- */
            ///
            /// Chunk
            ///
            /// <summary>
            /// Initializes a new instance of the Chunk class with the specified
            /// arguments.
            /// </summary>
            ///
            /// <param name="key">Key of the chunk.</param>
            /// <param name="enumerator">Source enumerator.</param>
            /// <param name="predicate">
            /// Value indicating whether the key of the provided source is
            /// contiguous.
            /// </param>
            ///
            /* ----------------------------------------------------------------- */
            public Chunk(TKey key, IEnumerator<TSource> enumerator, Func<TSource, bool> predicate)
            {
                Key = key;

                _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
                _predicate  = predicate  ?? throw new ArgumentNullException(nameof(predicate));

                _head = new(enumerator.Current);
                _tail = _head;
            }

            #endregion

            #region Properties

            /* ----------------------------------------------------------------- */
            ///
            /// Key
            ///
            /// <summary>
            /// Gets the key of the group.
            /// </summary>
            ///
            /* ----------------------------------------------------------------- */
            public TKey Key { get; }

            #endregion

            #region Methods

            /* ----------------------------------------------------------------- */
            ///
            /// GetEnumerator
            ///
            /// <summary>
            /// Invoked by the inner foreach loop. This method stays just one
            /// step ahead of the client requests. It adds the next element of
            /// the chunk only after the clients requests the last element in the
            /// list so far.
            /// </summary>
            ///
            /// <returns>
            /// Enumerator that can be used to iterate through the collection.
            /// </returns>
            ///
            /* ----------------------------------------------------------------- */
            public IEnumerator<TSource> GetEnumerator()
            {
                ChunkItem current = _head;
                while (current != null)
                {
                    yield return current.Value;
                    lock (_lock)
                    {
                        if (current == _tail) CopyNextChunkElement();
                    }
                    current = current.Next;
                }
            }

            /* --------------------------------------------------------------------- */
            ///
            /// GetEnumerator
            ///
            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            ///
            /// <returns>
            /// IEnumerator object that can be used to iterate through the
            /// collection.
            /// </returns>
            ///
            /* --------------------------------------------------------------------- */
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /* --------------------------------------------------------------------- */
            ///
            /// CopyAllChunkElements
            ///
            /// <summary>
            /// Called after the end of the last chunk was reached. It first checks
            /// whether there are more elements in the source sequence.
            /// </summary>
            ///
            /// <returns>true if enumerator for this chunk was exhausted.</returns>
            ///
            /* --------------------------------------------------------------------- */
            public bool CopyAllChunkElements()
            {
                while (true)
                {
                    lock (_lock)
                    {
                        // If _eos is false, it signals to the outer iterator to continue iterating.
                        if (DoneCopyingChunk()) return _eos;
                        else CopyNextChunkElement();
                    }
                }
            }

            #endregion

            #region Implementations

            /* --------------------------------------------------------------------- */
            ///
            /// DoneCopyingChunk
            ///
            /// <summary>
            /// Indicates that all chunk elements have been copied to the list of
            /// ChunkItems, and the source enumerator is either at the end, or else
            /// on an element with a new key. The tail of the linked list is set to
            /// null in the CopyNextChunkElement method if the key of the next
            /// element does not match the current chunk's key, or there are no more
            /// elements in the source.
            /// </summary>
            ///
            /* --------------------------------------------------------------------- */
            private bool DoneCopyingChunk() => _tail == null;

            /* --------------------------------------------------------------------- */
            ///
            /// CopyNextChunkElement
            ///
            /// <summary>
            /// Adds one ChunkItem to the current group
            /// </summary>
            ///
            /* --------------------------------------------------------------------- */
            private void CopyNextChunkElement()
            {
                _eos = !_enumerator.MoveNext();

                if (_eos || !_predicate(_enumerator.Current))
                {
                    _enumerator = null;
                    _predicate  = null;
                }
                else _tail.Next = new(_enumerator.Current);

                _tail = _tail.Next;
            }

            /* ----------------------------------------------------------------- */
            ///
            /// ChunkItem
            ///
            /// <summary>
            /// A Chunk has a linked list of ChunkItems, which represent the
            /// elements in the current chunk. Each ChunkItem has a reference
            /// to the next ChunkItem in the list.
            /// </summary>
            ///
            /* ----------------------------------------------------------------- */
            class ChunkItem
            {
                public ChunkItem(TSource value) => Value = value;
                public TSource Value { get; }
                public ChunkItem Next { get; set; } = null;
            }

            #endregion

            #region Fields
            private IEnumerator<TSource> _enumerator;
            private Func<TSource, bool> _predicate;
            private readonly ChunkItem _head;
            private ChunkItem _tail;
            private bool _eos = false;
            private readonly object _lock = new();
            #endregion
        }

        #endregion
    }
}
