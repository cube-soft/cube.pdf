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
using System.Linq;
using System.Security.Cryptography;
using Cube.FileSystem;
using Cube.Mixin.Collections;
using Cube.Mixin.String;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// DigestChecker
    ///
    /// <summary>
    /// Provides functionality to check the provided digest.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class DigestChecker
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DigestChecker
        ///
        /// <summary>
        /// Initializes a new instance of the DigestChecker class with the
        /// specified settings.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DigestChecker(SettingFolder src) { Settings = src; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Setting
        ///
        /// <summary>
        /// Gets the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder Settings { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the checking.
        /// </summary>
        ///
        /// <remarks>
        /// コマンドラインから入力ファイルに対する SHA-256 の値を指定された時のみ
        /// チェックし、それ以外の場合は何もせずに終了します。
        /// また、SHA256CryptoServiceProvider が PlatformNotSupportedException
        /// を送出した際、PlatformCompatible が有効な場合もこのチェックは無視
        /// されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke()
        {
            var src = Settings.Digest;
            if (!src.HasValue()) return;

            try
            {
                var cmp = Compute(Settings.Value.Source);
                if (!src.FuzzyEquals(cmp)) throw new CryptographicException();
            }
            catch (PlatformNotSupportedException)
            {
                if (!Settings.Value.PlatformCompatible) throw;
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Compute
        ///
        /// <summary>
        /// Computes the SHA-256 hash of the specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string Compute(string src) => IoEx.Load(src, e =>
            new SHA256CryptoServiceProvider().ComputeHash(e).Join("", b => $"{b:x2}"));


        #endregion
    }
}
