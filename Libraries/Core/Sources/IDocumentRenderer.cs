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
/// IDocumentRenderer
///
/// <summary>
/// Provides functionality to render the document contents.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public interface IDocumentRenderer
{
    /* --------------------------------------------------------------------- */
    ///
    /// Render
    ///
    /// <summary>
    /// Gets an Image object in which the Page content is rendered.
    /// </summary>
    ///
    /// <param name="page">Page object.</param>
    /// <param name="size">Rendering size.</param>
    ///
    /// <returns>Image object</returns>
    ///
    /* --------------------------------------------------------------------- */
    Image Render(Page page, SizeF size);

    /* --------------------------------------------------------------------- */
    ///
    /// Render
    ///
    /// <summary>
    /// Render the Page content to the Graphics object with the
    /// specified parameters
    /// </summary>
    ///
    /// <param name="dest">Graphics object.</param>
    /// <param name="page">Page object.</param>
    /// <param name="point">Start point to render.</param>
    /// <param name="size">Rendering size.</param>
    ///
    /* --------------------------------------------------------------------- */
    void Render(Graphics dest, Page page, PointF point, SizeF size);
}
