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
using System.Collections.Generic;

namespace Cube.Pdf.App.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// IClipView
    ///
    /// <summary>
    /// 添付画面を表すインターフェースです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface IClipView : Cube.Forms.IForm
    {
        /* ----------------------------------------------------------------- */
        ///
        /// IsBusy
        ///
        /// <summary>
        /// 処理中かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        bool IsBusy { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// PDF ファイルのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        string Source { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DataSource
        ///
        /// <summary>
        /// View に関連付けられるデータを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        object DataSource { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectedIndices
        ///
        /// <summary>
        /// 選択されているインデックスの一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerable<int> SelectedIndices { get; }
    }
}
