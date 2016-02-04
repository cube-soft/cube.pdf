/* ------------------------------------------------------------------------- */
///
/// PermissionMethod.cs
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
    /// PermissionMethod
    /// 
    /// <summary>
    /// PDF への各種操作に対して設定されている許可状態を示す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum PermissionMethod : uint
    {
        Deny     = 0,
        Restrict = 1,
        Allow    = 2
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PermissionMethodEx
    /// 
    /// <summary>
    /// PermissionMethod の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class PermissionMethodEx
    {
        /* ----------------------------------------------------------------- */
        ///
        /// IsAllow
        /// 
        /// <summary>
        /// 許可状態かどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsAllow(this PermissionMethod obj)
        {
            return obj == PermissionMethod.Allow;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsDeny
        /// 
        /// <summary>
        /// 拒否状態かどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsDeny(this PermissionMethod obj)
        {
            return obj == PermissionMethod.Deny;
        }
    }
}
