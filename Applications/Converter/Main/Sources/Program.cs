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
using System.Reflection;
using System.Windows.Forms;
using Cube.Collections;
using Cube.DataContract;
using Cube.Mixin.Collections;
using Cube.Pdf.Converter.Mixin;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// Program
    ///
    /// <summary>
    /// Represents the main program.
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
        /// Executes the main program of the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [STAThread]
        static void Main(string[] raw) => Source.LogError(() =>
        {
            _ = Logger.ObserveTaskException();
            Source.LogInfo(Assembly.GetExecutingAssembly());
            Source.LogInfo($"Ghostscript {GetGsVersion()}");
            Source.LogInfo($"[ {raw.Join(" ")} ]");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var args = new ArgumentCollection(raw, Argument.Windows, true);
            using var src = Create(args);
            src.Load();
            src.Normalize();
            src.Set(args);

            if (args.Options.ContainsKey("SkipUI")) Execute(src);
            else Show(src);
        });

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the SettingFolder class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SettingFolder Create(ArgumentCollection src) =>
            src.Options.TryGetValue("Setting", out var subkey) ?
            new(Format.Registry, subkey) :
            new();

        /* ----------------------------------------------------------------- */
        ///
        /// Show
        ///
        /// <summary>
        /// Shows the main window.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        private static void Show(SettingFolder src)
        {
            var view = new MainWindow();
            view.Bind(new MainViewModel(src));
            Application.Run(view);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// Executes the conversion directly.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        private static void Execute(SettingFolder src)
        {
            using var facade = new Facade(src);
            facade.Invoke();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetGsVersion
        ///
        /// <summary>
        /// Gets a version number of the Ghostscript.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static int GetGsVersion()
        {
            try { return Ghostscript.Converter.Revision; }
            catch (Exception err) { Source.LogWarn(err); }
            return -1;
        }

        #endregion

        #region Fields
        private static readonly Type Source = typeof(Program);
        #endregion
    }
}
