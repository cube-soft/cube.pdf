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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Cube.FileSystem;
using Cube.Logging;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;

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
            var dir  = Io.Get(asm.Location).DirectoryName;
            var dest = DocumentConverter.SupportedFormats.Contains(src.Value.Format) ?
                       CreateDocumentConverter(src) :
                       CreateImageConverter(src);

            dest.Quiet       = false;
            dest.Temp        = GetTempOrEmpty(src.Value);
            dest.Log         = Io.Combine(src.Value.Temp, src.Uid.ToString("N"), "console.log");
            dest.Resolution  = src.Value.Resolution;
            dest.Orientation = src.Value.Orientation;
            dest.Resources.Add(Io.Combine(dir, "lib"));

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
                if (!src.Log.HasValue() || !Io.Exists(src.Log)) return;
                using var ss = new StreamReader(Io.Open(src.Log));
                while (!ss.EndOfStream) src.GetType().LogDebug(ss.ReadLine());
            }
            catch (Exception err) { src.GetType().LogDebug(err.Message); }
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
                       new DocumentConverter(src.Value.Format);

            dest.ColorMode    = src.Value.Grayscale ? ColorMode.Grayscale : ColorMode.SameAsSource;
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
        private static PdfConverter CreatePdfConverter(SettingFolder src) => new()
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
            var key = new KeyValuePair<Format, bool>(src.Value.Format, src.Value.Grayscale);
            Debug.Assert(FormatMap.ContainsKey(key));
            return new ImageConverter(FormatMap[key]) { AntiAlias = true };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTempOrEmpty
        ///
        /// <summary>
        /// Gets a temporary directory if necessary.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetTempOrEmpty(SettingValue src)
        {
            var e0 = Environment.GetEnvironmentVariable("Tmp");
            var e1 = Environment.GetEnvironmentVariable("Temp");

            return e0.Length != System.Text.Encoding.UTF8.GetByteCount(e0) ||
                   e1.Length != System.Text.Encoding.UTF8.GetByteCount(e1) ?
                   src.Temp : string.Empty;
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
        /// Key is a (Format, Grayscale) pair.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static readonly Dictionary<KeyValuePair<Format, bool>, Format> FormatMap = new()
        {
            { new(Format.Jpeg, false), Format.Jpeg24bppRgb      },
            { new(Format.Jpeg, true ), Format.Jpeg8bppGrayscale },
            { new(Format.Png,  false), Format.Png24bppRgb       },
            { new(Format.Png,  true ), Format.Png8bppGrayscale  },
            { new(Format.Bmp,  false), Format.Bmp24bppRgb       },
            { new(Format.Bmp,  true ), Format.Bmp8bppGrayscale  },
            { new(Format.Tiff, false), Format.Tiff24bppRgb      },
            { new(Format.Tiff, true ), Format.Tiff8bppGrayscale },
        };

        #endregion
    }
}
