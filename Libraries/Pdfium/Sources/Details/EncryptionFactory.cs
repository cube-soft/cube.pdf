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
using System;

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionFactory
    ///
    /// <summary>
    /// Provides factory methods of the Encryption class.
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
        /// Creates a new instance of the Encryption class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="core">PDFium object.</param>
        /// <param name="password">Password</param>
        ///
        /// <returns>Encryption object.</returns>
        ///
        /// <remarks>
        /// 現在 FPDF_GetDocPermissions の結果で諸々の判定を行っているが
        /// 最終的には OwnerPassword で開いた状態でもオリジナルの
        /// Permission を取得する事を目指す。それに伴って、各種操作も
        /// 修正する必要がある。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Encryption Create(PdfiumReader core, string password)
        {
            var method   = core.Invoke(NativeMethods.FPDF_GetSecurityHandlerRevision);
            var value    = (uint)core.Invoke(NativeMethods.FPDF_GetDocPermissions);
            var restrict = value != 0xfffffffc && value != 0xffffffff;

            return method == -1 ?
                   new Encryption() :
                   new Encryption
                   {
                       Enabled          = true,
                       OwnerPassword    = restrict ? string.Empty : password,
                       OpenWithPassword = restrict,
                       UserPassword     = restrict ? password : string.Empty,
                       Method           = CreateMethod(method),
                       Permission       = new Permission(value),
                   };
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateMethod
        ///
        /// <summary>
        /// Gets the encryption method from the specified value.
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
