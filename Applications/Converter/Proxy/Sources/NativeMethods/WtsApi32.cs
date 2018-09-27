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
using System.Runtime.InteropServices;

namespace Cube.WtsApi32
{
    /* --------------------------------------------------------------------- */
    ///
    /// WtsApi32.NativeMethods
    ///
    /// <summary>
    /// wtsapi32.dll に定義された関数を宣言するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        /* ----------------------------------------------------------------- */
        ///
        /// WTSEnumerateSessions
        ///
        /// <summary>
        /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/aa383833.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern bool WTSEnumerateSessions(
            IntPtr hServer,
            uint Reserved,
            uint Version,
            ref IntPtr ppSessionInfo,
            ref uint pSessionInfoCount
        );

        /* ----------------------------------------------------------------- */
        ///
        /// WTSQuerySessionInformation
        ///
        /// <summary>
        /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/aa383838.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern bool WTSQuerySessionInformation(
            IntPtr hServer,
            int sessionId,
            WTS_INFO_CLASS wtsInfoClass,
            out IntPtr ppBuffer,
            out uint pBytesReturned
        );

        /* ----------------------------------------------------------------- */
        ///
        /// WTSQueryUserToken
        ///
        /// <summary>
        /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/aa383840.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern bool WTSQueryUserToken(uint sessionId, out IntPtr token);

        /* ----------------------------------------------------------------- */
        ///
        /// WTSFreeMemory
        ///
        /// <summary>
        /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/aa383834.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern void WTSFreeMemory(IntPtr pMemory);

        #region Fields
        const string LibName = "wtsapi32.dll";
        #endregion
    }
}
