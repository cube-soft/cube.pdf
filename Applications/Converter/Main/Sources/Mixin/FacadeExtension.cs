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
namespace Cube.Pdf.Converter.Mixin;

using Cube.FileSystem;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;

/* ------------------------------------------------------------------------- */
///
/// FacadeExtension
///
/// <summary>
/// Provides extended methods of the Facade class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
static class FacadeExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// InvokeEx
    ///
    /// <summary>
    /// Invokes main and some additional operations.
    /// </summary>
    ///
    /// <param name="src">Source facade.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void InvokeEx(this Facade src)
    {
        src.ChangeExtension();
        src.Invoke();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ChangeExtension
    ///
    /// <summary>
    /// Changes the extension of the Destination property based on the
    /// Format property.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static void ChangeExtension(this Facade src)
    {
        var prev = Io.Get(src.Settings.Value.Destination);
        var ext  = src.Settings.Value.Format.GetExtension();
        if (prev.Extension.FuzzyEquals(ext)) return;
        src.Settings.Value.Destination = Io.Combine(prev.DirectoryName, $"{prev.BaseName}{ext}");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetDestination
    ///
    /// <summary>
    /// Sets the message result to the Destination and Format
    /// properties.
    /// </summary>
    ///
    /// <param name="src">Source facade.</param>
    /// <param name="path">Path to save.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void SetDestination(this Facade src, string path)
    {
        src.Settings.Value.Destination = path;
        var cmp = path.ToLowerInvariant();
        foreach (var e in Resource.Extensions)
        {
            if (!cmp.EndsWith(e.Key)) continue;
            src.Settings.Value.Format = e.Value;
            break;
        }
    }

    #endregion
}
