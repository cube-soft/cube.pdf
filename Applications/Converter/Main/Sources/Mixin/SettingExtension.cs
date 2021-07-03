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
using System.Linq;
using System.Reflection;
using Cube.Mixin.Assembly;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;
using Cube.Pdf.Mixin;

namespace Cube.Pdf.Converter.Mixin
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingExtension
    ///
    /// <summary>
    /// Provides extended methods of the SettingFolder class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class SettingExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Normalize
        ///
        /// <summary>
        /// Normalizes the specified settings.
        /// </summary>
        ///
        /// <param name="src">Setting to be normalized.</param>
        ///
        /// <remarks>
        /// Starting with 1.0.0RC12, Resolution was changed from a value
        /// corresponding to the index of the ComboBox to a direct value.
        /// With this change, the value expected to point to the index is
        /// reset to its initial value.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Normalize(this SettingFolder src)
        {
            var value = src.Value;

            value.Format            = GetFormat(value);
            value.Resolution        = GetResolution(value);
            value.Orientation       = GetOrientation(value);
            value.Metadata.Creator  = GetCreator(value);
            value.Metadata.Producer = GetCreator(value);
            value.Encryption.Deny();
            value.Encryption.Permission.Accessibility = PermissionValue.Allow;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of main window.
        /// </summary>
        ///
        /// <param name="src">Setting to be normalized.</param>
        ///
        /// <returns>Title string.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetTitle(this SettingFolder src) =>
            src.DocumentName.Source.HasValue() ?
            $"{src.DocumentName.Source} - CubePDF {src.Version.ToString(3, true)}" :
            $"CubePDF {src.Version.ToString(3, true)}";

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormat
        ///
        /// <summary>
        /// Gets the normalized format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Format GetFormat(SettingValue src) =>
            ViewResource.Formats.Any(e => e.Value == src.Format) ?
            src.Format :
            Format.Pdf;

        /* ----------------------------------------------------------------- */
        ///
        /// GetOrientation
        ///
        /// <summary>
        /// Gets the normalized orientation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Orientation GetOrientation(SettingValue src) =>
            ViewResource.Orientations.Any(e => e.Value == src.Orientation) ?
            src.Orientation :
            Orientation.Auto;

        /* ----------------------------------------------------------------- */
        ///
        /// GetResolution
        ///
        /// <summary>
        /// Gets the normalized resolution.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static int GetResolution(SettingValue src) =>
            src.Resolution >= 72 ? src.Resolution : 600;

        /* ----------------------------------------------------------------- */
        ///
        /// GetCreator
        ///
        /// <summary>
        /// Gets the normalized creator.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetCreator(SettingValue src) =>
            src.Metadata.Creator.HasValue() ?
            src.Metadata.Creator :
            Assembly.GetExecutingAssembly().GetProduct();

        #endregion
    }
}
