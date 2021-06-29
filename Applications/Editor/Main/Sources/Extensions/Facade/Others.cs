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
using Cube.FileSystem;
using Cube.Mixin.Collections;
using Cube.Mixin.String;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// OtherExtension
    ///
    /// <summary>
    /// Represents the extended methods to handle documents.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class OtherExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Invokes some actions through the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="args">User arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Setup(this MainFacade src, IEnumerable<string> args)
        {
            foreach (var ps in src.Folder.GetSplashProcesses()) ps.Kill();
            var path = args.FirstPdf();
            if (path.HasValue()) src.Open(path);
            src.Backup.Cleanup();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Sets or resets the IsSelected property of all items according
        /// to the current condition.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Select(this MainFacade src) =>
            src.Select(src.Value.Images.Selection.Count < src.Value.Images.Count);

        /* ----------------------------------------------------------------- */
        ///
        /// Zoom
        ///
        /// <summary>
        /// Executes the Zoom command by using the current settings.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Zoom(this MainFacade src)
        {
            var items = src.Value.Images.Preferences.ItemSizeOptions;
            var prev  = src.Value.Images.Preferences.ItemSizeIndex;
            var next  = items.LastIndex(x => x <= src.Folder.Value.ItemSize);
            src.Zoom(next - prev);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Inserts the specified file behind the selected index.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="files">Collection of inserting files.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Insert(this MainFacade src, IEnumerable<string> files) =>
            src.Insert(src.Value.Images.Selection.Last + 1, files);

        /* ----------------------------------------------------------------- */
        ///
        /// CanInsert
        ///
        /// <summary>
        /// Gets the value indicating whether the specified file can be
        /// inserted.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="path">Path of the inserting file.</param>
        ///
        /// <remarks>
        /// TODO: 現在は拡張子で判断しているが、ファイル内容の Signature を
        /// 用いて判断するように修正する。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static bool CanInsert(this MainFacade src, string path)
        {
            var ext = Io.Get(path).Extension.ToLowerInvariant();
            var cmp = new List<string> { ".pdf", ".png", ".jpg", ".jpeg", ".bmp" };
            return cmp.Contains(ext);
        }

        #endregion
    }
}
