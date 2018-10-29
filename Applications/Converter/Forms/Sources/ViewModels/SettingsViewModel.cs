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
using Cube.Forms;
using Cube.Pdf.Ghostscript;
using System.Threading;

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
    public class SettingsViewModel : ViewModelBase<Messenger>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="model">設定情報</param>
        /// <param name="messenger">Messenger オブジェクト</param>
        /// <param name="context">同期用コンテキスト</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsViewModel(Settings model, Messenger messenger,
            SynchronizationContext context) : base(messenger, context)
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
        /// 設定情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Settings Model { get; }

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
            get => Model.Format;
            set
            {
                Model.Format = value;
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
            get => Model.FormatOption;
            set => Model.FormatOption = value;
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
            get => Model.SaveOption;
            set => Model.SaveOption = value;
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
            get => Model.PostProcess;
            set
            {
                Model.PostProcess = value;
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
            get => Model.Source;
            set => Model.Source = value;
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
            get => Model.Destination;
            set => Model.Destination = value;
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
            get => Model.UserProgram;
            set => Model.UserProgram = value;
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
            get => Model.Resolution;
            set => Model.Resolution = value;
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
            get => Model.Orientation == Orientation.Auto;
            set
            {
                if (value)
                {
                    Model.Orientation = Orientation.Auto;
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
            get => Model.Orientation == Orientation.Portrait;
            set
            {
                if (value)
                {
                    Model.Orientation = Orientation.Portrait;
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
            get => Model.Orientation == Orientation.Landscape;
            set
            {
                if (value)
                {
                    Model.Orientation = Orientation.Landscape;
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
            get => Model.Grayscale;
            set => Model.Grayscale = value;
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
            get => Model.ImageCompression;
            set => Model.ImageCompression = value;
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
            get => Model.Linearization;
            set => Model.Linearization = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// アップデートを確認するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CheckUpdate
        {
            get => Model.CheckUpdate;
            set => Model.CheckUpdate = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Language
        ///
        /// <summary>
        /// 表示言語を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Language Language
        {
            get => Model.Language;
            set => Model.Language = value;
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
        public bool SourceVisible => Model.SourceVisible;

        /* ----------------------------------------------------------------- */
        ///
        /// SourceEditable
        ///
        /// <summary>
        /// 入力ファイルを変更可能かどうかを示す値を取得します。
        /// </summary>
        ///
        /// <remarks>
        /// 現在は、プログラム引数で /DeleteOnClose オプションとともに
        /// 入力ファイルが指定された場合、変更不可能な形となっています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool SourceEditable => !Model.DeleteSource;

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
    }
}
