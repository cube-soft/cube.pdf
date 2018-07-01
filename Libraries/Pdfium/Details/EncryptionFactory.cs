/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.Pdf.Pdfium.PdfiumApi;
using System;

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionFactory
    ///
    /// <summary>
    /// Encryption の生成用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class EncryptionFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// 暗号化情報を取得します。
        /// </summary>
        ///
        /// <param name="core">PDFium 用オブジェクト</param>
        /// <param name="password">パスワード</param>
        ///
        /// <returns>Encryption</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Encryption Create(IntPtr core, string password)
        {
            var src = NativeMethods.FPDF_GetSecurityHandlerRevision(core);
            return src == -1 ?
                   new Encryption() :
                   new Encryption
                   {
                       Enabled          = true,
                       OwnerPassword    = password,
                       OpenWithPassword = false,
                       Method           = (EncryptionMethod)src,
                       Permission       = CreatePermission(core),
                   };
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreatePermission
        ///
        /// <summary>
        /// 各種操作に対する許可情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Permission CreatePermission(IntPtr core)
        {
            var src = NativeMethods.FPDF_GetDocPermissions(core);
            return src == ulong.MaxValue ?
                   new Permission() :
                   new Permission((long)src);
        }

        #endregion
    }
}
