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
/// MetadataException
///
/// <summary>
/// Represents an exception related to metadata-related operations.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class MetadataException : Exception
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataException
    ///
    /// <summary>
    /// Initializes a new instance of the MetadataException class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="message">Error message.</param>
    /// <param name="inner">Inner exception object.</param>
    ///
    /* --------------------------------------------------------------------- */
    public MetadataException(string message, Exception inner) : base(message, inner) { }
}
