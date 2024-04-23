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
namespace Cube.Pdf.Editor;

using Cube.Globalization;

/* ------------------------------------------------------------------------- */
///
/// Surface
///
/// <summary>
/// Provides resources for display.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class Surface
{
    /* --------------------------------------------------------------------- */
    ///
    /// Texts
    ///
    /// <summary>
    /// Get texts for UI.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static Text Texts { get; } = new();

    /* --------------------------------------------------------------------- */
    ///
    /// Texts
    ///
    /// <summary>
    /// Get the localizable object.
    /// </summary>
    ///
    /// <remarks>
    /// This object is mainly used for hooking in other DLLs.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static ILocalizable Localizable => Texts;
}