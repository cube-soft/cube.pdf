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
using Cube.Conversions;
using Cube.FileSystem;
using Cube.Generics;
using Cube.Xui.Converters;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Cube.Pdf.App.Editor
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
    public class TitleConverter : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// TitleConverter
        ///
        /// <summary>
        /// Initializes a new instance of the <c>TitleConverter</c> class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TitleConverter() : base(e =>
        {
            var app = Assembly.GetExecutingAssembly().GetReader().Title;
            return e is Information fi ? $"{fi.Name} - {app}" : app;
        }) { }
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
        /// Initializes a new instance of the <c>LanguageConverter</c>
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
        /// Initializes a new instance of the <c>ByteConverter</c> class.
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

    #region EncryptionMethodConverter

    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionMethodConverter
    ///
    /// <summary>
    /// Provides functionality to convert an <c>EncryptionMethod</c>.
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
        /// Initializes a new instance of the <c>EncryptionMethodConverter</c>
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethodConverter() : base(e =>
        {
            switch (e.TryCast<EncryptionMethod>())
            {
                case EncryptionMethod.Standard40:  return  "40-bit RC4";
                case EncryptionMethod.Standard128: return "128-bit RC4";
                case EncryptionMethod.Aes128:      return "128-bit AES";
                case EncryptionMethod.Aes256:      return "256-bit AES";
                case EncryptionMethod.Aes256r6:    return "256-bit AES (Revision 6)";
                default: return "Unknown";
            }
        }) { }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ViewerPreferencesConverter
    ///
    /// <summary>
    /// Provides functionality to convert a <c>ViewerPreferences</c>.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ViewerPreferencesConverter : SimplexConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ViewerPreferencesConverter
        ///
        /// <summary>
        /// Initializes a new instance of the <c>ViewerPreferencesConverter</c>
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ViewerPreferencesConverter() : base(e =>
        {
            var src = e.TryCast<ViewerPreferences>();
            if (src.HasFlag(ViewerPreferences.SinglePage))     return Properties.Resources.MenuViewSinglePage;
            if (src.HasFlag(ViewerPreferences.OneColumn))      return Properties.Resources.MenuViewOneColumn;
            if (src.HasFlag(ViewerPreferences.TwoPageLeft))    return Properties.Resources.MenuViewTwoPageLeft;
            if (src.HasFlag(ViewerPreferences.TwoPageRight))   return Properties.Resources.MenuViewTwoPageRight;
            if (src.HasFlag(ViewerPreferences.TwoColumnLeft))  return Properties.Resources.MenuViewTwoColumnLeft;
            if (src.HasFlag(ViewerPreferences.TwoColumnRight)) return Properties.Resources.MenuViewTwoColumnRight;
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
        /// Initializes a new instance of the <c>HasValueToVisibility</c>
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
        /// <c>HasValueToVisibilityInverse</c> class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public HasValueToVisibilityInverse() :
            base(e => e == null ? Visibility.Visible : Visibility.Collapsed) { }
    }

    #endregion
}
