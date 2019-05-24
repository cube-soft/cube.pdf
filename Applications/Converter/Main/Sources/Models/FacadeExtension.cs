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
using System.Diagnostics;
using System.Linq;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// FacadeExtension
    ///
    /// <summary>
    /// Provides extended methods of the Facade class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class FacadeExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// SetSource
        ///
        /// <summary>
        /// Source プロパティを更新します。
        /// </summary>
        ///
        /// <param name="src">Source facade.</param>
        /// <param name="e">Result message.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void SetSource(this Facade src, OpenFileMessage e)
        {
            if (!e.Cancel) src.Settings.Value.Source = e.Value.First();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetDestination
        ///
        /// <summary>
        /// Destination および Format プロパティを更新します。
        /// </summary>
        ///
        /// <param name="src">Source facade.</param>
        /// <param name="e">Result message.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void SetDestination(this Facade src, SaveFileMessage e)
        {
            if (e.Cancel) return;

            Debug.Assert(e.FilterIndex > 0);
            Debug.Assert(e.FilterIndex <= ViewResource.Formats.Count);

            src.Settings.Value.Destination = e.Value;
            src.Settings.Value.Format = ViewResource.Formats[e.FilterIndex - 1].Value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetUserProgram
        ///
        /// <summary>
        /// UserProgram プロパティを更新します。
        /// </summary>
        ///
        /// <param name="src">Source facade.</param>
        /// <param name="e">Result message.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void SetUserProgram(this Facade src, OpenFileMessage e)
        {
            if (!e.Cancel) src.Settings.Value.UserProgram = e.Value.First();
        }

        #endregion
    }
}
