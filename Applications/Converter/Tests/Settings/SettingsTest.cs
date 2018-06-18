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
    class SettingsTest
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
            Assert.That(dest.AutoSave,           Is.False);
            Assert.That(dest.Company,            Is.EqualTo("CubeSoft"));
            Assert.That(dest.Product,            Is.EqualTo("CubePDF"));
            Assert.That(dest.Location,           Is.EqualTo(@"CubeSoft\CubePDF\v2"));
            Assert.That(dest.WorkDirectory,      Does.EndWith("CubePDF"));
            Assert.That(dest.Version.ToString(), Is.EqualTo("1.0.0RC12"));
            Assert.That(dest.Startup.Name,       Is.EqualTo("cubepdf-checker"));
            Assert.That(dest.Startup.Command,    Does.EndWith("cubepdf-checker.exe\""));
            Assert.That(dest.Value,              Is.Not.Null);
        }

        #endregion
    }
}
