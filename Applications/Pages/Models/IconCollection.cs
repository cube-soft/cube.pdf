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
using Cube.Images.Icons;
using Cube.Log;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.Pdf.Pages.App
{
    /* --------------------------------------------------------------------- */
    ///
    /// IconCollection
    ///
    /// <summary>
    /// ListView に表示するアイコンを管理するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class IconCollection
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// IconCollection
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IconCollection()
        {
            ImageList = new ImageList();
            ImageList.ImageSize = new Size(16, 16);
            ImageList.ColorDepth = ColorDepth.Depth32Bit;
            ImageList.Images.Add(DefaultIcon());
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// ImageList
        ///
        /// <summary>
        /// ListView 用の ImageList を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageList ImageList { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// 新しい項目のアイコンを登録します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Register(MediaFile file)
        {
            var icon = file.RawObject.GetIcon(IconSize.Small);
            if (icon == null) return 0;

            var extension = file.Extension.ToLower();
            if (_map.ContainsKey(extension)) return _map[extension];

            var index = ImageList.Images.Count;
            ImageList.Images.Add(icon);
            _map.Add(extension, index);
            return index;
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// DefaultIcon
        ///
        /// <summary>
        /// 既定のアイコンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Icon DefaultIcon()
        {
            try { return IconFactory.Create(StockIcons.DocumentNotAssociated, IconSize.Small); }
            catch (Exception err)
            {
                this.LogError(err.Message, err);
                return Properties.Resources.NotAssociated;
            }
        }

        #endregion

        #region Fields
        private Dictionary<string, int> _map = new Dictionary<string, int>();
        #endregion
    }
}
