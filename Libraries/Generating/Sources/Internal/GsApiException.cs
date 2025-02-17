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
namespace Cube.Pdf.Ghostscript;

using System;

/* ------------------------------------------------------------------------- */
///
/// GsApiException
///
/// <summary>
/// Represents the Ghostscript API error.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class GsApiException : Exception
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// GsApiException
    ///
    /// <summary>
    /// Initializes a new instance of the GsApiException class with
    /// the specified status.
    /// </summary>
    ///
    /// <param name="status">Status code.</param>
    ///
    /* --------------------------------------------------------------------- */
    public GsApiException(int status) : this((GsApiStatus)Enum.ToObject(typeof(GsApiStatus), status)) { }

    /* --------------------------------------------------------------------- */
    ///
    /// GsApiException
    ///
    /// <summary>
    /// Initializes a new instance of the GsApiException class with
    /// the specified status.
    /// </summary>
    ///
    /// <param name="status">Status code.</param>
    ///
    /* --------------------------------------------------------------------- */
    public GsApiException(GsApiStatus status) : this(status, $"{status} ({status:D})") { }

    /* --------------------------------------------------------------------- */
    ///
    /// GsApiException
    ///
    /// <summary>
    /// Initializes a new instance of the GsApiException class with
    /// the specified status and message.
    /// </summary>
    ///
    /// <param name="status">Status code.</param>
    /// <param name="message">Message.</param>
    ///
    /* --------------------------------------------------------------------- */
    public GsApiException(GsApiStatus status, string message) : base(message) => Status = status;

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Status
    ///
    /// <summary>
    /// Gets the status code.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public GsApiStatus Status { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// logPath
    ///
    /// <summary>
    /// Gets the GS log filepath.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Log { get; init; }

    #endregion
}
