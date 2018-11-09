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
using Cube.Generics;
using Cube.Log;
using System.Runtime.CompilerServices;

namespace Cube.Pdf.App.Pinstaller.Log
{
    /* --------------------------------------------------------------------- */
    ///
    /// Logger
    ///
    /// <summary>
    /// Provides extended methods to put debug information.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Logger
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Debug
        ///
        /// <summary>
        /// Puts debug information to the log file.
        /// </summary>
        ///
        /// <param name="src">Port monitor object.</param>
        /// <param name="name">Method name.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Debug(this PortMonitor src, [CallerMemberName] string name = null) =>
            src.LogDebug(string.Join("\t",
                $"Method:{name.Quote()}",
                $"{nameof(src.Name)}:{src.Name.Quote()}",
                $"{nameof(src.FileName)}:{src.FileName.Quote()}",
                $"{nameof(src.Config)}:{src.Config.Quote()}",
                $"{nameof(src.Environment)}:{src.Environment.Quote()}"
            ));

        #endregion
    }
}
