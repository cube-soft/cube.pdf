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
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// GhostscriptFactory
    ///
    /// <summary>
    /// Provides functionality to create a new instance of the
    /// Ghostscript.Converter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class GhostscriptFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Initializes a new instance of the Ghostscript.Converter class
        /// with the specified settings.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        ///
        /// <returns>Ghostscript.Converter object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Ghostscript.Converter Create(SettingFolder src)
        {
            var asm  = Assembly.GetExecutingAssembly();
            var dir  = src.IO.Get(asm.Location).DirectoryName;
            var dest = DocumentConverter.SupportedFormats.Contains(src.Value.Format) ?
                       CreateDocumentConverter(src) :
                       CreateImageConverter(src);

            dest.Quiet       = false;
            dest.Temp        = src.Value.Temp;
            dest.Log         = src.IO.Combine(src.Value.Temp, src.Uid.ToString("D"), "console.log");
            dest.Resolution  = src.Value.Resolution;
            dest.Orientation = src.Value.Orientation;
            dest.Resources.Add(src.IO.Combine(dir, "lib"));

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// LogDebug
        ///
        /// <summary>
        /// Outputs log of the Ghostscript API.
        /// </summary>
        ///
        /// <param name="src">Ghostscript converter object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void LogDebug(this Ghostscript.Converter src)
        {
            try
            {
                if (!src.Log.HasValue() || !src.IO.Exists(src.Log)) return;
                using (var ss = new StreamReader(src.IO.OpenRead(src.Log)))
                {
                    while (!ss.EndOfStream) src.LogDebug(ss.ReadLine());
                }
            }
            catch (Exception err) { src.LogDebug(err.Message); }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDocumentConverter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentConverter class with
        /// the specified settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static DocumentConverter CreateDocumentConverter(SettingFolder src)
        {
            var dest = PdfConverter.SupportedFormats.Contains(src.Value.Format) ?
                       CreatePdfConverter(src) :
                       new DocumentConverter(src.Value.Format, src.IO);

            dest.ColorMode    = src.Value.Grayscale ? ColorMode.Grayscale : ColorMode.Rgb;
            dest.Downsampling = src.Value.Downsampling;
            dest.EmbedFonts   = src.Value.EmbedFonts;

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreatePdfConverter
        ///
        /// <summary>
        /// Initializes a new instance of the PdfConverter class with the
        /// specified settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static PdfConverter CreatePdfConverter(SettingFolder src) =>
            new PdfConverter(src.IO)
            {
                Version     = src.Value.Metadata.Version,
                Compression = src.Value.ImageFilter ? Encoding.Jpeg : Encoding.Flate,
            };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImageConverter
        ///
        /// <summary>
        /// Initializes a new instance of the ImageConverter class with
        /// the specified settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Ghostscript.Converter CreateImageConverter(SettingFolder src)
        {
            var key = KeyValuePair.Create(src.Value.Format, src.Value.Grayscale);
            Debug.Assert(FormatMap.ContainsKey(key));
            return new ImageConverter(FormatMap[key], src.IO) { AntiAlias = true };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FormatMap
        ///
        /// <summary>
        /// Gets the Format collection.
        /// </summary>
        ///
        /// <remarks>
        /// Key は (Format, Grayscale) のペアになります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static readonly IDictionary<KeyValuePair<Format, bool>, Format> FormatMap =
            new Dictionary<KeyValuePair<Format, bool>, Format>
            {
                { KeyValuePair.Create(Format.Jpeg, false), Format.Jpeg24bppRgb      },
                { KeyValuePair.Create(Format.Jpeg, true ), Format.Jpeg8bppGrayscale },
                { KeyValuePair.Create(Format.Png,  false), Format.Png24bppRgb       },
                { KeyValuePair.Create(Format.Png,  true ), Format.Png8bppGrayscale  },
                { KeyValuePair.Create(Format.Bmp,  false), Format.Bmp24bppRgb       },
                { KeyValuePair.Create(Format.Bmp,  true ), Format.Bmp8bppGrayscale  },
                { KeyValuePair.Create(Format.Tiff, false), Format.Tiff24bppRgb      },
                { KeyValuePair.Create(Format.Tiff, true ), Format.Tiff8bppGrayscale },
            };

        #endregion
    }
}
