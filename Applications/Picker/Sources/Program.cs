/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using Cube.Log;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace Cube.Pdf.App.Picker
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
            var name = Application.ProductName.ToLower();

            try
            {
                using (var m = new Cube.Ipc.Messenger<IEnumerable<string>>(name))
                {
                    if (!m.IsServer) m.Publish(args);
                    else
                    {
                        Logger.Configure();
                        Logger.Info(type, Assembly.GetExecutingAssembly());

                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new DropForm(args) { Activator = m });
                    }
                }
            }
            catch (Exception err) { Logger.Error(type, err.Message, err); }
        }
    }
}
