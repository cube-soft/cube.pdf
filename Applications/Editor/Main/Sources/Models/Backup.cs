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
using Cube.FileSystem;
using System;
using System.Linq;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// Backup
    ///
    /// <summary>
    /// Provides functionality to backup files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Backup
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DirectoryMonitor
        ///
        /// <summary>
        /// Initializes a new instance of the Backup class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="io">I/O handler</param>
        ///
        /* ----------------------------------------------------------------- */
        public Backup(IO io)
        {
            var app = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            IO        = io;
            Directory = io.Combine(app, "CubeSoft", "CubePdfUtility2", "Backup");
            KeepDays  = 5;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Directory
        ///
        /// <summary>
        /// Gets or sets the backup directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Directory { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// KeepDays
        ///
        /// <summary>
        /// Gets the days to keep backup files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int KeepDays { get; set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Copies the specified file to the backup directory.
        /// </summary>
        ///
        /// <param name="src">Source file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(Entity src)
        {
            if (!src.Exists) return;

            var date = DateTime.Today.ToString("yyyyMMdd");
            var dest = IO.Combine(Directory, date, src.Name);

            if (!IO.Exists(dest)) IO.Copy(src.FullName, dest, false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cleanup
        ///
        /// <summary>
        /// Deletes expired files.
        /// </summary>
        ///
        /// <remarks>
        /// 保持日数と同数のディレクトリまでは削除せずに保持する事とします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Cleanup()
        {
            var src = IO.GetDirectories(Directory);
            var n   = src.Length - KeepDays;

            if (n <= 0) return;
            foreach (var f in src.OrderBy(e => e).Take(n)) IO.TryDelete(f);
        }

        #endregion
    }
}
