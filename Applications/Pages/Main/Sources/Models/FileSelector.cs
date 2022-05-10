/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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
using System.Collections.Generic;
using System.Linq;
using Cube.Collections;
using Cube.FileSystem;
using Cube.Mixin.String;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileSelector
    ///
    /// <summary>
    /// Provides functionality to select target files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileSelector
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets the target files from the specified file collection.
        /// </summary>
        ///
        /// <param name="src">Source file collection.</param>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Get(IEnumerable<string> src) =>
            src.GroupBy(e => Io.Get(e).IsDirectory)
               .OrderByDescending(e => e.Key)
               .SelectMany(e => GetCore(e));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetCore
        ///
        /// <summary>
        /// Gets the target files from the specified file collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<string> GetCore(IGrouping<bool, string> src) =>
            src.Key ?
            src.OrderBy(e => e, new NumericStringComparer()).SelectMany(e => Filter(Io.GetFiles(e))) :
            Filter(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Filter
        ///
        /// <summary>
        /// Applies the filter to the specified files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<string> Filter(IEnumerable<string> src) =>
            src.Where(e => IsTarget(e)).OrderBy(e => e, new NumericStringComparer());

        /* ----------------------------------------------------------------- */
        ///
        /// IsTarget
        ///
        /// <summary>
        /// Determines whether the specified path is the target file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsTarget(string src)
        {
            var cmp = new[] { ".pdf", ".bmp", ".png", ".jpg", ".jpeg", ".tif", ".tiff" };
            var cvt = Io.Get(src);
            return !cvt.IsDirectory && cmp.Any(e => cvt.Extension.FuzzyEquals(e));
        }

        #endregion
    }
}
