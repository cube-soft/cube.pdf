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
        /// Execute
        ///
        /// <summary>
        /// Executes the conversion.
        /// </summary>
        ///
        /// <param name="src">Source facade.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Execute(this Facade src)
        {
            src.ChangeExtension();
            src.Convert();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetSource
        ///
        /// <summary>
        /// Sets the message result to the Source property.
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
        /// Sets the message result to the Destination and Format
        /// properties.
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
            Debug.Assert(e.FilterIndex <= ViewResources.Formats.Count);

            src.Settings.Value.Destination = e.Value;
            src.Settings.Value.Format = ViewResources.Formats[e.FilterIndex - 1].Value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetUserProgram
        ///
        /// <summary>
        /// Sets the message result to the UserProgram property.
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
