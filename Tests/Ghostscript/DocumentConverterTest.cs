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

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentConverterTest
    ///
    /// <summary>
    /// DocumentConverter のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DocumentConverterTest : ConverterFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// 変換処理テストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Invoke(string filename, Converter conv, string name)
        {
            var lib  = IO.Get(AssemblyReader.Default.Location).DirectoryName;
            var dest = GetResultsWith($"{name}{conv.Format.GetExtension()}");
            var src  = GetExamplesWith(filename);

            conv.Log   = GetResultsWith($"{name}.log");
            conv.Quiet = false;
            conv.Resources.Add(IO.Combine(lib, "lib"));
            conv.Invoke(src, dest);

            Assert.That(IO.Exists(dest), Is.True);
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// テストケース一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return TestCase("Sample.eps", new DocumentConverter(Format.Pdf)
                {
                    ColorMode   = ColorMode.Rgb,
                    Orientation = Orientation.Portrait,
                }, ColorMode.Rgb);

                yield return TestCase("Sample.eps", new DocumentConverter(Format.Pdf)
                {
                    ColorMode   = ColorMode.Cmyk,
                    Orientation = Orientation.Portrait,
                }, ColorMode.Cmyk);

                yield return TestCase("Sample.eps", new DocumentConverter(Format.Pdf)
                {
                    ColorMode   = ColorMode.Grayscale,
                    Orientation = Orientation.Portrait,
                }, ColorMode.Grayscale);
            }
        }

        #endregion
    }
}
