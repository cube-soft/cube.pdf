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
using NUnit.Framework;
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
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Invoke(Settings src, string[] args, string filename)
        {
            var ss = Create(Combine(args, filename));

            using (var vm = new MainViewModel(ss))
            {
                Set(vm, src);

                Assert.That(IO.Exists(vm.Settings.Source),      Is.True,  vm.Settings.Source);
                Assert.That(IO.Exists(vm.Settings.Destination), Is.False, vm.Settings.Destination);

                Assert.That(vm.IsBusy, Is.False);
                vm.Messenger.MessageBox.Subscribe(Error);
                vm.Convert();
                Assert.That(Wait(vm), Is.True, "Timeout");
            }

            Assert.That(IO.Exists(ss.Value.Source),      Is.False, ss.Value.Source);
            Assert.That(IO.Exists(ss.Value.Destination), Is.True,  ss.Value.Destination);
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
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return Create(
                    new Settings
                    {
                        Format      = Ghostscript.Format.Pdf,
                        Grayscale   = false,
                        Resolution  = 72,
                    },
                    CreateArgs("Pdf")
                );

                yield return Create(
                    new Settings
                    {
                        Format      = Ghostscript.Format.Pdf,
                        Grayscale   = true,
                        Resolution  = 72,
                    },
                    CreateArgs("Pdf_Grayscale")
                );

                yield return Create(
                    new Settings
                    {
                        Format      = Ghostscript.Format.Ps,
                        Grayscale   = false,
                        Resolution  = 72,
                    },
                    CreateArgs("Ps")
                );

                yield return Create(
                    new Settings
                    {
                        Format      = Ghostscript.Format.Ps,
                        Grayscale   = true,
                        Resolution  = 72,
                    },
                    CreateArgs("Ps_Grayscale")
                );

                yield return Create(
                    new Settings
                    {
                        Format      = Ghostscript.Format.Eps,
                        Grayscale   = false,
                        Resolution  = 72,
                    },
                    CreateArgs("Eps")
                );

                yield return Create(
                    new Settings
                    {
                        Format      = Ghostscript.Format.Eps,
                        Grayscale   = true,
                        Resolution  = 72,
                    },
                    CreateArgs("Eps_Grayscale")
                );
            }
        }

        #endregion
    }
}
