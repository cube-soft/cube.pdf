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
using Cube.Collections;
using Cube.Pdf.Ghostscript;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// GhostscriptFactory
    ///
    /// <summary>
    /// Ghostscript.Converter オブジェクトを生成するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class GhostscriptFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Ghostscript.Converter オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="src">設定情報</param>
        ///
        /// <returns>Ghostscript.Converter オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Ghostscript.Converter Create(SettingsFolder src)
        {
            var dir  = src.IO.Get(AssemblyReader.Default.Location).DirectoryName;
            var dest = DocumentConverter.SupportedFormats.Contains(src.Value.Format) ?
                       CreateDocumentConverter(src) :
                       CreateImageConverter(src);

            dest.Quiet         = false;
            dest.WorkDirectory = src.WorkDirectory;
            dest.Resolution    = src.Value.Resolution;
            dest.Orientation   = src.Value.Orientation;
            dest.Resources.Add(src.IO.Combine(dir, "lib"));

            return dest;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDocumentConverter
        ///
        /// <summary>
        /// DocumentConverter オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Ghostscript.Converter CreateDocumentConverter(SettingsFolder src) =>
            new DocumentConverter(src.Value.Format, src.IO)
            {
                ColorMode    = src.Value.Grayscale ? ColorMode.Grayscale : ColorMode.Rgb,
                Compression  = src.Value.ImageCompression ? Encoding.Jpeg : Encoding.Flate,
                Downsampling = src.Value.Downsampling,
                EmbedFonts   = src.Value.EmbedFonts,
                Version      = src.Value.FormatOption.GetVersion(),
            };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImageConverter
        ///
        /// <summary>
        /// ImageConverter オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Ghostscript.Converter CreateImageConverter(SettingsFolder src)
        {
            var key = KeyValuePair.Create(src.Value.Format, src.Value.Grayscale);
            var map = GetFormatMap();

            Debug.Assert(map.ContainsKey(key));

            return new ImageConverter(GetFormatMap()[key], src.IO)
            {
                AntiAlias = true,
            };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormatMap
        ///
        /// <summary>
        /// Format の対応関係を取得します。
        /// </summary>
        ///
        /// <remarks>
        /// Key は (Format, Grayscale) のペアになります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<KeyValuePair<Format, bool>, Format> GetFormatMap() => _formats ?? (
            _formats = new Dictionary<KeyValuePair<Format, bool>, Format>
            {
                { KeyValuePair.Create(Format.Jpeg, false), Format.Jpeg24bppRgb      },
                { KeyValuePair.Create(Format.Jpeg, true ), Format.Jpeg8bppGrayscale },
                { KeyValuePair.Create(Format.Png,  false), Format.Png24bppRgb       },
                { KeyValuePair.Create(Format.Png,  true ), Format.Png8bppGrayscale  },
                { KeyValuePair.Create(Format.Bmp,  false), Format.Bmp24bppRgb       },
                { KeyValuePair.Create(Format.Bmp,  true ), Format.Bmp8bppGrayscale  },
                { KeyValuePair.Create(Format.Tiff, false), Format.Tiff24bppRgb      },
                { KeyValuePair.Create(Format.Tiff, true ), Format.Tiff8bppGrayscale },
            }
        );

        #endregion

        #region Fields
        private static IDictionary<KeyValuePair<Format, bool>, Format> _formats;
        #endregion
    }
}
