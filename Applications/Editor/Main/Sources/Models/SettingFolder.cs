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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Cube.FileSystem;
using Cube.Mixin.Assembly;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingFolder
    ///
    /// <summary>
    /// Represents the application and/or user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SettingFolder : SettingFolder<SettingValue>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingFolder with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="assembly">Assembly information.</param>
        /// <param name="io">I/O handler</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder(Assembly assembly, IO io) :
            this(assembly, Cube.DataContract.Format.Registry, @"CubeSoft\CubePDF Utility2", io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingFolder with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="assembly">Assembly information.</param>
        /// <param name="format">Serialized format.</param>
        /// <param name="location">Location for the settings.</param>
        /// <param name="io">I/O handler</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder(Assembly assembly, Cube.DataContract.Format format, string location, IO io) :
            base(format, location, assembly.GetSoftwareVersion(), io)
        {
            Title          = assembly.GetTitle();
            AutoSave       = false;
            Version.Suffix = Properties.Resources.VersionSuffix;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// Gets the title of the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Title { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetSplashProcesses
        ///
        /// <summary>
        /// Gets the collection of splash window processes.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Process> GetSplashProcesses() =>
            Process.GetProcessesByName("CubePdfUtilitySplash");

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnLoaded
        ///
        /// <summary>
        /// Occurs when the Loaded event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnLoaded(ValueChangedEventArgs<SettingValue> e)
        {
            try { Locale.Set(e.NewValue.Language); }
            finally { base.OnLoaded(e); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSaved
        ///
        /// <summary>
        /// Occurs when the Saved event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSaved(KeyValueEventArgs<Cube.DataContract.Format, string> e)
        {
            try
            {
                var src = new Startup("cubepdf-utility-checker")
                {
                    Source  = IO.Combine(GetType().Assembly.GetDirectoryName(), "CubeChecker.exe"),
                    Enabled = Value?.CheckUpdate ?? false,
                };

                src.Arguments.Add("cubepdfutility");
                src.Arguments.Add("/subkey");
                src.Arguments.Add("CubePDF Utility2");
                src.Save(true);
            }
            finally { base.OnSaved(e); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnPropertyChanged
        ///
        /// <summary>
        /// Occurs when the PropertyChanged event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName != nameof(Value.Language)) return;
                Locale.Set(Value.Language);
            }
            finally { base.OnPropertyChanged(e); }
        }

        #endregion
    }
}
