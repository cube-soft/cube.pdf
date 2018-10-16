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
using Cube.Xui;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// InsertViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a InsertWindow instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class InsertViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// InsertViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the InsertViewModel with the
        /// specified argumetns.
        /// </summary>
        ///
        /// <param name="callback">Callback function.</param>
        /// <param name="i">Selected index.</param>
        /// <param name="n">Number of pages.</param>
        /// <param name="io">I/O handler.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public InsertViewModel(Action<int, IEnumerable<FileItem>> callback,
            int i, int n, IO io, SynchronizationContext context) :
            base(() => Properties.Resources.TitleInsert, new Messenger(), context)
        {
            Model    = new InsertFacade(i, n, io, context);
            Position = new InsertPosition(Data);

            SetCommands(callback);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Model
        ///
        /// <summary>
        /// Gets the model object of the ViewModel.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected InsertFacade Model { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        ///
        /// <summary>
        /// Gets data for binding to the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public InsertBindable Data => Model.Bindable;

        /* ----------------------------------------------------------------- */
        ///
        /// Position
        ///
        /// <summary>
        /// Gets the label that represents the insert position.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public InsertPosition Position { get; }

        #region Buttons

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Gets the menu that represents the add button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Add { get; } = new BindableElement(
            () => Properties.Resources.MenuAdd
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Gets the menu that represents the remove button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Remove { get; } = new BindableElement(
            () => Properties.Resources.MenuRemove
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Gets the menu that represents the clear button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Clear { get; } = new BindableElement(
            () => Properties.Resources.MenuClear
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Up
        ///
        /// <summary>
        /// Gets the menu that represents the up button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Up { get; } = new BindableElement(
            () => Properties.Resources.MenuUp
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Down
        ///
        /// <summary>
        /// Gets the menu that represents the down button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Down { get; } = new BindableElement(
            () => Properties.Resources.MenuDown
        );

        #endregion

        #region Contents

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        ///
        /// <summary>
        /// Gets the menu that represents the FileName column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement FileName { get; } = new BindableElement(
            () => Properties.Resources.MenuFilename
        );

        /* ----------------------------------------------------------------- */
        ///
        /// FileType
        ///
        /// <summary>
        /// Gets the menu that represents the FileType column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement FileType { get; } = new BindableElement(
            () => Properties.Resources.MenuFiletype
        );

        /* ----------------------------------------------------------------- */
        ///
        /// FileLength
        ///
        /// <summary>
        /// Gets the menu that represents the FileLength column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement FileLength { get; } = new BindableElement(
            () => Properties.Resources.MenuFilesize
        );

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        ///
        /// <summary>
        /// Gets the menu that represents the LastWriteTime column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement LastWriteTime { get; } = new BindableElement(
            () => Properties.Resources.MenuLastWriteTime
        );

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetCommands
        ///
        /// <summary>
        /// Sets commands of the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetCommands(Action<int, IEnumerable<FileItem>> callback)
        {
            Add.Command = new BindableCommand(() => PostOpen(e => Model.Add(e)), () => true);
            OK.Command = new BindableCommand(() =>
            {
                Send<CloseMessage>();
                callback?.Invoke(Data.Index.Value, Data.Files);
            },
            () => true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PostOpen
        ///
        /// <summary>
        /// Posts the message to show a dialog of the OpenFileDialog
        /// class, and executes the specified action as an asynchronous
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostOpen(Action<string> action) => Send(Factory.OpenMessage(e =>
            Post(() => { if (e.Result) action(e.FileName); })
        ));

        #endregion
    }
}
