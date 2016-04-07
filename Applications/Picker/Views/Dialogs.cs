/* ------------------------------------------------------------------------- */
///
/// Dialogs.cs
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
using System.Reflection;
using System.Windows.Forms;
using IoEx = System.IO;

namespace Cube.Pdf.App.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// Dialogs
    ///
    /// <summary>
    /// 各種ダイアログの生成を行うためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Dialogs
    {
        /* --------------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// 保存するフォルダを選択するためのダイアログを生成します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public static FolderBrowserDialog Save(string path)
        {
            var dest = new FolderBrowserDialog();
            dest.Description = Properties.Resources.SaveFolder;
            dest.SelectedPath = IoEx.Path.GetDirectoryName(path);
            return dest;
        }

        #region MessageBox

        /* --------------------------------------------------------------------- */
        ///
        /// Version
        /// 
        /// <summary>
        /// バージョン情報を表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public static DialogResult Version(Assembly assembly)
        {
            var dest = new Cube.Forms.VersionForm
            {
                Assembly = assembly,
                Logo = Properties.Resources.Logo,
                Description = string.Empty,
                Height = 280,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.CenterParent,
            };
            dest.Version.Digit = 3;

            return dest.ShowDialog();
        }

        #endregion
    }
}
