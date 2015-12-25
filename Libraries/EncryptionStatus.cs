/* ------------------------------------------------------------------------- */
///
/// EncryptionStatus.cs
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
    /// Cube.Pdf.EncryptionStatus
    /// 
    /// <summary>
    /// 暗号化されている PDF ファイルへのアクセス（許可）状態を定義した
    /// 列挙型です。
    /// </summary>
    /// 
    /// <remarks>
    /// EncriptionStatus の各値の意味は以下の通りです。
    /// 
    /// NotEncrypted     : このファイルは暗号化されていません
    /// RestrictedAccess : ユーザパスワードで開いています
    /// FullAccess       : オーナパスワードで開いています
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public enum EncryptionStatus
    {
        NotEncrypted     = 0,
        RestrictedAccess = 1,
        FullAccess       = 2,
    }
}
