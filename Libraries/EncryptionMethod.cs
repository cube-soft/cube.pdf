/* ------------------------------------------------------------------------- */
///
/// EncryptionMethod.cs
/// 
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionMethod
    /// 
    /// <summary>
    /// PDF の暗号化の際に使用可能な暗号化方式を定義した列挙型です。
    /// </summary>
    /// 
    /// <remarks>
    /// 現在のところ、以下の暗号化方式を使用する事ができます（括弧内の値は、
    /// 最初にサポートされた PDF バージョンを表します）。
    /// -  40bit RC4 (PDF 1.1)
    /// - 128bit RC4 (PDF 1.4)
    /// - 128bit AES (PDF 1.5)
    /// - 256bit AES (PDF 1.7 ExtensionLevel 3)
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public enum EncryptionMethod
    {
        Standard40,     //  40bit RC4
        Standard128,    // 128bit RC4
        Aes128,         // 128bit AES
        Aes256,         // 256bit AES
        Unknown = -1,
    }
}
