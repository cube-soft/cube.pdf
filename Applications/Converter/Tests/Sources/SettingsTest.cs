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
using Cube.FileSystem.TestService;
using Cube.Pdf.App.Converter;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;
using System;

namespace Cube.Pdf.Tests.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsTest
    ///
    /// <summary>
    /// SettingsFolder のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SettingsTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// オブジェクトの初期値を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create()
        {
            var dest = new SettingsFolder();

            Assert.That(dest.Format,             Is.EqualTo(Cube.DataContract.Format.Registry));
            Assert.That(dest.Location,           Is.EqualTo(@"CubeSoft\CubePDF\v2"));
            Assert.That(dest.WorkDirectory,      Does.Contain("CubePDF"));
            Assert.That(dest.AutoSave,           Is.False);
            Assert.That(dest.Assembly.Company,   Is.EqualTo("CubeSoft"));
            Assert.That(dest.Assembly.Product,   Is.EqualTo("CubePDF"));
            Assert.That(dest.MachineName,        Is.EqualTo(Environment.MachineName));
            Assert.That(dest.UserName,           Is.EqualTo(Environment.UserName));
            Assert.That(dest.DocumentName.Value, Is.Empty);
            Assert.That(dest.DocumentName.Name,  Is.EqualTo("CubePDF"));
            Assert.That(dest.Version.ToString(), Is.EqualTo("1.0.0RC16"));
            Assert.That(dest.Value,              Is.Not.Null);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Load
        ///
        /// <summary>
        /// 設定情報をロードするテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Load()
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var src     = new SettingsFolder(
                Cube.DataContract.Format.Registry,
                $@"CubeSoft\CubePDF\{nameof(SettingsTest)}",
                IO
            );

            src.Load();
            var dest = src.Value;

            Assert.That(dest.Format,           Is.EqualTo(Format.Pdf));
            Assert.That(dest.FormatOption,     Is.EqualTo(FormatOption.Pdf17));
            Assert.That(dest.SaveOption,       Is.EqualTo(SaveOption.Overwrite));
            Assert.That(dest.Grayscale,        Is.False);
            Assert.That(dest.EmbedFonts,       Is.True);
            Assert.That(dest.ImageCompression, Is.True);
            Assert.That(dest.Downsampling,     Is.EqualTo(Downsampling.None));
            Assert.That(dest.Resolution,       Is.AtLeast(72));
            Assert.That(dest.Orientation,      Is.EqualTo(Orientation.Auto));
            Assert.That(dest.CheckUpdate,      Is.True);
            Assert.That(dest.Linearization,    Is.False);
            Assert.That(dest.Language,         Is.EqualTo(Language.Auto));
            Assert.That(dest.PostProcess,      Is.EqualTo(PostProcess.Open));
            Assert.That(dest.UserProgram,      Is.Empty);
            Assert.That(dest.DeleteSource,     Is.False);
            Assert.That(dest.SourceVisible,    Is.False);
            Assert.That(dest.Source,           Is.Empty);
            Assert.That(dest.Destination,      Is.EqualTo(desktop));
            Assert.That(dest.IsBusy,           Is.False);

            var md = dest.Metadata;
            Assert.That(md.Title,              Is.Empty);
            Assert.That(md.Author,             Is.Empty);
            Assert.That(md.Subject,            Is.Empty);
            Assert.That(md.Keywords,           Is.Empty);
            Assert.That(md.Creator,            Is.EqualTo("CubePDF"));

            var ec = dest.Encryption;
            Assert.That(ec.Enabled,            Is.False);
            Assert.That(ec.Method,             Is.EqualTo(EncryptionMethod.Unknown));
            Assert.That(ec.OpenWithPassword,   Is.False);
            Assert.That(ec.OwnerPassword,      Is.Empty);
            Assert.That(ec.UserPassword,       Is.Empty);

            var pm = dest.Encryption.Permission;
            Assert.That(pm.Accessibility,      Is.EqualTo(PermissionValue.Allow), nameof(pm.Accessibility));
            Assert.That(pm.CopyContents,       Is.EqualTo(PermissionValue.Deny),  nameof(pm.CopyContents));
            Assert.That(pm.InputForm,          Is.EqualTo(PermissionValue.Deny),  nameof(pm.InputForm));
            Assert.That(pm.ModifyAnnotations,  Is.EqualTo(PermissionValue.Deny),  nameof(pm.ModifyAnnotations));
            Assert.That(pm.ModifyContents,     Is.EqualTo(PermissionValue.Deny),  nameof(pm.ModifyContents));
            Assert.That(pm.Print,              Is.EqualTo(PermissionValue.Deny),  nameof(pm.Print));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// プログラム引数で更新するテストを実行します
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Set()
        {
            var src = new[]
            {
                "/DeleteOnClose",
                "/DocumentName",
                "(234)?File.txt - Sample Application",
                "/InputFile",
                @"C:\WINDOWS\CubePDF\PS3AEE.tmp",
                "/MachineName",
                @"\\APOLLON",
                "/ThreadID",
                "15180",
                "/UserName",
                "clown",
                "/Exec",
                @"C:\Program Files\CubePDF\cubepdf.exe",
            };

            var dest = new SettingsFolder();
            dest.Set(src);

            var path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                System.IO.Path.ChangeExtension(dest.DocumentName.Name, ".pdf")
            );

            Assert.That(dest.MachineName,        Is.EqualTo(@"\\APOLLON"));
            Assert.That(dest.UserName,           Is.EqualTo("clown"));
            Assert.That(dest.DocumentName.Value, Is.EqualTo("(234)?File.txt - Sample Application"));
            Assert.That(dest.DocumentName.Name,  Is.EqualTo("(234)_File.txt"));
            Assert.That(dest.Value.DeleteSource, Is.True);
            Assert.That(dest.Value.Source,       Is.EqualTo(@"C:\WINDOWS\CubePDF\PS3AEE.tmp"));
            Assert.That(dest.Value.Destination,  Is.EqualTo(path));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set_Empty
        ///
        /// <summary>
        /// プログラム引数が空の時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Set_Empty()
        {
            var dest = new SettingsFolder();
            dest.Set(new string[0]);

            Assert.That(dest.MachineName,        Is.EqualTo(Environment.MachineName));
            Assert.That(dest.UserName,           Is.EqualTo(Environment.UserName));
            Assert.That(dest.DocumentName.Name,  Is.EqualTo("CubePDF"));
            Assert.That(dest.Value.DeleteSource, Is.False);
            Assert.That(dest.Value.Source,       Is.Empty);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// アップデート確認のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CheckUpdate() =>
            Assert.DoesNotThrow(() => new SettingsFolder().CheckUpdate());

        #endregion
    }
}
