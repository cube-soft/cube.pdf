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
                Logger.Info(LogType, $"Arguments:[ {string.Join(" ", args)} ]");

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
            var config = src.GetConfiguration();
            var engine = new Installer(Format.Json, config);
            var dir    = src.GetResourceDirectory();
            var app    = src.GetApplication();
            var args   = src.GetArguments();

            Logger.Debug(LogType, $"Method:{nameof(Install).Quote()}");
            Logger.Debug(LogType, $"Configuration:{config.Quote()}");
            Logger.Debug(LogType, $"Resource:{dir.Quote()}");
            Logger.Debug(LogType, $"Application:{app.Quote()}");
            Logger.Debug(LogType, $"Arguments:[ {args} ]");

            Normalize(engine.Config, app, args);
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
            var config = src.GetConfiguration();
            var engine = new Installer(Format.Json, config);

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
        private static void Normalize(DeviceConfig src, string app, string args)
        {
            var root = Environment.SpecialFolder.CommonApplicationData.GetName();
            foreach (var e in src.Ports) e.Temp = System.IO.Path.Combine(root, e.Temp);
            if (src.Ports.Count != 1 || !app.HasValue()) return;

            src.Ports[0].Application = app;
            src.Ports[0].Arguments   = args;
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
