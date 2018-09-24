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
    /// 各種 View の生成用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ViewFactory
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
        public virtual MainForm CreateMainView(string[] args)
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
        public virtual OpenFileDialog CreateOpenView() => new OpenFileDialog
        {
            Title           = Properties.Resources.SourceTitle,
            Filter          = Properties.Resources.SourceFilter,
            Multiselect     = false,
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
        public virtual OpenFileDialog CreateAttachView() => new OpenFileDialog
        {
            Title                        = Properties.Resources.AttachTitle,
            Filter                       = Properties.Resources.AttachFilter,
            Multiselect                  = true,
            CheckFileExists              = true,
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
        public virtual Cube.Forms.VersionForm CreateVersionView(string version, Icon icon) =>
            new Cube.Forms.VersionForm
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
        public virtual DialogResult ShowMessage(string message, MessageBoxButtons buttons) =>
            MessageBox.Show(
                message,
                Application.ProductName,
                buttons,
                MessageBoxIcon.Information
            );

        /* ----------------------------------------------------------------- */
        ///
        /// ShowError
        ///
        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        ///
        /// <param name="message">メッセージ</param>
        ///
        /// <returns>DialogResult オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public virtual DialogResult ShowError(string message) => MessageBox.Show(
            message,
            Application.ProductName,
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Views
    ///
    /// <summary>
    /// 各種 View の生成用クラスです。
    /// </summary>
    ///
    /// <remarks>
    /// Views は ViewFactory のプロキシとして実装されています。
    /// 実際の View 生成コードは ViewFactory および継承クラスで実装して
    /// 下さい。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static class Views
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Configure
        ///
        /// <summary>
        /// Facotry オブジェクトを設定します。
        /// </summary>
        ///
        /// <param name="factory">Factory オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Configure(ViewFactory factory)
            => _factory = factory;

        #region Factory methods

        public static MainForm CreateMainView(string[] args)
            => _factory?.CreateMainView(args);

        public static OpenFileDialog CreateOpenView()
            => _factory?.CreateOpenView();

        public static OpenFileDialog CreateAttachView()
            => _factory?.CreateAttachView();

        public static Cube.Forms.VersionForm CreateVersionView(string version, Icon icon)
            => _factory?.CreateVersionView(version, icon);

        public static DialogResult ShowMessage(string message)
            => ShowMessage(message, MessageBoxButtons.OK);

        public static DialogResult ShowMessage(string message, MessageBoxButtons buttons)
            => _factory?.ShowMessage(message, buttons) ?? DialogResult.Cancel;

        public static DialogResult ShowError(string message)
            => _factory?.ShowError(message) ?? DialogResult.OK;

        #endregion

        #region Fields
        private static ViewFactory _factory = new ViewFactory();
        #endregion
    }
}
