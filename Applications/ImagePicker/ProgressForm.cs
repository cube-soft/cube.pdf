/* ------------------------------------------------------------------------- */
///
/// ProgressForm.cs
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Windows.Forms;
using Cube.Pdf.ImageEx.Extensions;

namespace Cube.Pdf.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.ProgressForm
    ///
    /// <summary>
    /// 進捗状況を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class ProgressForm : Cube.Forms.NtsForm
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressForm
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ProgressForm()
        {
            InitializeComponent();

            ExitButton.Click += (s, e) => Close();
            SaveButton.Click += (s, e) => OnSave(e);
            PreviewButton.Click += (s, e) => OnPreview(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// AllowOperation
        /// 
        /// <summary>
        /// ユーザに各種処理を行う事を許可するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool AllowOperation
        {
            get { return _op; }
            set
            {
                _op = value;
                PreviewButton.UpdateAppearance(value);
                SaveButton.UpdateAppearance(value);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        /// 
        /// <summary>
        /// ファイル名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string FileName
        {
            get { return _filename; }
            set
            {
                if (_filename != value)
                {
                    _filename = value;
                    this.UpdateTitle(value);
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        /// 
        /// <summary>
        /// 進捗に関するメッセージを取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Message
        {
            get { return MessageLabel.Text; }
            set { MessageLabel.Text = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        /// 
        /// <summary>
        /// 進捗状況を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public int Value
        {
            get { return ProgressBar.Value; }
            set
            {
                ProgressBar.Style = (value < 0) ?
                                    ProgressBarStyle.Marquee :
                                    ProgressBarStyle.Continuous;
                ProgressBar.Value = Math.Max(Math.Min(value, 100), 0);
            }
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        /// 
        /// <summary>
        /// 抽出した画像のプレビュー画面を表示する時に発生するイベントです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public EventHandler Preview;

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// 抽出した画像を保存する時に発生するイベントです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public EventHandler Save;

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnPreview
        /// 
        /// <summary>
        /// 抽出画像のプレビュー画面を表示する時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected virtual void OnPreview(EventArgs e)
        {
            if (Preview != null) Preview(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        /// 
        /// <summary>
        /// 抽出画像を保存する時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected virtual void OnSave(EventArgs e)
        {
            if (Save != null) Save(this, e);
        }

        #endregion

        #region Fields
        private string _filename = string.Empty;
        private bool _op = false;
        #endregion
    }
}
