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
namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// Paper
    ///
    /// <summary>
    /// Ghostscript で指定可能な用紙サイズを定義した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Paper
    {
        /// <summary>自動（入力元と同じ用紙サイズ）</summary>
        Auto = 0,
        /// <summary>A3</summary>
        A3,
        /// <summary>A4</summary>
        A4,
        /// <summary>A5</summary>
        A5,
        /// <summary>B3</summary>
        B3,
        /// <summary>B4</summary>
        B4,
        /// <summary>B5</summary>
        B5,
        /// <summary>Ledger</summary>
        Ledger,
        /// <summary>Legal</summary>
        Legal,
        /// <summary>Letter</summary>
        Letter,
    }
}
