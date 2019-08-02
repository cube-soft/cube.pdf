/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// RenderFlags
    ///
    /// <summary>
    /// Specifies the flags for rendering.
    /// </summary>
    ///
    /// <seealso href="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h"/>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    internal enum RenderFlags
    {
        /// <summary>No flags.</summary>
        Empty = 0x000,
        /// <summary>Set if annotations are to be rendered.</summary>
        Annotation = 0x0001,
        /// <summary>Set if using text rendering optimized for LCD display.</summary>
        Lcd = 0x0002,
        /// <summary>Don't use the native text output available on some platforms.</summary>
        NoNativeText = 0x0004,
        /// <summary>Grayscale output.</summary>
        Grayscale = 0x0008,
        /// <summary>Set whether to render in a reverse Byte order, this flag is only used when rendering to a bitmap.</summary>
        ReverseByteOrder = 0x0010,
        /// <summary>Set if you want to get some debug info.</summary>
        Debug = 0x0080,
        /// <summary>Set if you don't want to catch exceptions.</summary>
        NoCatch = 0x0100,
        /// <summary>Limit image cache size.</summary>
        LimitedImageCache = 0x0200,
        /// <summary>Always use halftone for image stretching.</summary>
        ForceHalftone = 0x0400,
        /// <summary>Render for printing.</summary>
        Printng = 0x0800,
        /// <summary>Set to disable anti-aliasing on text.</summary>
        NoSmoothText = 0x1000,
        /// <summary>Set to disable anti-aliasing on images.</summary>
        NoSmoothImage = 0x2000,
        /// <summary>Set to disable anti-aliasing on paths.</summary>
        NoSmoothPath = 0x4000
    }
}
