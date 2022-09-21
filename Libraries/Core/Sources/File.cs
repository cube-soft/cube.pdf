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
using System.Drawing;
using Cube.FileSystem;

/* ------------------------------------------------------------------------- */
///
/// File
///
/// <summary>
/// Represents information of PDF and image files.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public abstract class File : Entity
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// File
    ///
    /// <summary>
    /// Initializes a new instance of the File class with the specified
    /// arguments.
    /// </summary>
    ///
    /// <param name="src">Source file information.</param>
    /// <param name="dispose">
    /// Value indicating whether to dispose the specified src object
    /// after initialization.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    protected File(EntitySource src, bool dispose) : base(src, dispose) { }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Count
    ///
    /// <summary>
    /// Gets the number of pages.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Count { get; init; }

    /* --------------------------------------------------------------------- */
    ///
    /// Resolution
    ///
    /// <summary>
    /// Gets the resolution of the PDF or image object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public PointF Resolution { get; init; }

    #endregion
}
