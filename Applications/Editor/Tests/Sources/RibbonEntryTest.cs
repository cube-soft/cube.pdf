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
using Cube.Pdf.App.Editor;
using NUnit.Framework;

namespace CubePdf.Tests.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RibbonEntryTest
    ///
    /// <summary>
    /// RibbonEntry のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class RibbonEntryTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Properties
        ///
        /// <summary>
        /// 各種プロパティ内容を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Properties()
        {
            var name = "Name";
            var text = "GetText";

            using (var dest = new RibbonEntry(name,() => text))
            {
                Assert.That(dest.Name,      Is.EqualTo(name));
                Assert.That(dest.Text,      Is.EqualTo(text));
                Assert.That(dest.LargeIcon, Is.EqualTo("pack://application:,,,/Assets/Large/Name.png"));
                Assert.That(dest.SmallIcon, Is.EqualTo("pack://application:,,,/Assets/Small/Name.png"));

                ResourceCulture.Set("fr");
                Assert.That(dest.Text,      Is.EqualTo(text));
            }

            ResourceCulture.Set("ja");
        }

        #endregion
    }
}
