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
/// IDocumentReader
///
/// <summary>
/// Represents properties and methods to read a PDF document.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public interface IDocumentReader : IDisposable
{
    /* --------------------------------------------------------------------- */
    ///
    /// File
    ///
    /// <summary>
    /// Gets a file information of the document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    File File { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Metadata
    ///
    /// <summary>
    /// Gets a PDF metadata of the document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    Metadata Metadata { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Encryption
    ///
    /// <summary>
    /// Gets an encryption settings of the document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    Encryption Encryption { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Pages
    ///
    /// <summary>
    /// Gets a collection of pages in the document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    IEnumerable<Page> Pages { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Attachments
    ///
    /// <summary>
    /// Gets a collection of attached files to the document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    IEnumerable<Attachment> Attachments { get; }
}
