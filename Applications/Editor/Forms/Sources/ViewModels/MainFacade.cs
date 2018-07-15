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
using Cube.Pdf.Pdfium;
using System;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainFacade
    ///
    /// <summary>
    /// MainViewModel と各種 Model の窓口となるクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MainFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainFacade
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="settings">設定情報</param>
        /// <param name="context">同期用コンテキスト</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(SettingsFolder settings, SynchronizationContext context)
        {
            Settings = settings;
            Data = new MainBindableData(new ImageList(context), settings);
            Data.Images.Preferences.BaseSize = settings.Value.ViewSize;
            Data.Images.Preferences.Margin = 3;
            Data.Images.Preferences.TextHeight = 25;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// 設定情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        ///
        /// <summary>
        /// Gets data related with the docuemnt.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindableData Data { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Core
        ///
        /// <summary>
        /// Gets or sets a core object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected DocumentReader Core
        {
            get => _core;
            set
            {
                if (_core == value) return;
                _core = value;
                Data.IsOpen.Value = _core != null;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Open a PDF document with the specified file path.
        /// </summary>
        ///
        /// <param name="src">File path.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(string src) => Invoke(() =>
        {
            Core = new DocumentReader(src);
            Data.Images.Renderer = Core;
            foreach (var page in Core.Pages) Data.Images.Add(page);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Closes the current PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Close() => Invoke(() =>
        {
            Data.Images.Clear();
            Core?.Dispose();
            Core = null;
        });

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the user action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action)
        {
            Data.IsBusy.Value = true;
            try { action(); }
            finally { Data.IsBusy.Value = false; }
        }

        #endregion

        #region Fields
        private DocumentReader _core;
        #endregion
    }
}
