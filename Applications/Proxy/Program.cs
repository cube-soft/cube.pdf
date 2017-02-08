/* ------------------------------------------------------------------------- */
///
/// Program.cs
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        [STAThread]
        static void Main(string[] args)
        {
            var type = typeof(Program);
            Cube.Log.Operations.Configure();
            Cube.Log.Operations.Info(type, System.Reflection.Assembly.GetExecutingAssembly());

            try
            {
                var parser = new Arguments(args, '/');
                if (!parser.HasOption("Exec"))
                {
                    Cube.Log.Operations.Warn(type, "Exec not found");
                    return;
                }

                Cube.Processes.Process.StartAs(
                    parser.Get("Exec"),
                    args,
                    parser.HasOption("UserName") ? parser.Get("UserName") : Environment.UserName
                );
            }
            catch (Exception err) { Cube.Log.Operations.Error(typeof(Program), err.Message, err); }

            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Form1());
        }
    }
}
