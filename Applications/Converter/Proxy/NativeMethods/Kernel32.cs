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

namespace Cube.Kernel32
{
    /* --------------------------------------------------------------------- */
    ///
    /// Kernel32.NativeMethods
    ///
    /// <summary>
    /// kernel32.dll に定義された関数を宣言するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        /* ----------------------------------------------------------------- */
        ///
        /// OpenThread
        ///
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms684335.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern IntPtr OpenThread(
            uint dwDesiredAccess,
            bool bInheritHandle,
            uint dwThreadId
        );

        /* ----------------------------------------------------------------- */
        ///
        /// CloseHandle
        ///
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms724211.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        #region Fields
        const string LibName = "kernel32.dll";
        #endregion
    }
}
