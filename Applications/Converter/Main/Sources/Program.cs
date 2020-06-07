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
using Cube.Collections;
using Cube.DataContract;
using Cube.FileSystem;
using System;
using System.Reflection;
using System.Windows.Forms;

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
        static void Main(string[] raw)
        {
            try
            {
                Logger.ObserveTaskException();
                Logger.Info(LogType, Assembly.GetExecutingAssembly());
                Logger.Info(LogType, $"Ghostscript {GetGsVersion()}");
                Logger.Info(LogType, $"[ {string.Join(" ", raw)} ]");

                ApplicationSetting.Configure();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var args = new ArgumentCollection(raw, Argument.Windows, true);
                var setting = CreateSetting(args, Assembly.GetExecutingAssembly());
                setting.Load();
                setting.Normalize();
                setting.Set(args);

                if (args.Options.ContainsKey("SkipUI")) Execute(setting);
                else Show(setting);
            }
            catch (Exception err) { Logger.Error(LogType, err); }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Show
        ///
        /// <summary>
        /// Shows the main window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Show(SettingFolder settings)
        {
            var view = new MainWindow();
            view.Bind(new MainViewModel(settings));
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
        /* ----------------------------------------------------------------- */
        private static void Execute(SettingFolder settings)
        {
            using (var src = new Facade(settings)) src.Invoke();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateSetting
        ///
        /// <summary>
        /// Creates a new instance of the SettingFolder class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SettingFolder CreateSetting(ArgumentCollection src, Assembly asm) =>
            src.Options.TryGetValue("Setting", out var subkey) ?
            new SettingFolder(asm, Format.Registry, subkey, new IO()) :
            new SettingFolder(asm);

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
            catch (Exception err) { Logger.Warn(LogType, err); }
            return -1;
        }

        #endregion

        #region Fields
        private static readonly Type LogType = typeof(Program);
        #endregion
    }
}
