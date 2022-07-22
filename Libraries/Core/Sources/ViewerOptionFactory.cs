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
namespace Cube.Pdf;

using System.Collections.Generic;
using System.Linq;
using Cube.Mixin.String;

/* ------------------------------------------------------------------------- */
///
/// ViewerOptionFactory
///
/// <summary>
/// Provides extended methods of the ViewerOption.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class ViewerOptionFactory
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Create
    ///
    /// <summary>
    /// Creates a new ViewerOption value from the specified value.
    /// </summary>
    ///
    /// <param name="src">Value for options.</param>
    ///
    /// <returns>ViewerOption objects.</returns>
    ///
    /// <remarks>
    /// Ignores flags that do not define in the ViewerOption.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static ViewerOption Create(int src) => (ViewerOption)(src & 0x0fff);

    /* --------------------------------------------------------------------- */
    ///
    /// Create
    ///
    /// <summary>
    /// Creates a new ViewerOption value from the specified arguments.
    /// </summary>
    ///
    /// <param name="layout">PDF name for the page layout.</param>
    /// <param name="mode">PDF name for the page mode.</param>
    ///
    /// <returns>ViewerOption objects.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static ViewerOption Create(string layout, string mode)
    {
        var dest = ViewerOption.None;
        if (layout.HasValue()) dest |= _Layouts.FirstOrDefault(e => e.ToName().Equals(layout));
        if (mode.HasValue())   dest |= _Modes.FirstOrDefault(e => e.ToName().Equals(mode));
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ToPageLayout
    ///
    /// <summary>
    /// Converts to the page layout option fromt the specified viewer
    /// option.
    /// </summary>
    ///
    /// <param name="src">Viewer options.</param>
    ///
    /// <returns>Page layout option.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static ViewerOption ToPageLayout(this ViewerOption src) => src & _LayoutMask;

    /* --------------------------------------------------------------------- */
    ///
    /// ToPageLayout
    ///
    /// <summary>
    /// Converts to the page mode option fromt the specified viewer
    /// option.
    /// </summary>
    ///
    /// <param name="src">Viewer options.</param>
    ///
    /// <returns>Page mode option.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static ViewerOption ToPageMode(this ViewerOption src) => src & _ModeMask;

    /* --------------------------------------------------------------------- */
    ///
    /// ToName
    ///
    /// <summary>
    /// Converts to the PDF name fromt the specified viewer option.
    /// </summary>
    ///
    /// <param name="src">Viewer options.</param>
    ///
    /// <returns>PDF name.</returns>
    ///
    /// <remarks>
    /// If the specified value has more than one ViewerOption enum,
    /// the first matching string will be returned.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static string ToName(this ViewerOption src)
    {
        var pl = src.ToPageLayout();
        if (pl != ViewerOption.None) return _Layouts.First(e => pl.HasFlag(e)).ToString();

        var pm = src.ToPageMode();
        if (pm.HasFlag(ViewerOption.Outline)) return "UseOutlines";
        if (pm.HasFlag(ViewerOption.Thumbnail)) return "UseThumbs";
        if (pm.HasFlag(ViewerOption.FullScreen)) return "FullScreen";
        if (pm.HasFlag(ViewerOption.OptionalContent)) return "UseOC";
        if (pm.HasFlag(ViewerOption.Attachment)) return "UseAttachments";
        return "UseNone";
    }

    #endregion

    #region Fields

    private static readonly List<ViewerOption> _Layouts = new()
    {
        ViewerOption.SinglePage,
        ViewerOption.OneColumn,
        ViewerOption.TwoColumnLeft,
        ViewerOption.TwoColumnRight,
        ViewerOption.TwoPageLeft,
        ViewerOption.TwoPageRight,
    };
    private static readonly ViewerOption _LayoutMask = _Layouts.Aggregate((x, e) => x | e);

    private static readonly List<ViewerOption> _Modes = new()
    {
        ViewerOption.None,
        ViewerOption.Outline,
        ViewerOption.Thumbnail,
        ViewerOption.FullScreen,
        ViewerOption.OptionalContent,
        ViewerOption.Attachment,
    };
    private static readonly ViewerOption _ModeMask = _Modes.Aggregate((x, e) => x | e);

    #endregion
}
