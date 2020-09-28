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
using System.IO;
using System.Linq;

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// ConverterTest
    ///
    /// <summary>
    /// Represents tests of the Converter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ConverterTest : ConverterFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Revision
        ///
        /// <summary>
        /// Confirms the revision number of Ghostscript.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Revision()
        {
            Assert.That(Converter.Revision, Is.EqualTo(9532));
            Assert.That(Converter.Revision, Is.EqualTo(9532));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SupportedFormats
        ///
        /// <summary>
        /// Confirms the number of supported formats.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SupportedFormats()
        {
            Assert.That(Converter.SupportedFormats.Count(), Is.EqualTo(34));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Executes the test to convert.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Invoke(int id, Converter cv, string srcname, string destname)
        {
            Assert.That(IO.Exists(Run(cv, srcname, destname)), Is.True, $"No.{id}");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke_Cjk_Failed
        ///
        /// <summary>
        /// Confirm the behavior when CJK characters are set as output
        /// path.
        /// </summary>
        ///
        /// <remarks>
        /// Ghostscript API はマルチバイト文字を含むパスを指定した場合、
        /// 変換処理に失敗します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Invoke_Cjk_Failed()
        {
            Assert.That(() =>
            {
                var dest = Run(new Converter(Format.Pdf),
                    "Sample.eps",
                    "日本語のファイル",
                    "Invoke_Cjk_Failed");
                if (!IO.Exists(dest)) throw new FileNotFoundException("ErrorTest");
            }, Throws.TypeOf<FileNotFoundException>().Or.TypeOf<GsApiException>());
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
        /// Paper の設定は入力ファイルによっては反映されない場合がある。
        /// 例えば、SampleCjk.ps を変換すると Paper の設定に関わらず常に
        /// A4 サイズで変換される。原因を要調査。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                var n = 0;

                /* --------------------------------------------------------- */
                // Orientation
                /* --------------------------------------------------------- */
                yield return TestCase(n++, new Converter(Format.Pdf)
                {
                    Orientation = Orientation.Portrait,
                }, "Sample.ps", Orientation.Portrait);

                yield return TestCase(n++, new Converter(Format.Pdf)
                {
                    Orientation = Orientation.UpsideDown,
                }, "Sample.ps", Orientation.UpsideDown);

                yield return TestCase(n++, new Converter(Format.Pdf)
                {
                    Orientation = Orientation.Landscape,
                }, "Sample.ps", Orientation.Landscape);

                yield return TestCase(n++, new Converter(Format.Pdf)
                {
                    Orientation = Orientation.Seascape,
                }, "Sample.ps", Orientation.Seascape);

                /* --------------------------------------------------------- */
                // Paper
                /* --------------------------------------------------------- */
                yield return TestCase(n++, new Converter(Format.Pdf)
                {
                    Paper = Paper.IsoB4,
                }, "Sample.ps", Paper.IsoB4);

                yield return TestCase(n++, new Converter(Format.Pdf)
                {
                    Paper = Paper.JisB4,
                }, "Sample.ps", Paper.JisB4);

                yield return TestCase(n++, new Converter(Format.Pdf)
                {
                    Paper = Paper.Letter,
                }, "Sample.ps", Paper.Letter);
            }
        }

        #endregion
    }
}
