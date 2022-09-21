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

/* ------------------------------------------------------------------------- */
///
/// Page
///
/// <summary>
/// Represents information of a document page, and some more user
/// extended values.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class Page : PageBase
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// Page
    ///
    /// <summary>
    /// Initializes a new instance of the Page class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Page() { }

    /* --------------------------------------------------------------------- */
    ///
    /// Page
    ///
    /// <summary>
    /// Initializes a new instance of the Page class with the specified
    /// source.
    /// </summary>
    ///
    /// <param name="src">Source page information.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Page(PageBase src)
    {
        File       = src.File;
        Number     = src.Number;
        Resolution = src.Resolution;
        Rotation   = src.Rotation;
        Size       = src.Size;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Delta
    ///
    /// <summary>
    /// Gets or sets the angle you rotate this Page.
    /// </summary>
    ///
    /// <remarks>
    /// The rotation result of this Page is calculated by
    /// Rotation + Delta.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public Angle Delta
    {
        get => Get(() => new Angle());
        set => Set(value);
    }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Reset
    ///
    /// <summary>
    /// Reset the values of editable properties.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Reset() => OnReset();

    /* --------------------------------------------------------------------- */
    ///
    /// OnReset
    ///
    /// <summary>
    /// Reset the values of editable properties.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected virtual void OnReset() => Delta = new();

    #endregion
}
