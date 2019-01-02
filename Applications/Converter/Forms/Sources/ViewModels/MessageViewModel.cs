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
using Cube.Forms;
using Cube.Generics;
using Cube.Log;
using Cube.Pdf.Ghostscript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
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
        /// Invoke
        ///
        /// <summary>
        /// Checks properties and invokes the specified action.
        /// </summary>
        ///
        /// <param name="src">ViewModel object.</param>
        /// <param name="action">User action.</param>
        ///
        /// <remarks>
        /// 事前チェックおよびエラー発生時にメッセージを表示するための
        /// イベントを発行します。また、処理実行後は成功・失敗に
        /// 関わらず Close イベントを発行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Invoke(this MainViewModel src, Action action)
        {
            if (!src.Settings.Source.HasValue()) return;
            if (!ValidateOwnerPassword(src)) return;
            if (!ValidateUserPassword(src)) return;
            if (!ValidateDestination(src)) return;

            try { action(); }
            catch (Exception err) { src.Show(err); }
            finally { src.Post(() => src.Messenger.Close.Publish()); }
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
        /* ----------------------------------------------------------------- */
        public static void Show(this MainViewModel src, Exception err)
        {
            if (err is OperationCanceledException) return;

            src.LogError(err.ToString(), err);
            var msg = err is GsApiException gse         ? CreateMessage(gse) :
                      err is EncryptionException ece    ? CreateMessage(ece) :
                      err is CryptographicException cpe ? CreateMessage(cpe) :
                      $"{err.Message} ({err.GetType().Name})";
            src.Show(() => MessageFactory.CreateError(msg));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Show
        ///
        /// <summary>
        /// メッセージを表示します。
        /// </summary>
        ///
        /// <param name="src">MainViewModel</param>
        /// <param name="get">メッセージ生成オブジェクト</param>
        ///
        /// <remarks>
        /// メッセージのローカライズのためには UICulture の都合上、
        /// オブジェクトを UI スレッドで生成する必要があります。そのため、
        /// メッセージオブジェクトを直接指定せず関数オブジェクトを経由する
        /// 形にしています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Show(this MainViewModel src, Func<MessageEventArgs> get) =>
            src.Send(() => src.Messenger.MessageBox.Publish(get()));

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

            var args = default(MessageEventArgs);
            src.Show(() => args = MessageFactory.CreateWarning(CreateMessage(dest, so)));
            Debug.Assert(args != null);
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

            src.Show(() => MessageFactory.CreateError(Properties.Resources.MessagePassword));
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

            src.Show(() => MessageFactory.CreateError(Properties.Resources.MessagePassword));
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
            var s0 = string.Format(Properties.Resources.MessageExists, path);
            var ok = new Dictionary<SaveOption, string>
            {
                { SaveOption.Overwrite, Properties.Resources.MessageOverwrite },
                { SaveOption.MergeHead, Properties.Resources.MessageMergeHead },
                { SaveOption.MergeTail, Properties.Resources.MessageMergeTail },
            }.TryGetValue(option, out var s1);

            Debug.Assert(ok);
            return $"{s0} {s1}";
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateMessage
        ///
        /// <summary>
        /// Gets the error message when a CryptographicException
        /// exception occurs.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string CreateMessage(CryptographicException err) =>
            Properties.Resources.MessageDigest;

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
            string.Format(Properties.Resources.MessageGhostscript, err.Status);

        /* ----------------------------------------------------------------- */
        ///
        /// CreateMessage
        ///
        /// <summary>
        /// PDF の結合中に暗号化に関わるエラーが発生した時のメッセージを
        /// 生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string CreateMessage(EncryptionException err) =>
            Properties.Resources.MessageMergePassword;

        #endregion

        #endregion
    }
}
