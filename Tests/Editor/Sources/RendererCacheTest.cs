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
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// RendererCacheTest
    ///
    /// <summary>
    /// Tests the RendererCache class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class RendererCacheTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// GetOrAdd
        ///
        /// <summary>
        /// Tests the GetOrAdd method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf", "")]
        [TestCase("Sample.jpg", "")]
        public void GetOrAdd(string filename, string password)
        {
            using var obj = new RendererCache(() => new Query<string>(e => e.Value = password));

            var src  = GetSource(filename);
            var dest = obj.GetOrAdd(src);
            Assert.That(dest, Is.Not.Null);
            Assert.That(dest, Is.EqualTo(obj.GetOrAdd(src)));
        }

        #endregion
    }
}
