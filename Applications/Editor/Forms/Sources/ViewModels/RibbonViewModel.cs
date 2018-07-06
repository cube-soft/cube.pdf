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
using Cube.Xui;
using GalaSoft.MvvmLight;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RibbonViewModel
    ///
    /// <summary>
    /// Ribbon の ViewModel クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class RibbonViewModel : ViewModelBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// 開くアイコンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<RibbonIcon> Open { get; } = new RibbonIcon(nameof(Open)).ToBindable();

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 保存アイコンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<RibbonIcon> Save { get; } = new RibbonIcon(nameof(Save)).ToBindable();

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// 閉じるアイコンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<RibbonIcon> Close { get; } = new RibbonIcon(nameof(Close)).ToBindable();

        #endregion
    }
}
