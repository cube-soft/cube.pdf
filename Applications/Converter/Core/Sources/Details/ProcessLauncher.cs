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
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProcessLauncher
    ///
    /// <summary>
    /// Provides functionality to execute the provided post process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class ProcessLauncher
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProcessLauncher
        ///
        /// <summary>
        /// Initializes a new instance of the ProcessLauncher class with
        /// the specified settings.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ProcessLauncher(SettingsFolder src)
        {
            Settings  = src;
            _handlers = new Dictionary<PostProcess, Action<IEnumerable<string>>>
            {
                { PostProcess.Open,          Open           },
                { PostProcess.OpenDirectory, OpenDirectory  },
                { PostProcess.Others,        RunUserProgram },
            };
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder Settings { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Executes the post process with the specified files.
        /// </summary>
        ///
        /// <param name="src">Source files.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(IEnumerable<string> src)
        {
            if (_handlers.TryGetValue(Settings.Value.PostProcess, out var dest)) dest(src);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Opens the specified files with the associated program.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Open(IEnumerable<string> src) => Start(Create(src.First(), string.Empty));

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectory
        ///
        /// <summary>
        /// Opens the directory at which the specified files are located.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void OpenDirectory(IEnumerable<string> src) => Start(Create(
            "explorer.exe",
            Settings.IO.Get(src.First()).DirectoryName.Quote()
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// RunUserProgram
        ///
        /// <summary>
        /// Executes the specified program.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RunUserProgram(IEnumerable<string> src)
        {
            if (!Settings.Value.UserProgram.HasValue()) return;
            Start(Create(Settings.Value.UserProgram, src.First().Quote()));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        ///
        /// <summary>
        /// Executes the process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Start(ProcessStartInfo src) => new Process { StartInfo = src }.Start();

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the ProcessStartInfo class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ProcessStartInfo Create(string exec, string args) => new ProcessStartInfo
        {
            FileName        = exec,
            Arguments       = args,
            CreateNoWindow  = false,
            UseShellExecute = true,
            LoadUserProfile = false,
            WindowStyle     = ProcessWindowStyle.Normal,
        };

        #endregion

        #region Fields
        private readonly IDictionary<PostProcess, Action<IEnumerable<string>>> _handlers;
        #endregion
    }
}
