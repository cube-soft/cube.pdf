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
using Cube.FileSystem;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

namespace Cube.Pdf.Converter.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModelTest
    ///
    /// <summary>
    /// Tests the MainViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class MainViewModelTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Tests the conversion process with CubePDF.
        /// </summary>
        ///
        /// <remarks>
        /// Some options are not settable from the main screen, so they are
        /// set directly for testing purposes.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Invoke(int id, SettingValue src, IEnumerable<string> args, string filename, bool precopy)
        {
            var dest = Create(Combine(args, filename));

            // see remarks
            dest.Value.EmbedFonts   = src.EmbedFonts;
            dest.Value.Downsampling = src.Downsampling;

            using (var vm = new MainViewModel(dest))
            {
                Set(vm, src);

                var vms = vm.General;
                Assert.That(vms.Destination, Does.EndWith(vms.Format.GetExtension()));

                Assert.That(Io.Exists(vms.Source),      Is.True,  vms.Source);
                Assert.That(Io.Exists(vms.Destination), Is.False, vms.Destination);

                // Test for SaveOption
                if (precopy) Io.Copy(GetSource("Sample.pdf"), vms.Destination, true);

                using (vm.Subscribe<DialogMessage>(SetMessage))
                {
                    Assert.That(Test(vm), Is.True, $"Timeout (No.{id})");
                }
            }

            Assert.That(Io.Exists(dest.Value.Source),      Is.False, dest.Value.Source);
            Assert.That(IsCreated(dest.Value.Destination), Is.True,  dest.DocumentName.Source);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UserProgram_Error
        ///
        /// <summary>
        /// Check the behavior when an error occurs during the execution of
        /// a user program.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void UserProgram_Error()
        {
            var exec = "NotFound.exe";
            var args = GetArgs(nameof(UserProgram_Error));
            var dest = Create(Combine(args, "Sample.ps"));

            using (var vm = new MainViewModel(dest))
            using (vm.Subscribe<DialogMessage>(SetMessage))
            using (vm.Subscribe<OpenFileMessage>(e => e.Value = new[] { exec }))
            {
                vm.General.PostProcess = PostProcess.Others;
                Assert.That(vm.Busy, Is.False);
                Assert.That(TestError(vm), Is.True, "Timeout");
            }
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets test cases.
        /// </summary>
        ///
        /// <remarks>
        /// The test cases should be specified in the following order:
        /// - Settings
        /// - Program arguments
        /// - Source filename (SampleMix.ps as default)
        /// - Whether or not to pre-generate a file with the same name as
        ///   the output (optional)
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases { get
        {
            var n = 0;

            yield return Create(n++,
                new SettingValue
                {
                    Format          = Format.Pdf,
                    Grayscale       = true,
                    Resolution      = 72,
                    ImageFilter     = false,
                },
                GetArgs("PDF テスト")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format          = Format.Pdf,
                    Grayscale       = false,
                    Resolution      = 72,
                    ImageFilter     = true,
                },
                GetArgs("PDF テスト (Jpeg)")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format          = Format.Pdf,
                    Grayscale       = true,
                    Resolution      = 72,
                    ImageFilter     = false,
                    Downsampling    = Downsampling.Bicubic,
                },
                GetArgs("PDF テスト (Bicubic)")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Pdf,
                    Grayscale        = true,
                    Resolution       = 72,
                    PostProcess      = PostProcess.Others,
                },
                GetArgs("PDF テスト (Gray)")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Pdf,
                    Grayscale        = true,
                    Resolution       = 72,
                    EmbedFonts       = false,
                },
                GetArgs("PDF テスト (NoEmbed)")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Pdf,
                    Grayscale        = false,
                    Resolution       = 72,
                    SaveOption       = SaveOption.MergeHead,
                },
                GetArgs("PDF テスト (MergeHead)"),
                true // pre-copy
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Pdf,
                    Grayscale        = false,
                    Resolution       = 72,
                    SaveOption       = SaveOption.MergeTail,
                },
                GetArgs("PDF テスト (MergeTail)"),
                true // pre-copy
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Pdf,
                    Grayscale        = false,
                    Resolution       = 72,
                    SaveOption       = SaveOption.Rename,
                },
                GetArgs("PDF テスト (Rename)"),
                true // pre-copy
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Pdf,
                    Grayscale        = false,
                    Resolution       = 72,
                    Linearization    = true,
                    Metadata         = new Metadata
                    {
                        Title    = "Linearization test title",
                        Author   = "Linearization test author",
                        Subject  = "Linearization test Subject",
                        Keywords = "Linearization test keywords",
                        Creator  = "Linearization test creator",
                        Options  = ViewerOption.SinglePage,
                    }
                },
                GetArgs("PDF テスト (Linearization)")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Pdf,
                    Grayscale        = false,
                    Resolution       = 72,
                    Linearization    = true, // ignore
                    Encryption       = new Encryption
                    {
                        Enabled          = true,
                        OwnerPassword    = "Password",
                        UserPassword     = "User",
                        OpenWithPassword = true,
                        Permission       = new Permission
                        {
                            Accessibility     = PermissionValue.Allow,
                            CopyContents      = PermissionValue.Deny,
                            InputForm         = PermissionValue.Allow,
                            ModifyAnnotations = PermissionValue.Deny,
                            ModifyContents    = PermissionValue.Deny,
                            Print             = PermissionValue.Deny,
                        }
                    },
                    Metadata = new Metadata
                    {
                        Title    = "Encryption test title",
                        Author   = "Encryption test author",
                        Subject  = "Encryption test Subject",
                        Keywords = "Encryption test keywords",
                        Creator  = "Encryption test creator",
                        Options  = ViewerOption.SinglePage,
                    }
                },
                GetArgs("PDF テスト (Encryption)")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Ps,
                    Grayscale        = false,
                    Resolution       = 72,
                },
                GetArgs("PS テスト")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Ps,
                    Grayscale        = true,
                    Resolution       = 72,
                },
                GetArgs("PS テスト (Gray)")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Eps,
                    Grayscale        = false,
                    Resolution       = 72,
                },
                GetArgs("EPS テスト")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Eps,
                    Grayscale        = true,
                    Resolution       = 72,
                },
                GetArgs("EPS テスト (Gray)")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Png,
                    Grayscale        = false,
                    Resolution       = 72,
                },
                GetArgs("PNG テスト")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Png,
                    Grayscale        = false,
                    Resolution       = 144,
                },
                GetArgs("PNG テスト (144 dpi)")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Png,
                    Grayscale        = true,
                    Resolution       = 72,
                },
                GetArgs("PNG テスト (Gray)")
            );

            yield return Create(n++,
                new SettingValue
                {
                    Format           = Format.Png,
                    Grayscale        = true,
                    Resolution       = 72,
                },
                GetArgs("PNG テスト (複数ファイル)"),
                "SampleCjk.ps",
                false
            );
        }}

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// IsCreated
        ///
        /// <summary>
        /// Determines if a file has been generated.
        /// </summary>
        ///
        /// <remarks>
        /// If the file is larger than 1 KB, it is considered to have been
        /// generated successfully. In addition, if the file is converted in
        /// image format, it will be generated with a sequential number
        /// added to the original file name, so this check is performed.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsCreated(string dest)
        {
            var delta = 1000;

            if (Io.Exists(dest)) return Io.Get(dest).Length > delta;

            var info = Io.Get(dest);
            var name = $"{info.BaseName}-01{info.Extension}";
            var cvt = Io.Combine(info.DirectoryName, name);

            return Io.Get(cvt).Length > delta;
        }

        #endregion
    }
}
