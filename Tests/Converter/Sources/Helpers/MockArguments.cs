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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Cube.FileSystem;
using Cube.Mixin.Collections;
using Cube.Mixin.String;

/* ------------------------------------------------------------------------- */
///
/// MockArguments
///
/// <summary>
/// Represents the pseudo-command line arguments for CubePDF.
/// </summary>
///
/* ------------------------------------------------------------------------- */
class MockArguments : IEnumerable<string>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// MockArguments
    ///
    /// <summary>
    /// Initializes a new instance of the MockArguments class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="name">Test name.</param>
    /// <param name="src">Path of the source file.</param>
    /// <param name="tmp">Path of the temp directory.</param>
    ///
    /* --------------------------------------------------------------------- */
    public MockArguments(string name, string src, string tmp)
    {
        Name   = name;
        Source = src;
        Temp   = tmp;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Name
    ///
    /// <summary>
    /// Gets the document name.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Name { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the path of the source file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Source { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Temp
    ///
    /// <summary>
    /// Gets the path of the temp directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Temp { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Digest
    ///
    /// <summary>
    /// Ges the SHA-256 hash value of the provided source file.
    /// If the value is null or empty, the MockArguments class will
    /// calculate the value.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Digest { get; init; }

    #endregion

    #region Methods

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
    public IEnumerator<string> GetEnumerator()
    {
        yield return "-DeleteOnClose";
        yield return "-DocumentName";
        yield return Name;
        yield return "-MachineName";
        yield return Environment.MachineName;
        yield return "-UserName";
        yield return Environment.UserName;
        yield return "-InputFile";
        yield return CopySource();
        yield return "-Digest";
        yield return GetDigest();
        yield return "-Exec";
        yield return GetType().Assembly.Location;
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
    /// IEnumerator object that can be used to iterate through the collection.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// CopySource
    ///
    /// <summary>
    /// Copies the provided source file to the provided temp directory.
    /// </summary>
    ///
    /// <returns>Destination path to be copied.</returns>
    ///
    /* --------------------------------------------------------------------- */
    private string CopySource()
    {
        var dest = Io.Combine(Temp, Guid.NewGuid().ToString("N"));
        Io.Copy(Source, dest, true);
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetDigest
    ///
    /// <summary>
    /// Calculates the SHA-256 hash value of the provided source file.
    /// </summary>
    ///
    /// <returns>SHA-256 hash value.</returns>
    ///
    /* --------------------------------------------------------------------- */
    private string GetDigest()
    {
        if (Digest.HasValue()) return Digest;
        var sp = new SHA256CryptoServiceProvider();
        var dest = IoEx.Load(Source, e => sp.ComputeHash(e).Join("", b => $"{b:X2}"));
        return dest;
    }

    #endregion
}
