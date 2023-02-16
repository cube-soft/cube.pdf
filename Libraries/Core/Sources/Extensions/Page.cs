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

/* ------------------------------------------------------------------------- */
///
/// PageExtension
///
/// <summary>
/// Provides extended methods of the Page class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class PageExtension
{
    /* --------------------------------------------------------------------- */
    ///
    /// Copy
    ///
    /// <summary>
    /// Gets the copied Page object. Since the copied Page object uses the
    /// PageSource object, it is portable.
    /// </summary>
    ///
    /// <param name="src">Original object.</param>
    ///
    /// <returns>Copied object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Page Copy(this Page src) => new(new PageSource
    {
        File       = src.File,
        Number     = src.Number,
        Resolution = src.Resolution,
        Size       = src.Size,
        Rotation   = src.Rotation,
    });
}
