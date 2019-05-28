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
using Cube.Tests;
using NUnit.Framework;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace Cube.Pdf.Converter.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FacadeTest
    ///
    /// <summary>
    /// Tests the Facade class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class FacadeTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Tests the Convert method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Convert()
        {
            using (var e = new Facade(Assembly.GetExecutingAssembly()))
            {
                var dest = Get($"{nameof(Convert)}.pdf");

                e.Settings.Value.Source = GetSource("Sample.ps");
                e.Settings.Value.Destination = dest;
                e.Settings.Value.PostProcess = PostProcess.None;
                e.Invoke();

                Assert.That(e.Settings.Value.Busy, Is.False);
                Assert.That(e.Results.Count(),     Is.EqualTo(1));
                Assert.That(e.Results.First(),     Is.EqualTo(dest));
                Assert.That(IO.Exists(dest),       Is.True);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_Png
        ///
        /// <summary>
        /// Tests the Convert method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Convert_Png()
        {
            using (var e = new Facade(Assembly.GetExecutingAssembly()))
            {
                var dest = Get($"{nameof(Convert)}.png");

                e.Settings.Value.Source = GetSource("SampleCjk.ps");
                e.Settings.Value.Destination = dest;
                e.Settings.Value.PostProcess = PostProcess.None;
                e.Settings.Value.Format      = Ghostscript.Format.Png;
                e.Settings.Value.Resolution  = 72;
                e.Invoke();

                Assert.That(e.Settings.Value.Busy, Is.False);
                Assert.That(e.Results.Count(),     Is.EqualTo(5));
                Assert.That(e.Results.First(),     Does.EndWith($"{nameof(Convert)}-01.png"));
                Assert.That(IO.Exists(dest),       Is.False);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_SaveOption
        ///
        /// <summary>
        /// Tests the Convert method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(SaveOption.Overwrite)]
        [TestCase(SaveOption.MergeHead)]
        [TestCase(SaveOption.MergeTail)]
        [TestCase(SaveOption.Rename)]
        public void Convert_SaveOption(SaveOption so)
        {
            var dest = Get($"{nameof(Convert)}_{so}.pdf");
            IO.Copy(GetSource("Sample.pdf"), dest, true);

            using (var e = new Facade(Assembly.GetExecutingAssembly()))
            {
                e.Settings.Value.Source = GetSource("Sample.ps");
                e.Settings.Value.Destination = dest;
                e.Settings.Value.SaveOption  = so;
                e.Settings.Value.PostProcess = PostProcess.None;
                e.Invoke();

                Assert.That(e.Settings.Value.Busy, Is.False);
                Assert.That(e.Results.Count(),     Is.EqualTo(1));
                Assert.That(IO.Exists(dest),       Is.True);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_CryptographicException
        ///
        /// <summary>
        /// Tests the Convert method with the invalid digest.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Convert_CryptographicException()
        {
            var dest = Get($"{nameof(Convert_CryptographicException)}.pdf");
            var args = new ArgumentCollection(new[] { "-Digest", "dummy" }, Argument.Windows, true);
            var settings = new SettingsFolder(Assembly.GetExecutingAssembly());

            settings.Value.Source = GetSource("Sample.ps");
            settings.Value.Destination = dest;
            settings.Value.PostProcess = PostProcess.None;
            settings.Set(args);

            using (var e = new Facade(settings))
            {
                Assert.That(() => e.Invoke(), Throws.TypeOf<CryptographicException>());
            }
        }

        #endregion
    }
}
