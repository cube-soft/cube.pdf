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
using Cube.Pdf.Ghostscript;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsViewModel
    ///
    /// <summary>
    /// 一般およびその他タブを表す ViewModel です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingsViewModel : Cube.Forms.ViewModelBase<Messenger>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// GeneralViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="model">設定情報</param>
        /// <param name="messenger">Messenger オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsViewModel(Settings model, Messenger messenger) : base(messenger)
        {
            _model = model;
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// 変換形式を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format
        {
            get => _model.Format;
            set
            {
                _model.Format = value;
                RaisePropertyChanged(nameof(EnableFormatOption));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FormatOption
        ///
        /// <summary>
        /// 変換形式に関するオプションを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FormatOption FormatOption
        {
            get => _model.FormatOption;
            set => _model.FormatOption = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOption
        ///
        /// <summary>
        /// 保存オプションを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SaveOption SaveOption
        {
            get => _model.SaveOption;
            set => _model.SaveOption = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PostProcess
        ///
        /// <summary>
        /// ポストプロセスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PostProcess PostProcess
        {
            get => _model.PostProcess;
            set
            {
                _model.PostProcess = value;
                RaisePropertyChanged(nameof(EnableUserProgram));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 入力ファイルのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source
        {
            get => _model.Source;
            set => _model.Source = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// 保存パスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination
        {
            get => _model.Destination;
            set => _model.Destination = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UserProgram
        ///
        /// <summary>
        /// ユーザプログラムのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string UserProgram
        {
            get => _model.UserProgram;
            set => _model.UserProgram = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// 解像度を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Resolution
        {
            get => _model.Resolution;
            set => _model.Resolution = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsAutoOrientation
        ///
        /// <summary>
        /// ページの向きが自動かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsAutoOrientation
        {
            get => _model.Orientation == Orientation.Auto;
            set
            {
                if (value)
                {
                    _model.Orientation = Orientation.Auto;
                    RaisePropertyChanged(nameof(IsAutoOrientation));
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsPortrait
        ///
        /// <summary>
        /// ページが縦向きかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsPortrait
        {
            get => _model.Orientation == Orientation.Portrait;
            set
            {
                if (value)
                {
                    _model.Orientation = Orientation.Portrait;
                    RaisePropertyChanged(nameof(IsPortrait));
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsLandscape
        ///
        /// <summary>
        /// ページが横向きかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsLandscape
        {
            get => _model.Orientation == Orientation.Landscape;
            set
            {
                if (value)
                {
                    _model.Orientation = Orientation.Landscape;
                    RaisePropertyChanged(nameof(IsLandscape));
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Grayscale
        ///
        /// <summary>
        /// グレースケールかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Grayscale
        {
            get => _model.Grayscale;
            set => _model.Grayscale = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageCompression
        ///
        /// <summary>
        /// PDF 中の画像を JPEG 形式で圧縮するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ImageCompression
        {
            get => _model.ImageCompression;
            set => _model.ImageCompression = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Linearization
        ///
        /// <summary>
        /// PDF ファイルを Web 表示用に最適化するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Linearization
        {
            get => _model.Linearization;
            set => _model.Linearization = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SourceVisible
        ///
        /// <summary>
        /// 入力ファイルを表示するかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SourceVisible => _model.SourceVisible;

        /* ----------------------------------------------------------------- */
        ///
        /// EnableFormatOption
        ///
        /// <summary>
        /// FormatOption の項目が選択可能かどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EnableFormatOption => Format == Format.Pdf;

        /* ----------------------------------------------------------------- */
        ///
        /// EnableUserProgram
        ///
        /// <summary>
        /// UserProgram が入力可能かどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EnableUserProgram => PostProcess == PostProcess.Others;

        #endregion

        #region Fields
        private readonly Settings _model;
        #endregion
    }
}
