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
using Cube.FileSystem;
using Cube.Mixin.Assembly;
using Cube.Mixin.Environment;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;
using Cube.Mixin.Pdf;
using System;
using System.Linq;
using System.Reflection;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsExtension
    ///
    /// <summary>
    /// Provides extended methods of the SettingsFolder class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class SettingsExtension
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
        /// <param name="src">Settings to be normalized.</param>
        ///
        /// <remarks>
        /// 1.0.0RC12 より Resolution を ComboBox のインデックスに対応
        /// する値から直接の値に変更しました。これに伴い、インデックスを
        /// 指していると予想される値を初期値にリセットしています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Normalize(this SettingsFolder src)
        {
            var value = src.Value;

            value.Format            = GetFormat(value);
            value.Resolution        = GetResolution(value);
            value.Orientation       = GetOrientation(value);
            value.Destination       = GetDestination(value, src.IO);
            value.Metadata.Creator  = GetCreator(value);
            value.Metadata.Producer = GetCreator(value);
            value.Encryption.Deny();
            value.Encryption.Permission.Accessibility = PermissionValue.Allow;
        }

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
        private static Format GetFormat(SettingsValue src) =>
            ViewResources.Formats.Any(e => e.Value == src.Format) ?
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
        private static Orientation GetOrientation(SettingsValue src) =>
            ViewResources.Orientations.Any(e => e.Value == src.Orientation) ?
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
        private static int GetResolution(SettingsValue src) =>
            src.Resolution >= 72 ? src.Resolution : 600;

        /* ----------------------------------------------------------------- */
        ///
        /// GetDestination
        ///
        /// <summary>
        /// Gets the normalized destination.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetDestination(SettingsValue src, IO io)
        {
            var desktop = Environment.SpecialFolder.Desktop.GetName();

            try
            {
                if (!src.Destination.HasValue()) return desktop;
                var dest = io.Get(src.Destination);
                return dest.IsDirectory ? dest.FullName : dest.DirectoryName;
            }
            catch { return desktop; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetCreator
        ///
        /// <summary>
        /// Gets the normalized creator.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetCreator(SettingsValue src) =>
            src.Metadata.Creator.HasValue() ?
            src.Metadata.Creator :
            Assembly.GetExecutingAssembly().GetProduct();

        #endregion
    }
}
