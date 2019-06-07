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
using System.Reflection;
using System.Windows.Forms;

namespace Cube.Pdf.Pages
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
        /* --------------------------------------------------------------------- */
        ///
        /// CreateAddView
        ///
        /// <summary>
        /// 追加するファイルを選択するための View を生成します。
        /// </summary>
        ///
        /// <returns>ファイル選択画面</returns>
        ///
        /* --------------------------------------------------------------------- */
        public OpenFileDialog CreateAddView() => new OpenFileDialog
        {
            CheckFileExists = true,
            Multiselect     = true,
            Title = Properties.Resources.OpenFileTitle,
            Filter = Properties.Resources.OpenFileFilter,
        };

        /* --------------------------------------------------------------------- */
        ///
        /// CreatePasswordView
        ///
        /// <summary>
        /// パスワードを入力するための View を生成します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public PasswordForm CreatePasswordView(string path) => new PasswordForm
        {
            Path = path,
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.CenterParent,
        };

        /* --------------------------------------------------------------------- */
        ///
        /// CreateMergeView
        ///
        /// <summary>
        /// 結合したファイルの保存先を選択するためのView を生成します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public SaveFileDialog CreateMergeView() => new SaveFileDialog
        {
            OverwritePrompt = true,
            Title = Properties.Resources.MergeTitle,
            Filter = Properties.Resources.SaveFileFilter,
        };

        /* --------------------------------------------------------------------- */
        ///
        /// CreateSplitView
        ///
        /// <summary>
        /// 分割したファイルの保存先を選択するための View を生成します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public FolderBrowserDialog CreateSplitView() => new FolderBrowserDialog
        {
            Description = Properties.Resources.SplitDescription,
            ShowNewFolderButton = true,
        };

        /* --------------------------------------------------------------------- */
        ///
        /// CreateVersionView
        ///
        /// <summary>
        /// バージョン情報を表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public void CreateVersionView(Assembly assembly)
        {
            var sv = new SoftwareVersion(assembly) { Digit = 3 };
            using (var dialog = new Cube.Forms.VersionForm
            {
                Version = sv.ToString(true),
                Image = Properties.Resources.Logo,
                Description = string.Empty,
                Height = 280,
                StartPosition = FormStartPosition.CenterParent,
            }) dialog.ShowDialog();
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ShowConfirmMessage
        ///
        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public DialogResult ShowConfirmMessage(string message) => MessageBox.Show(
            message,
            Application.ProductName,
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information
        );

        /* --------------------------------------------------------------------- */
        ///
        /// ShowErrorMessage
        ///
        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public DialogResult ShowErrorMessage(Exception err) => MessageBox.Show(
            err.Message,
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
        #region Factory methods

        /* --------------------------------------------------------------------- */
        ///
        /// CreateAddView
        ///
        /// <summary>
        /// 追加するファイルを選択するための View を生成します。
        /// </summary>
        ///
        /// <returns>ファイル選択画面</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static OpenFileDialog CreateAddView()
            => _factory?.CreateAddView();

        /* --------------------------------------------------------------------- */
        ///
        /// CreatePasswordView
        ///
        /// <summary>
        /// パスワードを入力するための View を生成します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public static PasswordForm CreatePasswordView(string path)
            => _factory?.CreatePasswordView(path);

        /* --------------------------------------------------------------------- */
        ///
        /// CreateMergeView
        ///
        /// <summary>
        /// 結合したファイルの保存先を選択するためのView を生成します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public static SaveFileDialog CreateMergeView()
            => _factory?.CreateMergeView();

        /* --------------------------------------------------------------------- */
        ///
        /// CreateSplitView
        ///
        /// <summary>
        /// 分割したファイルの保存先を選択するための View を生成します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public static FolderBrowserDialog CreateSplitView()
            => _factory?.CreateSplitView();

        /* --------------------------------------------------------------------- */
        ///
        /// CreateVersionView
        ///
        /// <summary>
        /// バージョン情報を表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public static void CreateVersionView(Assembly assembly)
            => _factory?.CreateVersionView(assembly);

        /* --------------------------------------------------------------------- */
        ///
        /// ShowConfirmMessage
        ///
        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public static DialogResult ShowConfirmMessage(string message)
            => _factory?.ShowConfirmMessage(message) ?? DialogResult.Cancel;

        /* --------------------------------------------------------------------- */
        ///
        /// ShowErrorMessage
        ///
        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public static DialogResult ShowErrorMessage(Exception err)
            => _factory?.ShowErrorMessage(err) ?? DialogResult.Cancel;

        #endregion

        #region Fields
        private static readonly ViewFactory _factory = new ViewFactory();
        #endregion
    }
}
