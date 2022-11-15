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
namespace Cube.Pdf.Extensions;

using System.Drawing;

/* ------------------------------------------------------------------------- */
///
/// DocumentRendererExtension
///
/// <summary>
/// Describes extended methods for the IDocumentRenderer interface.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class DocumentRendererExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Render
    ///
    /// <summary>
    /// Renders the Page content to the Graphics object.
    /// </summary>
    ///
    /// <param name="src">Renderer object.</param>
    /// <param name="dest">Graphics object.</param>
    /// <param name="page">Page object.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void Render(this IDocumentRenderer src, Graphics dest, Page2 page) =>
        src.Render(dest, page, new PointF(0, 0), dest.VisibleClipBounds.Size);

    /* --------------------------------------------------------------------- */
    ///
    /// Render
    ///
    /// <summary>
    /// Gets an Image object in which the Page content is rendered.
    /// </summary>
    ///
    /// <param name="src">Renderer object.</param>
    /// <param name="page">Page object.</param>
    ///
    /// <returns>Image object</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Image Render(this IDocumentRenderer src, Page2 page) =>
        src.Render(page, 1.0);

    /* --------------------------------------------------------------------- */
    ///
    /// Render
    ///
    /// <summary>
    /// Gets an Image object in which the Page content is rendered.
    /// </summary>
    ///
    /// <param name="src">Renderer object.</param>
    /// <param name="page">Page object.</param>
    /// <param name="scale">Scale factor.</param>
    ///
    /// <returns>Image object</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Image Render(this IDocumentRenderer src, Page2 page, double scale) =>
        src.Render(page, page.GetViewSize(scale));

    #endregion
}
