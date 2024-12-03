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
namespace Cube.Pdf.Converter.Tests.Sdk;

using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// DocumentNameTest
///
/// <summary>
/// Tests the DocumentName class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class DocumentNameTest
{
    /* --------------------------------------------------------------------- */
    ///
    /// Test
    ///
    /// <summary>
    /// Tests the document name conversion.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase("Sample", "Sample")]
    [TestCase("Sample.png", "Sample.png")]
    [TestCase("Symbol*:?\"<>|", "Symbol_______")]
    [TestCase("Head.txt - Apps", "Head.txt")]
    [TestCase("Head.txt - Apps - Sub", "Head.txt")]
    [TestCase("Apps - Tail.txt", "Tail.txt")]
    [TestCase("Apps - Foo - Bar.txt", "Foo - Bar.txt")]
    [TestCase("Apps - Foo - Bar", "Apps - Foo - Bar")]
    [TestCase("Apps-Sub - Foo - Bar.txt", "Foo - Bar.txt")]
    [TestCase("Foo - Bar - Apps", "Foo - Bar - Apps")]
    [TestCase("Foo - Bar.txt - Apps - Sub", "Foo - Bar.txt")]
    [TestCase("Foo - Bar.abcde - Apps - Sub", "Foo - Bar.abcde")]
    [TestCase("Foo - Bar.abcdef - Apps - Sub", "Foo - Bar.abcdef - Apps - Sub")]
    [TestCase("Foo  -  Bar  -  Apps", "Foo - Bar - Apps")]
    [TestCase("Microsoft Word - File", "File")]
    [TestCase("Microsoft Excel - File", "File")]
    [TestCase("Microsoft PowerPoint - File", "File")]
    [TestCase("- Apps", "- Apps")]
    [TestCase(" - Apps", "Apps")]
    [TestCase(" - Apps - Foo", "Apps - Foo")]
    [TestCase(" - Apps - - - Foo", "Apps - Foo")]
    [TestCase("File - .txt", ".txt")] // any better way?
    [TestCase("http://www.example.com/index.html", "index.html")]
    [TestCase("Apps - http://www.example.com/index.html", "index.html")]
    [TestCase("-", "Default")]
    [TestCase("- - -", "Default")]
    [TestCase("", "Default")]
    [TestCase(null, "Default")]
    public void Test(string src, string expected) =>
        Assert.That(new DocumentName(src, "Default").Value, Is.EqualTo(expected));
}
