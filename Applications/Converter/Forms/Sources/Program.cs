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

namespace Cube.Pdf.App.Converter
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
        static void Main(string[] args)
        {
            try
            {
                Logger.Configure();
                Logger.ObserveTaskException();
                Logger.Info(LogType, Assembly.GetExecutingAssembly());
                Logger.Info(LogType, $"[ {string.Join(" ", args)} ]");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var src      = new ArgumentCollection(args, '/', true);
                var settings = CreateSettings(src);
                settings.Load();
                settings.Set(src);

                if (settings.Value.SkipUi) Invoke(settings);
                else Show(settings);
            }
            catch (Exception err) { Logger.Error(LogType, err); }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateSettings
        ///
        /// <summary>
        /// Creates a new instance of the SettingsFolder class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SettingsFolder CreateSettings(ArgumentCollection src) =>
            src.Options.TryGetValue("Settings", out var subkey) ?
            new SettingsFolder(Format.Registry, subkey, new IO()) :
            new SettingsFolder();

        /* ----------------------------------------------------------------- */
        ///
        /// Show
        ///
        /// <summary>
        /// Shows the main window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Show(SettingsFolder settings)
        {
            var view = new MainForm();
            using (var vm = new MainViewModel(settings))
            {
                view.Bind(vm);
                Application.Run(view);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the conversion directly.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Invoke(SettingsFolder settings)
        {
            using (var src = new MainFacade(settings))
            {
                src.UpdateExtension();
                src.Convert();
            }
        }

        #endregion

        #region Fields
        private static readonly Type LogType = typeof(Program);
        #endregion
    }
}
