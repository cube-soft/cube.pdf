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
using System.ComponentModel;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// MenuPresenter
    ///
    /// <summary>
    /// MainForm とモデルを対応付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MenuPresenter
        : Cube.Forms.PresenterBase<MainForm, Settings>
    {
        #region Constructors

        /* --------------------------------------------------------------------- */
        ///
        /// MenuPresenter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public MenuPresenter(MainForm view, Settings model, IEventHub events)
            : base(view, model, events)
        {
            EventHub.GetEvents()?.Refresh.Subscribe(Refresh_Handle);
            EventHub.GetEvents()?.Version.Subscribe(Version_Handle);
            Model.PropertyChanged += Settings_PropertyChanged;
        }

        #endregion

        #region Event handlers

        #region EventHub

        /* --------------------------------------------------------------------- */
        ///
        /// Refresh_Handle
        ///
        /// <summary>
        /// 再描画イベント発生時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Refresh_Handle()
            => Sync(() => View.Refresh());

        /* --------------------------------------------------------------------- */
        ///
        /// Version_Handle
        ///
        /// <summary>
        /// バージョン情報の表示イベント発生時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Version_Handle()
            => Sync(() => Views.CreateVersionView(Model.Assembly));

        #endregion

        #region Settings

        /* --------------------------------------------------------------------- */
        ///
        /// Settings_PropertyChanged
        ///
        /// <summary>
        /// Settings の内容が変化した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Model.AllowOperation):
                    Sync(() => View.AllowOperation = Model.AllowOperation);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion
    }
}
