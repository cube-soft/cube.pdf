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
using Cube.Mixin.Environment;
using Cube.Mixin.Registry;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;
using Microsoft.Win32;
using System;

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
        /* ----------------------------------------------------------------- */
        public SettingsFolder() : this(
            Cube.DataContract.Format.Registry,
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
        /// <param name="format">Serialization format.</param>
        /// <param name="path">Path to save settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Cube.DataContract.Format format, string path) :
            this(format, path, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingsFolder class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="format">Serialization format.</param>
        /// <param name="path">Path to save settings.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Cube.DataContract.Format format, string path, IO io) :
            base(System.Reflection.Assembly.GetExecutingAssembly(), format, path, io)
        {
            AutoSave       = false;
            Document       = GetDocumentName(string.Empty);
            Temp           = GetTemp();
            Version.Digit  = 3;
            Version.Suffix = "";
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Uri
        ///
        /// <summary>
        /// Gets the URL of the Web page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Uri Uri { get; } = new Uri("https://www.cube-soft.jp/cubepdf/");

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

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the path of the working directory.
        /// </summary>
        ///
        /// <remarks>
        /// Ghostscript はパスにマルチバイト文字が含まれる場合、処理に
        /// 失敗する場合があります。そのため、マルチバイト文字の含まれない
        /// ディレクトリに移動して処理を実行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string Temp { get; set; }

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
            Value.SkipUi       = op.ContainsKey("SkipUI");
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
                    Enabled = Value.CheckUpdate && !IO.Exists(exe),
                };

                dest.Save();
            }
            finally { base.OnSaved(e); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTemp
        ///
        /// <summary>
        /// Gets the path of the working directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetTemp()
        {
            var sk    = $@"Software\{Assembly.GetCompany()}\{Assembly.GetProduct()}";
            var value = Registry.LocalMachine.GetValue<string>(sk, "LibPath");
            var root  = value.HasValue() ?
                        value :
                        IO.Combine(Environment.SpecialFolder.CommonApplicationData.GetName(),
                            Assembly.GetCompany(), Assembly.GetProduct());
            return IO.Combine(root, Guid.NewGuid().ToString("D"));
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
