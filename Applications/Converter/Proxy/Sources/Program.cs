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
using Cube.Log;
using System;

namespace Cube.Pdf.App.Proxy
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
            var type = typeof(Program);

            try
            {
                Logger.Configure();
                Logger.Info(type, System.Reflection.Assembly.GetExecutingAssembly());
                Logger.Info(type, $"Arguments:{string.Join(" ", args)}");

                var proc = StartAs(args);
                proc.EnableRaisingEvents = true;
                proc.Exited += (s, e) =>
                {
                    if (s is System.Diagnostics.Process p)
                    {
                        Logger.Info(type, $"ExitCode:{p.ExitCode}");
                    }
                };
                proc.WaitForExit();
            }
            catch (Exception err) { Logger.Error(type, err.Message, err); }
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
            var parser = new ArgumentCollection(args, '/');
            if (!parser.Options.TryGetValue("Exec", out var exec)) throw new ArgumentException("Exec");

            try
            {
                parser.Options.TryGetValue("UserName", out var user);
                return Cube.Processes.Process.StartAs(user, exec, args);
            }
            catch (Exception e)
            {
                if (!parser.Options.TryGetValue("ThreadID", out var id)) throw;
                else
                {
                    Logger.Warn(typeof(Program), e.Message, e);
                    Logger.Info(typeof(Program), $"Use ThreadID ({id})");
                    return Cube.Processes.Process.StartAs(uint.Parse(id), exec, args);
                }
            }
        }
    }
}
