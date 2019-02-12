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
using Cube.Iteration;
using Cube.Pdf.App.Pinstaller.Debug;
using System;
using System.Reflection;

namespace Cube.Pdf.App.Pinstaller
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
        /// Executes the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                Logger.Configure();
                Logger.Info(LogType, Assembly.GetExecutingAssembly());
                Logger.Info(LogType, $"[ {string.Join(" ", args)} ]");

                foreach (var e in Printer.GetElements()) e.Log();

                var src = new ArgumentCollection(args, '/', true);
                var cmd = src.GetCommand();

                if (src.Count <= 0) Logger.Warn(LogType, "Configuration not found");
                else if (!cmd.HasValue()) Logger.Warn(LogType, "Command not found");
                else if (cmd.FuzzyEquals("Install")) Install(src, false);
                else if (cmd.FuzzyEquals("Reinstall")) Install(src, true);
                else if (cmd.FuzzyEquals("Uninstall")) Uninstall(src);
                else Logger.Warn(LogType, $"{cmd}:Unexpected command");
                return 0;
            }
            catch (Exception err) { Logger.Error(LogType, err.ToString()); }
            return -1;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Install
        ///
        /// <summary>
        /// Executes the installation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Install(ArgumentCollection src, bool reinstall)
        {
            var sec    = src.GetTimeout();
            var engine = src.CreateInstaller();

            Logger.Debug(LogType, $"Method:{nameof(Install).Quote()}");
            Logger.Debug(LogType, $"Configuration:{engine.Location.Quote()}");
            Logger.Debug(LogType, $"Resource:{engine.ResourceDirectory.Quote()}");

            engine.Try(src.GetRetryCount(), i =>
            {
                engine.Reinstall = reinstall;
                engine.Timeout   = TimeSpan.FromSeconds(sec * (i + 1));
                engine.Install();
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Executes the uninstallation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Uninstall(ArgumentCollection src)
        {
            var sec    = src.GetTimeout();
            var engine = src.CreateInstaller();

            Logger.Debug(LogType, $"Method:{nameof(Uninstall).Quote()}");
            Logger.Debug(LogType, $"Configuration:{engine.Location.Quote()}");

            engine.Try(src.GetRetryCount(), i =>
            {
                engine.Timeout = TimeSpan.FromSeconds(sec * (i + 1));
                engine.Uninstall();
            });
        }

        #endregion

        #region Fields
        private static readonly Type LogType = typeof(Program);
        #endregion
    }
}
