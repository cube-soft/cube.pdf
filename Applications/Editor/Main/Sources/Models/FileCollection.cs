using Cube.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileCollection
    ///
    /// <summary>
    /// Represents a collection of files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class FileCollection : ObservableBase<FileItem>, IReadOnlyList<FileItem>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileCollection
        ///
        /// <summary>
        /// Initializes a new instance of the FileCollection class.
        /// </summary>
        ///
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileCollection(IDispatcher dispatcher) : base(dispatcher)
        {
            _inner = new ObservableCollection<FileItem>();
            _inner.CollectionChanged += (s, e) => OnCollectionChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => _inner.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// Item(int)
        ///
        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem this[int index] => _inner[index];

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// IndexOf
        ///
        /// <summary>
        /// Gets the index of the specified item.
        /// </summary>
        ///
        /// <param name="src">Source item.</param>
        ///
        /// <returns>Index in the collection.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int IndexOf(FileItem src) => _inner.IndexOf(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        ///
        /// <param name="src">Item to add.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(FileItem src) => _inner.Add(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the specified item.
        /// </summary>
        ///
        /// <param name="src">Item to remove.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(FileItem src) => _inner.Remove(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the item of the specified index to the specified new
        /// index.
        /// </summary>
        ///
        /// <param name="oldindex">Index of the item to move.</param>
        /// <param name="newindex">Target index.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(int oldindex, int newindex) => _inner.Move(oldindex, newindex);

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears all objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear() => _inner.Clear();

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(FileItem) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<FileItem> GetEnumerator() => _inner.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the
        /// ImageCollection and optionally releases the managed
        /// resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion

        #region Fields
        private readonly ObservableCollection<FileItem> _inner;
        #endregion
    }
}
