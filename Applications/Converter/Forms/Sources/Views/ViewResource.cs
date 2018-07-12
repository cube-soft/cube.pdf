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
using Cube.Pdf.Ghostscript;
using System.Collections.Generic;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewResource
    ///
    /// <summary>
    /// 表示時に使用するリソースを定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ViewResource
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Formats
        ///
        /// <summary>
        /// 表示文字列と Format の対応関係を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, Format>> Formats => _formats ?? (
            _formats = new []
            {
                Pair("PDF",  Format.Pdf),
                Pair("PS",   Format.Ps),
                Pair("EPS",  Format.Eps),
                Pair("PNG",  Format.Png),
                Pair("JPEG", Format.Jpeg),
                Pair("BMP",  Format.Bmp),
                Pair("TIFF", Format.Tiff),
            }
        );

        /* ----------------------------------------------------------------- */
        ///
        /// FormatOptions
        ///
        /// <summary>
        /// 表示文字列と FormatOption の対応関係を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, FormatOption>> FormatOptions =>
            _formatOptions ?? (_formatOptions = new []
            {
                Pair("PDF 1.7", FormatOption.Pdf17),
                Pair("PDF 1.6", FormatOption.Pdf16),
                Pair("PDF 1.5", FormatOption.Pdf15),
                Pair("PDF 1.4", FormatOption.Pdf14),
                Pair("PDF 1.3", FormatOption.Pdf13),
                Pair("PDF 1.2", FormatOption.Pdf12),
            }
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOptions
        ///
        /// <summary>
        /// 表示文字列と SaveOption の対応関係を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, SaveOption>> SaveOptions => new []
        {
            Pair(Properties.Resources.MenuOverwrite, SaveOption.Overwrite),
            Pair(Properties.Resources.MenuMergeHead, SaveOption.MergeHead),
            Pair(Properties.Resources.MenuMergeTail, SaveOption.MergeTail),
            Pair(Properties.Resources.MenuRename,    SaveOption.Rename),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// DisplayOptions
        ///
        /// <summary>
        /// 表示文字列と DisplayOptions の対応関係を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, DisplayOptions>> DisplayOptions => new []
        {
            Pair(Properties.Resources.MenuSinglePage,     Pdf.DisplayOptions.SinglePage),
            Pair(Properties.Resources.MenuOneColumn,      Pdf.DisplayOptions.OneColumn),
            Pair(Properties.Resources.MenuTwoPageLeft,    Pdf.DisplayOptions.TwoPageLeft),
            Pair(Properties.Resources.MenuTwoPageRight,   Pdf.DisplayOptions.TwoPageRight),
            Pair(Properties.Resources.MenuTwoColumnLeft,  Pdf.DisplayOptions.TwoColumnLeft),
            Pair(Properties.Resources.MenuTwoColumnRight, Pdf.DisplayOptions.TwoColumnRight),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// PostProcesses
        ///
        /// <summary>
        /// 表示文字列と PostProcess の対応関係を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, PostProcess>> PostProcesses => new []
        {
            Pair(Properties.Resources.MenuOpen,          PostProcess.Open),
            Pair(Properties.Resources.MenuOpenDirectory, PostProcess.OpenDirectory),
            Pair(Properties.Resources.MenuNone,          PostProcess.None),
            Pair(Properties.Resources.MenuOthers,        PostProcess.Others),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Orientations
        ///
        /// <summary>
        /// 表示文字列と Orientation の対応関係を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, Orientation>> Orientations => new []
        {
            Pair(Properties.Resources.MenuPortrait,  Orientation.Portrait),
            Pair(Properties.Resources.MenuLandscape, Orientation.Landscape),
            Pair(Properties.Resources.MenuAuto,      Orientation.Auto),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Languages
        ///
        /// <summary>
        /// 表示文字列と Language の対応関係を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, Language>> Languages => new []
        {
            Pair(Properties.Resources.MenuAuto,     Language.Auto),
            Pair(Properties.Resources.MenuEnglish,  Language.English),
            Pair(Properties.Resources.MenuJapanese, Language.Japanese),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// SourceFilters
        ///
        /// <summary>
        /// 入力ファイルのフィルタ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<DisplayFilter> SourceFilters => new[]
        {
            new DisplayFilter(Properties.Resources.FilterPs,  ".ps"),
            new DisplayFilter(Properties.Resources.FilterEps, ".eps"),
            new DisplayFilter(Properties.Resources.FilterPdf, ".pdf"),
            new DisplayFilter(Properties.Resources.FilterAll, ".*"),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// DestinationFilters
        ///
        /// <summary>
        /// 保存パスのフィルタ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<DisplayFilter> DestinationFilters => new[]
        {
            new DisplayFilter(Properties.Resources.FilterPdf,  ".pdf"),
            new DisplayFilter(Properties.Resources.FilterPs,   ".ps"),
            new DisplayFilter(Properties.Resources.FilterEps,  ".eps"),
            new DisplayFilter(Properties.Resources.FilterPng,  ".png"),
            new DisplayFilter(Properties.Resources.FilterJpeg, ".jpg", ".jpeg"),
            new DisplayFilter(Properties.Resources.FilterBmp,  ".bmp"),
            new DisplayFilter(Properties.Resources.FilterTiff, ".tiff", ".tif"),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// UserProgramFilters
        ///
        /// <summary>
        /// ユーザプログラムのフィルタ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<DisplayFilter> UserProgramFilters => new[]
        {
            new DisplayFilter(Properties.Resources.FilterExecutable, ".exe", ".bat"),
            new DisplayFilter(Properties.Resources.FilterAll, ".*"),
        };

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Pair(T, U)
        ///
        /// <summary>
        /// KeyValuePair(T, U) を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static KeyValuePair<K, V> Pair<K, V>(K key, V value) =>
            new KeyValuePair<K, V>(key, value);

        #endregion

        #region Fields
        private static IList<KeyValuePair<string, Format>> _formats;
        private static IList<KeyValuePair<string, FormatOption>> _formatOptions;
        #endregion
    }
}
