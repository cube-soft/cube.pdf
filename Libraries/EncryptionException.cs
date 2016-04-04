/* ------------------------------------------------------------------------- */
///
/// EncryptionException.cs
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
using System;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionException
    /// 
    /// <summary>
    /// 暗号化に関する例外を送出するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class EncryptionException : Exception
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionException
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionException() : base() { }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionException
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionException(string message) : base(message) { }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionException
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionException(string message, Exception innerException) : base(message, innerException) { }

        #endregion
    }
}
