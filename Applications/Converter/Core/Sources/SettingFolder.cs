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
using System.Reflection;
using Cube.Collections;
using Cube.FileSystem;
using Cube.Mixin.Assembly;
using Cube.Mixin.Environment;
using Cube.Mixin.IO;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;

namespace Cube.Pdf.Converter
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
    public class SettingFolder : SettingFolder<SettingValue>
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
        /// <param name="assembly">Assembly object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder(Assembly assembly) : this(
            assembly,
            DataContract.Format.Registry,
            @"CubeSoft\CubePDF\v2"
        ) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingFolder class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="assembly">Assembly object.</param>
        /// <param name="format">Serialization format.</param>
        /// <param name="path">Path to save settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder(Assembly assembly, DataContract.Format format, string path) :
            this(assembly, format, path, new()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingFolder class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="assembly">Assembly object.</param>
        /// <param name="format">Serialization format.</param>
        /// <param name="path">Path to save settings.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder(Assembly assembly, DataContract.Format format, string path, IO io) :
            base(format, path, assembly.GetSoftwareVersion(), io)
        {
            Assembly       = assembly;
            AutoSave       = false;
            DocumentName   = GetDocumentName(string.Empty);
            Version.Suffix = "";
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Assembly
        ///
        /// <summary>
        /// Gets the Assembly object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Assembly Assembly { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Uid
        ///
        /// <summary>
        /// Gets the unique ID of the instance.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Guid Uid { get; } = Guid.NewGuid();

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentName
        ///
        /// <summary>
        /// Gets the normalized document name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentName DocumentName { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Digest
        ///
        /// <summary>
        /// Gets the SHA-256 message digest of the source file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Digest { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets values based on the specified arguments.
        /// </summary>
        ///
        /// <param name="args">Program arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(IEnumerable<string> args) =>
            Set(new(args, Collections.Argument.Windows, true));

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets values based on the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Program arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(ArgumentCollection src)
        {
            var op = src.Options;
            if (op.TryGetValue("DocumentName", out var doc)) DocumentName = GetDocumentName(doc);
            if (op.TryGetValue("InputFile", out var input)) Value.Source = input;
            if (op.TryGetValue("Digest", out var digest)) Digest = digest;

            var dest = IO.Get(IO.Combine(GetDirectoryName(Value.Destination), DocumentName.Value));
            var name = dest.BaseName;
            var ext  = Value.Format.GetExtension();

            Value.Destination  = IO.Combine(dest.DirectoryName, $"{name}{ext}");
            Value.DeleteSource = op.ContainsKey("DeleteOnClose");
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnSaved
        ///
        /// <summary>
        /// Occurs when the settings are saved.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSaved(KeyValueEventArgs<Cube.DataContract.Format, string> e)
        {
            try
            {
                if (Value == null) return;

                var name = "cubepdf-checker";
                var exe  = IO.Combine(Assembly.GetDirectoryName(), "CubeChecker.exe").Quote();
                var args = "CubePDF".Quote();

                new Startup(name)
                {
                    Command = $"{exe} {args}",
                    Enabled = Value.CheckUpdate && IO.Exists(exe),
                }.Save();
            }
            finally { base.OnSaved(e); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectoryName
        ///
        /// <summary>
        /// Gets the directory name of the specified path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetDirectoryName(string src)
        {
            var desktop = Environment.SpecialFolder.Desktop.GetName();

            try
            {
                if (!src.HasValue()) return desktop;
                var dest = IO.Get(src);
                return dest.IsDirectory ? dest.FullName : dest.DirectoryName;
            }
            catch { return desktop; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDocumentName
        ///
        /// <summary>
        /// Gets an instance of the DocumentName class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private DocumentName GetDocumentName(string src) => new(src, "CubePDF", IO);

        #endregion
    }
}
