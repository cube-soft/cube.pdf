﻿/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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
using System;
using System.Windows.Forms;
using Cube.Collections.Extensions;
using Cube.Globalization;

namespace Cube.Pdf.Pages
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
        static void Main(string[] args) => Logger.Try(() =>
        {
            Logger.Configure(new Logging.NLog.LoggerSource());
            Logger.ObserveTaskException();
            Logger.Info(typeof(Program).Assembly);
            Logger.Info($"[ {args.Join(" ")} ]");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var src = new SettingFolder();
            src.Load();

            Locale.Subscribe(Surface.Texts);
            var view = new MainWindow();
            view.Bind(new MainViewModel(src, args));
            Application.Run(view);
        });
    }
}
