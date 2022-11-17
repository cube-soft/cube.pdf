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
using System.Collections.Generic;

/* ------------------------------------------------------------------------- */
///
/// IDocumentWriter
///
/// <summary>
/// Represents properties and methods to create or modify a PDF document.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public interface IDocumentWriter : IDisposable
{
    /* --------------------------------------------------------------------- */
    ///
    /// Reset
    ///
    /// <summary>
    /// Resets values.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    void Reset();

    /* --------------------------------------------------------------------- */
    ///
    /// Save
    ///
    /// <summary>
    /// Saves the document to the specified path.
    /// </summary>
    ///
    /// <param name="dest">Path to save.</param>
    ///
    /* --------------------------------------------------------------------- */
    void Save(string dest);

    /* --------------------------------------------------------------------- */
    ///
    /// Add
    ///
    /// <summary>
    /// Adds pages to the document.
    /// </summary>
    ///
    /// <param name="pages">Collection of pages.</param>
    ///
    /* --------------------------------------------------------------------- */
    void Add(IEnumerable<NewPage> pages);

    /* --------------------------------------------------------------------- */
    ///
    /// Add
    ///
    /// <summary>
    /// Adds pages to the document.
    /// </summary>
    ///
    /// <param name="pages">Collection of pages.</param>
    /// <param name="hint">
    /// Document reader object to get more detailed information about
    /// the specified pages.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    void Add(IEnumerable<NewPage> pages, IDocumentReader hint);

    /* --------------------------------------------------------------------- */
    ///
    /// Add
    ///
    /// <summary>
    /// Adds attached objects to the document.
    /// </summary>
    ///
    /// <param name="files">Collection of attached files.</param>
    ///
    /* --------------------------------------------------------------------- */
    void Add(IEnumerable<Attachment> files);

    /* --------------------------------------------------------------------- */
    ///
    /// Set
    ///
    /// <summary>
    /// Sets the PDF metadata.
    /// </summary>
    ///
    /// <param name="metadata">PDF metadata.</param>
    ///
    /* --------------------------------------------------------------------- */
    void Set(Metadata metadata);

    /* --------------------------------------------------------------------- */
    ///
    /// Set
    ///
    /// <summary>
    /// Sets the encryption settings.
    /// </summary>
    ///
    /// <param name="encryption">Encryption settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    void Set(Encryption encryption);
}
