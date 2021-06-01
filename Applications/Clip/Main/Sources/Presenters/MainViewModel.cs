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
using Cube.FileSystem;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Cube.Pdf.Clip
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

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel() : this(SynchronizationContext.Current) { }

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SynchronizationContext context) : base(
            new MainFacade(new IO(), context),
            new Aggregator(),
            context)
        {
            Clips = new BindingSource { DataSource = Facade.Clips };

            Facade.CollectionChanged += (s, e) => Send<CollectionMessage>();
            Facade.PropertyChanged   += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the path of the source file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source => Facade.Source;

        /* ----------------------------------------------------------------- */
        ///
        /// Clips
        ///
        /// <summary>
        /// Gets the collection of attached files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindingSource Clips { get; }

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

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Opens a PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Open() => Track(e => Facade.Open(e.First()), MessageFactory.CreateForOpen());

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// Attaches files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach() => Track(Facade.Attach, MessageFactory.CreateForAttach());

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// Detaches the selected files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Detach(IEnumerable<int> indices) => Track(() => Facade.Detach(indices));

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Overwrites the provided PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save() => Track(Facade.Save);

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets to the state when the provided PDF was loaded.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset() => Track(Facade.Reset);

        #endregion
    }
}
