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
namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RibbonItem
    ///
    /// <summary>
    /// Ribbon の項目を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class RibbonItem : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// RibbonItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="name">名前</param>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem(string name) : this(name, name) { }

        /* ----------------------------------------------------------------- */
        ///
        /// RibbonItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="name">名前</param>
        /// <param name="text">表示テキスト</param>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem(string name, string text)
        {
            Name = name;
            Text = text;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Assets
        ///
        /// <summary>
        /// リソースの保存されているディレクトリを表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string Assets { get; } = "pack://application:,,,/Assets";

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// アイコンを示す名前を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Text
        ///
        /// <summary>
        /// 表示テキストを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Text { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// LargeIcon
        ///
        /// <summary>
        /// 大きいサイズのアイコンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string LargeIcon => $"{Assets}/Large/{Name}.png";

        /* ----------------------------------------------------------------- */
        ///
        /// SmallIcon
        ///
        /// <summary>
        /// 小さいサイズのアイコンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SmallIcon => $"{Assets}/Small/{Name}.png";

        #endregion
    }
}
