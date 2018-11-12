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
        static void Main(string[] args)
        {
            var type = typeof(Program);

            try
            {
                Logger.Configure();
                Logger.ObserveTaskException();
                Logger.Info(type, Assembly.GetExecutingAssembly());
                Logger.Info(type, $"Arguments:{string.Join(" ", args)}");

                var src = new ArgumentCollection(args, '-');
                var cmd = src.GetCommand();

                if (src.Count <= 0) Logger.Warn(type, "Configuration not found");
                else if (!cmd.HasValue()) Logger.Warn(type, "Command not found");
                else if (cmd == "install") Install(src);
                else if (cmd == "uninstall") Uninstall(src);
                else Logger.Warn(type, $"{cmd}:Unexpected command");
            }
            catch (Exception err)
            {
                Environment.ExitCode = -1;
                Logger.Error(type, err.ToString());
            }
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
            var src = new Installer(Format.Json, args[0]);
            var dir = args.GetResourceDirectory();
            Try(args.GetRetryCount(), () => src.Install(dir, true));
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
            var src = new Installer(Format.Json, args[0]);
            Try(args.GetRetryCount(), () => src.Uninstall());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Try
        ///
        /// <summary>
        /// Executes the specified action until it succeeds.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Try(int n, Action action)
        {
            for (var i = 0; i < n; ++i)
            {
                try { action(); break; }
                catch (Exception e) { Logger.Warn(typeof(Program), e.ToString(), e); }
            }
        }

        #endregion
    }
}
