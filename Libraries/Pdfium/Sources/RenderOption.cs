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
namespace Cube.Pdf.Pdfium;

using System.Drawing;

/* ------------------------------------------------------------------------- */
///
/// RenderOption
///
/// <summary>
/// Represents the options to render a PDF page with the DocumentReader
/// class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class RenderOption
{
    /* --------------------------------------------------------------------- */
    ///
    /// Background
    ///
    /// <summary>
    /// Gets or sets the background color.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Color Background { get; set; } = Color.Transparent;

    /* --------------------------------------------------------------------- */
    ///
    /// Background
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to render annotations.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Annotation { get; set; } = false;

    /* --------------------------------------------------------------------- */
    ///
    /// AntiAlias
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to enable anti-aliasing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool AntiAlias { get; set; } = true;

    /* --------------------------------------------------------------------- */
    ///
    /// Grayscale
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to render in grayscale.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Grayscale { get; set; } = false;

    /* --------------------------------------------------------------------- */
    ///
    /// Print
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to render for printing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Print { get; set; } = false;
}
