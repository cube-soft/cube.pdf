/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// Program
    ///
    /// <summary>
    /// Represents the main program of the CubePDF Utility.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static class Program
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Main
        ///
        /// <summary>
        /// Executes the main program.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [STAThread]
        static void Main(string[] args)
        {
            var mutex = new Mutex(true, "CubePdfUtilitySplash", out var created);
            if (!created) return;

            try
            {
                if (IsSkip()) { Start(args); return; }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var view = new MainWindow();
                view.Shown += (s, e) =>
                {
                    try { Start(args); }
                    catch (Exception err) { view.Error(err); }
                };

                Application.Run(view);
            }
            catch (Exception err) { Trace.WriteLine(err.ToString()); }
            finally { mutex.ReleaseMutex(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        ///
        /// <summary>
        /// Starts the main process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static void Start(string[] args)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Process.Start(new ProcessStartInfo
            {
                FileName  = Path.Combine(dir, $"{Target}.exe"),
                Arguments = string.Join(" ", args.Select(s => $"\"{s}\"")),
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsSkip
        ///
        /// <summary>
        /// Gets a value indicating whether to skip displaying the splash
        /// window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static bool IsSkip()
        {
            var p = Process.GetProcessesByName(Target);
            var n = p?.Length ?? 0;
            return n > 0;
        }

        #endregion

        #region Fields
        private static readonly string Target = "CubePdfUtility";
        #endregion
    }
}
