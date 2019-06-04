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
using System;

namespace Cube.Pdf.Converter.Proxy
{
    /* --------------------------------------------------------------------- */
    ///
    /// Program
    ///
    /// <summary>
    /// メインプログラムを表すクラスです。
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
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        ///
        /// <param name="args">プログラム引数</param>
        ///
        /* ----------------------------------------------------------------- */
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Logger.Configure();
                Logger.Info(LogType, System.Reflection.Assembly.GetExecutingAssembly());
                Logger.Info(LogType, $"[ {string.Join(" ", args)} ]");

                var proc = StartAs(args);
                proc.EnableRaisingEvents = true;
                proc.Exited += (s, e) =>
                {
                    if (s is System.Diagnostics.Process p)
                    {
                        Logger.Info(LogType, $"ExitCode:{(uint)p.ExitCode}");
                    }
                };
                proc.WaitForExit();
            }
            catch (Exception err) { Logger.Error(LogType, err); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// Process オブジェクトを生成し、開始します。
        /// </summary>
        ///
        /// <param name="args">プログラム引数</param>
        ///
        /// <remarks>
        /// 開始された Process オブジェクト
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        static System.Diagnostics.Process StartAs(string[] args)
        {
            var src = new ArgumentCollection(args, Argument.Windows, true);
            if (!src.Options.TryGetValue("Exec", out var exec)) throw new ArgumentException("Exec");

            try
            {
                src.Options.TryGetValue("UserName", out var user);
                return Process.StartAs(user, exec, args);
            }
            catch (Exception err)
            {
                if (!src.Options.TryGetValue("ThreadID", out var id)) throw;
                Logger.Warn(LogType, err);
                Logger.Info(LogType, $"Use ThreadID ({id})");
                return Process.StartAs(uint.Parse(id), exec, args);
            }
        }

        #endregion

        #region Fields
        private static readonly Type LogType = typeof(Program);
        #endregion
    }
}
