namespace Cube.Pdf.App.Converter
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SettingsPanel = new Cube.Forms.SettingsControl();
            this.SettingsTabControl = new System.Windows.Forms.TabControl();
            this.GeneralTabPage = new System.Windows.Forms.TabPage();
            this.GeneralPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ResolutionControl = new System.Windows.Forms.NumericUpDown();
            this.SettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PostProcessComboBox = new System.Windows.Forms.ComboBox();
            this.UserProgramPanel = new System.Windows.Forms.TableLayoutPanel();
            this.UserProgramButton = new System.Windows.Forms.Button();
            this.UserProgramTextBox = new System.Windows.Forms.TextBox();
            this.OrientationPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.PortraitRadioButton = new System.Windows.Forms.RadioButton();
            this.LandscapeRadioButton = new System.Windows.Forms.RadioButton();
            this.AutoRadioButton = new System.Windows.Forms.RadioButton();
            this.OrientationLabel = new System.Windows.Forms.Label();
            this.SourcePanel = new System.Windows.Forms.TableLayoutPanel();
            this.SourceButton = new System.Windows.Forms.Button();
            this.SourceTextBox = new System.Windows.Forms.TextBox();
            this.SourceLabel = new System.Windows.Forms.Label();
            this.PostProcessLabel = new System.Windows.Forms.Label();
            this.DestinationLabel = new System.Windows.Forms.Label();
            this.ResolutionLabel = new System.Windows.Forms.Label();
            this.FormatLabel = new System.Windows.Forms.Label();
            this.DestinationPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SaveOptionComboBox = new System.Windows.Forms.ComboBox();
            this.DestinationButton = new System.Windows.Forms.Button();
            this.DestinationTextBox = new System.Windows.Forms.TextBox();
            this.FormatPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FormatOptionComboBox = new System.Windows.Forms.ComboBox();
            this.FormatComboBox = new System.Windows.Forms.ComboBox();
            this.DocumentPage = new System.Windows.Forms.TabPage();
            this.DocumentPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ViewerPreferencesComboBox = new System.Windows.Forms.ComboBox();
            this.MetadataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ViewerPreferencesLabel = new System.Windows.Forms.Label();
            this.CreatorTextBox = new System.Windows.Forms.TextBox();
            this.CreatorLabel = new System.Windows.Forms.Label();
            this.KeywordsTextBox = new System.Windows.Forms.TextBox();
            this.KeywordsLabel = new System.Windows.Forms.Label();
            this.SubjectTextBox = new System.Windows.Forms.TextBox();
            this.SubjectLabel = new System.Windows.Forms.Label();
            this.AuthorTextBox = new System.Windows.Forms.TextBox();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.EncryptionTabPage = new System.Windows.Forms.TabPage();
            this.EncryptionOuterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.EnableEncryptionCheckBox = new System.Windows.Forms.CheckBox();
            this.EncryptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.EncryptionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.UserPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.OperationLabel = new System.Windows.Forms.Label();
            this.OwnerConfirmTextBox = new System.Windows.Forms.TextBox();
            this.OwnerConfirmLabel = new System.Windows.Forms.Label();
            this.OwnerPasswordTextBox = new System.Windows.Forms.TextBox();
            this.OwnerPasswordLabel = new System.Windows.Forms.Label();
            this.OperationPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SharePasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.UserPasswordPanel = new System.Windows.Forms.TableLayoutPanel();
            this.UserConfirmTextBox = new System.Windows.Forms.TextBox();
            this.UserPasswordTextBox = new System.Windows.Forms.TextBox();
            this.UserConfirmLabel = new System.Windows.Forms.Label();
            this.UserPasswordLabel = new System.Windows.Forms.Label();
            this.PermissionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AllowModifyCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowFormCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowCopyCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowPrintCheckBox = new System.Windows.Forms.CheckBox();
            this.OthersTabPage = new System.Windows.Forms.TabPage();
            this.OthersPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LanguageLabel = new System.Windows.Forms.Label();
            this.GrayscaleCheckBox = new System.Windows.Forms.CheckBox();
            this.OptionsLabel = new System.Windows.Forms.Label();
            this.ImageCompressionCheckBox = new System.Windows.Forms.CheckBox();
            this.LinearizationCheckBox = new System.Windows.Forms.CheckBox();
            this.UpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.LanguageComboBox = new System.Windows.Forms.ComboBox();
            this.AboutLabel = new System.Windows.Forms.Label();
            this.VersionPanel = new Cube.Forms.VersionControl();
            this.MainBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ToolsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ConvertProgressBar = new System.Windows.Forms.ProgressBar();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.ConvertButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.HeaderPictureBox = new System.Windows.Forms.PictureBox();
            this.PathToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RootPanel.SuspendLayout();
            this.SettingsPanel.SuspendLayout();
            this.SettingsTabControl.SuspendLayout();
            this.GeneralTabPage.SuspendLayout();
            this.GeneralPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResolutionControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsBindingSource)).BeginInit();
            this.UserProgramPanel.SuspendLayout();
            this.OrientationPanel.SuspendLayout();
            this.SourcePanel.SuspendLayout();
            this.DestinationPanel.SuspendLayout();
            this.FormatPanel.SuspendLayout();
            this.DocumentPage.SuspendLayout();
            this.DocumentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetadataBindingSource)).BeginInit();
            this.EncryptionTabPage.SuspendLayout();
            this.EncryptionOuterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EncryptionBindingSource)).BeginInit();
            this.EncryptionPanel.SuspendLayout();
            this.OperationPanel.SuspendLayout();
            this.UserPasswordPanel.SuspendLayout();
            this.PermissionPanel.SuspendLayout();
            this.OthersTabPage.SuspendLayout();
            this.OthersPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainBindingSource)).BeginInit();
            this.FooterPanel.SuspendLayout();
            this.ToolsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPictureBox)).BeginInit();
            this.SuspendLayout();
            //
            // RootPanel
            //
            resources.ApplyResources(this.RootPanel, "RootPanel");
            this.RootPanel.Controls.Add(this.SettingsPanel, 0, 1);
            this.RootPanel.Controls.Add(this.FooterPanel, 0, 2);
            this.RootPanel.Controls.Add(this.HeaderPictureBox, 0, 0);
            this.RootPanel.Name = "RootPanel";
            //
            // SettingsPanel
            //
            this.SettingsPanel.Controls.Add(this.SettingsTabControl);
            resources.ApplyResources(this.SettingsPanel, "SettingsPanel");
            this.SettingsPanel.Name = "SettingsPanel";
            //
            // SettingsTabControl
            //
            this.SettingsTabControl.Controls.Add(this.GeneralTabPage);
            this.SettingsTabControl.Controls.Add(this.DocumentPage);
            this.SettingsTabControl.Controls.Add(this.EncryptionTabPage);
            this.SettingsTabControl.Controls.Add(this.OthersTabPage);
            resources.ApplyResources(this.SettingsTabControl, "SettingsTabControl");
            this.SettingsTabControl.Name = "SettingsTabControl";
            this.SettingsTabControl.SelectedIndex = 0;
            this.SettingsTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            //
            // GeneralTabPage
            //
            resources.ApplyResources(this.GeneralTabPage, "GeneralTabPage");
            this.GeneralTabPage.Controls.Add(this.GeneralPanel);
            this.GeneralTabPage.Name = "GeneralTabPage";
            this.GeneralTabPage.UseVisualStyleBackColor = true;
            //
            // GeneralPanel
            //
            resources.ApplyResources(this.GeneralPanel, "GeneralPanel");
            this.GeneralPanel.Controls.Add(this.ResolutionControl, 1, 1);
            this.GeneralPanel.Controls.Add(this.PostProcessComboBox, 1, 4);
            this.GeneralPanel.Controls.Add(this.UserProgramPanel, 1, 5);
            this.GeneralPanel.Controls.Add(this.OrientationPanel, 1, 2);
            this.GeneralPanel.Controls.Add(this.OrientationLabel, 0, 2);
            this.GeneralPanel.Controls.Add(this.SourcePanel, 1, 6);
            this.GeneralPanel.Controls.Add(this.SourceLabel, 0, 6);
            this.GeneralPanel.Controls.Add(this.PostProcessLabel, 0, 4);
            this.GeneralPanel.Controls.Add(this.DestinationLabel, 0, 3);
            this.GeneralPanel.Controls.Add(this.ResolutionLabel, 0, 1);
            this.GeneralPanel.Controls.Add(this.FormatLabel, 0, 0);
            this.GeneralPanel.Controls.Add(this.DestinationPanel, 1, 3);
            this.GeneralPanel.Controls.Add(this.FormatPanel, 1, 0);
            this.GeneralPanel.Name = "GeneralPanel";
            //
            // ResolutionControl
            //
            this.ResolutionControl.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.SettingsBindingSource, "Resolution", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.ResolutionControl, "ResolutionControl");
            this.ResolutionControl.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ResolutionControl.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.ResolutionControl.Minimum = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.ResolutionControl.Name = "ResolutionControl";
            this.ResolutionControl.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            //
            // SettingsBindingSource
            //
            this.SettingsBindingSource.DataSource = typeof(Cube.Pdf.App.Converter.SettingsViewModel);
            //
            // PostProcessComboBox
            //
            this.PostProcessComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.SettingsBindingSource, "PostProcess", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.PostProcessComboBox, "PostProcessComboBox");
            this.PostProcessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PostProcessComboBox.FormattingEnabled = true;
            this.PostProcessComboBox.Name = "PostProcessComboBox";
            //
            // UserProgramPanel
            //
            resources.ApplyResources(this.UserProgramPanel, "UserProgramPanel");
            this.UserProgramPanel.Controls.Add(this.UserProgramButton, 0, 0);
            this.UserProgramPanel.Controls.Add(this.UserProgramTextBox, 0, 0);
            this.UserProgramPanel.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.SettingsBindingSource, "EnableUserProgram", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.UserProgramPanel.Name = "UserProgramPanel";
            //
            // UserProgramButton
            //
            resources.ApplyResources(this.UserProgramButton, "UserProgramButton");
            this.UserProgramButton.Name = "UserProgramButton";
            this.UserProgramButton.UseVisualStyleBackColor = true;
            //
            // UserProgramTextBox
            //
            this.UserProgramTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.SettingsBindingSource, "UserProgram", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.UserProgramTextBox, "UserProgramTextBox");
            this.UserProgramTextBox.Name = "UserProgramTextBox";
            //
            // OrientationPanel
            //
            this.OrientationPanel.Controls.Add(this.PortraitRadioButton);
            this.OrientationPanel.Controls.Add(this.LandscapeRadioButton);
            this.OrientationPanel.Controls.Add(this.AutoRadioButton);
            resources.ApplyResources(this.OrientationPanel, "OrientationPanel");
            this.OrientationPanel.Name = "OrientationPanel";
            //
            // PortraitRadioButton
            //
            resources.ApplyResources(this.PortraitRadioButton, "PortraitRadioButton");
            this.PortraitRadioButton.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsBindingSource, "IsPortrait", true));
            this.PortraitRadioButton.Name = "PortraitRadioButton";
            this.PortraitRadioButton.TabStop = true;
            this.PortraitRadioButton.UseVisualStyleBackColor = true;
            //
            // LandscapeRadioButton
            //
            resources.ApplyResources(this.LandscapeRadioButton, "LandscapeRadioButton");
            this.LandscapeRadioButton.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsBindingSource, "IsLandscape", true));
            this.LandscapeRadioButton.Name = "LandscapeRadioButton";
            this.LandscapeRadioButton.TabStop = true;
            this.LandscapeRadioButton.UseVisualStyleBackColor = true;
            //
            // AutoRadioButton
            //
            resources.ApplyResources(this.AutoRadioButton, "AutoRadioButton");
            this.AutoRadioButton.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsBindingSource, "IsAutoOrientation", true));
            this.AutoRadioButton.Name = "AutoRadioButton";
            this.AutoRadioButton.TabStop = true;
            this.AutoRadioButton.UseVisualStyleBackColor = true;
            //
            // OrientationLabel
            //
            resources.ApplyResources(this.OrientationLabel, "OrientationLabel");
            this.OrientationLabel.Name = "OrientationLabel";
            //
            // SourcePanel
            //
            resources.ApplyResources(this.SourcePanel, "SourcePanel");
            this.SourcePanel.Controls.Add(this.SourceButton, 0, 0);
            this.SourcePanel.Controls.Add(this.SourceTextBox, 0, 0);
            this.SourcePanel.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.SettingsBindingSource, "SourceEditable", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.SourcePanel.Name = "SourcePanel";
            //
            // SourceButton
            //
            resources.ApplyResources(this.SourceButton, "SourceButton");
            this.SourceButton.Name = "SourceButton";
            this.SourceButton.UseVisualStyleBackColor = true;
            //
            // SourceTextBox
            //
            this.SourceTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.SettingsBindingSource, "Source", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.SourceTextBox, "SourceTextBox");
            this.SourceTextBox.Name = "SourceTextBox";
            //
            // SourceLabel
            //
            resources.ApplyResources(this.SourceLabel, "SourceLabel");
            this.SourceLabel.Name = "SourceLabel";
            //
            // PostProcessLabel
            //
            resources.ApplyResources(this.PostProcessLabel, "PostProcessLabel");
            this.PostProcessLabel.Name = "PostProcessLabel";
            //
            // DestinationLabel
            //
            resources.ApplyResources(this.DestinationLabel, "DestinationLabel");
            this.DestinationLabel.Name = "DestinationLabel";
            //
            // ResolutionLabel
            //
            resources.ApplyResources(this.ResolutionLabel, "ResolutionLabel");
            this.ResolutionLabel.Name = "ResolutionLabel";
            //
            // FormatLabel
            //
            resources.ApplyResources(this.FormatLabel, "FormatLabel");
            this.FormatLabel.Name = "FormatLabel";
            //
            // DestinationPanel
            //
            resources.ApplyResources(this.DestinationPanel, "DestinationPanel");
            this.DestinationPanel.Controls.Add(this.SaveOptionComboBox, 2, 0);
            this.DestinationPanel.Controls.Add(this.DestinationButton, 1, 0);
            this.DestinationPanel.Controls.Add(this.DestinationTextBox, 0, 0);
            this.DestinationPanel.Name = "DestinationPanel";
            //
            // SaveOptionComboBox
            //
            this.SaveOptionComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.SettingsBindingSource, "SaveOption", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.SaveOptionComboBox, "SaveOptionComboBox");
            this.SaveOptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SaveOptionComboBox.FormattingEnabled = true;
            this.SaveOptionComboBox.Name = "SaveOptionComboBox";
            //
            // DestinationButton
            //
            resources.ApplyResources(this.DestinationButton, "DestinationButton");
            this.DestinationButton.Name = "DestinationButton";
            this.DestinationButton.UseVisualStyleBackColor = true;
            //
            // DestinationTextBox
            //
            this.DestinationTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.SettingsBindingSource, "Destination", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.DestinationTextBox, "DestinationTextBox");
            this.DestinationTextBox.Name = "DestinationTextBox";
            //
            // FormatPanel
            //
            resources.ApplyResources(this.FormatPanel, "FormatPanel");
            this.FormatPanel.Controls.Add(this.FormatOptionComboBox, 0, 0);
            this.FormatPanel.Controls.Add(this.FormatComboBox, 0, 0);
            this.FormatPanel.Name = "FormatPanel";
            //
            // FormatOptionComboBox
            //
            this.FormatOptionComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.SettingsBindingSource, "FormatOption", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FormatOptionComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.SettingsBindingSource, "EnableFormatOption", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            resources.ApplyResources(this.FormatOptionComboBox, "FormatOptionComboBox");
            this.FormatOptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatOptionComboBox.FormattingEnabled = true;
            this.FormatOptionComboBox.Name = "FormatOptionComboBox";
            //
            // FormatComboBox
            //
            this.FormatComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.SettingsBindingSource, "Format", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.FormatComboBox, "FormatComboBox");
            this.FormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatComboBox.FormattingEnabled = true;
            this.FormatComboBox.Name = "FormatComboBox";
            //
            // DocumentPage
            //
            resources.ApplyResources(this.DocumentPage, "DocumentPage");
            this.DocumentPage.Controls.Add(this.DocumentPanel);
            this.DocumentPage.Name = "DocumentPage";
            this.DocumentPage.UseVisualStyleBackColor = true;
            //
            // DocumentPanel
            //
            resources.ApplyResources(this.DocumentPanel, "DocumentPanel");
            this.DocumentPanel.Controls.Add(this.ViewerPreferencesComboBox, 1, 5);
            this.DocumentPanel.Controls.Add(this.ViewerPreferencesLabel, 0, 5);
            this.DocumentPanel.Controls.Add(this.CreatorTextBox, 1, 4);
            this.DocumentPanel.Controls.Add(this.CreatorLabel, 0, 4);
            this.DocumentPanel.Controls.Add(this.KeywordsTextBox, 1, 3);
            this.DocumentPanel.Controls.Add(this.KeywordsLabel, 0, 3);
            this.DocumentPanel.Controls.Add(this.SubjectTextBox, 1, 2);
            this.DocumentPanel.Controls.Add(this.SubjectLabel, 0, 2);
            this.DocumentPanel.Controls.Add(this.AuthorTextBox, 1, 1);
            this.DocumentPanel.Controls.Add(this.AuthorLabel, 0, 1);
            this.DocumentPanel.Controls.Add(this.TitleTextBox, 1, 0);
            this.DocumentPanel.Controls.Add(this.TitleLabel, 0, 0);
            this.DocumentPanel.Name = "DocumentPanel";
            //
            // ViewerPreferencesComboBox
            //
            this.ViewerPreferencesComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.MetadataBindingSource, "Options", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.ViewerPreferencesComboBox, "ViewerPreferencesComboBox");
            this.ViewerPreferencesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ViewerPreferencesComboBox.FormattingEnabled = true;
            this.ViewerPreferencesComboBox.Name = "ViewerPreferencesComboBox";
            //
            // MetadataBindingSource
            //
            this.MetadataBindingSource.DataSource = typeof(Cube.Pdf.App.Converter.MetadataViewModel);
            //
            // ViewerPreferencesLabel
            //
            resources.ApplyResources(this.ViewerPreferencesLabel, "ViewerPreferencesLabel");
            this.ViewerPreferencesLabel.Name = "ViewerPreferencesLabel";
            //
            // CreatorTextBox
            //
            this.CreatorTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.MetadataBindingSource, "Creator", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.CreatorTextBox, "CreatorTextBox");
            this.CreatorTextBox.Name = "CreatorTextBox";
            //
            // CreatorLabel
            //
            resources.ApplyResources(this.CreatorLabel, "CreatorLabel");
            this.CreatorLabel.Name = "CreatorLabel";
            //
            // KeywordsTextBox
            //
            this.KeywordsTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.MetadataBindingSource, "Keywords", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.KeywordsTextBox, "KeywordsTextBox");
            this.KeywordsTextBox.Name = "KeywordsTextBox";
            //
            // KeywordsLabel
            //
            resources.ApplyResources(this.KeywordsLabel, "KeywordsLabel");
            this.KeywordsLabel.Name = "KeywordsLabel";
            //
            // SubjectTextBox
            //
            this.SubjectTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.MetadataBindingSource, "Subject", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.SubjectTextBox, "SubjectTextBox");
            this.SubjectTextBox.Name = "SubjectTextBox";
            //
            // SubjectLabel
            //
            resources.ApplyResources(this.SubjectLabel, "SubjectLabel");
            this.SubjectLabel.Name = "SubjectLabel";
            //
            // AuthorTextBox
            //
            this.AuthorTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.MetadataBindingSource, "Author", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.AuthorTextBox, "AuthorTextBox");
            this.AuthorTextBox.Name = "AuthorTextBox";
            //
            // AuthorLabel
            //
            resources.ApplyResources(this.AuthorLabel, "AuthorLabel");
            this.AuthorLabel.Name = "AuthorLabel";
            //
            // TitleTextBox
            //
            this.TitleTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.MetadataBindingSource, "Title", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TitleTextBox, "TitleTextBox");
            this.TitleTextBox.Name = "TitleTextBox";
            //
            // TitleLabel
            //
            resources.ApplyResources(this.TitleLabel, "TitleLabel");
            this.TitleLabel.Name = "TitleLabel";
            //
            // EncryptionTabPage
            //
            resources.ApplyResources(this.EncryptionTabPage, "EncryptionTabPage");
            this.EncryptionTabPage.Controls.Add(this.EncryptionOuterPanel);
            this.EncryptionTabPage.Name = "EncryptionTabPage";
            this.EncryptionTabPage.UseVisualStyleBackColor = true;
            //
            // EncryptionOuterPanel
            //
            resources.ApplyResources(this.EncryptionOuterPanel, "EncryptionOuterPanel");
            this.EncryptionOuterPanel.Controls.Add(this.EnableEncryptionCheckBox, 0, 0);
            this.EncryptionOuterPanel.Controls.Add(this.EncryptionPanel, 0, 1);
            this.EncryptionOuterPanel.Name = "EncryptionOuterPanel";
            //
            // EnableEncryptionCheckBox
            //
            resources.ApplyResources(this.EnableEncryptionCheckBox, "EnableEncryptionCheckBox");
            this.EnableEncryptionCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.EncryptionBindingSource, "Enabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.EnableEncryptionCheckBox.Name = "EnableEncryptionCheckBox";
            this.EnableEncryptionCheckBox.UseVisualStyleBackColor = true;
            //
            // EncryptionBindingSource
            //
            this.EncryptionBindingSource.DataSource = typeof(Cube.Pdf.App.Converter.EncryptionViewModel);
            //
            // EncryptionPanel
            //
            resources.ApplyResources(this.EncryptionPanel, "EncryptionPanel");
            this.EncryptionPanel.Controls.Add(this.UserPasswordCheckBox, 1, 2);
            this.EncryptionPanel.Controls.Add(this.OperationLabel, 0, 2);
            this.EncryptionPanel.Controls.Add(this.OwnerConfirmTextBox, 1, 1);
            this.EncryptionPanel.Controls.Add(this.OwnerConfirmLabel, 0, 1);
            this.EncryptionPanel.Controls.Add(this.OwnerPasswordTextBox, 1, 0);
            this.EncryptionPanel.Controls.Add(this.OwnerPasswordLabel, 0, 0);
            this.EncryptionPanel.Controls.Add(this.OperationPanel, 1, 3);
            this.EncryptionPanel.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.EncryptionBindingSource, "Enabled", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.EncryptionPanel.Name = "EncryptionPanel";
            //
            // UserPasswordCheckBox
            //
            resources.ApplyResources(this.UserPasswordCheckBox, "UserPasswordCheckBox");
            this.UserPasswordCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.EncryptionBindingSource, "OpenWithPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.UserPasswordCheckBox.Name = "UserPasswordCheckBox";
            this.UserPasswordCheckBox.UseVisualStyleBackColor = true;
            //
            // OperationLabel
            //
            resources.ApplyResources(this.OperationLabel, "OperationLabel");
            this.OperationLabel.Name = "OperationLabel";
            this.EncryptionPanel.SetRowSpan(this.OperationLabel, 2);
            //
            // OwnerConfirmTextBox
            //
            this.OwnerConfirmTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.EncryptionBindingSource, "OwnerConfirm", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.OwnerConfirmTextBox, "OwnerConfirmTextBox");
            this.OwnerConfirmTextBox.Name = "OwnerConfirmTextBox";
            this.OwnerConfirmTextBox.UseSystemPasswordChar = true;
            //
            // OwnerConfirmLabel
            //
            resources.ApplyResources(this.OwnerConfirmLabel, "OwnerConfirmLabel");
            this.OwnerConfirmLabel.Name = "OwnerConfirmLabel";
            //
            // OwnerPasswordTextBox
            //
            this.OwnerPasswordTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.EncryptionBindingSource, "OwnerPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.OwnerPasswordTextBox, "OwnerPasswordTextBox");
            this.OwnerPasswordTextBox.Name = "OwnerPasswordTextBox";
            this.OwnerPasswordTextBox.UseSystemPasswordChar = true;
            //
            // OwnerPasswordLabel
            //
            resources.ApplyResources(this.OwnerPasswordLabel, "OwnerPasswordLabel");
            this.OwnerPasswordLabel.Name = "OwnerPasswordLabel";
            //
            // OperationPanel
            //
            resources.ApplyResources(this.OperationPanel, "OperationPanel");
            this.OperationPanel.Controls.Add(this.SharePasswordCheckBox, 0, 1);
            this.OperationPanel.Controls.Add(this.UserPasswordPanel, 0, 0);
            this.OperationPanel.Controls.Add(this.PermissionPanel, 0, 2);
            this.OperationPanel.Name = "OperationPanel";
            //
            // SharePasswordCheckBox
            //
            resources.ApplyResources(this.SharePasswordCheckBox, "SharePasswordCheckBox");
            this.SharePasswordCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.EncryptionBindingSource, "UseOwnerPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SharePasswordCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.EncryptionBindingSource, "OpenWithPassword", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.SharePasswordCheckBox.Name = "SharePasswordCheckBox";
            this.SharePasswordCheckBox.UseVisualStyleBackColor = true;
            //
            // UserPasswordPanel
            //
            resources.ApplyResources(this.UserPasswordPanel, "UserPasswordPanel");
            this.UserPasswordPanel.Controls.Add(this.UserConfirmTextBox, 1, 1);
            this.UserPasswordPanel.Controls.Add(this.UserPasswordTextBox, 1, 0);
            this.UserPasswordPanel.Controls.Add(this.UserConfirmLabel, 0, 1);
            this.UserPasswordPanel.Controls.Add(this.UserPasswordLabel, 0, 0);
            this.UserPasswordPanel.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.EncryptionBindingSource, "EnableUserPassword", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.UserPasswordPanel.Name = "UserPasswordPanel";
            //
            // UserConfirmTextBox
            //
            this.UserConfirmTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.EncryptionBindingSource, "UserConfirm", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.UserConfirmTextBox, "UserConfirmTextBox");
            this.UserConfirmTextBox.Name = "UserConfirmTextBox";
            this.UserConfirmTextBox.UseSystemPasswordChar = true;
            //
            // UserPasswordTextBox
            //
            this.UserPasswordTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.EncryptionBindingSource, "UserPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.UserPasswordTextBox, "UserPasswordTextBox");
            this.UserPasswordTextBox.Name = "UserPasswordTextBox";
            this.UserPasswordTextBox.UseSystemPasswordChar = true;
            //
            // UserConfirmLabel
            //
            resources.ApplyResources(this.UserConfirmLabel, "UserConfirmLabel");
            this.UserConfirmLabel.Name = "UserConfirmLabel";
            //
            // UserPasswordLabel
            //
            resources.ApplyResources(this.UserPasswordLabel, "UserPasswordLabel");
            this.UserPasswordLabel.Name = "UserPasswordLabel";
            //
            // PermissionPanel
            //
            resources.ApplyResources(this.PermissionPanel, "PermissionPanel");
            this.PermissionPanel.Controls.Add(this.AllowModifyCheckBox, 0, 3);
            this.PermissionPanel.Controls.Add(this.AllowFormCheckBox, 0, 2);
            this.PermissionPanel.Controls.Add(this.AllowCopyCheckBox, 0, 1);
            this.PermissionPanel.Controls.Add(this.AllowPrintCheckBox, 0, 0);
            this.PermissionPanel.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.EncryptionBindingSource, "EnablePermission", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.PermissionPanel.Name = "PermissionPanel";
            //
            // AllowModifyCheckBox
            //
            resources.ApplyResources(this.AllowModifyCheckBox, "AllowModifyCheckBox");
            this.AllowModifyCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.EncryptionBindingSource, "AllowModify", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AllowModifyCheckBox.Name = "AllowModifyCheckBox";
            this.AllowModifyCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowFormCheckBox
            //
            resources.ApplyResources(this.AllowFormCheckBox, "AllowFormCheckBox");
            this.AllowFormCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.EncryptionBindingSource, "AllowInputForm", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AllowFormCheckBox.Name = "AllowFormCheckBox";
            this.AllowFormCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowCopyCheckBox
            //
            resources.ApplyResources(this.AllowCopyCheckBox, "AllowCopyCheckBox");
            this.AllowCopyCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.EncryptionBindingSource, "AllowCopy", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AllowCopyCheckBox.Name = "AllowCopyCheckBox";
            this.AllowCopyCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowPrintCheckBox
            //
            resources.ApplyResources(this.AllowPrintCheckBox, "AllowPrintCheckBox");
            this.AllowPrintCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.EncryptionBindingSource, "AllowPrint", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AllowPrintCheckBox.Name = "AllowPrintCheckBox";
            this.AllowPrintCheckBox.UseVisualStyleBackColor = true;
            //
            // OthersTabPage
            //
            resources.ApplyResources(this.OthersTabPage, "OthersTabPage");
            this.OthersTabPage.Controls.Add(this.OthersPanel);
            this.OthersTabPage.Name = "OthersTabPage";
            this.OthersTabPage.UseVisualStyleBackColor = true;
            //
            // OthersPanel
            //
            resources.ApplyResources(this.OthersPanel, "OthersPanel");
            this.OthersPanel.Controls.Add(this.LanguageLabel, 0, 6);
            this.OthersPanel.Controls.Add(this.GrayscaleCheckBox, 1, 0);
            this.OthersPanel.Controls.Add(this.OptionsLabel, 0, 0);
            this.OthersPanel.Controls.Add(this.ImageCompressionCheckBox, 1, 1);
            this.OthersPanel.Controls.Add(this.LinearizationCheckBox, 1, 2);
            this.OthersPanel.Controls.Add(this.UpdateCheckBox, 1, 5);
            this.OthersPanel.Controls.Add(this.LanguageComboBox, 1, 6);
            this.OthersPanel.Controls.Add(this.AboutLabel, 0, 4);
            this.OthersPanel.Controls.Add(this.VersionPanel, 1, 4);
            this.OthersPanel.Name = "OthersPanel";
            //
            // LanguageLabel
            //
            resources.ApplyResources(this.LanguageLabel, "LanguageLabel");
            this.LanguageLabel.Name = "LanguageLabel";
            //
            // GrayscaleCheckBox
            //
            resources.ApplyResources(this.GrayscaleCheckBox, "GrayscaleCheckBox");
            this.GrayscaleCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsBindingSource, "Grayscale", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.GrayscaleCheckBox.Name = "GrayscaleCheckBox";
            this.GrayscaleCheckBox.UseVisualStyleBackColor = true;
            //
            // OptionsLabel
            //
            resources.ApplyResources(this.OptionsLabel, "OptionsLabel");
            this.OptionsLabel.Name = "OptionsLabel";
            //
            // ImageCompressionCheckBox
            //
            resources.ApplyResources(this.ImageCompressionCheckBox, "ImageCompressionCheckBox");
            this.ImageCompressionCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsBindingSource, "ImageCompression", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ImageCompressionCheckBox.Name = "ImageCompressionCheckBox";
            this.ImageCompressionCheckBox.UseVisualStyleBackColor = true;
            //
            // LinearizationCheckBox
            //
            resources.ApplyResources(this.LinearizationCheckBox, "LinearizationCheckBox");
            this.LinearizationCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsBindingSource, "Linearization", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LinearizationCheckBox.Name = "LinearizationCheckBox";
            this.LinearizationCheckBox.UseVisualStyleBackColor = true;
            //
            // UpdateCheckBox
            //
            resources.ApplyResources(this.UpdateCheckBox, "UpdateCheckBox");
            this.UpdateCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsBindingSource, "CheckUpdate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.UpdateCheckBox.Name = "UpdateCheckBox";
            this.UpdateCheckBox.UseVisualStyleBackColor = true;
            //
            // LanguageComboBox
            //
            this.LanguageComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.SettingsBindingSource, "Language", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.LanguageComboBox, "LanguageComboBox");
            this.LanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguageComboBox.FormattingEnabled = true;
            this.LanguageComboBox.Name = "LanguageComboBox";
            //
            // AboutLabel
            //
            resources.ApplyResources(this.AboutLabel, "AboutLabel");
            this.AboutLabel.Name = "AboutLabel";
            this.OthersPanel.SetRowSpan(this.AboutLabel, 2);
            //
            // VersionPanel
            //
            this.VersionPanel.Copyright = "Copyright © 2010 CubeSoft, Inc.";
            this.VersionPanel.DataBindings.Add(new System.Windows.Forms.Binding("Product", this.MainBindingSource, "Product", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.VersionPanel.DataBindings.Add(new System.Windows.Forms.Binding("Uri", this.MainBindingSource, "Uri", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.VersionPanel.DataBindings.Add(new System.Windows.Forms.Binding("Version", this.MainBindingSource, "Version", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.VersionPanel.Description = "";
            resources.ApplyResources(this.VersionPanel, "VersionPanel");
            this.VersionPanel.Image = null;
            this.VersionPanel.Name = "VersionPanel";
            this.VersionPanel.OneLine = true;
            this.VersionPanel.Product = "CubePDF";
            this.VersionPanel.TabStop = false;
            this.VersionPanel.Uri = null;
            this.VersionPanel.Version = "1.0.0 (x64)";
            //
            // MainBindingSource
            //
            this.MainBindingSource.DataSource = typeof(Cube.Pdf.App.Converter.MainViewModel);
            //
            // FooterPanel
            //
            resources.ApplyResources(this.FooterPanel, "FooterPanel");
            this.FooterPanel.Controls.Add(this.ToolsPanel, 0, 0);
            this.FooterPanel.Controls.Add(this.ConvertButton, 1, 0);
            this.FooterPanel.Controls.Add(this.ExitButton, 2, 0);
            this.FooterPanel.Name = "FooterPanel";
            //
            // ToolsPanel
            //
            this.ToolsPanel.Controls.Add(this.ConvertProgressBar);
            this.ToolsPanel.Controls.Add(this.ApplyButton);
            resources.ApplyResources(this.ToolsPanel, "ToolsPanel");
            this.ToolsPanel.Name = "ToolsPanel";
            //
            // ConvertProgressBar
            //
            resources.ApplyResources(this.ConvertProgressBar, "ConvertProgressBar");
            this.ConvertProgressBar.Name = "ConvertProgressBar";
            this.ConvertProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            //
            // ApplyButton
            //
            this.ApplyButton.BackColor = System.Drawing.Color.White;
            this.ApplyButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            resources.ApplyResources(this.ApplyButton, "ApplyButton");
            this.ApplyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.UseVisualStyleBackColor = false;
            //
            // ConvertButton
            //
            this.ConvertButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.ConvertButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConvertButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.ConvertButton, "ConvertButton");
            this.ConvertButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ConvertButton.ForeColor = System.Drawing.Color.White;
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.UseVisualStyleBackColor = false;
            //
            // ExitButton
            //
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.ExitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.ExitButton, "ExitButton");
            this.ExitButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ExitButton.ForeColor = System.Drawing.Color.White;
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.UseVisualStyleBackColor = false;
            //
            // HeaderPictureBox
            //
            resources.ApplyResources(this.HeaderPictureBox, "HeaderPictureBox");
            this.HeaderPictureBox.Image = global::Cube.Pdf.App.Converter.Properties.Resources.Header;
            this.HeaderPictureBox.Name = "HeaderPictureBox";
            this.HeaderPictureBox.TabStop = false;
            //
            // MainToolTip
            //
            this.MainToolTip.AutoPopDelay = 30000;
            this.MainToolTip.InitialDelay = 100;
            this.MainToolTip.ReshowDelay = 100;
            //
            // MainForm
            //
            this.AcceptButton = this.ConvertButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.ExitButton;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.RootPanel);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.RootPanel.ResumeLayout(false);
            this.SettingsPanel.ResumeLayout(false);
            this.SettingsTabControl.ResumeLayout(false);
            this.GeneralTabPage.ResumeLayout(false);
            this.GeneralPanel.ResumeLayout(false);
            this.GeneralPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResolutionControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsBindingSource)).EndInit();
            this.UserProgramPanel.ResumeLayout(false);
            this.UserProgramPanel.PerformLayout();
            this.OrientationPanel.ResumeLayout(false);
            this.OrientationPanel.PerformLayout();
            this.SourcePanel.ResumeLayout(false);
            this.SourcePanel.PerformLayout();
            this.DestinationPanel.ResumeLayout(false);
            this.DestinationPanel.PerformLayout();
            this.FormatPanel.ResumeLayout(false);
            this.DocumentPage.ResumeLayout(false);
            this.DocumentPanel.ResumeLayout(false);
            this.DocumentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetadataBindingSource)).EndInit();
            this.EncryptionTabPage.ResumeLayout(false);
            this.EncryptionOuterPanel.ResumeLayout(false);
            this.EncryptionOuterPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EncryptionBindingSource)).EndInit();
            this.EncryptionPanel.ResumeLayout(false);
            this.EncryptionPanel.PerformLayout();
            this.OperationPanel.ResumeLayout(false);
            this.OperationPanel.PerformLayout();
            this.UserPasswordPanel.ResumeLayout(false);
            this.UserPasswordPanel.PerformLayout();
            this.PermissionPanel.ResumeLayout(false);
            this.PermissionPanel.PerformLayout();
            this.OthersTabPage.ResumeLayout(false);
            this.OthersPanel.ResumeLayout(false);
            this.OthersPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainBindingSource)).EndInit();
            this.FooterPanel.ResumeLayout(false);
            this.ToolsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.PictureBox HeaderPictureBox;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private System.Windows.Forms.TabControl SettingsTabControl;
        private System.Windows.Forms.TabPage GeneralTabPage;
        private System.Windows.Forms.TabPage DocumentPage;
        private System.Windows.Forms.TabPage EncryptionTabPage;
        private System.Windows.Forms.TabPage OthersTabPage;
        private System.Windows.Forms.TableLayoutPanel GeneralPanel;
        private System.Windows.Forms.Label FormatLabel;
        private System.Windows.Forms.Label ResolutionLabel;
        private System.Windows.Forms.Label DestinationLabel;
        private System.Windows.Forms.TableLayoutPanel DestinationPanel;
        private System.Windows.Forms.Label PostProcessLabel;
        private System.Windows.Forms.TableLayoutPanel SourcePanel;
        private System.Windows.Forms.Label SourceLabel;
        private System.Windows.Forms.Button SourceButton;
        private System.Windows.Forms.TextBox SourceTextBox;
        private System.Windows.Forms.Button DestinationButton;
        private System.Windows.Forms.TextBox DestinationTextBox;
        private System.Windows.Forms.TableLayoutPanel DocumentPanel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.Label AuthorLabel;
        private System.Windows.Forms.TextBox AuthorTextBox;
        private System.Windows.Forms.Label SubjectLabel;
        private System.Windows.Forms.Label KeywordsLabel;
        private System.Windows.Forms.TextBox SubjectTextBox;
        private System.Windows.Forms.Label CreatorLabel;
        private System.Windows.Forms.TextBox KeywordsTextBox;
        private System.Windows.Forms.TextBox CreatorTextBox;
        private System.Windows.Forms.Label ViewerPreferencesLabel;
        private System.Windows.Forms.ComboBox ViewerPreferencesComboBox;
        private System.Windows.Forms.TableLayoutPanel EncryptionOuterPanel;
        private System.Windows.Forms.CheckBox EnableEncryptionCheckBox;
        private System.Windows.Forms.TableLayoutPanel EncryptionPanel;
        private System.Windows.Forms.Label OwnerPasswordLabel;
        private System.Windows.Forms.TextBox OwnerPasswordTextBox;
        private System.Windows.Forms.TextBox OwnerConfirmTextBox;
        private System.Windows.Forms.Label OwnerConfirmLabel;
        private System.Windows.Forms.Label OperationLabel;
        private System.Windows.Forms.CheckBox UserPasswordCheckBox;
        private System.Windows.Forms.TableLayoutPanel OperationPanel;
        private System.Windows.Forms.TableLayoutPanel UserPasswordPanel;
        private System.Windows.Forms.CheckBox SharePasswordCheckBox;
        private System.Windows.Forms.TextBox UserConfirmTextBox;
        private System.Windows.Forms.TextBox UserPasswordTextBox;
        private System.Windows.Forms.Label UserConfirmLabel;
        private System.Windows.Forms.Label UserPasswordLabel;
        private System.Windows.Forms.TableLayoutPanel PermissionPanel;
        private System.Windows.Forms.CheckBox AllowModifyCheckBox;
        private System.Windows.Forms.CheckBox AllowFormCheckBox;
        private System.Windows.Forms.CheckBox AllowCopyCheckBox;
        private System.Windows.Forms.CheckBox AllowPrintCheckBox;
        private System.Windows.Forms.TableLayoutPanel OthersPanel;
        private System.Windows.Forms.ComboBox LanguageComboBox;
        private System.Windows.Forms.Label AboutLabel;
        private System.Windows.Forms.Label OrientationLabel;
        private System.Windows.Forms.FlowLayoutPanel OrientationPanel;
        private System.Windows.Forms.RadioButton PortraitRadioButton;
        private System.Windows.Forms.RadioButton LandscapeRadioButton;
        private System.Windows.Forms.RadioButton AutoRadioButton;
        private System.Windows.Forms.CheckBox UpdateCheckBox;
        private System.Windows.Forms.CheckBox ImageCompressionCheckBox;
        private System.Windows.Forms.CheckBox LinearizationCheckBox;
        private System.Windows.Forms.TableLayoutPanel FormatPanel;
        private System.Windows.Forms.ComboBox FormatOptionComboBox;
        private System.Windows.Forms.ComboBox FormatComboBox;
        private System.Windows.Forms.Label OptionsLabel;
        private System.Windows.Forms.ComboBox SaveOptionComboBox;
        private System.Windows.Forms.ComboBox PostProcessComboBox;
        private System.Windows.Forms.TableLayoutPanel UserProgramPanel;
        private System.Windows.Forms.Button UserProgramButton;
        private System.Windows.Forms.TextBox UserProgramTextBox;
        private System.Windows.Forms.CheckBox GrayscaleCheckBox;
        private System.Windows.Forms.NumericUpDown ResolutionControl;
        private System.Windows.Forms.Button ConvertButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.FlowLayoutPanel ToolsPanel;
        private System.Windows.Forms.ProgressBar ConvertProgressBar;
        private System.Windows.Forms.Button ApplyButton;
        private Cube.Forms.VersionControl VersionPanel;
        private Cube.Forms.SettingsControl SettingsPanel;
        private System.Windows.Forms.BindingSource MainBindingSource;
        private System.Windows.Forms.BindingSource SettingsBindingSource;
        private System.Windows.Forms.BindingSource MetadataBindingSource;
        private System.Windows.Forms.BindingSource EncryptionBindingSource;
        private System.Windows.Forms.Label LanguageLabel;
        private System.Windows.Forms.ToolTip PathToolTip;
        private System.Windows.Forms.ToolTip MainToolTip;
    }
}

