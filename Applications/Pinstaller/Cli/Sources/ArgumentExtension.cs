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
using Cube.Collections;
using Cube.Generics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArgumentExtension
    ///
    /// <summary>
    /// Provides extended methods of the ArgumentCollection class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ArgumentExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetCommand
        ///
        /// <summary>
        /// Gets the command name from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source arguments.</param>
        ///
        /// <returns>Command name.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetCommand(this ArgumentCollection src) =>
            src.Options.TryGetValue("Command", out var dest) ? dest : string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// GetConfiguration
        ///
        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        ///
        /// <param name="src">Source arguments.</param>
        ///
        /// <returns>Configuration path.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetConfiguration(this ArgumentCollection src) =>
            src.GetPath(src.FirstOrDefault());

        /* ----------------------------------------------------------------- */
        ///
        /// GetResourceDirectory
        ///
        /// <summary>
        /// Gets the resource directory from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source arguments.</param>
        ///
        /// <returns>Resource directory.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetResourceDirectory(this ArgumentCollection src) =>
            src.Options.TryGetValue("Resource", out var dest) ? src.GetPath(dest) : _current;

        /* ----------------------------------------------------------------- */
        ///
        /// GetTimeout
        ///
        /// <summary>
        /// Gets the timeout value in seconds.
        /// </summary>
        ///
        /// <param name="src">Source arguments.</param>
        ///
        /// <returns>Timeout value.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static int GetTimeout(this ArgumentCollection src)
        {
            if (!src.Options.TryGetValue("Timeout", out var str)) return 30;
            if (int.TryParse(str, out var dest)) return dest;
            else return 30;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRetryCount
        ///
        /// <summary>
        /// Gets the maximum retry count.
        /// </summary>
        ///
        /// <param name="src">Source arguments.</param>
        ///
        /// <returns>Retry count.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static int GetRetryCount(this ArgumentCollection src)
        {
            if (!src.Options.TryGetValue("Retry", out var str)) return 1;
            if (int.TryParse(str, out var dest)) return dest;
            else return 1;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// HasForceOption
        ///
        /// <summary>
        /// Gets a value indicating whether to uninstall devices forcibly.
        /// </summary>
        ///
        /// <param name="src">Source arguments.</param>
        ///
        /// <returns>
        /// true for having the Force option; otherwise false.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool HasForceOption(this ArgumentCollection src) =>
            src.Options.ContainsKey("Force");

        /* ----------------------------------------------------------------- */
        ///
        /// ReplaceDirectory
        ///
        /// <summary>
        /// Replace all %%DIR%% strings with the current directory.
        /// </summary>
        ///
        /// <param name="src">Source arguments.</param>
        /// <param name="input">Source string.</param>
        ///
        /// <returns>Replaced string.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string ReplaceDirectory(this ArgumentCollection src, string input) =>
            input.HasValue() ?
            Regex.Replace(input, "%%DIR%%", _current, RegexOptions.IgnoreCase) :
            input;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetPath
        ///
        /// <summary>
        /// Gets the path from the specified parameters.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetPath(this ArgumentCollection src, string path) =>
            path.HasValue() && src.Options.ContainsKey("Relative") ?
            System.IO.Path.Combine(_current, path) :
            path;

        #endregion

        #region Fields
        private static readonly string _current = new AssemblyReader(Assembly.GetExecutingAssembly()).DirectoryName;
        #endregion
    }
}
