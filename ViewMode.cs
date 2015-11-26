/* ------------------------------------------------------------------------- */
///
/// ViewMode.cs
/// 
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ViewMode
    /// 
    /// <summary>
    /// PDF ファイルの表示方法を定義した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum ViewMode
    {
        None       = 0x0040,
        Outline    = 0x0080,
        Thumbnail  = 0x0100,
        FullScreen = 0x0200,
        OC         = 0x0400,
        Attachment = 0x0800
    }
}
