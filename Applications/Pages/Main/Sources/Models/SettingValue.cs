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
using System.Runtime.Serialization;
using Cube.DataContract;
using Cube.Globalization;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingValue
    ///
    /// <summary>
    /// Represents the user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class SettingValue : SerializableBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Language
        ///
        /// <summary>
        /// Gets or sets the displayed language.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Language Language
        {
            get => Get(() => Language.Auto);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the path of the working directory. If this value is
        /// empty, the program will use the same directory as the destination
        /// path as its working directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Temp
        {
            get => Get(System.IO.Path.GetTempPath);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShrinkResources
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to shrink deduplicated
        /// resources when saving PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool ShrinkResources
        {
            get => Get(() => true);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// KeepOutlines
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to keep outline
        /// information when saving PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool KeepOutlines
        {
            get => Get(() => true);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AutoSort
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to sort automatically
        /// when multiple files are selected.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool AutoSort
        {
            get => Get(() => true);
            set => Set(value);
        }

        #endregion
    }
}
