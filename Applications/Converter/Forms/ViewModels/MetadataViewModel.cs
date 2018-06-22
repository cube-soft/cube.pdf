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
namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataViewModel
    ///
    /// <summary>
    /// 文書プロパティタブを表す ViewModel です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MetadataViewModel : Cube.Forms.ViewModelBase<Messenger>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MetadataViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="model">PDF メタ情報</param>
        /// <param name="messenger">Messenger オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public MetadataViewModel(Metadata model, Messenger messenger) : base(messenger)
        {
            Model = model;
            Model.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Model
        ///
        /// <summary>
        /// PDF メタ情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Metadata Model { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// タイトルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Title
        {
            get => Model.Title;
            set => Model.Title = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Author
        ///
        /// <summary>
        /// 作成者を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Author
        {
            get => Model.Author;
            set => Model.Author = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Subtitle
        ///
        /// <summary>
        /// サブタイトルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Subtitle
        {
            get => Model.Subtitle;
            set => Model.Subtitle = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Keywords
        ///
        /// <summary>
        /// キーワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Keywords
        {
            get => Model.Keywords;
            set => Model.Keywords = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Creator
        ///
        /// <summary>
        /// アプリケーションを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Creator
        {
            get => Model.Creator;
            set => Model.Creator = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ViewOption
        ///
        /// <summary>
        /// 表示オプションを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ViewOption ViewOption
        {
            get => Model.ViewOption;
            set => Model.ViewOption = value;
        }

        #endregion
    }
}
