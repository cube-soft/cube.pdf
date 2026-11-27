/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cube.Collections;
using Cube.Tasks.Extensions;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// CacheCollection(TKey, TValue)
    ///
    /// <summary>
    /// Provides a cache manager of TValue objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class CacheCollection<TKey, TValue> : EnumerableBase<KeyValuePair<TKey, TValue>>,
        IReadOnlyCollection<KeyValuePair<TKey, TValue>>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CacheCollection
        ///
        /// <summary>
        /// Initializes a new instance of the CacheCollection class with
        /// the specified creating action.
        /// </summary>
        ///
        /// <param name="creator">Action that creates an item.</param>
        ///
        /// <remarks>
        /// The creator is executed as an asynchronous operation.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public CacheCollection(Func<TKey, TValue> creator) : this(creator, null) { }

        /* ----------------------------------------------------------------- */
        ///
        /// CacheCollection
        ///
        /// <summary>
        /// Initializes a new instance of the CacheCollection class with
        /// the specified parameters.
        /// </summary>
        ///
        /// <param name="creator">Action that creates an item.</param>
        /// <param name="disposer">
        /// Action that releases a removed value or a value created after
        /// its request was canceled.
        /// </param>
        ///
        /// <remarks>
        /// The creator is executed as an asynchronous operation.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public CacheCollection(Func<TKey, TValue> creator, Action<TKey, TValue> disposer)
        {
            if (creator == null) throw new ArgumentNullException(nameof(creator));
            _factory  = key => () => creator(key);
            _disposer = disposer;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CacheCollection
        ///
        /// <summary>
        /// Initializes a cache that prepares work on the requesting thread.
        /// </summary>
        ///
        /// <param name="factory">
        /// Function that returns the background operation for a key.
        /// </param>
        /// <param name="disposer">
        /// Action that releases a removed or canceled value.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        internal CacheCollection(Func<TKey, Func<TValue>> factory, Action<TKey, TValue> disposer)
        {
            _factory  = factory ?? throw new ArgumentNullException(nameof(factory));
            _disposer = disposer;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of created items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => _created.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// Keys
        ///
        /// <summary>
        /// Gets a snapshot of the keys for both cached and pending items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        internal TKey[] Keys
        {
            get { lock (_pending) return [.. _created.Keys, .. _pending.Keys]; }
        }

        #endregion

        #region Events

        #region Created

        /* ----------------------------------------------------------------- */
        ///
        /// Created
        ///
        /// <summary>
        /// Occurs when the creating request is complete.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event KeyValueEventHandler<TKey, TValue> Created;

        /* ----------------------------------------------------------------- */
        ///
        /// OnCreated
        ///
        /// <summary>
        /// Raises the Created event with the provided arguments.
        /// </summary>
        ///
        /// <param name="e">Arguments of the event being raised.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnCreated(KeyValueEventArgs<TKey, TValue> e) =>
            Created?.Invoke(this, e);

        #endregion

        #region Failed

        /* ----------------------------------------------------------------- */
        ///
        /// Failed
        ///
        /// <summary>
        /// Occurs when the creating request is failed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event KeyValueEventHandler<TKey, Exception> Failed;

        /* ----------------------------------------------------------------- */
        ///
        /// OnFailed
        ///
        /// <summary>
        /// Raises the Failed event with the provided arguments.
        /// </summary>
        ///
        /// <param name="e">Arguments of the event being raised.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnFailed(KeyValueEventArgs<TKey, Exception> e) =>
            Failed?.Invoke(this, e);

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetOrCreate
        ///
        /// <summary>
        /// Gets the item associated with the specified TKey object, or
        /// creates it as an asynchronous operation.
        /// </summary>
        ///
        /// <param name="src">Key value.</param>
        ///
        /// <returns>
        /// default value of the type if the item is under creating.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public TValue GetOrCreate(TKey src)
        {
            object ticket;
            lock (_pending)
            {
                if (_created.TryGetValue(src, out var dest)) return dest;
                if (Disposed || _pending.ContainsKey(src)) return default;

                ticket = new object();
                _pending.Add(src, ticket);
            }

            try
            {
                var creator = _factory(src);
                TaskEx.Run(() => Create(src, ticket, creator)).Forget();
            }
            catch (Exception err) { Fail(src, ticket, err); }
            return default;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TryGetValue
        ///
        /// <summary>
        /// Attempts to get the item associated with the specified key
        /// from the inner cache collection.
        /// </summary>
        ///
        /// <param name="src">Key value.</param>
        /// <param name="dest">
        /// When this method returns, contains the object from the
        /// inner collection that has the specified key, or the
        /// default value of the type if the operation failed.
        /// </param>
        ///
        /// <returns>
        /// true if the key was found in the inner collection;
        /// otherwise, false.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool TryGetValue(TKey src, out TValue dest) =>
            _created.TryGetValue(src, out dest);

        /* ----------------------------------------------------------------- */
        ///
        /// Contains
        ///
        /// <summary>
        /// Gets a value that determines the item associated with the
        /// specified key exists in the inner cache collection.
        /// </summary>
        ///
        /// <param name="src">Key value.</param>
        ///
        /// <returns>
        /// true if a value has been cached for the key.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool Contains(TKey src) => _created.ContainsKey(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the item associated with the specified key.
        /// </summary>
        ///
        /// <param name="src">Key value.</param>
        ///
        /// <returns>
        /// true if the item was removed successfully; otherwise, false.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool Remove(TKey src)
        {
            TValue obj;
            lock (_pending)
            {
                _pending.Remove(src);
                if (!_created.TryRemove(src, out obj)) return false;
            }
            _disposer?.Invoke(src, obj);
            return true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Removes all of the created items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear()
        {
            KeyValuePair<TKey, TValue>[] items;
            lock (_pending)
            {
                _pending.Clear();
                items = _disposer != null ? _created.ToArray() : null;
                _created.Clear();
            }
            if (items == null) return;
            foreach (var kv in items) _disposer(kv.Key, kv.Value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(KeyValuePair(TKey, TValue)) object for this
        /// collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
            _created.GetEnumerator();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (disposing) Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Runs a pending request and notifies listeners when its value is cached.
        /// </summary>
        ///
        /// <param name="src">Cache key.</param>
        /// <param name="ticket">Identity of the request.</param>
        /// <param name="creator">Operation that creates the value.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void Create(TKey src, object ticket, Func<TValue> creator)
        {
            TValue dest;
            try
            {
                lock (_pending)
                {
                    if (!IsPending(src, ticket)) return;
                }
                dest = creator();
            }
            catch (Exception err)
            {
                Fail(src, ticket, err);
                return;
            }

            try
            {
                if (TryStore(src, ticket, dest)) OnCreated(KeyValueEventArgs.Create(src, dest));
                else _disposer?.Invoke(src, dest);
            }
            catch (Exception err) { OnFailed(KeyValueEventArgs.Create(src, err)); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TryStore
        ///
        /// <summary>
        /// Stores the result if the request still owns the key.
        /// </summary>
        ///
        /// <param name="src">Cache key.</param>
        /// <param name="ticket">Identity of the request.</param>
        /// <param name="value">Value created by the request.</param>
        ///
        /// <returns>true if the value was stored.</returns>
        ///
        /* ----------------------------------------------------------------- */
        private bool TryStore(TKey src, object ticket, TValue value)
        {
            lock (_pending)
            {
                if (!IsPending(src, ticket)) return false;
                _pending.Remove(src);
                _created[src] = value;
                return true;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Fail
        ///
        /// <summary>
        /// Removes a failed request and reports its error if it is still pending.
        /// </summary>
        ///
        /// <param name="src">Cache key.</param>
        /// <param name="ticket">Identity of the request.</param>
        /// <param name="error">Error raised while preparing or creating the value.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void Fail(TKey src, object ticket, Exception error)
        {
            lock (_pending)
            {
                if (!IsPending(src, ticket)) return;
                _pending.Remove(src);
            }
            OnFailed(KeyValueEventArgs.Create(src, error));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsPending
        ///
        /// <summary>
        /// Determines whether the key still belongs to the specified request.
        /// </summary>
        ///
        /// <param name="src">Cache key.</param>
        /// <param name="ticket">Identity of the request.</param>
        ///
        /// <returns>true if the request is still pending.</returns>
        ///
        /// <remarks>
        /// The caller must hold the lock on the pending requests.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsPending(TKey src, object ticket) =>
            _pending.TryGetValue(src, out var current) && current == ticket;

        #endregion

        #region Fields
        private readonly Func<TKey, Func<TValue>> _factory;
        private readonly Action<TKey, TValue> _disposer;
        private readonly ConcurrentDictionary<TKey, TValue> _created = new();
        private readonly Dictionary<TKey, object> _pending = [];
        #endregion
    }
}
