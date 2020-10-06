/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Cube.FileSystem;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for the MainWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MainViewModel : Presentable<MainFacade>
    {
        #region Constructors

        /* --------------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public MainViewModel() : this(SynchronizationContext.Current) { }

        /* --------------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class with the
        /// specified context.
        /// </summary>
        ///
        /// <param name="context">Synchronization context.</param>
        ///
        /* --------------------------------------------------------------------- */
        public MainViewModel(SynchronizationContext context) : base(
            new MainFacade(new IO(), context),
            new Aggregator(),
            context
        ) {
            Files = new BindingSource { DataSource = Facade.Files };

            Facade.Query = new Query<string>(e => Send(new PasswordViewModel(e, context)));
            Facade.CollectionChanged += (s, e) => Send<CollectionMessage>();
            Facade.PropertyChanged   += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Files
        ///
        /// <summary>
        /// Gets the collection of target files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindingSource Files { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO => Facade.Settings.IO;

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets a value indicating whether the class is busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Busy => Facade.Busy;

        #endregion

        #region Methods

        /* --------------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// Invokes the Merge command.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public void Merge() => Send(MessageFactory.CreateForMerge(), e => Facade.Merge(e));

        /* --------------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// Invokes the Split command.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public void Split() => Send(MessageFactory.CreateForSplit(), e => Facade.Split(e, new List<string>()));

        /* --------------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Invokes the Add command.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public void Add() => Send(MessageFactory.CreateForAdd(), e => Add(e));

        /* --------------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Invokes the Add command with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Files to add.</param>
        ///
        /* --------------------------------------------------------------------- */
        public void Add(IEnumerable<string> src) => Track(() => Facade.Add(src), true);

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the specified indices.
        /// </summary>
        ///
        /// <param name="indices">
        /// Collection of indices to be removed.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(IEnumerable<int> indices) => Track(() => Facade.Remove(indices), true);

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears the added files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear() => Track(Facade.Clear, true);

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the specified items by the specified offset.
        /// </summary>
        ///
        /// <param name="indices">Indices of files.</param>
        /// <param name="offset">Offset to move.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(IEnumerable<int> indices, int offset) => Track(() =>
        {
            Facade.Move(indices, offset);
            Send(MessageFactory.CreateForSelect(indices, offset, Facade.Files.Count));
        }, true);

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// Preview the PDF file of the specified indices.
        /// </summary>
        ///
        /// <param name="indices">Indices of files.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Preview(IEnumerable<int> indices)
        {
            if (indices.Count() > 0) Send(new PreviewMessage
            {
                Value = Facade.Files[indices.First()].FullName,
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// About
        ///
        /// <summary>
        /// Shows the version dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void About() => Send(new VersionViewModel(Facade.Settings, Context));

        #endregion
    }
}
