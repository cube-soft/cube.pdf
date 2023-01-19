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
namespace Cube.Pdf.Converter;

using System;

/* ------------------------------------------------------------------------- */
///
/// PostProcessException
///
/// <summary>
/// Represents an exception about the post-process.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class PostProcessException : Exception
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// PostProcessException
    ///
    /// <summary>
    /// Initializes a new instance of the PostProcessException class
    /// with the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Kind of post process.</param>
    /// <param name="user">Path of the user program.</param>
    /// <param name="inner">Inner exception object.</param>
    ///
    /* --------------------------------------------------------------------- */
    public PostProcessException(PostProcess src, string user, Exception inner) :
        base("Post process failed", inner)
    {
        PostProcess = src;
        UserProgram = user;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// PostProcess
    ///
    /// <summary>
    /// Gets the value that represents the post-process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public PostProcess PostProcess { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// UserProgram
    ///
    /// <summary>
    /// Gets the path of the user program that executes after converting.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string UserProgram { get; }

    #endregion
}
