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
namespace Cube.Pdf.Mixin;

using System;
using System.Drawing;

/* ------------------------------------------------------------------------- */
///
/// PageExtension
///
/// <summary>
/// Provides extended methods for the Page class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class PageExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetViewSize
    ///
    /// <summary>
    /// Gets the display size of this Page.
    /// </summary>
    ///
    /// <param name="src">Page object.</param>
    ///
    /// <remarks>Display size.</remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static SizeF GetViewSize(this Page src) => src.GetViewSize(1.0);

    /* --------------------------------------------------------------------- */
    ///
    /// GetViewSize
    ///
    /// <summary>
    /// Gets the display size of this Page from the specified values.
    /// </summary>
    ///
    /// <param name="src">Page object.</param>
    /// <param name="scale">Scale factor.</param>
    ///
    /// <remarks>Display size.</remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static SizeF GetViewSize(this Page src, double scale)
    {
        var angle  = src.Rotation + src.Delta;
        var sin    = Math.Abs(Math.Sin(angle.Radian));
        var cos    = Math.Abs(Math.Cos(angle.Radian));
        var width  = src.Size.Width * cos + src.Size.Height * sin;
        var height = src.Size.Width * sin + src.Size.Height * cos;
        return new((float)(width * scale), (float)(height * scale));
    }

    #endregion
}
