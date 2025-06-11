﻿/* ------------------------------------------------------------------------- */
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

/* ------------------------------------------------------------------------- */
///
/// PdfiumStatus
///
/// <summary>
/// Specifies the status code of PDFium API.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum PdfiumStatus
{
    /// <summary>No error, success.</summary>
    Success,
    /// <summary>Unknown error.</summary>
    Unknown,
    /// <summary>File not found or could not be opened.</summary>
    NotFound,
    /// <summary>File not in PDF format or corrupted.</summary>
    FormatError,
    /// <summary>Password required or incorrect password.</summary>
    PasswordError,
    /// <summary>Unsupported security scheme.</summary>
    UnsupportedEncryption,
    /// <summary>Page not found or content error.</summary>
    PageError,
}
