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

namespace Cube.AdvApi32
{
    /* --------------------------------------------------------------------- */
    ///
    /// AdvApi32.NativeMethods
    ///
    /// <summary>
    /// advapi32.dll に定義された関数を宣言するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateProcessAsUser
        ///
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms682429.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation
        );

        /* ----------------------------------------------------------------- */
        ///
        /// OpenThreadToken
        ///
        /// <summary>
        /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/aa379296.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern bool OpenThreadToken(
            IntPtr ThreadHandle,
            uint DesiredAccess,
            bool OpenAsSelf,
            ref IntPtr TokenHandle
        );

        /* ----------------------------------------------------------------- */
        ///
        /// DuplicateTokenEx
        ///
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa446617.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            uint dwDesiredAccess,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            int ImpersonationLevel,
            int dwTokenType,
            ref IntPtr phNewToken
        );

        /* ----------------------------------------------------------------- */
        ///
        /// RevertToSelf
        ///
        /// <summary>
        /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/aa379317.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern bool RevertToSelf();

        #endregion

        #region Fields
        private const string LibName = "advapi32.dll";
        #endregion
    }
}
