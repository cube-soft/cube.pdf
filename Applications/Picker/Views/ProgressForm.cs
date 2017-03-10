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
using System.ComponentModel;
using System.Windows.Forms;
using Cube.Forms.Controls;

namespace Cube.Pdf.App.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressForm
    ///
    /// <summary>
    /// 進捗状況を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class ProgressForm : Cube.Forms.FormBase
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

            ExitButton.Click    += (s, e) => Close();
            SaveButton.Click    += (s, e) => EventAggregator.GetEvents()?.Save.Publish(null);
            PreviewButton.Click += (s, e) => EventAggregator.GetEvents()?.Preview.Publish();
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
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowOperation
        {
            get { return _op; }
            set
            {
                if (_op == value) return;
                _op = value;
                PreviewButton.Enabled = value;
                SaveButton.Enabled = value;
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
                if (_filename == value) return;
                _filename = value;
                this.UpdateText(value);
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

        #region Fields
        private string _filename = string.Empty;
        private bool _op = true;
        #endregion
    }
}
