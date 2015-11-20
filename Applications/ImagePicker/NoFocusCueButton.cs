/* ------------------------------------------------------------------------- */
///
/// NoFocusCueButton.cs
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
using System.Windows.Forms;

namespace Cube.Pdf.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.NoFocusCueButton
    ///
    /// <summary>
    /// フォーカス時に描画される枠線を無効化したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class NoFocusCueButton : Button
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ShowFocusCues
        /// 
        /// <summary>
        /// フォーカス時に枠線を表示するかどうかを示す値を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected override bool ShowFocusCues
        {
            get { return false; }
        }
    }
}
