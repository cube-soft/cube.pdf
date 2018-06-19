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
using Cube.Log;
using Cube.Pdf.Ghostscript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MessageViewModel
    ///
    /// <summary>
    /// 各種メッセージを表示するための ViewModel です。
    /// </summary>
    ///
    /// <remarks>
    /// MainViewModel の拡張クラスとして実装しています。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static class MessageViewModel
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Validate
        ///
        /// <summary>
        /// 各種プロパティの値をチェックします。
        /// </summary>
        ///
        /// <param name="src">MainViewModel</param>
        ///
        /// <returns>正常な値かどうか</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool Validate(this MainViewModel src)
        {
            if (!ValidateDestination(src)) return false;
            if (!ValidateOwnerPassword(src)) return false;
            if (!ValidateUserPassword(src)) return false;
            return true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Show
        ///
        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        ///
        /// <param name="src">MainViewModel</param>
        /// <param name="err">例外オブジェクト</param>
        ///
        /// <remarks>
        /// OperationCanceledException 以外の例外が発生した場合、
        /// エラーメッセージ表示後に Close イベントを発行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Show(this MainViewModel src, Exception err)
        {
            if (err is OperationCanceledException) return;

            src.LogError(err.ToString(), err);
            var msg  = err is GsApiException gse ? CreateMessage(gse) : err.Message;
            var args = MessageFactory.CreateError(msg);
            src.Messenger.MessageBox.Publish(args);
            src.Messenger.Close.Publish();
        }

        #endregion

        #region Implementations

        #region Validate

        /* ----------------------------------------------------------------- */
        ///
        /// ValidateDestination
        ///
        /// <summary>
        /// 保存パスのチェックを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static bool ValidateDestination(MainViewModel src)
        {
            var dest = src.Settings.Destination;
            var so   = src.Settings.SaveOption;

            if (!src.IO.Exists(dest) || so == SaveOption.Rename) return true;

            var msg  = CreateMessage(dest, so);
            var args = MessageFactory.CreateWarning(msg);

            src.Messenger.MessageBox.Publish(args);
            return args.Result != DialogResult.Cancel;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ValidateOwnerPassword
        ///
        /// <summary>
        /// 管理用パスワードのチェックを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static bool ValidateOwnerPassword(MainViewModel src)
        {
            var eo  = src.Encryption;
            var opt = StringComparison.InvariantCultureIgnoreCase;
            if (!eo.Enabled || eo.OwnerPassword.Equals(eo.OwnerConfirm, opt)) return true;

            var args = MessageFactory.CreateError(Properties.Resources.MessagePassword);
            src.Messenger.MessageBox.Publish(args);
            return false;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ValidateUserPassword
        ///
        /// <summary>
        /// 閲覧用パスワードのチェックを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static bool ValidateUserPassword(MainViewModel src)
        {
            var eo  = src.Encryption;
            var opt = StringComparison.InvariantCultureIgnoreCase;
            if (!eo.Enabled || !eo.OpenWithPassword || eo.UseOwnerPassword) return true;
            if (eo.UserPassword.Equals(eo.UserConfirm, opt)) return true;

            var args = MessageFactory.CreateError(Properties.Resources.MessagePassword);
            src.Messenger.MessageBox.Publish(args);
            return false;
        }

        #endregion

        #region CreateMessage

        /* ----------------------------------------------------------------- */
        ///
        /// CreateMessage
        ///
        /// <summary>
        /// ユーザに表示するメッセージを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string CreateMessage(string path, SaveOption option)
        {
            var dic = new Dictionary<SaveOption, string>
            {
                { SaveOption.Overwrite, Properties.Resources.MessageOverwrite },
                { SaveOption.MergeHead, Properties.Resources.MessageMergeHead },
                { SaveOption.MergeTail, Properties.Resources.MessageMergeTail },
            };

            var head = string.Format(Properties.Resources.MessageExists, path);
            var status = dic.TryGetValue(option, out var tail);
            Debug.Assert(status);

            return $"{head} {tail}";
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateMessage
        ///
        /// <summary>
        /// Ghostscript API の実行中にエラーが発生した時のメッセージを
        /// 生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string CreateMessage(GsApiException err) =>
            string.Format(Properties.Resources.MessageGhostscript, err.ErrorCode);

        #endregion

        #endregion
    }
}
