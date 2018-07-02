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
namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionMethod
    ///
    /// <summary>
    /// PDF で使用可能な暗号化方式を定義した列挙型です。
    /// </summary>
    ///
    /// <remarks>
    /// 現在のところ、以下の暗号化方式を使用する事ができます。
    /// 括弧内の値は、最初にサポートされた PDF バージョンを表します。
    /// -  40bit RC4 (PDF 1.1)
    /// - 128bit RC4 (PDF 1.4)
    /// - 128bit AES (PDF 1.5)
    /// - 256bit AES (PDF 1.7 ExtensionLevel 3)
    /// - 256bit AES Revision 6 (Acrobat X)
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public enum EncryptionMethod
    {
        /// <summary>Unknown</summary>
        Unknown = 0,
        /// <summary>Revision 2, 40bit RC4 (supported from PDF 1.1)</summary>
        Standard40 = 2,
        /// <summary>Revision 3, 128bit RC4 (supported from PDF 1.4)</summary>
        Standard128 = 3,
        /// <summary>Revision 4, 128bit AES (supported from PDF 1.5)</summary>
        Aes128 = 4,
        /// <summary>Revision 5, 256bit AES (supported from PDF 1.7 ExtensionLevel 3)</summary>
        Aes256 = 5,
        /// <summary>Revision 6, 256bit AES (supported from Acrobat X)</summary>
        Aes256r6 = 6,
    }
}
