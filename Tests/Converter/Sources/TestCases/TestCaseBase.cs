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
namespace Cube.Pdf.Converter.Tests;

using System.Collections;
using System.Collections.Generic;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// TestCaseBase
///
/// <summary>
/// Represents the base class of CubePDF test cases.
/// </summary>
///
/* ------------------------------------------------------------------------- */
abstract class TestCaseBase<T> : SourceFileFixture, IEnumerable<TestCaseData>
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
    protected TestCaseBase() : this("Sample3pMix.ps") { }

    /* --------------------------------------------------------------------- */
    ///
    /// TestCaseBase
    ///
    /// <summary>
    /// Initializes a new instance of the TestCaseBase class with the
    /// specified source filename.
    /// </summary>
    ///
    /// <param name="src">Source filename.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected TestCaseBase(string src)
    {
        Source   = src;
        Category = GetType().Name.Replace("TestCase", "");
    }

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

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the default source filename.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected string Source { get; }

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
    /// Sets up additional settings for the specified settings before
    /// creating a new TestCaseData object.
    /// </summary>
    ///
    /// <param name="value">Value for testing.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected virtual void OnMake(T value) { }

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
    /// <param name="value">Value for testing.</param>
    ///
    /// <returns>TestCaseData object.</returns>
    ///
    /// <remarks>
    /// The method uses Sample3pMix.ps as the source filename.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    protected TestCaseData Make(string name, T value) => Make(name, Source, value);

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
    /// <param name="value">Value for testing.</param>
    ///
    /// <returns>TestCaseData object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected TestCaseData Make(string name, string src, T value)
    {
        OnMake(value);
        return new(Category, name, src, value);
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
