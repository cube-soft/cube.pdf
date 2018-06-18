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
using System.Threading;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainFacade
    ///
    /// <summary>
    /// メイン処理を表すクラスです。
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
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(SettingsFolder settings)
        {
            IO = new IO();
            Settings = settings;
            SystemLanguageName = Thread.CurrentThread.CurrentUICulture.Name;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

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
        /// SystemLanguageName
        ///
        /// <summary>
        /// システムの言語名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SystemLanguageName { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// 変換処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Convert()
        {
            try
            {
                Settings.Value.IsBusy = true;
                var gs = GhostscriptFactory.Create(Settings);
                gs.Invoke(Settings.Value.Source, Settings.Value.Destination);
            }
            finally { Settings.Value.IsBusy = false; }
        }

        #endregion
    }
}
