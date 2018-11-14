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
                Logger.Info(LogType, $"Arguments:{string.Join(" ", args)}");

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
        private static void Install(ArgumentCollection args)
        {
            var config = args.GetConfiguration();
            var src    = new Installer(Format.Json, config);
            var dir    = args.GetResourceDirectory();
            var app    = args.GetApplication();
            var proxy  = args.GetProxy();

            if (app.HasValue())
            {
                if (proxy.HasValue())
                {
                    src.Application = proxy;
                    src.Arguments   = $"/Exec {app.Quote()}";
                }
                else src.Application = app;
            }

            Logger.Debug(LogType, $"Method:{nameof(Install)}");
            Logger.Debug(LogType, $"Configuration:{config}");
            Logger.Debug(LogType, $"Resource:{dir}");
            Logger.Debug(LogType, $"{nameof(src.Application)}:{src.Application}");
            Logger.Debug(LogType, $"{nameof(src.Arguments)}:{src.Arguments}");

            Invoke(args.GetRetryCount(), () => src.Install(dir, true));
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
        private static void Uninstall(ArgumentCollection args)
        {
            var config = args.GetConfiguration();
            var src    = new Installer(Format.Json, config);

            Logger.Debug(LogType, $"Method:{nameof(Uninstall)}");
            Logger.Debug(LogType, $"Configuration:{config}");

            Invoke(args.GetRetryCount(), () => src.Uninstall());
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
                catch (Exception e) { Logger.Warn(typeof(Program), e.ToString(), e); }
            }
            throw new ArgumentException($"Try {n} times.");
        }

        #endregion

        #region Fields
        private static readonly Type LogType = typeof(Program);
        #endregion
    }
}
