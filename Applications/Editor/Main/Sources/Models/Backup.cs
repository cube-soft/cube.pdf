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
using System;
using System.Linq;
using Cube.FileSystem;

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
    public sealed class Backup
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
        /* ----------------------------------------------------------------- */
        public Backup(SettingFolder settings) => _settings = settings;

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
            if (!_settings.Value.BackupEnabled || !src.Exists) return;

            var date = DateTime.Today.ToString("yyyyMMdd");
            var dest = Io.Combine(_settings.Value.Backup, date, src.Name);

            if (!Io.Exists(dest)) Io.Copy(src.FullName, dest, false);
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
        /// Up to the number of directories equal to the number of days
        /// retained will be retained without deleting them.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Cleanup()
        {
            if (!_settings.Value.BackupEnabled) return;

            var src = Io.GetDirectories(_settings.Value.Backup);
            var n   = src.Count() - _settings.Value.BackupDays;

            if (n <= 0) return;
            foreach (var f in src.OrderBy(e => e).Take(n)) GetType().LogWarn(() => Io.Delete(f));
        }

        #endregion

        #region Fields
        private readonly SettingFolder _settings;
        #endregion
    }
}
