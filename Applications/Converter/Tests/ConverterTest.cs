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
using Cube.Pdf.App.Converter;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Cube.Pdf.Tests.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// ConverterTest
    ///
    /// <summary>
    /// CubePDF による変換処理をテストするためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ConverterTest : ViewModelFixture
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
        public void Invoke(Settings src, string[] args, string filename)
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
                Assert.That(vm.Title,        Does.StartWith(dest.DocumentName.Value));
                Assert.That(vm.Title,        Does.Contain(vm.Product));
                Assert.That(vm.Title,        Does.Contain(vm.Version));
                Assert.That(vm.Uri,          Is.EqualTo(new Uri("https://www.cube-soft.jp/cubepdf/")));

                Assert.That(IO.Exists(vms.Source),      Is.True,  vms.Source);
                Assert.That(IO.Exists(vms.Destination), Is.False, vms.Destination);

                // Test for SaveOption
                IO.Copy(GetExamplesWith("Sample.pdf"), vms.Destination);

                Assert.That(vm.IsBusy, Is.False);
                vm.Messenger.MessageBox.Subscribe(Error);
                vm.Convert();
                Assert.That(Wait(vm), Is.True, "Timeout");
            }

            Assert.That(IO.Exists(dest.Value.Source),      Is.False, dest.Value.Source);
            Assert.That(IsCreated(dest.Value.Destination), Is.True,  dest.DocumentName.Value);
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
            var args = CreateArgs(nameof(UserProgram_Error));
            var dest = Create(Combine(args, "Sample.ps"));

            using (var vm = new MainViewModel(dest))
            {
                vm.Messenger.MessageBox.Subscribe(Error);
                vm.Messenger.OpenFileDialog.Subscribe(e => e.FileName = exec);
                vm.Settings.PostProcess = PostProcess.Others;

                Assert.That(vm.Settings.UserProgram, Is.EqualTo(exec));
                Assert.That(vm.IsBusy, Is.False);
                vm.Convert();
                Assert.That(WaitError(vm), Is.True, "Timeout (error)");
            }

            Assert.Pass(Message);
        }

        #endregion

        #region Helper methods

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
            var name = $"{info.NameWithoutExtension}-01{info.Extension}";
            var cvt  = IO.Combine(info.DirectoryName, name);

            return IO.Get(cvt).Length > delta;
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
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return Create(
                    new Settings
                    {
                        Format           = Format.Pdf,
                        Grayscale        = true,
                        Resolution       = 72,
                        ImageCompression = false,
                    },
                    CreateArgs("Pdf")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Pdf,
                        Grayscale        = false,
                        Resolution       = 72,
                        ImageCompression = true,
                    },
                    CreateArgs("Pdf_Jpeg")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Pdf,
                        Grayscale        = true,
                        Resolution       = 72,
                        ImageCompression = false,
                        Downsampling     = Downsampling.Bicubic,
                    },
                    CreateArgs("Pdf_Bicubic")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Pdf,
                        Grayscale        = true,
                        Resolution       = 72,
                        PostProcess      = PostProcess.Others,
                    },
                    CreateArgs("Pdf_Gray")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Pdf,
                        Grayscale        = true,
                        Resolution       = 72,
                        EmbedFonts       = false,
                    },
                    CreateArgs("Pdf_NoEmbed")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Pdf,
                        Grayscale        = false,
                        Resolution       = 72,
                        SaveOption       = SaveOption.MergeHead,
                    },
                    CreateArgs("Pdf_MergeHead")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Pdf,
                        Grayscale        = false,
                        Resolution       = 72,
                        SaveOption       = SaveOption.MergeTail,
                    },
                    CreateArgs("Pdf_MergeTail")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Pdf,
                        Grayscale        = false,
                        Resolution       = 72,
                        SaveOption       = SaveOption.Rename,
                    },
                    CreateArgs("Pdf_Rename")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Pdf,
                        Grayscale        = false,
                        Resolution       = 72,
                        Linearization    = true,
                    },
                    CreateArgs("Pdf_Linearization")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Ps,
                        Grayscale        = false,
                        Resolution       = 72,
                    },
                    CreateArgs("Ps")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Ps,
                        Grayscale        = true,
                        Resolution       = 72,
                    },
                    CreateArgs("Ps_Gray")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Eps,
                        Grayscale        = false,
                        Resolution       = 72,
                    },
                    CreateArgs("Eps")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Eps,
                        Grayscale        = true,
                        Resolution       = 72,
                    },
                    CreateArgs("Eps_Gray")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Png,
                        Grayscale        = false,
                        Resolution       = 72,
                    },
                    CreateArgs("Png")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Png,
                        Grayscale        = false,
                        Resolution       = 144,
                    },
                    CreateArgs("Png_144")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Png,
                        Grayscale        = true,
                        Resolution       = 72,
                    },
                    CreateArgs("Png_Gray")
                );

                yield return Create(
                    new Settings
                    {
                        Format           = Format.Png,
                        Grayscale        = true,
                        Resolution       = 72,
                    },
                    CreateArgs("Png_Multi"),
                    "SampleCjk.ps"
                );
            }
        }

        #endregion
    }
}
