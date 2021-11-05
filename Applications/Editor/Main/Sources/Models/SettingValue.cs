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
using System.Runtime.Serialization;
using Cube.DataContract;

namespace Cube.Pdf.Editor
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
        /// Width
        ///
        /// <summary>
        /// Gets or sets the width of main window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int Width
        {
            get => Get(() => 800);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Height
        ///
        /// <summary>
        /// Gets or sets the height of main window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int Height
        {
            get => Get(() => 600);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ItemSize
        ///
        /// <summary>
        /// Gets or sets the displayed item size.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "ViewSize")]
        public int ItemSize
        {
            get => Get(() => 250);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FrameOnly
        ///
        /// <summary>
        /// Gets or sets the value indicating whether only the frame
        /// of each item is displayed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool FrameOnly
        {
            get => Get(() => false);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RecentVisible
        ///
        /// <summary>
        /// Gets or sets the value indicating whether to show recent files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool RecentVisible
        {
            get => Get(() => true);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Smart
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to use the smart mode
        /// when saving PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool Smart
        {
            get => Get(() => true);
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
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Language
        ///
        /// <summary>
        /// Gets or sets the display language.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Language Language
        {
            get => Get(() => Language.Auto);
            set => Set(value);
        }

        #endregion
    }
}
