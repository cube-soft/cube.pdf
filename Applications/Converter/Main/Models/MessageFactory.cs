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
using Cube.Forms;
using Cube.Generics;
using System;
using System.Linq;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MessageFactory
    ///
    /// <summary>
    /// 各種メッセージを生成するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class MessageFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateSource
        ///
        /// <summary>
        /// 入力ファイルを選択するためのメッセージを生成します。
        /// </summary>
        ///
        /// <param name="src">入力ファイルの初期値</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /// <returns>OpenFileEventArgs</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileEventArgs CreateSource(string src, IO io) => new OpenFileEventArgs
        {
            Title            = Properties.Resources.TitleBrowseSource,
            FileName         = GetFileName(src, io),
            InitialDirectory = GetDirectoryName(src, io),
            Multiselect      = false,
            Filter           = GetFilter(GetSourceFilters()),
            FilterIndex      = GetFilterIndex(src, io, GetSourceFilters()),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDestination
        ///
        /// <summary>
        /// 出力ファイルを選択するためのメッセージを生成します。
        /// </summary>
        ///
        /// <param name="src">保存パスの初期値</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /// <returns>SaveFileEventArgs</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveFileEventArgs CreateDestination(string src, IO io) => new SaveFileEventArgs
        {
            Title            = Properties.Resources.TitleBroseDestination,
            FileName         = GetFileName(src, io),
            InitialDirectory = GetDirectoryName(src, io),
            OverwritePrompt  = false,
            Filter           = GetFilter(GetDestinationFilters()),
            FilterIndex      = GetFilterIndex(src, io, GetDestinationFilters()),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateUserProgram
        ///
        /// <summary>
        /// ユーザプログラムを選択するためのメッセージを生成します。
        /// </summary>
        ///
        /// <param name="src">ユーザプログラムの初期値</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /// <returns>OpenFileEventArgs</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileEventArgs CreateUserProgram(string src, IO io) => new OpenFileEventArgs
        {
            Title            = Properties.Resources.TitleBroseUserProgram,
            FileName         = GetFileName(src, io),
            InitialDirectory = GetDirectoryName(src, io),
            Multiselect      = false,
            Filter           = GetFilter(GetUserProgramFilters()),
        };

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// ファイル名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetFileName(string src, IO io) =>
            src.HasValue() ? io.Get(src).NameWithoutExtension : string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectoryName
        ///
        /// <summary>
        /// ディレクトリ名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetDirectoryName(string src, IO io) =>
            src.HasValue() ? io.Get(src).DirectoryName : string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilter
        ///
        /// <summary>
        /// フィルタを表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetFilter(DisplayFilter[] src) =>
            src.Select(e => e.ToString()).Aggregate((x, y) => $"{x}|{y}");

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilterIndex
        ///
        /// <summary>
        /// 現在のフィルタを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static int GetFilterIndex(string src, IO io, DisplayFilter[] filters)
        {
            if (src.HasValue())
            {
                var ext = io.Get(src).Extension;
                for (var i = 0; i < filters.Length; ++i)
                {
                    var cmp = filters[i];
                    var opt = StringComparison.InvariantCultureIgnoreCase;
                    if (cmp.Extensions.Any(e => e.Equals(ext, opt))) return i + 1;
                }
            }
            return 0;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetSourceFilters
        ///
        /// <summary>
        /// 入力ファイルのフィルタ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static DisplayFilter[] GetSourceFilters() => _src ?? (
            _src = new[]
            {
                new DisplayFilter(Properties.Resources.FilterPs,  ".ps"),
                new DisplayFilter(Properties.Resources.FilterEps, ".eps"),
                new DisplayFilter(Properties.Resources.FilterPdf, ".pdf"),
                new DisplayFilter(Properties.Resources.FilterAll, ".*"),
            }
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetDestinationFilters
        ///
        /// <summary>
        /// 保存パスのフィルタ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static DisplayFilter[] GetDestinationFilters() => _dest ?? (
            _dest = new[]
            {
                new DisplayFilter(Properties.Resources.FilterPdf,  ".pdf"),
                new DisplayFilter(Properties.Resources.FilterPs,   ".ps"),
                new DisplayFilter(Properties.Resources.FilterEps,  ".eps"),
                new DisplayFilter(Properties.Resources.FilterPng,  ".png"),
                new DisplayFilter(Properties.Resources.FilterJpeg, ".jpg", ".jpeg"),
                new DisplayFilter(Properties.Resources.FilterBmp,  ".bmp"),
                new DisplayFilter(Properties.Resources.FilterTiff, ".tiff", ".tif"),
          }
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetUserProgramFilters
        ///
        /// <summary>
        /// ユーザプログラムのフィルタ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static DisplayFilter[] GetUserProgramFilters() => _user ?? (
            _user = new[]
            {
                new DisplayFilter(Properties.Resources.FilterExecutable, ".exe", ".bat"),
                new DisplayFilter(Properties.Resources.FilterAll, ".*"),
            }
        );

        #endregion

        #region Fields
        private static DisplayFilter[] _src;
        private static DisplayFilter[] _dest;
        private static DisplayFilter[] _user;
        #endregion
    }
}
