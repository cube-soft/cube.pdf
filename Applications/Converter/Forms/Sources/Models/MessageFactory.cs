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
using System.Collections.Generic;
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
            Filter           = GetFilter(ViewResource.SourceFilters),
            FilterIndex      = GetFilterIndex(src, io, ViewResource.SourceFilters),
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
            Filter           = GetFilter(ViewResource.DestinationFilters),
            FilterIndex      = GetFilterIndex(src, io, ViewResource.DestinationFilters),
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
            Filter           = GetFilter(ViewResource.UserProgramFilters),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateWarning
        ///
        /// <summary>
        /// 警告メッセージを生成します。
        /// </summary>
        ///
        /// <param name="src">メッセージ内容</param>
        ///
        /// <returns>MessageEventArgs</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static MessageEventArgs CreateWarning(string src) => new MessageEventArgs(
            src,
            Properties.Resources.TitleWarning,
            System.Windows.Forms.MessageBoxButtons.OKCancel,
            System.Windows.Forms.MessageBoxIcon.Warning
        );

        /* ----------------------------------------------------------------- */
        ///
        /// CreateError
        ///
        /// <summary>
        /// エラーメッセージを生成します。
        /// </summary>
        ///
        /// <param name="src">メッセージ内容</param>
        ///
        /// <returns>MessageEventArgs</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static MessageEventArgs CreateError(string src) => new MessageEventArgs(
            src,
            Properties.Resources.TitleError,
            System.Windows.Forms.MessageBoxButtons.OK,
            System.Windows.Forms.MessageBoxIcon.Error
        );

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
        private static string GetFilter(IList<DisplayFilter> src) =>
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
        private static int GetFilterIndex(string src, IO io, IList<DisplayFilter> filters)
        {
            if (src.HasValue())
            {
                var ext = io.Get(src).Extension;
                for (var i = 0; i < filters.Count; ++i)
                {
                    var cmp = filters[i];
                    var opt = StringComparison.InvariantCultureIgnoreCase;
                    if (cmp.Extensions.Any(e => e.Equals(ext, opt))) return i + 1;
                }
            }
            return 0;
        }

        #endregion
    }
}
