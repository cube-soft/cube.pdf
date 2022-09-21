/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf;

using System;
using Cube.FileSystem;

/* ------------------------------------------------------------------------- */
///
/// PdfFile
///
/// <summary>
/// Represents information of a PDF file.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class PdfFile : File
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// PdfFile
    ///
    /// <summary>
    /// Initializes a new instance of the PdfFile class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path of the PDF file.</param>
    ///
    /* --------------------------------------------------------------------- */
    public PdfFile(string src) : base(IoEx.GetEntitySource(src), true)
    {
        Resolution = new(Point, Point);
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Point
    ///
    /// <summary>
    /// Gets the DPI value of the "Point" unit.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static float Point => 72.0F;

    /* --------------------------------------------------------------------- */
    ///
    /// Password
    ///
    /// <summary>
    /// Gets or sets the owner or user password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Password { get; init; } = string.Empty;

    /* --------------------------------------------------------------------- */
    ///
    /// FullAccess
    ///
    /// <summary>
    /// Gets or sets the value indicating whether you can access
    /// all contents of the PDF document.
    /// </summary>
    ///
    /// <remarks>
    /// This property will be set to false if the PDF file is encrypted
    /// with a password and the file is opened with the user password.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public bool FullAccess { get; init; } = true;

    #endregion
}
