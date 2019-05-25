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
using Cube.Collections;
using Cube.FileSystem;
using Cube.Mixin.Assembly;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;
using System;
using System.Reflection;

namespace Cube.Pdf.Converter
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
    public class SettingsFolder : SettingsFolder<SettingsValue>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingsFolder class.
        /// </summary>
        ///
        /// <param name="assembly">Assembly object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Assembly assembly) : this(
            assembly,
            DataContract.Format.Registry,
            @"CubeSoft\CubePDF\v2"
        ) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingsFolder class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="assembly">Assembly object.</param>
        /// <param name="format">Serialization format.</param>
        /// <param name="path">Path to save settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Assembly assembly, DataContract.Format format, string path) :
            this(assembly, format, path, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingsFolder class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="assembly">Assembly object.</param>
        /// <param name="format">Serialization format.</param>
        /// <param name="path">Path to save settings.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Assembly assembly, DataContract.Format format, string path, IO io) :
            base(assembly, format, path, io)
        {
            AutoSave       = false;
            Document       = GetDocumentName(string.Empty);
            Version.Digit  = 3;
            Version.Suffix = "";
        }

        #endregion

        #region Properties

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
        /// Document
        ///
        /// <summary>
        /// Gets the document name object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentName Document { get; private set; }

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
        /// <param name="src">Program arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(ArgumentCollection src)
        {
            var op = src.Options;
            if (op.TryGetValue("DocumentName", out var doc)) Document = GetDocumentName(doc);
            if (op.TryGetValue("InputFile", out var input)) Value.Source = input;
            if (op.TryGetValue("Digest", out var digest)) Digest = digest;

            var dest = IO.Get(IO.Combine(Value.Destination, Document.Name));
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
                var exe  = IO.Combine(Assembly.GetDirectoryName(), "CubeChecker.exe");
                var args = Assembly.GetProduct().Quote();
                var dest = new Startup(name)
                {
                    Command = $"{exe.Quote()} {args}",
                    Enabled = Value.CheckUpdate && IO.Exists(exe),
                };

                dest.Save();
            }
            finally { base.OnSaved(e); }
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
        private DocumentName GetDocumentName(string src) =>
            new DocumentName(src, Assembly.GetProduct(), IO);

        #endregion
    }
}
