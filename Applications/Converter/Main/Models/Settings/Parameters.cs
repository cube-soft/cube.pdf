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
namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SaveOption
    ///
    /// <summary>
    /// 保存オプションを表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum SaveOption
    {
        /// <summary>上書き</summary>
        Overwrite = 0,
        /// <summary>先頭に結合</summary>
        MergeHead = 1,
        /// <summary>末尾に結合</summary>
        MergeTail = 2,
        /// <summary>リネーム</summary>
        Rename = 3,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PostProcess
    ///
    /// <summary>
    /// ポストプロセスを表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum PostProcess
    {
        /// <summary>開く</summary>
        Open = 0,
        /// <summary>保存フォルダを開く</summary>
        OpenFolder = 3,
        /// <summary>何もしない</summary>
        None = 1,
        /// <summary>その他（ユーザプログラム）</summary>
        Others = 2,
    }
}
