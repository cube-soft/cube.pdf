/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf.Converter;

using System.Runtime.Serialization;

/* ------------------------------------------------------------------------- */
///
/// SettingValueEx
///
/// <summary>
/// Represents user settings not directly related to the main conversion
/// process.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[DataContract]
public class SettingValueEx : DataContract.SerializableBase
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// SourceVisible
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to display the
    /// path of the source file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool SourceVisible
    {
        get => Get(() => false);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ExplicitDirectory
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to set a value to the
    /// InitialDirectory property explicitly when showing a dialog
    /// that selects the file or directory name.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool ExplicitDirectory
    {
        get => Get(() => false);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Language
    ///
    /// <summary>
    /// Gets or sets the displayed language.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public Language Language
    {
        get => Get(() => Language.Auto);
        set => Set(value);
    }

    #endregion
}
