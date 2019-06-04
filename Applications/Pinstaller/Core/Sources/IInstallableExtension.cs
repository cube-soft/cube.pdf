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
using Cube.FileSystem;
using Cube.Mixin.Iteration;
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// IInstallableExtension
    ///
    /// <summary>
    /// Provides extended methods for IInstallable implemented classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class IInstallableExtension
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
        public static string GetEnvironment(this IInstallable src) =>
            (IntPtr.Size == 4) ? "Windows NT x86" : "Windows x64";

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// Copies the specified file.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        /// <param name="filename">Filename to be copied.</param>
        /// <param name="from">Source directory.</param>
        /// <param name="to">Destination directory.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Copy(this IO io, string filename, string from, string to)
        {
            if (!filename.HasValue()) return;
            var src  = io.Combine(from, filename);
            var dest = io.Combine(to, filename);
            if (!io.Exists(src)) return;

            io.LogDebug($"[{nameof(Copy)}]", $"From:{src.Quote()}", $"To:{dest.Quote()}");
            io.Copy(src, dest, true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Try
        ///
        /// <summary>
        /// Tries the specified action.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="action">Action to be invoked.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Try(this IInstallable src, Action<int> action)
        {
            var errors = new List<Exception>();
            src.RetryCount.Try(i => action(i), errors);
            if (errors.Any()) throw errors.Last();
        }

        #endregion
    }
}
