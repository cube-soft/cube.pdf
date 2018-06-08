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
using System.Threading;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// OnceQuery
    ///
    /// <summary>
    /// 予め指定されたパスワードを一度だけ返す Query クラスです。
    /// </summary>
    ///
    /// <remarks>
    /// 空文字を指定した場合は初回実行時、それ以外の場合は 2 回目以降の
    /// 実行時に EncryptionException 例外を送出します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class OnceQuery : Query<string>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// OneTimeQuery
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="password">パスワード</param>
        ///
        /* ----------------------------------------------------------------- */
        public OnceQuery(string password)
        {
            var field = password;

            Requested += (s, e) =>
            {
                e.Cancel = false;
                e.Result = Interlocked.Exchange(ref field, null);

                if (string.IsNullOrEmpty(e.Result))
                {
                    var msg = Properties.Resources.ErrorPassword;
                    throw new EncryptionException(msg);
                }
            };
        }

        #endregion
    }
}
