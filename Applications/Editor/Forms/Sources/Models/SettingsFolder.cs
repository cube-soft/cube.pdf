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
using Cube.Generics;
using Cube.Xui;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsFolder
    ///
    /// <summary>
    /// Represents the application and/or user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingsFolder : SettingsFolder<Settings>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// Initializes static fields.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static SettingsFolder()
        {
            ResourceCulture.Configure(e =>
            {
                var src = e ?? string.Empty;
                var cmp = Properties.Resources.Culture?.Name;
                var opt = StringComparison.InvariantCultureIgnoreCase;
                if (cmp.HasValue() && cmp.Equals(src, opt)) return false;
                Properties.Resources.Culture = new CultureInfo(src);
                return true;
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingsFolder with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="assembly">Assembly information.</param>
        /// <param name="io">I/O handler</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Assembly assembly, IO io) :
            this(assembly, Cube.DataContract.Format.Registry, @"CubeSoft\CubePDF Utility2", io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingsFolder with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="assembly">Assembly information.</param>
        /// <param name="format">Serialized format.</param>
        /// <param name="location">Location for the settings.</param>
        /// <param name="io">I/O handler</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Assembly assembly, Cube.DataContract.Format format, string location, IO io) :
            base(assembly, format, location, io)
        {
            var asm = assembly.GetReader();
            Title          = asm.Title;
            AutoSave       = false;
            Version.Digit  = 3;
            Version.Suffix = Properties.Resources.VersionSuffix;

            var dir  = IO.Get(asm.Location).DirectoryName;
            var exec = IO.Combine(dir, $"UpdateChecker.exe");
            var args = "CubePDF Utility2";

            Startup.Name    = $"{Title} UpdateChecker";
            Startup.Command = $"{exec.Quote()} {args.Quote()}";
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
        protected override void OnLoaded(ValueChangedEventArgs<Settings> e)
        {
            try { ResourceCulture.Set(e.NewValue.Language.GetName()); }
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
            if (Value != null) Startup.Enabled = Value.CheckUpdate;
            base.OnSaved(e);
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
                ResourceCulture.Set(Value.Language.GetName());
            }
            finally { base.OnPropertyChanged(e); }
        }

        #endregion
    }
}
