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
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Cube.Mixin.Observing;

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
    public sealed class MainViewModel : PresentableBase<MainFacade>
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
        /// <param name="src">User settings.</param>
        /// <param name="args">Program arguments.</param>
        ///
        /* --------------------------------------------------------------------- */
        public MainViewModel(SettingFolder src, IEnumerable<string> args) :
            this(src, args, SynchronizationContext.Current) { }

        /* --------------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class with the
        /// specified context.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        /// <param name="args">Program arguments.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* --------------------------------------------------------------------- */
        public MainViewModel(SettingFolder src, IEnumerable<string> args, SynchronizationContext context) :
            base(new(src, context), new(12), context)
        {
            Locale.Set(src.Value.Language);

            Arguments = args;
            Files = new() { DataSource = Facade.Files };
            Facade.Query = new Query<string>(e => Send(new PasswordViewModel(e, context)));

            Assets.Add(new ObservableProxy(Facade, this));
            Assets.Add(ObserveCollection());
            Assets.Add(src.Subscribe(e => {
                if (e == nameof(src.Value.Language)) Locale.Set(src.Value.Language);
            }));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Arguments
        ///
        /// <summary>
        /// Gets the program arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Arguments { get; }

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
        /// Gets a value indicating whether the class is busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Busy => Facade.Busy;

        /* ----------------------------------------------------------------- */
        ///
        /// Ready
        ///
        /// <summary>
        /// Gets a value indicating whether the Merge or Split operation
        /// is available.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Ready => Facade.Files.Count > 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Language
        ///
        /// <summary>
        /// Gets the displayed language.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Language Language => Facade.Settings.Value.Language;

        #endregion

        #region Methods

        /* --------------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Invokes the Setup command.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public void Setup() { if (Arguments.Any()) Add(Arguments); }

        /* --------------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// Invokes the Merge command.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public void Merge() => Send(Message.ForMerge(), Facade.Merge, false);

        /* --------------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Invokes the Metadata command.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public void Metadata() => Send(new MetadataViewModel(Facade.Metadata, Facade.Encryption, Context));

        /* --------------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// Invokes the Split command.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public void Split() => Send(Message.ForSplit(), Facade.Split, false);

        /* --------------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Invokes the Add command.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public void Add() => Send(Message.ForAdd(), Facade.Add, true);

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
        public void Add(IEnumerable<string> src) => Run(() => Facade.Add(src), true);

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
        public void Remove(IEnumerable<int> indices) => Run(() => Facade.Remove(indices), true);

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears the added files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear() => Run(Facade.Reset, true);

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
        public void Move(IEnumerable<int> indices, int offset) => Run(() =>
        {
            if (Facade.Move(indices, offset)) Send(Message.ForSelect(
                indices, offset, Facade.Files.Count
            ));
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
            if (indices.Count() > 0) Send(Message.ForPreview(Facade.Files, indices));
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

        #region Implementations

        /* --------------------------------------------------------------------- */
        ///
        /// ObserveCollection
        ///
        /// <summary>
        /// Starts to observe the CollectionChanged event.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private IDisposable ObserveCollection()
        {
            void handler(object s, NotifyCollectionChangedEventArgs e)
            {
                Refresh(nameof(Ready));
                Send(new UpdateListMessage());
            }

            Facade.CollectionChanged += handler;
            return Disposable.Create(() => Facade.CollectionChanged -= handler);
        }

        #endregion
    }
}
