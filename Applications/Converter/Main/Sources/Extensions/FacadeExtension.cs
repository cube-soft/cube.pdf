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

using System;
using System.Linq;
using Cube.FileSystem;
using Cube.Pdf.Ghostscript;
using Cube.Text.Extensions;

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
        var ss    = src.Settings.Value;
        var prev  = Io.Get(ss.Destination);
        var items = ss.Extensions.GetCandidates(ss.Format);

        if (items.Any(e => prev.Extension.FuzzyEquals(e))) return;
        ss.Destination = Io.Combine(prev.DirectoryName, $"{prev.BaseName}{items.First()}");
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
        var ss  = src.Settings.Value;
        var cmp = path.ToLowerInvariant();

        ss.Destination = path;

        foreach (Format fmt in Enum.GetValues(typeof(Format)))
        {
            if (ss.Extensions.GetCandidates(fmt).Any(e => cmp.EndsWith(e)))
            {
                ss.Format = fmt;
                break;
            }
        }
    }

    #endregion
}
