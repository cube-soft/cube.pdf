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
using System.Linq;
using System.Reflection;
using Cube.FileSystem;
using Cube.Mixin.Environment;
using Cube.Pdf.Converter.Mixin;
using Cube.Pdf.Ghostscript;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Converter.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingTest
    ///
    /// <summary>
    /// Tests the SettingFolder class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SettingTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Tests the properties of the SettingFolder class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create()
        {
            var dest = new SettingFolder(Assembly.GetExecutingAssembly());
            Assert.That(dest.Format,              Is.EqualTo(DataContract.Format.Registry));
            Assert.That(dest.Location,            Is.EqualTo(@"CubeSoft\CubePDF\v3"));
            Assert.That(dest.AutoSave,            Is.False);
            Assert.That(dest.DocumentName.Source, Is.Empty);
            Assert.That(dest.DocumentName.Value,  Is.EqualTo("CubePDF"));
            Assert.That(dest.Version.ToString(),  Is.EqualTo("2.0.3"));
            Assert.That(dest.Value,               Is.Not.Null);
            Assert.That(dest.Digest,              Is.Null);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Load
        ///
        /// <summary>
        /// Tests the Load method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Load()
        {
            var temp    = Io.Combine(Environment.SpecialFolder.CommonApplicationData.GetName(), @"CubeSoft\CubePDF");
            var desktop = Environment.SpecialFolder.Desktop.GetName();
            var src     = new SettingFolder(
                Assembly.GetExecutingAssembly(),
                DataContract.Format.Registry,
                $@"CubeSoft\CubePDF\{nameof(SettingTest)}"
            );

            src.Load();

            var dest = src.Value;
            Assert.That(dest.Format,             Is.EqualTo(Format.Pdf));
            Assert.That(dest.SaveOption,         Is.EqualTo(SaveOption.Overwrite));
            Assert.That(dest.ColorMode,          Is.EqualTo(ColorMode.SameAsSource));
            Assert.That(dest.EmbedFonts,         Is.True);
            Assert.That(dest.Encoding,           Is.EqualTo(Encoding.Jpeg));
            Assert.That(dest.Downsampling,       Is.EqualTo(Downsampling.Bicubic));
            Assert.That(dest.Resolution,         Is.AtLeast(72));
            Assert.That(dest.Orientation,        Is.EqualTo(Orientation.Auto));
            Assert.That(dest.Linearization,      Is.False);
            Assert.That(dest.PostProcess,        Is.EqualTo(PostProcess.None));
            Assert.That(dest.UserProgram,        Is.Empty);
            Assert.That(dest.PlatformCompatible, Is.True);
            Assert.That(dest.DeleteSource,       Is.False);
            Assert.That(dest.Source,             Is.Empty);
            Assert.That(dest.Destination,        Is.EqualTo(desktop));
            Assert.That(dest.Temp,               Is.EqualTo(temp));

            var view = dest.View;
            Assert.That(view.Language,           Is.EqualTo(Language.Auto));
            Assert.That(view.ExplicitDirectory,  Is.False);
            Assert.That(view.SourceVisible,      Is.False);

            var md = dest.Metadata;
            Assert.That(md.Title,                Is.Empty);
            Assert.That(md.Author,               Is.Empty);
            Assert.That(md.Subject,              Is.Empty);
            Assert.That(md.Keywords,             Is.Empty);
            Assert.That(md.Creator,              Is.Empty);
            Assert.That(md.Version.Major,        Is.EqualTo(1));
            Assert.That(md.Version.Minor,        Is.EqualTo(7));

            var ec = dest.Encryption;
            Assert.That(ec.Enabled,              Is.False);
            Assert.That(ec.Method,               Is.EqualTo(EncryptionMethod.Unknown));
            Assert.That(ec.OpenWithPassword,     Is.False);
            Assert.That(ec.OwnerPassword,        Is.Empty);
            Assert.That(ec.UserPassword,         Is.Empty);

            var pm = dest.Encryption.Permission;
            Assert.That(pm.Accessibility,        Is.EqualTo(PermissionValue.Allow), nameof(pm.Accessibility));
            Assert.That(pm.CopyContents,         Is.EqualTo(PermissionValue.Allow), nameof(pm.CopyContents));
            Assert.That(pm.InputForm,            Is.EqualTo(PermissionValue.Allow), nameof(pm.InputForm));
            Assert.That(pm.ModifyAnnotations,    Is.EqualTo(PermissionValue.Allow), nameof(pm.ModifyAnnotations));
            Assert.That(pm.ModifyContents,       Is.EqualTo(PermissionValue.Allow), nameof(pm.ModifyContents));
            Assert.That(pm.Print,                Is.EqualTo(PermissionValue.Allow), nameof(pm.Print));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Normalize
        ///
        /// <summary>
        /// Tests the Normalize extended method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Normalize()
        {
            var src = new SettingFolder(Assembly.GetExecutingAssembly());
            src.Normalize();

            var dest = src.Value;
            Assert.That(dest.Format,      Is.EqualTo(Format.Pdf));
            Assert.That(dest.Resolution,  Is.EqualTo(600));
            Assert.That(dest.Orientation, Is.EqualTo(Orientation.Auto));

            var md = dest.Metadata;
            Assert.That(md.Title,         Is.Empty);
            Assert.That(md.Author,        Is.Empty);
            Assert.That(md.Subject,       Is.Empty);
            Assert.That(md.Keywords,      Is.Empty);
            Assert.That(md.Creator,       Is.EqualTo("CubePDF"));
            Assert.That(md.Version.Major, Is.EqualTo(1));
            Assert.That(md.Version.Minor, Is.EqualTo(7));

            var pm = dest.Encryption.Permission;
            Assert.That(pm.Accessibility,     Is.EqualTo(PermissionValue.Allow), nameof(pm.Accessibility));
            Assert.That(pm.CopyContents,      Is.EqualTo(PermissionValue.Allow), nameof(pm.CopyContents));
            Assert.That(pm.InputForm,         Is.EqualTo(PermissionValue.Allow), nameof(pm.InputForm));
            Assert.That(pm.ModifyAnnotations, Is.EqualTo(PermissionValue.Allow), nameof(pm.ModifyAnnotations));
            Assert.That(pm.ModifyContents,    Is.EqualTo(PermissionValue.Allow), nameof(pm.ModifyContents));
            Assert.That(pm.Print,             Is.EqualTo(PermissionValue.Allow), nameof(pm.Print));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Tests the Set method.
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
                "/UserName",
                "clown",
                "/Exec",
                @"C:\Program Files\CubePDF\cubepdf.exe",
            };

            var dest = new SettingFolder(Assembly.GetExecutingAssembly());
            dest.Set(src);

            var path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                System.IO.Path.ChangeExtension(dest.DocumentName.Value, ".pdf")
            );

            Assert.That(dest.Digest,              Is.Null);
            Assert.That(dest.DocumentName.Source, Is.EqualTo("(234)?File.txt - Sample Application"));
            Assert.That(dest.DocumentName.Value,  Is.EqualTo("(234)_File.txt"));
            Assert.That(dest.Value.DeleteSource,  Is.True);
            Assert.That(dest.Value.Source,        Is.EqualTo(@"C:\WINDOWS\CubePDF\PS3AEE.tmp"));
            Assert.That(dest.Value.Destination,   Is.EqualTo(path));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set_Empty
        ///
        /// <summary>
        /// Tests the Set method with the empty arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Set_Empty()
        {
            var dest = new SettingFolder(Assembly.GetExecutingAssembly());
            dest.Set(Enumerable.Empty<string>());

            Assert.That(dest.Digest,             Is.Null);
            Assert.That(dest.DocumentName.Value, Is.EqualTo("CubePDF"));
            Assert.That(dest.Value.DeleteSource, Is.False);
            Assert.That(dest.Value.Source,       Is.Empty);
        }

        #endregion
    }
}
