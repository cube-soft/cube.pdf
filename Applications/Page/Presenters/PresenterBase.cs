/* ------------------------------------------------------------------------- */
///
/// PresenterBase.cs
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// PresenterBase
    /// 
    /// <summary>
    /// CubePDF Page で作成する Presenter の基底となるクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class PresenterBase<TView, TModel>
        : Cube.Forms.PresenterBase<TView, TModel>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PresenterBase
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PresenterBase(TView view, TModel model,
            SettingsValue settings, EventAggregator events)
            : base(view, model)
        {
            Settings = settings;
            Events = events;
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
        public SettingsValue Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Events
        /// 
        /// <summary>
        /// イベント情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventAggregator Events { get; }

        #endregion
    }
}
