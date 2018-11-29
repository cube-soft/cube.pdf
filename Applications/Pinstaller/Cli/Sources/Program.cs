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
using Cube.DataContract;
using Cube.Generics;
using Cube.Log;
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
                var sop = StringComparison.InvariantCultureIgnoreCase;

                if (src.Count <= 0) Logger.Warn(LogType, "Configuration not found");
                else if (!cmd.HasValue()) Logger.Warn(LogType, "Command not found");
                else if (cmd.Equals("install", sop)) Install(src);
                else if (cmd.Equals("uninstall", sop)) Uninstall(src);
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
        private static void Install(ArgumentCollection src)
        {
            var ts     = TimeSpan.FromSeconds(src.GetTimeout());
            var config = src.GetConfiguration();
            var engine = new Installer(Format.Json, config) { Timeout = ts };
            var dir    = src.GetResourceDirectory();

            Logger.Debug(LogType, $"Method:{nameof(Install).Quote()}");
            Logger.Debug(LogType, $"Configuration:{config.Quote()}");
            Logger.Debug(LogType, $"Resource:{dir.Quote()}");

            Normalize(src, engine.Config);
            Invoke(src.GetRetryCount(), () => engine.Install(dir, true));
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
            var ts     = TimeSpan.FromSeconds(src.GetTimeout());
            var config = src.GetConfiguration();
            var engine = new Installer(Format.Json, config) { Timeout = ts };

            Logger.Debug(LogType, $"Method:{nameof(Uninstall).Quote()}");
            Logger.Debug(LogType, $"Configuration:{config.Quote()}");

            Invoke(src.GetRetryCount(), () => engine.Uninstall());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Normalize
        ///
        /// <summary>
        /// Normalizes some information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Normalize(ArgumentCollection src, DeviceConfig config)
        {
            var ca = Environment.SpecialFolder.CommonApplicationData.GetName();
            foreach (var e in config.Ports)
            {
                e.Temp        = System.IO.Path.Combine(ca, e.Temp);
                e.Application = src.ReplaceDirectory(e.Application);
                e.Arguments   = src.ReplaceDirectory(e.Arguments);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Executes the specified action until it succeeds.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Invoke(int n, Action action)
        {
            for (var i = 0; i < n; ++i)
            {
                try { action(); return; }
                catch (Exception e) { Logger.Warn(LogType, e.ToString(), e); }
            }
            throw new ArgumentException($"Try {n} times.");
        }

        #endregion

        #region Fields
        private static readonly Type LogType = typeof(Program);
        #endregion
    }
}
