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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// IInstallerExtension
    ///
    /// <summary>
    /// Provides extended methods for IInstaller implemented classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class IInstallerExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnvironment
        ///
        /// <summary>
        /// Gets the name of current architecture.
        /// </summary>
        ///
        /// <param name="src">IInstaller implementation.</param>
        ///
        /// <returns>Name of architecture.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetEnvironment(this IInstaller src) =>
            (IntPtr.Size == 4) ? "Windows NT x86" : "Windows x64";

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified function and throws an exception if
        /// error occurs.
        /// </summary>
        ///
        /// <param name="src">IInstaller implementation.</param>
        /// <param name="func">Callback function.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Invoke(this IInstaller src, Func<int> func)
        {
            if (func() == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        #endregion
    }
}
