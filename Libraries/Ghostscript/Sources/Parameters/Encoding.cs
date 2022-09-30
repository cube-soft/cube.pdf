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
namespace Cube.Pdf.Ghostscript;

/* ------------------------------------------------------------------------- */
///
/// Encoding
///
/// <summary>
/// Specifies encoding methods.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum Encoding
{
    /// <summary>None</summary>
    None,
    /// <summary>Flate encoding</summary>
    Flate,
    /// <summary>LZW encoding</summary>
    Lzw,
    /// <summary>DCT encoding that is used in JPEG compression</summary>
    Jpeg,
    /// <summary>G3 Fax encoding</summary>
    G3Fax,
    /// <summary>G4 Fax encoding</summary>
    G4Fax,
    /// <summary>JBIG2 encoding</summary>
    Jbig2,
    /// <summary>Run Length Encoding (RLE)</summary>
    Rle,
    /// <summary>PackBits RLE</summary>
    PackBits,
    /// <summary>Base64 encoding</summary>
    Base64,
    /// <summary>Base85 encoding</summary>
    Base85,
}
