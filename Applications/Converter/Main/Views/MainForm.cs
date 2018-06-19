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
using Cube.Forms.Behaviors;
using Cube.Forms.Controls;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainForm
    ///
    /// <summary>
    /// CubePDF メイン画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainForm : Cube.Forms.StandardForm
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainForm()
        {
            InitializeComponent();

            ExitButton.Click += (s, e) => Close();

            new PasswordBehavior(OwnerPasswordTextBox, OwnerConfirmTextBox);
            new PasswordBehavior(UserPasswordTextBox, UserConfirmTextBox);

            SetComboBox();
            SettingsPanel.ApplyButton = ApplyButton;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// オブジェクトを関連付けます。
        /// </summary>
        ///
        /// <param name="vm">ViewModel オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Bind(MainViewModel vm)
        {
            if (vm == null) return;

            MainBindingSource.DataSource       = vm;
            SettingsBindingSource.DataSource   = vm.Settings;
            MetadataBindingSource.DataSource   = vm.Metadata;
            EncryptionBindingSource.DataSource = vm.Encryption;

            vm.Messenger.Close.Subscribe(() => Close());
            vm.Messenger.SetCulture.Subscribe(e => SetCulture(e));
            vm.Messenger.MessageBox.Subscribe(e => new MessageBoxBehavior().Invoke(e));
            vm.Messenger.OpenFileDialog.Subscribe(e => new OpenFileBehavior().Invoke(e));
            vm.Messenger.SaveFileDialog.Subscribe(e => new SaveFileBehavior().Invoke(e));

            SourceButton.Click      += (s, e) => vm.BrowseSource();
            DestinationButton.Click += (s, e) => vm.BrowseDestination();
            UserProgramButton.Click += (s, e) => vm.BrowseUserProgram();
            ConvertButton.Click     += (s, e) => vm.Convert();
            SettingsPanel.Apply     += (s, e) => vm.Save();

            DataBindings.Add(new Binding(nameof(IsBusy), MainBindingSource,
                nameof(IsBusy), false, DataSourceUpdateMode.OnPropertyChanged));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IsBusy
        ///
        /// <summary>
        /// 実行中かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsBusy
        {
            get => _busy;
            set
            {
                _busy = value;
                ConvertButton.Enabled = !value;
                SettingsTabControl.Enabled = !value;
                ApplyButton.Visible = !value;
                ConvertProgressBar.Visible = value;
                Cursor = value ? Cursors.WaitCursor : Cursors.Default;
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetCulture
        ///
        /// <summary>
        /// 表示言語を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetCulture(string name)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);
            ComponentResourceManager src = new ComponentResourceManager(typeof(MainForm));
            src.ApplyResources(this, "$this");
            SetCulture(src, Controls);
            SetComboBox();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCulture
        ///
        /// <summary>
        /// 表示言語を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetCulture(ComponentResourceManager src, Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                src.ApplyResources(control, control.Name);
                SetCulture(src, control.Controls);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetComboBox
        ///
        /// <summary>
        /// ComboBox の内容を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetComboBox()
        {
            Preserve(FormatComboBox,       e => e.Bind(ViewResource.Formats));
            Preserve(FormatOptionComboBox, e => e.Bind(ViewResource.FormatOptions));
            Preserve(SaveOptionComboBox,   e => e.Bind(ViewResource.SaveOptions));
            Preserve(ViewOptionComboBox,   e => e.Bind(ViewResource.ViewOptions));
            Preserve(PostProcessComboBox,  e => e.Bind(ViewResource.PostProcesses));
            Preserve(LanguageComboBox,     e => e.Bind(ViewResource.Languages));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Preserve
        ///
        /// <summary>
        /// ComboBox.SelectedValue の内容を保持したまま処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Preserve(ComboBox src, Action<ComboBox> action)
        {
            var value = src.SelectedValue;
            action(src);
            if (value != null) src.SelectedValue = value;
        }

        #endregion

        #region Fields
        private bool _busy = false;
        #endregion
    }
}
