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
    /// FormatOption
    ///
    /// <summary>
    /// ファイル形式のオプションを表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum FormatOption
    {
        /// <summary>PDF 1.7</summary>
        Pdf17,
        /// <summary>PDF 1.6</summary>
        Pdf16,
        /// <summary>PDF 1.5</summary>
        Pdf15,
        /// <summary>PDF 1.4</summary>
        Pdf14,
        /// <summary>PDF 1.3</summary>
        Pdf13,
        /// <summary>PDF 1.2</summary>
        Pdf12,
    }

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
        Overwrite,
        /// <summary>先頭に結合</summary>
        MergeHead,
        /// <summary>末尾に結合</summary>
        MergeTail,
        /// <summary>リネーム</summary>
        Rename,
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
        Open,
        /// <summary>何もしない</summary>
        None,
        /// <summary>その他（ユーザプログラム）</summary>
        Others,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Language
    ///
    /// <summary>
    /// 表示言語を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Language
    {
        /// <summary>自動</summary>
        Auto,
        /// <summary>英語</summary>
        English,
        /// <summary>日本語</summary>
        Japanese,
    }
}
