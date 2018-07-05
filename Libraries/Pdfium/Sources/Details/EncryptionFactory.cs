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
    internal static class EncryptionFactory
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
        /// <remarks>
        /// 現在 FPDF_GetDocPermissions の結果で諸々の判定を行っているが
        /// 最終的には OwnerPassword で開いた状態でもオリジナルの
        /// Permission を取得する事を目指す。それに伴って、各種操作も
        /// 修正する必要がある。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Encryption Create(IntPtr core, string password)
        {
            var method     = Facade.FPDF_GetSecurityHandlerRevision(core);
            var permission = Facade.FPDF_GetDocPermissions(core);
            var restrict   = permission != 0xfffffffc;

            return method == -1 ?
                   new Encryption() :
                   new Encryption
                   {
                       Enabled          = true,
                       OwnerPassword    = restrict ? string.Empty : password,
                       OpenWithPassword = restrict,
                       UserPassword     = restrict ? password : string.Empty,
                       Method           = CreateMethod(method),
                       Permission       = new Permission((long)permission),
                   };
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateMethod
        ///
        /// <summary>
        /// 暗号化方式を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static EncryptionMethod CreateMethod(int src) =>
            Enum.IsDefined(typeof(EncryptionMethod), src) ?
            (EncryptionMethod)src :
            EncryptionMethod.Unknown;

        #endregion
    }
}
