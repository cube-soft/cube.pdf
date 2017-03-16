/* ------------------------------------------------------------------------- */
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
    /// Attachment
    /// 
    /// <summary>
    /// 添付ファイルを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Attachment
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        /// 
        /// <summary>
        /// 添付ファイルの名前を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// File
        /// 
        /// <summary>
        /// 添付オブジェクトが属するファイルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileBase File { get; set; } = null;

        #endregion
    }
}
