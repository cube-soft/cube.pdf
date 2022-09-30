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

using System.Collections;
using System.Collections.Generic;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// TestCaseBase(T)
///
/// <summary>
/// Represents the base class of Ghostscript test cases.
/// </summary>
///
/* ------------------------------------------------------------------------- */
abstract class TestCaseBase<T> : IEnumerable<TestCaseData> where T : Converter
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// TestCaseBase
    ///
    /// <summary>
    /// Initializes a new instance of the TestCaseBase class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected TestCaseBase() => Category = GetType().Name.Replace("TestCase", "");

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Category
    ///
    /// <summary>
    /// Gets the test category name.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected string Category { get; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets the collection of test cases.
    /// </summary>
    ///
    /// <returns>Collection of test cases.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected abstract IEnumerable<TestCaseData> Get();

    /* --------------------------------------------------------------------- */
    ///
    /// OnMake
    ///
    /// <summary>
    /// Sets up additional settings for the specified converter before
    /// creating a new TestCaseData object.
    /// </summary>
    ///
    /// <param name="converter">Ghostscript converter.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected virtual void OnMake(T converter) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Make
    ///
    /// <summary>
    /// Creates a new TestCaseData object with the specified arguments.
    /// </summary>
    ///
    /// <param name="name">
    /// Test name, which is used for a part of the destination path.
    /// </param>
    ///
    /// <param name="converter">Converter object.</param>
    ///
    /// <returns>TestCaseData object.</returns>
    ///
    /// <remarks>
    /// The method uses SampleMix.ps as the source filename.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    protected TestCaseData Make(string name, T converter) =>
        Make(name, "SampleMix.ps", converter);

    /* --------------------------------------------------------------------- */
    ///
    /// Make
    ///
    /// <summary>
    /// Creates a new TestCaseData object with the specified arguments.
    /// </summary>
    ///
    /// <param name="name">
    /// Test name, which is used for a part of the destination path.
    /// </param>
    ///
    /// <param name="src">Source filename.</param>
    /// <param name="converter">Converter object.</param>
    ///
    /// <returns>TestCaseData object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected TestCaseData Make(string name, string src, T converter)
    {
        OnMake(converter);
        return new(Category, name, src, converter);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetEnumerator
    ///
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    ///
    /// <returns>
    /// Enumerator that can be used to iterate through the collection.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    public IEnumerator<TestCaseData> GetEnumerator() => Get().GetEnumerator();

    /* --------------------------------------------------------------------- */
    ///
    /// GetEnumerator
    ///
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    ///
    /// <returns>
    /// IEnumerator object that can be used to iterate through the collection.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}
