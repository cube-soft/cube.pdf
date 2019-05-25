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
using Cube.Pdf.Ghostscript;
using NUnit.Framework;
using System.Collections.Generic;

namespace Cube.Pdf.Converter.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModelTest
    ///
    /// <summary>
    /// CubePDF による変換処理をテストするためのクラスです。
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
        /// CubePDF による変換処理のテストを実行します。
        /// </summary>
        ///
        /// <remarks>
        /// いくつかのオプションはメイン画面から設定できないようにして
        /// いるため、テスト用として直接設定しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Invoke(int id, SettingsValue src, IEnumerable<string> args, string filename, bool precopy)
        {
            var dest = Create(Combine(args, filename));

            // see remarks
            dest.Value.EmbedFonts   = src.EmbedFonts;
            dest.Value.Downsampling = src.Downsampling;

            using (var vm = new MainViewModel(dest))
            {
                Set(vm, src);

                var vms = vm.Settings;
                Assert.That(vms.Destination, Does.EndWith(vms.Format.GetExtension()));

                Assert.That(IO.Exists(vms.Source),      Is.True,  vms.Source);
                Assert.That(IO.Exists(vms.Destination), Is.False, vms.Destination);

                // Test for SaveOption
                if (precopy) IO.Copy(GetSource("Sample.pdf"), vms.Destination);

                vm.Subscribe<DialogMessage>(SetMessage);
                Assert.That(WaitConv(vm), Is.True, $"Timeout (No.{id})");
            }

            Assert.That(IO.Exists(dest.Value.Source),      Is.False, dest.Value.Source);
            Assert.That(IsCreated(dest.Value.Destination), Is.True,  dest.Document.Value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UserProgram_Error
        ///
        /// <summary>
        /// ユーザプログラムの実行中にエラーが発生した時の挙動を確認します。
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
            {
                vm.Subscribe<DialogMessage>(SetMessage);
                vm.Subscribe<OpenFileMessage>(e => e.Value = new[] { exec });
                vm.Settings.PostProcess = PostProcess.Others;

                Assert.That(vm.Settings.UserProgram, Is.EqualTo(exec));
                Assert.That(vm.Busy, Is.False);
                Assert.That(WaitMessage(vm), Is.True, "Timeout (error)");
            }

            Assert.Pass(Message);
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// テストケースを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// テストケースには、以下の順で指定します。
        /// - 設定情報
        /// - プログラム引数一覧
        /// - 入力ファイル名（省略時 SampleMix.ps）
        /// - 出力と同名のファイルを事前に生成しておくかどうか（オプション）
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                var n = 0;

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format          = Format.Pdf,
                        Grayscale       = true,
                        Resolution      = 72,
                        ImageFilter     = false,
                    },
                    GetArgs("PDF テスト")
                );

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format          = Format.Pdf,
                        Grayscale       = false,
                        Resolution      = 72,
                        ImageFilter     = true,
                    },
                    GetArgs("PDF テスト (Jpeg)")
                );

                yield return Create(n++,
                    new SettingsValue
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
                    new SettingsValue
                    {
                        Format           = Format.Pdf,
                        Grayscale        = true,
                        Resolution       = 72,
                        PostProcess      = PostProcess.Others,
                    },
                    GetArgs("PDF テスト (Gray)")
                );

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format           = Format.Pdf,
                        Grayscale        = true,
                        Resolution       = 72,
                        EmbedFonts       = false,
                    },
                    GetArgs("PDF テスト (NoEmbed)")
                );

                yield return Create(n++,
                    new SettingsValue
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
                    new SettingsValue
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
                    new SettingsValue
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
                    new SettingsValue
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
                            Options  = ViewerOptions.SinglePage,
                        }
                    },
                    GetArgs("PDF テスト (Linearization)")
                );

                yield return Create(n++,
                    new SettingsValue
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
                            Options  = ViewerOptions.SinglePage,
                        }
                    },
                    GetArgs("PDF テスト (Encryption)")
                );

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format           = Format.Ps,
                        Grayscale        = false,
                        Resolution       = 72,
                    },
                    GetArgs("PS テスト")
                );

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format           = Format.Ps,
                        Grayscale        = true,
                        Resolution       = 72,
                    },
                    GetArgs("PS テスト (Gray)")
                );

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format           = Format.Eps,
                        Grayscale        = false,
                        Resolution       = 72,
                    },
                    GetArgs("EPS テスト")
                );

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format           = Format.Eps,
                        Grayscale        = true,
                        Resolution       = 72,
                    },
                    GetArgs("EPS テスト (Gray)")
                );

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format           = Format.Png,
                        Grayscale        = false,
                        Resolution       = 72,
                    },
                    GetArgs("PNG テスト")
                );

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format           = Format.Png,
                        Grayscale        = false,
                        Resolution       = 144,
                    },
                    GetArgs("PNG テスト (144 dpi)")
                );

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format           = Format.Png,
                        Grayscale        = true,
                        Resolution       = 72,
                    },
                    GetArgs("PNG テスト (Gray)")
                );

                yield return Create(n++,
                    new SettingsValue
                    {
                        Format           = Format.Png,
                        Grayscale        = true,
                        Resolution       = 72,
                    },
                    GetArgs("PNG テスト (複数ファイル)"),
                    "SampleCjk.ps",
                    false
                );
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// IsCreated
        ///
        /// <summary>
        /// ファイルが生成されたかどうかを判別します。
        /// </summary>
        ///
        /// <remarks>
        /// 不完全なファイルが生成されたかどうかも併せて判別するため、
        /// 1 KB 以上のファイルであれば正常に生成されたと見なしています。
        /// また、画像形式で変換した場合、元のファイル名に連番が付与された
        /// ファイルが生成されるため、そのチェックを実行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsCreated(string dest)
        {
            var delta = 1000;

            if (IO.Exists(dest)) return IO.Get(dest).Length > delta;

            var info = IO.Get(dest);
            var name = $"{info.BaseName}-01{info.Extension}";
            var cvt = IO.Combine(info.DirectoryName, name);

            return IO.Get(cvt).Length > delta;
        }

        #endregion
    }
}
