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
using Cube.Conversions;
using Cube.FileSystem.Files;
using System.Windows.Forms;
using IoEx = System.IO;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileConverter
    ///
    /// <summary>
    /// FileBase から ListViewItem へ変換するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileConverter : Cube.Forms.IListViewItemConverter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileConverte
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileConverter(IconCollection icons)
        {
            Icons = icons;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Icons
        ///
        /// <summary>
        /// ListView で使用するアイコン一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IconCollection Icons { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// ListViewItem オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ListViewItem Convert<T>(T src)
        {
            var file = src as MediaFile;
            if (file == null) return new ListViewItem(src.ToString());

            var space    = " ";
            var filename = IoEx.Path.GetFileName(file.FullName);
            var type     = file.RawObject.GetTypeName();
            var pages    = file.PageCount.ToString();
            var date     = file.LastWriteTime.ToString("yyyy/MM/dd hh:mm");
            var bytes    = file.Length.ToRoughBytes();

            return new ListViewItem(new string[] { space + filename, type, pages, date, bytes })
            {
                ToolTipText = file.FullName,
                ImageIndex  = Icons.Register(file)
            };
        }

        #endregion
    }
}
