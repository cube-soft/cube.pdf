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
using Cube.DataContract;

/* ------------------------------------------------------------------------- */
///
/// NewPage
///
/// <summary>
/// Represents information for an editing page.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public sealed class NewPage : SerializableBase
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the information of the original PDF page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Page Source { get; init; }

    /* --------------------------------------------------------------------- */
    ///
    /// Options
    ///
    /// <summary>
    /// Gets or sets the editing options to the source page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public PageOption Options
    {
        get => Get(() => new PageOption());
        set => Set(value);
    }

    #endregion
}
