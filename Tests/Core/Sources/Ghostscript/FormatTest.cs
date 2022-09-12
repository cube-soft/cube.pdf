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
namespace Cube.Pdf.Tests.Ghostscript;

using System;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// FormatTest
///
/// <summary>
/// Tests the Format enum.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class FormatTest
{
    /* --------------------------------------------------------------------- */
    ///
    /// Count
    ///
    /// <summary>
    /// Checks the number of formats belonging to the specified category.
    /// </summary>
    ///
    /// <param name="category">Format category.</param>
    /// <param name="expected">
    /// Number of formats belonging to the specified category.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase("Pdf",  1)]
    [TestCase("Eps",  1)]
    [TestCase("Png",  7)]
    [TestCase("Jpeg", 4)]
    [TestCase("Bmp",  7)]
    [TestCase("Tiff", 8)]
    public void Count(string category, int expected)
    {
        var n = 0;

        foreach (Format e in Enum.GetValues(typeof(Format)))
        {
            if (e.ToString().StartsWith(category)) ++n;
        }

        Assert.That(n, Is.EqualTo(expected));
    }
}
