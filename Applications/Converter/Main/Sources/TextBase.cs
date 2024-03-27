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

using System.Threading;
using Cube.Globalization;

/* ------------------------------------------------------------------------- */
///
/// TextBase
///
/// <summary>
/// Provides functionality to switch texts.
/// </summary>
///
/* ------------------------------------------------------------------------- */
abstract class TextBase : LocalizableText
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Make
    ///
    /// <summary>
    /// Gets the text group from the specified Language value.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static TextGroup Make(Language src) => src switch
    {
        Language.Auto              => Make(Locale.GetDefaultLanguage()),
        Language.English           => EnglishText.Get(),
        Language.German            => GermanText.Get(),
        Language.Japanese          => JapaneseText.Get(),
        Language.SimplifiedChinese => SimplifiedChineseText.Get(),
        _ => default,
    };

    /* --------------------------------------------------------------------- */
    ///
    /// Text
    ///
    /// <summary>
    /// Initializes a new instance of the Text class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected TextBase() : base(Make, EnglishText.Get()) { }

    /* --------------------------------------------------------------------- */
    ///
    /// OnReset
    ///
    /// <summary>
    /// Occurs when the Reset method is invoked.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override void OnReset(Language src)
    {
        base.OnReset(src);

        var ci = src.ToCultureInfo();
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
        Properties.Resources.Culture = ci;
    }

    #endregion
}
