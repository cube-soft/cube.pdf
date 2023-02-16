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
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Extensions;

/* ------------------------------------------------------------------------- */
///
/// DocumentWriterExtension
///
/// <summary>
/// Provides extended methods of the IDocumentWriter class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class DocumentWriterExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Add
    ///
    /// <summary>
    /// Adds a new page.
    /// </summary>
    ///
    /// <param name="src">Source writer object.</param>
    /// <param name="page">Page information.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void Add(this IDocumentWriter src, NewPage page) => src.Add(new[] { page });

    /* --------------------------------------------------------------------- */
    ///
    /// Add
    ///
    /// <summary>
    /// Adds a new page.
    /// </summary>
    ///
    /// <param name="src">Source writer object.</param>
    /// <param name="page">Page information.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void Add(this IDocumentWriter src, Page page) => src.Add(new[] { page });

    /* --------------------------------------------------------------------- */
    ///
    /// Add
    ///
    /// <summary>
    /// Adds new pages.
    /// </summary>
    ///
    /// <param name="src">Source writer object.</param>
    /// <param name="pages">Page collection.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void Add(this IDocumentWriter src, IEnumerable<Page> pages) =>
        src.Add(pages.Select(e => new NewPage { Source = e, Options = new() }));

    /* --------------------------------------------------------------------- */
    ///
    /// Add
    ///
    /// <summary>
    /// Adds all pages of the specified document.
    /// </summary>
    ///
    /// <param name="src">Source writer object.</param>
    /// <param name="reader">IDocumentReader object.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void Add(this IDocumentWriter src, IDocumentReader reader) => src.Add(reader.Pages);

    /* --------------------------------------------------------------------- */
    ///
    /// Add
    ///
    /// <summary>
    /// Adds a new attached file.
    /// </summary>
    ///
    /// <param name="src">Source writer object.</param>
    /// <param name="file">Attached file.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void Add(this IDocumentWriter src, Attachment file) => src.Add(new[] { file });

    #endregion
}
