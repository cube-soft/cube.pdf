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
    public class MetadataViewModel : ObservableProperty
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
        ///
        /* ----------------------------------------------------------------- */
        public MetadataViewModel(Metadata model)
        {
            _model = model;
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

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
            get => _model.Title;
            set => _model.Title = value;
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
            get => _model.Author;
            set => _model.Author = value;
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
            get => _model.Subtitle;
            set => _model.Subtitle = value;
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
            get => _model.Keywords;
            set => _model.Keywords = value;
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
            get => _model.Creator;
            set => _model.Creator = value;
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
            get => _model.ViewOption;
            set => _model.ViewOption = value;
        }

        #endregion

        #region Fields
        private readonly Metadata _model;
        #endregion
    }
}
