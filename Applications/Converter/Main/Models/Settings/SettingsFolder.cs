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
using Cube.Pdf.Ghostscript;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Reflection;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsFolder
    ///
    /// <summary>
    /// 各種設定を保持するためのクラスです。
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
        /// オブジェクトを初期化します。
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
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="format">設定情報の保存方法</param>
        /// <param name="path">設定情報の保存パス</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Cube.DataContract.Format format, string path, IO io) :
            base(format, path, Assembly.GetExecutingAssembly())
        {
            IO            = io;
            AutoSave      = false;
            MachineName   = Environment.MachineName;
            UserName      = Environment.UserName;
            DocumentName  = new DocumentName(string.Empty, Product, IO);
            WorkDirectory = GetWorkDirectory();

            var asm = new AssemblyReader(Assembly.GetExecutingAssembly());
            Version.Digit  = 3;
            Version.Suffix = $"RC{asm.Version.Revision}";

            var dir = IO.Get(asm.Location).DirectoryName;
            Startup.Command = $"\"{IO.Combine(dir, "cubepdf-checker.exe")}\"";
            Startup.Name    = "cubepdf-checker";
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentName
        ///
        /// <summary>
        /// ドキュメント名を取得します。
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
        /// 端末名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string MachineName { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// MachineName
        ///
        /// <summary>
        /// ユーザ名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string UserName { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// WorkDirectory
        ///
        /// <summary>
        /// 作業ディレクトリのパスを取得または設定します。
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

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// プログラム引数の内容を設定します。
        /// </summary>
        ///
        /// <param name="args">プログラム引数</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(string[] args)
        {
            var src = new ArgumentCollection(args, '/');
            var opt = src.Options;

            if (opt.TryGetValue("MachineName", out var pc)) MachineName = pc;
            if (opt.TryGetValue("UserName", out var user)) UserName = user;
            if (opt.TryGetValue("DocumentName", out var doc)) DocumentName = new DocumentName(doc, Product, IO);
            if (opt.TryGetValue("InputFile", out var input)) Value.Source = input;

            var dest = IO.Get(IO.Combine(Value.Destination, DocumentName.Name));
            var name = dest.NameWithoutExtension;
            var ext  = Value.Format.GetExtension();

            Value.Destination  = IO.Combine(dest.DirectoryName, $"{name}{ext}");
            Value.DeleteSource = opt.ContainsKey("DeleteOnClose");
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnLoaded
        ///
        /// <summary>
        /// 読み込み時に実行されます。
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
            e.NewValue.Metadata.Creator = Product;
            e.NewValue.Metadata.ViewOption = ViewOption.OneColumn;
            e.NewValue.Encryption.DenyAll();
            e.NewValue.Encryption.Permission.Accessibility = PermissionMethod.Allow;

            base.OnLoaded(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSaved
        ///
        /// <summary>
        /// 保存時に実行されます。
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
        /// GetWorkDirectory
        ///
        /// <summary>
        /// 作業ディレクトリのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetWorkDirectory()
        {
            var name = $@"Software\{Company}\{Product}";
            using (var key = Registry.LocalMachine.OpenSubKey(name, false))
            {
                if (key != null)
                {
                    var dest = key.GetValue("LibPath") as string;
                    if (dest.HasValue()) return dest;
                }
            }

            return IO.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                Company,
                Product
            );
        }

        #region Normalize

        /* ----------------------------------------------------------------- */
        ///
        /// NormalizeFormat
        ///
        /// <summary>
        /// Format の値を正規化します。
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
        /// Orientation の値を正規化します。
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
        /// Resolution の値を正規化します。
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
        /// 保存パスを正規化します。
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
            if (!src.Destination.HasValue()) return desktop;

            var dest = IO.Get(src.Destination);
            return dest.IsDirectory ? dest.FullName : dest.DirectoryName;
        }

        #endregion

        #endregion
    }
}
