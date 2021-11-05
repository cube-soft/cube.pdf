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
using System;
using Cube.Collections;
using Cube.Mixin.Collections;

namespace Cube.Pdf.Converter.Proxy
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
        static void Main(string[] args) => Source.LogError(() =>
        {
            Source.LogInfo(System.Reflection.Assembly.GetExecutingAssembly());
            Source.LogInfo($"[ {args.Join(" ")} ]");

            var proc = StartAs(args);
            proc.EnableRaisingEvents = true;
            proc.Exited += (s, e) =>
            {
                if (s is System.Diagnostics.Process p)
                {
                    Source.LogInfo($"ExitCode:{(uint)p.ExitCode}");
                }
            };
            proc.WaitForExit();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// Creates a new instance of the Process class with the specified
        /// arguments and executes it.
        /// </summary>
        ///
        /// <param name="args">Program arguments.</param>
        ///
        /// <returns>Process object that was started.</returns>
        ///
        /* ----------------------------------------------------------------- */
        static System.Diagnostics.Process StartAs(string[] args)
        {
            var src = new ArgumentCollection(args, Argument.Windows, true);
            if (!src.Options.TryGetValue("Exec", out var exec)) throw new ArgumentException("Exec");

            try
            {
                _ = src.Options.TryGetValue("UserName", out var user);
                return Process.StartAs(user, exec, args);
            }
            catch (Exception err)
            {
                if (!src.Options.TryGetValue("ThreadID", out var id)) throw;
                Source.LogWarn(err);
                Source.LogInfo($"Use ThreadID ({id})");
                return Process.StartAs(uint.Parse(id), exec, args);
            }
        }

        #endregion

        #region Fields
        private static readonly Type Source = typeof(Program);
        #endregion
    }
}
