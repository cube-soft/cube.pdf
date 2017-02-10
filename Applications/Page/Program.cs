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
using System.Reflection;
using System.Windows.Forms;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.App.Page.Program
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
            var name = Application.ProductName.ToLower();
            using (var bootstrap = new Cube.Processes.Bootstrap(name))
            {
                if (bootstrap.Exists)
                {
                    bootstrap.Send(args);
                    return;
                }

                InitLog();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var form = new MainForm(args);
                form.Bootstrap = bootstrap;
                Application.Run(form);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitLog
        /// 
        /// <summary>
        /// ログを出力します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static void InitLog()
        {
            var reader = new AssemblyReader(Assembly.GetExecutingAssembly());
            var edition = (IntPtr.Size == 4) ? "x86" : "x64";
            var type = typeof(Program);

            Cube.Log.Operations.Configure();
            Cube.Log.Operations.Info(type, $"{reader.Product} {reader.Version} ({edition})");
            Cube.Log.Operations.Info(type, $"{Environment.OSVersion}");
            Cube.Log.Operations.Info(type, $"{Environment.Version}");
        }
    }
}
