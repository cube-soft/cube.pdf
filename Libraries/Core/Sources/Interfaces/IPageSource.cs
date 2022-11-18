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

using System.Drawing;

/* ------------------------------------------------------------------------- */
///
/// IPageSource
///
/// <summary>
/// Represents information of a document page.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public interface IPageSource
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// File
    ///
    /// <summary>
    /// Gets the file information that owns this Page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public File File { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Number
    ///
    /// <summary>
    /// Gets the page number.
    /// Note that the first page of a document is 1 (not 0).
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Number { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Rotation
    ///
    /// <summary>
    /// Gets the rotation of the page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Angle Rotation { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Resolution
    ///
    /// <summary>
    /// Gets the horizontal and vertical resolution (dpi) of the page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public PointF Resolution { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Size
    ///
    /// <summary>
    /// Gets the page size.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SizeF Size { get; }

    #endregion
}
