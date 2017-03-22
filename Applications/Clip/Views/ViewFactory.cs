/* ------------------------------------------------------------------------- */
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
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.Pdf.App.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewFactory
    ///
    /// <summary>
    /// View の生成用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ViewFactory
    {
        /* ----------------------------------------------------------------- */
        ///
        /// CreateMainView
        /// 
        /// <summary>
        /// メイン画面を生成します。
        /// </summary>
        /// 
        /// <returns>メイン画面</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static MainForm CreateMainView(string[] args)
            => new MainForm(args);

        /* ----------------------------------------------------------------- */
        ///
        /// CreateOpenView
        /// 
        /// <summary>
        /// PDF ファイルの選択画面を生成します。
        /// </summary>
        /// 
        /// <returns>PDF ファイルの選択画面</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileDialog CreateOpenView()
            => new OpenFileDialog
        {
            Title  = Properties.Resources.SourceTitle,
            Filter = Properties.Resources.SourceFilter,
            Multiselect = false,
            CheckFileExists = true,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateAttachView
        /// 
        /// <summary>
        /// 添付ファイルの選択画面を生成します。
        /// </summary>
        /// 
        /// <returns>添付ファイルの選択画面</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileDialog CreateAttachView()
            => new OpenFileDialog
        {
            Title = Properties.Resources.AttachTitle,
            Filter = Properties.Resources.AttachFilter,
            Multiselect = true,
            CheckFileExists = true,
            SupportMultiDottedExtensions = true,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateVersionView
        /// 
        /// <summary>
        /// バージョン画面を生成します。
        /// </summary>
        /// 
        /// <param name="version">バージョン</param>
        /// <param name="icon">アイコン画像</param>
        /// 
        /// <returns>バージョン画面</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Cube.Forms.VersionForm CreateVersionView(string version, Icon icon)
            => new Cube.Forms.VersionForm
        {
            AutoScaleMode = AutoScaleMode.None,
            Icon          = icon,
            Image         = Properties.Resources.Logo,
            StartPosition = FormStartPosition.CenterParent,
            Text          = Properties.Resources.VersionTitle,
            Uri           = new Uri(Properties.Resources.VersionWeb),
            Version       = version,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ShowMessage
        /// 
        /// <summary>
        /// メッセージを表示します。
        /// </summary>
        /// 
        /// <param name="message">メッセージ</param>
        /// 
        /// <returns>DialogResult オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static DialogResult ShowMessage(string message)
            => MessageBox.Show(
                message,
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
    }
}
