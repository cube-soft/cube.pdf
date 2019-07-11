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
using Cube.Images.Icons;
using Cube.Mixin.Assembly;
using Cube.Mixin.ByteFormat;
using Cube.Mixin.Generics;
using Cube.Xui.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace Cube.Pdf.Editor
{
    #region TitleConverter

    /* --------------------------------------------------------------------- */
    ///
    /// TitleConverter
    ///
    /// <summary>
    /// Provides functionality to convert a string.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class TitleConverter : MarkupExtension, IMultiValueConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Converts to the title from the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object Convert(object[] values, Type target, object parameter, CultureInfo culture)
        {
            var app = Assembly.GetExecutingAssembly().GetTitle();
            if (values.Length < 2) return app;

            var m = values[1].TryCast<bool>() ? "*" : "";
            return values[0] is Information fi ? $"{fi.Name}{m} - {app}" : app;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ConvertBack
        ///
        /// <summary>
        /// Does not support the method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object[] ConvertBack(object value, Type[] targets, object parameter, CultureInfo culture)
            => throw new NotSupportedException();

        /* ----------------------------------------------------------------- */
        ///
        /// ProvideValue
        ///
        /// <summary>
        /// Gets the this instance.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }

    #endregion

    #region IconConverter

    /* --------------------------------------------------------------------- */
    ///
    /// IconConverter
    ///
    /// <summary>
    /// Provides functionality to convert from the specified value to the
    /// icon image.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class IconConverter : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SelectionToText
        ///
        /// <summary>
        /// Initializes a new instance of the SelectionToText class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IconConverter() : base(e => (e as Information)?.GetIconImage(IconSize.Small)) { }
    }

    #endregion

    #region LanguageConverter

    /* --------------------------------------------------------------------- */
    ///
    /// LanguageConverter
    ///
    /// <summary>
    /// Provides functionality to convert a Language value.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class LanguageConverter : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// LanguageConverter
        ///
        /// <summary>
        /// Initializes a new instance of the LanguageConverter
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public LanguageConverter() : base(e => new Dictionary<Language, string>
            {
                { Language.Auto,     Properties.Resources.MenuLanguageAuto     },
                { Language.English,  Properties.Resources.MenuLanguageEnglish  },
                { Language.Japanese, Properties.Resources.MenuLanguageJapanese },
            }.TryGetValue(e.TryCast<Language>(), out var dest) ? dest : Properties.Resources.MenuLanguageAuto
        ) { }
    }

    #endregion

    #region ByteConverter

    /* --------------------------------------------------------------------- */
    ///
    /// ByteConverter
    ///
    /// <summary>
    /// Provides functionality to convert a string.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ByteConverter : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ByteConverter
        ///
        /// <summary>
        /// Initializes a new instance of the ByteConverter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ByteConverter() : base(e =>
        {
            var n  = e.TryCast<long>();
            return string.Format("{0} ({1:#,0} {2})", n.ToRoughBytes(), n, Properties.Resources.UnitByte);
        }) { }
    }

    #endregion

    #region ByteConverterLite

    /* --------------------------------------------------------------------- */
    ///
    /// ByteConverterLite
    ///
    /// <summary>
    /// Provides functionality to convert a string.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ByteConverterLite : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ByteConverterLite
        ///
        /// <summary>
        /// Initializes a new instance of the ByteConverterLite class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ByteConverterLite() : base(e => e.TryCast<long>().ToRoughBytes()) { }
    }

    #endregion

    #region EncryptionMethodConverter

    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionMethodConverter
    ///
    /// <summary>
    /// Provides functionality to convert an EncryptionMethod.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class EncryptionMethodConverter : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethodConverter
        ///
        /// <summary>
        /// Initializes a new instance of the EncryptionMethodConverter
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethodConverter() : base(e => new Dictionary<EncryptionMethod, string>
        {
            { EncryptionMethod.Standard40,   "40-bit RC4"              },
            { EncryptionMethod.Standard128, "128-bit RC4"              },
            { EncryptionMethod.Aes128,      "128-bit AES"              },
            { EncryptionMethod.Aes256,      "256-bit AES"              },
            { EncryptionMethod.Aes256r6,    "256-bit AES (Revision 6)" },
        }.TryGetValue(e.TryCast<EncryptionMethod>(), out var dest) ? dest : "Unknown") { }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ViewerOptionsConverter
    ///
    /// <summary>
    /// Provides functionality to convert a ViewerPreferences.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ViewerOptionsConverter : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ViewerPreferencesConverter
        ///
        /// <summary>
        /// Initializes a new instance of the ViewerPreferencesConverter
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ViewerOptionsConverter() : base(e =>
        {
            var src = e.TryCast<ViewerOption>();
            if (src.HasFlag(ViewerOption.SinglePage))     return Properties.Resources.MenuViewSinglePage;
            if (src.HasFlag(ViewerOption.OneColumn))      return Properties.Resources.MenuViewOneColumn;
            if (src.HasFlag(ViewerOption.TwoPageLeft))    return Properties.Resources.MenuViewTwoPageLeft;
            if (src.HasFlag(ViewerOption.TwoPageRight))   return Properties.Resources.MenuViewTwoPageRight;
            if (src.HasFlag(ViewerOption.TwoColumnLeft))  return Properties.Resources.MenuViewTwoColumnLeft;
            if (src.HasFlag(ViewerOption.TwoColumnRight)) return Properties.Resources.MenuViewTwoColumnRight;
            return "Unknown";
        }) { }
    }

    #endregion

    #region BooleanToCursor

    /* --------------------------------------------------------------------- */
    ///
    /// BooleanToCursor
    ///
    /// <summary>
    /// Provides functionality to convert a boolean to the Cursor.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class BooleanToCursor : BooleanToValue<Cursor>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// BooleanToCursor
        ///
        /// <summary>
        /// Initializes a new instance of the BooleanToCursor class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BooleanToCursor() : base(Cursors.Wait, Cursors.Arrow) { }
    }

    #endregion

    #region CountToText

    /* --------------------------------------------------------------------- */
    ///
    /// CountToText
    ///
    /// <summary>
    /// Provides functionality to convert from the specified value to the
    /// display text.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class CountToText : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// CountToText
        ///
        /// <summary>
        /// Initializes a new instance of the CountToText class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CountToText() : base(e => string.Format(
            Properties.Resources.MessageTotalPage, e.TryCast<int>()
        )) { }
    }

    #endregion

    #region IndexToText

    /* --------------------------------------------------------------------- */
    ///
    /// IndexToText
    ///
    /// <summary>
    /// Provides functionality to convert an index to the display text.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class IndexToText : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// IndexToText
        ///
        /// <summary>
        /// Initializes a new instance of the IndexToText class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IndexToText() : base(e => (e.TryCast<int>() + 1).ToString()) { }
    }

    #endregion

    #region SelectionToText

    /* --------------------------------------------------------------------- */
    ///
    /// SelectionToText
    ///
    /// <summary>
    /// Provides functionality to convert from the specified value to the
    /// display text.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SelectionToText : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SelectionToText
        ///
        /// <summary>
        /// Initializes a new instance of the SelectionToText class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SelectionToText() : base(e => string.Format(
            Properties.Resources.MessageSelection, e.TryCast<int>()
        )) { }
    }

    #endregion

    #region SelectionToVisibility

    /* --------------------------------------------------------------------- */
    ///
    /// SelectionToVisibility
    ///
    /// <summary>
    /// Provides functionality to convert from the specified value to the
    /// visibility.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SelectionToVisibility : BooleanToVisibility
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SelectionToVisibility
        ///
        /// <summary>
        /// Initializes a new instance of the SelectionToVisibility class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SelectionToVisibility() : base(e => e.TryCast<int>() > 0) { }
    }

    #endregion

    #region HasValueToVisibility

    /* --------------------------------------------------------------------- */
    ///
    /// HasValueToVisibility
    ///
    /// <summary>
    /// Provides functionality to convert from the specified value to the
    /// visibility.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class HasValueToVisibility : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// HasValueToVisibility
        ///
        /// <summary>
        /// Initializes a new instance of the HasValueToVisibility
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public HasValueToVisibility() :
            base(e => e != null ? Visibility.Visible : Visibility.Collapsed) { }
    }

    #endregion

    #region HasValueToVisibilityInverse

    /* --------------------------------------------------------------------- */
    ///
    /// HasValueToVisibilityInverse
    ///
    /// <summary>
    /// Provides functionality to convert from the specified value to the
    /// visibility. Note that the class returns Collapsed when the
    /// specified value is null.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class HasValueToVisibilityInverse : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// HasValueToVisibilityInverse
        ///
        /// <summary>
        /// Initializes a new instance of the
        /// HasValueToVisibilityInverse class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public HasValueToVisibilityInverse() :
            base(e => e == null ? Visibility.Visible : Visibility.Collapsed) { }
    }

    #endregion
}
