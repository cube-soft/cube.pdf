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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Cube.FileSystem;
using Cube.FileSystem.DataContract;
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
        /// Initializes a new instance of the SettingFolder class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder() : this(typeof(App).Assembly, Format.Registry, @"CubeSoft\CubePDF Utility2") { }

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
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder(Assembly assembly, Format format, string location) :
            base(format, location, assembly.GetSoftwareVersion())
        {
            var exe = Io.Combine(assembly.GetDirectoryName(), "CubeChecker.exe");

            Title    = assembly.GetTitle();
            AutoSave = false;
            Startup  = new("cubepdf-utility-checker") { Source = exe };

            Startup.Arguments.Add("cubepdfutility");
            Startup.Arguments.Add("/subkey");
            Startup.Arguments.Add("CubePDF Utility2");
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Startup
        ///
        /// <summary>
        /// Get the startup registration object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Startup Startup { get; }

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

        /* ----------------------------------------------------------------- */
        ///
        /// ProductUri
        ///
        /// <summary>
        /// Gets the URL of the product Web page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Uri ProductUri { get; } = new("https://www.cube-soft.jp/cubepdfutility/");

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentUri
        ///
        /// <summary>
        /// Gets the URL of the document Web page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Uri DocumentUri { get; } = new("https://docs.cube-soft.jp/entry/cubepdf-utility");

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
        /// OnLoad
        ///
        /// <summary>
        /// Loads the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnLoad()
        {
            base.OnLoad();
            Locale.Set(Value.Language);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        ///
        /// <summary>
        /// Saves the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSave()
        {
            base.OnSave();
            Startup.Save(true);
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
