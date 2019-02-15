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
namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SaveOption
    ///
    /// <summary>
    /// Specifies how to save when the specified path exists.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum SaveOption
    {
        /// <summary>Overwrite</summary>
        Overwrite = 0,
        /// <summary>Merge at the beginning</summary>
        MergeHead = 1,
        /// <summary>Merge at the end</summary>
        MergeTail = 2,
        /// <summary>Rename</summary>
        Rename = 3,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PostProcess
    ///
    /// <summary>
    /// Specifies the post process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum PostProcess
    {
        /// <summary>Open</summary>
        Open = 0,
        /// <summary>Open the directory that the saved file exists</summary>
        OpenDirectory = 3,
        /// <summary>Nothing</summary>
        None = 1,
        /// <summary>Executes the user specified program</summary>
        Others = 2,
    }
}
