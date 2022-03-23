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
using Cube.Mixin.Tasks;

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
        /// Action that executes when either the Remove or Clear method
        /// is called.
        /// </param>
        ///
        /// <remarks>
        /// The creator is executed as an asynchronous operation.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public CacheCollection(Func<TKey, TValue> creator, Action<TKey, TValue> disposer)
        {
            _disposer = disposer;
            _creator  = creator ?? throw new ArgumentNullException(nameof(creator));
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
            if (_created.TryGetValue(src, out var dest)) return dest;
            TaskEx.Run(() => Create(src)).Forget();
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
        /// default(TValue) if the item is under creating.
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
            _ = _creating.TryRemove(src, out _);
            var dest = _created.TryRemove(src, out var obj);
            if (dest) _disposer?.Invoke(src, obj);
            return dest;
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
            _creating.Clear();
            if (_disposer != null)
            {
                foreach (var kv in _created) _disposer(kv.Key, kv.Value);
            }
            _created.Clear();
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
            if (!disposing) return;
            _creating.Clear();
            if (_disposer != null)
            {
                foreach (var kv in _created) _disposer(kv.Key, kv.Value);
            }
            _created.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates the item associated with the specified key.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Create(TKey src)
        {
            if (_created.ContainsKey(src) || !_creating.TryAdd(src, 0)) return;

            try
            {
                var dest = _creator(src);
                if (_creating.ContainsKey(src) && _created.TryAdd(src, dest))
                {
                    OnCreated(KeyValueEventArgs.Create(src, dest));
                }
                else _disposer?.Invoke(src, dest);
            }
            catch (Exception err) { OnFailed(KeyValueEventArgs.Create(src, err)); }
            finally { _= _creating.TryRemove(src, out _); }
        }

        #endregion

        #region Fields
        private readonly Func<TKey, TValue> _creator;
        private readonly Action<TKey, TValue> _disposer;
        private readonly ConcurrentDictionary<TKey, TValue> _created = new();
        private readonly ConcurrentDictionary<TKey, byte> _creating = new();
        #endregion
    }
}
