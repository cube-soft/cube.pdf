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
/// PageOption
///
/// <summary>
/// Represents editing options for an existing PDF page.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class PageOption : SerializableBase
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Rotation
    ///
    /// <summary>
    /// Gets or sets the angle to rotate a an existing PDF page.
    /// </summary>
    /// 
    /// <remarks>
    /// The final angle may be calculated by adding the value of the
    /// Page.Rotation property to this value.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public Angle Rotation
    {
        get => Get(() => new Angle());
        set => Set(value);
    }

    #endregion
}
