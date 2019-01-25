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
using Cube.Generics;
using Cube.Log;
using Cube.Pdf.Ghostscript;
using Cube.Pdf.Mixin;
using Microsoft.Win32;
using System;
using System.Linq;

namespace Cube.Pdf.App.Converter
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
            Locale.Configure(e =>
            {
                var src = e.ToCultureInfo();
                var cmp = Properties.Resources.Culture?.Name;
                var opt = StringComparison.InvariantCultureIgnoreCase;
                if (cmp.HasValue() && cmp.Equals(src.Name, opt)) return false;
                Properties.Resources.Culture = src;
                return true;
            });
        }

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
            @"CubeSoft\CubePDF\v2",
            new IO()
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
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Cube.DataContract.Format format, string path, IO io) :
            base(System.Reflection.Assembly.GetExecutingAssembly(), format, path, io)
        {
            AutoSave       = false;
            MachineName    = Environment.MachineName;
            UserName       = Environment.UserName;
            DocumentName   = new DocumentName(string.Empty, Assembly.Product, IO);
            WorkDirectory  = GetWorkDirectory();
            Version.Digit  = 3;
            Version.Suffix = $"RC{Assembly.Version.Revision}";
            UpdateProgram  = IO.Combine(IO.Get(Assembly.Location).DirectoryName, "UpdateChecker.exe");
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
        /// DocumentName
        ///
        /// <summary>
        /// Gets the document name.
        /// </summary>
        ///
        /// <remarks>
        /// 主に仮想プリンタ経由時に指定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentName DocumentName { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// MachineName
        ///
        /// <summary>
        /// Gets the machine name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string MachineName { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// MachineName
        ///
        /// <summary>
        /// Gets the user name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string UserName { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Digest
        ///
        /// <summary>
        /// Gets the SHA-256 message digest of the source file that
        /// specified at command line.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Digest { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// WorkDirectory
        ///
        /// <summary>
        /// Gets the path of the working directory.
        /// </summary>
        ///
        /// <remarks>
        /// Ghostscript はパスにマルチバイト文字が含まれる場合、処理に
        /// 失敗する場合があります。そのため、マルチバイト文字の含まれない
        /// ディレクトリに移動して処理を実行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string WorkDirectory { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateProgram
        ///
        /// <summary>
        /// Gets the path of the update program.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string UpdateProgram { get; }

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
        public void Set(string[] args)
        {
            var src = new ArgumentCollection(args, '/', true);
            var op  = src.Options;

            if (op.TryGetValue(nameof(MachineName), out var pc)) MachineName = pc;
            if (op.TryGetValue(nameof(UserName), out var user)) UserName = user;
            if (op.TryGetValue(nameof(DocumentName), out var doc)) DocumentName = new DocumentName(doc, Assembly.Product, IO);
            if (op.TryGetValue(nameof(Digest), out var digest)) Digest = digest;
            if (op.TryGetValue("InputFile", out var input)) Value.Source = input;

            var dest = IO.Get(IO.Combine(Value.Destination, DocumentName.Name));
            var name = dest.NameWithoutExtension;
            var ext  = Value.Format.GetExtension();

            Value.Destination  = IO.Combine(dest.DirectoryName, $"{name}{ext}");
            Value.DeleteSource = op.ContainsKey("DeleteOnClose");
            Value.SkipUi       = op.ContainsKey("SkipUI");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// Checks if the application has been updated.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void CheckUpdate()
        {
            if (Value.CheckUpdate) this.CheckUpdate(UpdateProgram, Assembly.Product);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnLoaded
        ///
        /// <summary>
        /// Occurs when the settings are loaded.
        /// </summary>
        ///
        /// <remarks>
        /// 1.0.0RC12 より Resolution を ComboBox のインデックスに対応
        /// する値から直接の値に変更しました。これに伴い、インデックスを
        /// 指していると予想される値を初期値にリセットしています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnLoaded(ValueChangedEventArgs<Settings> e)
        {
            e.NewValue.Format = NormalizeFormat(e.NewValue);
            e.NewValue.Resolution = NormalizeResolution(e.NewValue);
            e.NewValue.Orientation = NormalizeOrientation(e.NewValue);
            e.NewValue.Destination = NormalizeDestination(e.NewValue);
            e.NewValue.Metadata.Creator = Assembly.Product;
            e.NewValue.Metadata.Options = ViewerOptions.OneColumn;
            e.NewValue.Encryption.Deny();
            e.NewValue.Encryption.Permission.Accessibility = PermissionValue.Allow;

            base.OnLoaded(e);
        }

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

                new Startup("cubepdf-checker")
                {
                    Command = $"{UpdateProgram.Quote()} {Assembly.Product}",
                    Enabled = Value.CheckUpdate,
                }.Save();
            }
            finally { base.OnSaved(e); }
        }

        #region Get

        /* ----------------------------------------------------------------- */
        ///
        /// GetWorkDirectory
        ///
        /// <summary>
        /// Gets the path of the working directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetWorkDirectory()
        {
            var str   = this.GetValue(Registry.LocalMachine, "LibPath");
            var root  = str.HasValue() ?
                        str :
                        IO.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                            Assembly.Company, Assembly.Product
                        );
            return IO.Combine(root, Guid.NewGuid().ToString("D"));
        }

        #endregion

        #region Normalize

        /* ----------------------------------------------------------------- */
        ///
        /// NormalizeFormat
        ///
        /// <summary>
        /// Normalizes the specified Format value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Format NormalizeFormat(Settings src) =>
            ViewResource.Formats.Any(e => e.Value == src.Format) ?
            src.Format :
            Ghostscript.Format.Pdf;

        /* ----------------------------------------------------------------- */
        ///
        /// NormalizeOrientation
        ///
        /// <summary>
        /// Normalizes the specified Orientation value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Orientation NormalizeOrientation(Settings src) =>
            ViewResource.Orientations.Any(e => e.Value == src.Orientation) ?
            src.Orientation :
            Orientation.Auto;

        /* ----------------------------------------------------------------- */
        ///
        /// NormalizeResolution
        ///
        /// <summary>
        /// Normalizes the specified Resolution value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int NormalizeResolution(Settings src) =>
            src.Resolution >= 72 ?
            src.Resolution :
            600;

        /* ----------------------------------------------------------------- */
        ///
        /// NormalizeDestination
        ///
        /// <summary>
        /// Normalizes the path of serialized data.
        /// </summary>
        ///
        /// <remarks>
        /// パスにファイル名が残っている場合、ファイル名部分を除去します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private string NormalizeDestination(Settings src)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            try
            {
                if (!src.Destination.HasValue()) return desktop;
                var dest = IO.Get(src.Destination);
                return dest.IsDirectory ? dest.FullName : dest.DirectoryName;
            }
            catch (Exception err)
            {
                this.LogWarn(err.ToString(), err);
                return desktop;
            }
        }

        #endregion

        #endregion
    }
}
