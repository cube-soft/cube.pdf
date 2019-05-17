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
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Cube.Pdf.Converter
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
        /// Save
        ///
        /// <summary>
        /// Saves the current settings.
        /// </summary>
        ///
        /// <param name="src">MainViewModel</param>
        /// <param name="model">Model object for the MainViewModel.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Save(this MainViewModel src, MainFacade model)
        {
            var data = model.Settings.Value.Metadata;
            var show = data.Title.HasValue()   ||
                       data.Author.HasValue()  ||
                       data.Subject.HasValue() ||
                       data.Keywords.HasValue();

            if (show)
            {
                var msg = MessageFactory.CreateWarningMessage(Properties.Resources.MessageSave);
                src.Show(msg);
                Debug.Assert(msg != null);
                if (msg.Result == DialogResult.Cancel) return;
            }

            model.Save();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Show
        ///
        /// <summary>
        /// Shows an error message.
        /// </summary>
        ///
        /// <param name="src">MainViewModel</param>
        /// <param name="error">Exception object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Show(this MainViewModel src, Exception error)
        {
            if (error is OperationCanceledException) return;

            src.LogError(error);
            var msg = error is GsApiException gse         ? CreateMessage(gse) :
                      error is EncryptionException ece    ? CreateMessage(ece) :
                      error is CryptographicException cpe ? CreateMessage(cpe) :
                      $"{error.Message} ({error.GetType().Name})";
            src.Show(MessageFactory.CreateErrorMessage(msg));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Show
        ///
        /// <summary>
        /// Shows the specified message.
        /// </summary>
        ///
        /// <param name="src">MainViewModel</param>
        /// <param name="msg">Message object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Show(this MainViewModel src, MessageEventArgs msg) =>
            src.Send(() => src.Messenger.MessageBox.Publish(msg));

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

            var msg = MessageFactory.CreateWarningMessage(CreateMessage(dest, so));
            src.Show(msg);
            Debug.Assert(msg != null);
            return msg.Result != DialogResult.Cancel;
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
            var eo = src.Encryption;
            if (!eo.Enabled || eo.OwnerPassword.FuzzyEquals(eo.OwnerConfirm)) return true;

            src.Show(MessageFactory.CreateErrorMessage(Properties.Resources.MessagePassword));
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
            var eo = src.Encryption;
            if (!eo.Enabled || !eo.OpenWithPassword || eo.UseOwnerPassword) return true;
            if (eo.UserPassword.FuzzyEquals(eo.UserConfirm)) return true;

            src.Show(MessageFactory.CreateErrorMessage(Properties.Resources.MessagePassword));
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
