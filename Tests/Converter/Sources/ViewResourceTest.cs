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
using System;

namespace Cube.Pdf.Converter.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewResourceTest
    ///
    /// <summary>
    /// 表示文字列のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ViewResourceTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Formats
        ///
        /// <summary>
        /// Format に関する表示文字列を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Formats() => Create(vm =>
        {
            var v = ViewResource.Formats;
            Assert.That(v.Count,    Is.EqualTo(7));
            Assert.That(v[0].Value, Is.EqualTo(Format.Pdf));
            Assert.That(v[1].Value, Is.EqualTo(Format.Ps));
            Assert.That(v[2].Value, Is.EqualTo(Format.Eps));
            Assert.That(v[3].Value, Is.EqualTo(Format.Png));
            Assert.That(v[4].Value, Is.EqualTo(Format.Jpeg));
            Assert.That(v[5].Value, Is.EqualTo(Format.Bmp));
            Assert.That(v[6].Value, Is.EqualTo(Format.Tiff));

            vm.General.Language = Language.English;
            var en = ViewResource.Formats;
            Assert.That(en[0].Key, Is.EqualTo("PDF"));
            Assert.That(en[1].Key, Is.EqualTo("PS"));
            Assert.That(en[2].Key, Is.EqualTo("EPS"));
            Assert.That(en[3].Key, Is.EqualTo("PNG"));
            Assert.That(en[4].Key, Is.EqualTo("JPEG"));
            Assert.That(en[5].Key, Is.EqualTo("BMP"));
            Assert.That(en[6].Key, Is.EqualTo("TIFF"));

            vm.General.Language = Language.Japanese;
            var ja = ViewResource.Formats;
            Assert.That(ja[0].Key, Is.EqualTo("PDF"));
            Assert.That(ja[1].Key, Is.EqualTo("PS"));
            Assert.That(ja[2].Key, Is.EqualTo("EPS"));
            Assert.That(ja[3].Key, Is.EqualTo("PNG"));
            Assert.That(ja[4].Key, Is.EqualTo("JPEG"));
            Assert.That(ja[5].Key, Is.EqualTo("BMP"));
            Assert.That(ja[6].Key, Is.EqualTo("TIFF"));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// FormatOptions
        ///
        /// <summary>
        /// FormatOption に関する表示文字列を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void FormatOptions() => Create(vm =>
        {
            var v = ViewResource.PdfVersions;
            Assert.That(v.Count,    Is.EqualTo(6));
            Assert.That(v[0].Value, Is.EqualTo(7));
            Assert.That(v[1].Value, Is.EqualTo(6));
            Assert.That(v[2].Value, Is.EqualTo(5));
            Assert.That(v[3].Value, Is.EqualTo(4));
            Assert.That(v[4].Value, Is.EqualTo(3));
            Assert.That(v[5].Value, Is.EqualTo(2));

            vm.General.Language = Language.English;
            var en = ViewResource.PdfVersions;
            Assert.That(v[0].Key, Is.EqualTo("PDF 1.7"));
            Assert.That(v[1].Key, Is.EqualTo("PDF 1.6"));
            Assert.That(v[2].Key, Is.EqualTo("PDF 1.5"));
            Assert.That(v[3].Key, Is.EqualTo("PDF 1.4"));
            Assert.That(v[4].Key, Is.EqualTo("PDF 1.3"));
            Assert.That(v[5].Key, Is.EqualTo("PDF 1.2"));

            vm.General.Language = Language.Japanese;
            var ja = ViewResource.PdfVersions;
            Assert.That(v[0].Key, Is.EqualTo("PDF 1.7"));
            Assert.That(v[1].Key, Is.EqualTo("PDF 1.6"));
            Assert.That(v[2].Key, Is.EqualTo("PDF 1.5"));
            Assert.That(v[3].Key, Is.EqualTo("PDF 1.4"));
            Assert.That(v[4].Key, Is.EqualTo("PDF 1.3"));
            Assert.That(v[5].Key, Is.EqualTo("PDF 1.2"));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOptions
        ///
        /// <summary>
        /// SaveOption に関する表示文字列を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SaveOptions() => Create(vm =>
        {
            var v = ViewResource.SaveOptions;
            Assert.That(v.Count,    Is.EqualTo(4));
            Assert.That(v[0].Value, Is.EqualTo(SaveOption.Overwrite));
            Assert.That(v[1].Value, Is.EqualTo(SaveOption.MergeHead));
            Assert.That(v[2].Value, Is.EqualTo(SaveOption.MergeTail));
            Assert.That(v[3].Value, Is.EqualTo(SaveOption.Rename));

            vm.General.Language = Language.English;
            var en = ViewResource.SaveOptions;
            Assert.That(en[0].Key, Is.EqualTo("Overwrite"));
            Assert.That(en[1].Key, Is.EqualTo("Merge head"));
            Assert.That(en[2].Key, Is.EqualTo("Merge tail"));
            Assert.That(en[3].Key, Is.EqualTo("Rename"));

            vm.General.Language = Language.Japanese;
            var ja = ViewResource.SaveOptions;
            Assert.That(ja[0].Key, Is.EqualTo("上書き"));
            Assert.That(ja[1].Key, Is.EqualTo("先頭に結合"));
            Assert.That(ja[2].Key, Is.EqualTo("末尾に結合"));
            Assert.That(ja[3].Key, Is.EqualTo("リネーム"));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Orientations
        ///
        /// <summary>
        /// Orientation に関する表示文字列を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Orientations() => Create(vm =>
        {
            var v = ViewResource.Orientations;
            Assert.That(v.Count,    Is.EqualTo(3));
            Assert.That(v[0].Value, Is.EqualTo(Orientation.Portrait));
            Assert.That(v[1].Value, Is.EqualTo(Orientation.Landscape));
            Assert.That(v[2].Value, Is.EqualTo(Orientation.Auto));

            vm.General.Language = Language.English;
            var en = ViewResource.Orientations;
            Assert.That(en[0].Key, Is.EqualTo("Portrait"));
            Assert.That(en[1].Key, Is.EqualTo("Landscape"));
            Assert.That(en[2].Key, Is.EqualTo("Auto"));

            vm.General.Language = Language.Japanese;
            var ja = ViewResource.Orientations;
            Assert.That(ja[0].Key, Is.EqualTo("縦"));
            Assert.That(ja[1].Key, Is.EqualTo("横"));
            Assert.That(ja[2].Key, Is.EqualTo("自動"));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// PostProcesses
        ///
        /// <summary>
        /// PostProcess に関する表示文字列を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void PostProcesses() => Create(vm =>
        {
            var v = ViewResource.PostProcesses;
            Assert.That(v.Count,    Is.EqualTo(4));
            Assert.That(v[0].Value, Is.EqualTo(PostProcess.Open));
            Assert.That(v[1].Value, Is.EqualTo(PostProcess.OpenDirectory));
            Assert.That(v[2].Value, Is.EqualTo(PostProcess.None));
            Assert.That(v[3].Value, Is.EqualTo(PostProcess.Others));

            vm.General.Language = Language.English;
            var en = ViewResource.PostProcesses;
            Assert.That(en[0].Key, Is.EqualTo("Open"));
            Assert.That(en[1].Key, Is.EqualTo("Open directory"));
            Assert.That(en[2].Key, Is.EqualTo("None"));
            Assert.That(en[3].Key, Is.EqualTo("Others"));

            vm.General.Language = Language.Japanese;
            var ja = ViewResource.PostProcesses;
            Assert.That(ja[0].Key, Is.EqualTo("開く"));
            Assert.That(ja[1].Key, Is.EqualTo("フォルダを開く"));
            Assert.That(ja[2].Key, Is.EqualTo("何もしない"));
            Assert.That(ja[3].Key, Is.EqualTo("その他"));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// ViewerPreferences
        ///
        /// <summary>
        /// ViewerPreferences に関する表示文字列を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void ViewerPreferences() => Create(vm =>
        {
            var v = ViewResource.ViewerOptions;
            Assert.That(v.Count, Is.EqualTo(6));
            Assert.That(v[0].Value, Is.EqualTo(Pdf.ViewerOption.SinglePage));
            Assert.That(v[1].Value, Is.EqualTo(Pdf.ViewerOption.OneColumn));
            Assert.That(v[2].Value, Is.EqualTo(Pdf.ViewerOption.TwoPageLeft));
            Assert.That(v[3].Value, Is.EqualTo(Pdf.ViewerOption.TwoPageRight));
            Assert.That(v[4].Value, Is.EqualTo(Pdf.ViewerOption.TwoColumnLeft));
            Assert.That(v[5].Value, Is.EqualTo(Pdf.ViewerOption.TwoColumnRight));

            vm.General.Language = Language.English;
            var en = ViewResource.ViewerOptions;
            Assert.That(en[0].Key, Is.EqualTo("Single page"));
            Assert.That(en[1].Key, Is.EqualTo("One column"));
            Assert.That(en[2].Key, Is.EqualTo("Two page (left)"));
            Assert.That(en[3].Key, Is.EqualTo("Two page (right)"));
            Assert.That(en[4].Key, Is.EqualTo("Two column (left)"));
            Assert.That(en[5].Key, Is.EqualTo("Two column (right)"));

            vm.General.Language = Language.Japanese;
            var ja = ViewResource.ViewerOptions;
            Assert.That(ja[0].Key, Is.EqualTo("単一ページ"));
            Assert.That(ja[1].Key, Is.EqualTo("連続ページ"));
            Assert.That(ja[2].Key, Is.EqualTo("見開きページ（左綴じ）"));
            Assert.That(ja[3].Key, Is.EqualTo("見開きページ（右綴じ）"));
            Assert.That(ja[4].Key, Is.EqualTo("連続見開きページ（左綴じ）"));
            Assert.That(ja[5].Key, Is.EqualTo("連続見開きページ（右綴じ）"));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Languages
        ///
        /// <summary>
        /// Language に関する表示文字列を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Languages() => Create(vm =>
        {
            var v = ViewResource.Languages;
            Assert.That(v.Count,    Is.EqualTo(3));
            Assert.That(v[0].Value, Is.EqualTo(Language.Auto));
            Assert.That(v[1].Value, Is.EqualTo(Language.English));
            Assert.That(v[2].Value, Is.EqualTo(Language.Japanese));

            vm.General.Language = Language.English;
            var en = ViewResource.Languages;
            Assert.That(en[0].Key, Is.EqualTo("Auto"));
            Assert.That(en[1].Key, Is.EqualTo("English"));
            Assert.That(en[2].Key, Is.EqualTo("Japanese"));

            vm.General.Language = Language.Japanese;
            var ja = ViewResource.Languages;
            Assert.That(ja[0].Key, Is.EqualTo("自動"));
            Assert.That(ja[1].Key, Is.EqualTo("英語"));
            Assert.That(ja[2].Key, Is.EqualTo("日本語"));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SourceFilters
        ///
        /// <summary>
        /// 入力ファイル選択画面のフィルタに関する表示文字列を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SourceFilters() => Create(vm =>
        {
            var v = ViewResource.SourceFilters;
            Assert.That(v.Count, Is.EqualTo(4));

            vm.General.Language = Language.English;
            var en = ViewResource.SourceFilters;
            Assert.That(en[0].ToString(), Does.StartWith("PS files"));
            Assert.That(en[1].ToString(), Does.StartWith("EPS files"));
            Assert.That(en[2].ToString(), Does.StartWith("PDF files"));
            Assert.That(en[3].ToString(), Does.StartWith("All files"));

            vm.General.Language = Language.Japanese;
            var ja = ViewResource.SourceFilters;
            Assert.That(ja[0].ToString(), Does.StartWith("PS ファイル"));
            Assert.That(ja[1].ToString(), Does.StartWith("EPS ファイル"));
            Assert.That(ja[2].ToString(), Does.StartWith("PDF ファイル"));
            Assert.That(ja[3].ToString(), Does.StartWith("すべてのファイル"));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// DestinationFilters
        ///
        /// <summary>
        /// 保存パス選択画面のフィルタに関する表示文字列を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DestinationFilters() => Create(vm =>
        {
            var v = ViewResource.DestinationFilters;
            Assert.That(v.Count, Is.EqualTo(7));

            vm.General.Language = Language.English;
            var en = ViewResource.DestinationFilters;
            Assert.That(en[0].ToString(), Does.StartWith("PDF files"));
            Assert.That(en[1].ToString(), Does.StartWith("PS files"));
            Assert.That(en[2].ToString(), Does.StartWith("EPS files"));
            Assert.That(en[3].ToString(), Does.StartWith("PNG files"));
            Assert.That(en[4].ToString(), Does.StartWith("JPEG files"));
            Assert.That(en[5].ToString(), Does.StartWith("BMP files"));
            Assert.That(en[6].ToString(), Does.StartWith("TIFF files"));

            vm.General.Language = Language.Japanese;
            var ja = ViewResource.DestinationFilters;
            Assert.That(ja[0].ToString(), Does.StartWith("PDF ファイル"));
            Assert.That(ja[1].ToString(), Does.StartWith("PS ファイル"));
            Assert.That(ja[2].ToString(), Does.StartWith("EPS ファイル"));
            Assert.That(ja[3].ToString(), Does.StartWith("PNG ファイル"));
            Assert.That(ja[4].ToString(), Does.StartWith("JPEG ファイル"));
            Assert.That(ja[5].ToString(), Does.StartWith("BMP ファイル"));
            Assert.That(ja[6].ToString(), Does.StartWith("TIFF ファイル"));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// UserProgramFilters
        ///
        /// <summary>
        /// ユーザプログラム選択画面のフィルタに関する表示文字列を
        /// 確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void UserProgramFilters() => Create(vm =>
        {
            var v = ViewResource.UserProgramFilters;
            Assert.That(v.Count, Is.EqualTo(2));

            vm.General.Language = Language.English;
            var en = ViewResource.UserProgramFilters;
            Assert.That(en[0].ToString(), Does.StartWith("Executable files"));
            Assert.That(en[1].ToString(), Does.StartWith("All files"));

            vm.General.Language = Language.Japanese;
            var ja = ViewResource.UserProgramFilters;
            Assert.That(ja[0].ToString(), Does.StartWith("実行可能なファイル"));
            Assert.That(ja[1].ToString(), Does.StartWith("すべてのファイル"));
        });

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// テスト用の ViewModel を生成します。
        /// </summary>
        ///
        ///
        /* ----------------------------------------------------------------- */
        private void Create(Action<MainViewModel> action)
        {
            var src = Create(Combine(GetArgs(nameof(ViewResourceTest)), "Sample.ps"));

            using (Locale.Subscribe(SetUiCulture))
            using (var vm = new MainViewModel(src))
            {
                vm.General.Language = Language.Auto;
                action(vm);
            }
        }

        #endregion
    }
}
