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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderButton = new Cube.Forms.FlatButton();
            this.SettingsPanel = new Cube.Forms.SettingsControl();
            this.SettingsTabControl = new System.Windows.Forms.TabControl();
            this.GeneralTabPage = new System.Windows.Forms.TabPage();
            this.GeneralPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SourcePanel = new System.Windows.Forms.TableLayoutPanel();
            this.SourceButton = new System.Windows.Forms.Button();
            this.SourceTextBox = new System.Windows.Forms.TextBox();
            this.SourceLabel = new System.Windows.Forms.Label();
            this.PostProcessPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PostProcessButton = new System.Windows.Forms.Button();
            this.PostProcessTextBox = new System.Windows.Forms.TextBox();
            this.PostProcessComboBox = new System.Windows.Forms.ComboBox();
            this.PostProcessLabel = new System.Windows.Forms.Label();
            this.DestinationLabel = new System.Windows.Forms.Label();
            this.ResolutionLabel = new System.Windows.Forms.Label();
            this.VersionComboBox = new System.Windows.Forms.ComboBox();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.FormatLabel = new System.Windows.Forms.Label();
            this.FormatComboBox = new System.Windows.Forms.ComboBox();
            this.ResolutionControl = new System.Windows.Forms.NumericUpDown();
            this.DestinationPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DestinationButton = new System.Windows.Forms.Button();
            this.DestinationTextBox = new System.Windows.Forms.TextBox();
            this.DestinationComboBox = new System.Windows.Forms.ComboBox();
            this.DocumentPage = new System.Windows.Forms.TabPage();
            this.DocumentPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ViewOptionComboBox = new System.Windows.Forms.ComboBox();
            this.ViewOptionLabel = new System.Windows.Forms.Label();
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
            this.OthersTabPage = new System.Windows.Forms.TabPage();
            this.FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ToolsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ConvertProgressBar = new System.Windows.Forms.ProgressBar();
            this.ApplyButton = new Cube.Forms.FlatButton();
            this.ConvertButton = new Cube.Forms.FlatButton();
            this.ExitButton = new Cube.Forms.FlatButton();
            this.AllowPrintCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowCopyCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowFormCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowModifyCheckBox = new System.Windows.Forms.CheckBox();
            this.PermissionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RootPanel.SuspendLayout();
            this.SettingsPanel.SuspendLayout();
            this.SettingsTabControl.SuspendLayout();
            this.GeneralTabPage.SuspendLayout();
            this.GeneralPanel.SuspendLayout();
            this.SourcePanel.SuspendLayout();
            this.PostProcessPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResolutionControl)).BeginInit();
            this.DestinationPanel.SuspendLayout();
            this.DocumentPage.SuspendLayout();
            this.DocumentPanel.SuspendLayout();
            this.EncryptionTabPage.SuspendLayout();
            this.EncryptionOuterPanel.SuspendLayout();
            this.EncryptionPanel.SuspendLayout();
            this.OperationPanel.SuspendLayout();
            this.UserPasswordPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.ToolsPanel.SuspendLayout();
            this.PermissionPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootPanel
            //
            this.RootPanel.ColumnCount = 1;
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.Controls.Add(this.HeaderButton, 0, 0);
            this.RootPanel.Controls.Add(this.SettingsPanel, 0, 1);
            this.RootPanel.Controls.Add(this.FooterPanel, 0, 2);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 3;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.RootPanel.Size = new System.Drawing.Size(484, 511);
            this.RootPanel.TabIndex = 0;
            //
            // HeaderButton
            //
            this.HeaderButton.Content = "";
            this.HeaderButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HeaderButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderButton.FlatAppearance.BorderSize = 0;
            this.HeaderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HeaderButton.Image = null;
            this.HeaderButton.Location = new System.Drawing.Point(0, 0);
            this.HeaderButton.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderButton.Name = "HeaderButton";
            this.HeaderButton.Size = new System.Drawing.Size(484, 50);
            this.HeaderButton.Styles.NormalStyle.BackgroundImage = global::Cube.Pdf.App.Converter.Properties.Resources.Header;
            this.HeaderButton.Styles.NormalStyle.BorderSize = 0;
            this.HeaderButton.TabIndex = 0;
            this.HeaderButton.TabStop = false;
            this.HeaderButton.Text = "button1";
            this.HeaderButton.UseVisualStyleBackColor = true;
            //
            // SettingsPanel
            //
            this.SettingsPanel.Controls.Add(this.SettingsTabControl);
            this.SettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsPanel.Location = new System.Drawing.Point(9, 59);
            this.SettingsPanel.Margin = new System.Windows.Forms.Padding(9, 9, 9, 3);
            this.SettingsPanel.Name = "SettingsPanel";
            this.SettingsPanel.Padding = new System.Windows.Forms.Padding(3);
            this.SettingsPanel.Size = new System.Drawing.Size(466, 396);
            this.SettingsPanel.TabIndex = 1;
            //
            // SettingsTabControl
            //
            this.SettingsTabControl.Controls.Add(this.GeneralTabPage);
            this.SettingsTabControl.Controls.Add(this.DocumentPage);
            this.SettingsTabControl.Controls.Add(this.EncryptionTabPage);
            this.SettingsTabControl.Controls.Add(this.OthersTabPage);
            this.SettingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsTabControl.ItemSize = new System.Drawing.Size(100, 20);
            this.SettingsTabControl.Location = new System.Drawing.Point(3, 3);
            this.SettingsTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsTabControl.Name = "SettingsTabControl";
            this.SettingsTabControl.SelectedIndex = 0;
            this.SettingsTabControl.Size = new System.Drawing.Size(460, 390);
            this.SettingsTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.SettingsTabControl.TabIndex = 0;
            //
            // GeneralTabPage
            //
            this.GeneralTabPage.AutoScroll = true;
            this.GeneralTabPage.Controls.Add(this.GeneralPanel);
            this.GeneralTabPage.Location = new System.Drawing.Point(4, 24);
            this.GeneralTabPage.Name = "GeneralTabPage";
            this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(9, 18, 9, 9);
            this.GeneralTabPage.Size = new System.Drawing.Size(452, 362);
            this.GeneralTabPage.TabIndex = 0;
            this.GeneralTabPage.Text = "General";
            this.GeneralTabPage.UseVisualStyleBackColor = true;
            //
            // GeneralPanel
            //
            this.GeneralPanel.ColumnCount = 2;
            this.GeneralPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.GeneralPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GeneralPanel.Controls.Add(this.SourcePanel, 1, 5);
            this.GeneralPanel.Controls.Add(this.SourceLabel, 0, 5);
            this.GeneralPanel.Controls.Add(this.PostProcessPanel, 1, 4);
            this.GeneralPanel.Controls.Add(this.PostProcessLabel, 0, 4);
            this.GeneralPanel.Controls.Add(this.DestinationLabel, 0, 3);
            this.GeneralPanel.Controls.Add(this.ResolutionLabel, 0, 2);
            this.GeneralPanel.Controls.Add(this.VersionComboBox, 1, 1);
            this.GeneralPanel.Controls.Add(this.VersionLabel, 0, 1);
            this.GeneralPanel.Controls.Add(this.FormatLabel, 0, 0);
            this.GeneralPanel.Controls.Add(this.FormatComboBox, 1, 0);
            this.GeneralPanel.Controls.Add(this.ResolutionControl, 1, 2);
            this.GeneralPanel.Controls.Add(this.DestinationPanel, 1, 3);
            this.GeneralPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.GeneralPanel.Location = new System.Drawing.Point(9, 18);
            this.GeneralPanel.Name = "GeneralPanel";
            this.GeneralPanel.RowCount = 6;
            this.GeneralPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.GeneralPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.GeneralPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.GeneralPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.GeneralPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.GeneralPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.GeneralPanel.Size = new System.Drawing.Size(434, 180);
            this.GeneralPanel.TabIndex = 0;
            //
            // SourcePanel
            //
            this.SourcePanel.ColumnCount = 2;
            this.SourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.SourcePanel.Controls.Add(this.SourceButton, 0, 0);
            this.SourcePanel.Controls.Add(this.SourceTextBox, 0, 0);
            this.SourcePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourcePanel.Location = new System.Drawing.Point(100, 150);
            this.SourcePanel.Margin = new System.Windows.Forms.Padding(0);
            this.SourcePanel.Name = "SourcePanel";
            this.SourcePanel.RowCount = 1;
            this.SourcePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SourcePanel.Size = new System.Drawing.Size(334, 30);
            this.SourcePanel.TabIndex = 5;
            //
            // SourceButton
            //
            this.SourceButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceButton.Location = new System.Drawing.Point(291, 3);
            this.SourceButton.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.SourceButton.Name = "SourceButton";
            this.SourceButton.Size = new System.Drawing.Size(40, 24);
            this.SourceButton.TabIndex = 1;
            this.SourceButton.Text = "...";
            this.SourceButton.UseVisualStyleBackColor = true;
            //
            // SourceTextBox
            //
            this.SourceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceTextBox.Location = new System.Drawing.Point(3, 3);
            this.SourceTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.SourceTextBox.Name = "SourceTextBox";
            this.SourceTextBox.Size = new System.Drawing.Size(286, 23);
            this.SourceTextBox.TabIndex = 0;
            //
            // SourceLabel
            //
            this.SourceLabel.AutoSize = true;
            this.SourceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceLabel.Location = new System.Drawing.Point(3, 153);
            this.SourceLabel.Margin = new System.Windows.Forms.Padding(3);
            this.SourceLabel.Name = "SourceLabel";
            this.SourceLabel.Size = new System.Drawing.Size(94, 24);
            this.SourceLabel.TabIndex = 0;
            this.SourceLabel.Text = "Source";
            this.SourceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // PostProcessPanel
            //
            this.PostProcessPanel.ColumnCount = 3;
            this.PostProcessPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.PostProcessPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PostProcessPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.PostProcessPanel.Controls.Add(this.PostProcessButton, 0, 0);
            this.PostProcessPanel.Controls.Add(this.PostProcessTextBox, 0, 0);
            this.PostProcessPanel.Controls.Add(this.PostProcessComboBox, 0, 0);
            this.PostProcessPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PostProcessPanel.Location = new System.Drawing.Point(100, 120);
            this.PostProcessPanel.Margin = new System.Windows.Forms.Padding(0);
            this.PostProcessPanel.Name = "PostProcessPanel";
            this.PostProcessPanel.RowCount = 1;
            this.PostProcessPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PostProcessPanel.Size = new System.Drawing.Size(334, 30);
            this.PostProcessPanel.TabIndex = 4;
            //
            // PostProcessButton
            //
            this.PostProcessButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PostProcessButton.Location = new System.Drawing.Point(291, 3);
            this.PostProcessButton.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.PostProcessButton.Name = "PostProcessButton";
            this.PostProcessButton.Size = new System.Drawing.Size(40, 24);
            this.PostProcessButton.TabIndex = 2;
            this.PostProcessButton.Text = "...";
            this.PostProcessButton.UseVisualStyleBackColor = true;
            //
            // PostProcessTextBox
            //
            this.PostProcessTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PostProcessTextBox.Location = new System.Drawing.Point(91, 3);
            this.PostProcessTextBox.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.PostProcessTextBox.Name = "PostProcessTextBox";
            this.PostProcessTextBox.Size = new System.Drawing.Size(198, 23);
            this.PostProcessTextBox.TabIndex = 1;
            //
            // PostProcessComboBox
            //
            this.PostProcessComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PostProcessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PostProcessComboBox.FormattingEnabled = true;
            this.PostProcessComboBox.Location = new System.Drawing.Point(3, 3);
            this.PostProcessComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.PostProcessComboBox.Name = "PostProcessComboBox";
            this.PostProcessComboBox.Size = new System.Drawing.Size(86, 23);
            this.PostProcessComboBox.TabIndex = 0;
            //
            // PostProcessLabel
            //
            this.PostProcessLabel.AutoSize = true;
            this.PostProcessLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PostProcessLabel.Location = new System.Drawing.Point(3, 123);
            this.PostProcessLabel.Margin = new System.Windows.Forms.Padding(3);
            this.PostProcessLabel.Name = "PostProcessLabel";
            this.PostProcessLabel.Size = new System.Drawing.Size(94, 24);
            this.PostProcessLabel.TabIndex = 0;
            this.PostProcessLabel.Text = "Post process";
            this.PostProcessLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // DestinationLabel
            //
            this.DestinationLabel.AutoSize = true;
            this.DestinationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationLabel.Location = new System.Drawing.Point(3, 93);
            this.DestinationLabel.Margin = new System.Windows.Forms.Padding(3);
            this.DestinationLabel.Name = "DestinationLabel";
            this.DestinationLabel.Size = new System.Drawing.Size(94, 24);
            this.DestinationLabel.TabIndex = 0;
            this.DestinationLabel.Text = "Destination";
            this.DestinationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // ResolutionLabel
            //
            this.ResolutionLabel.AutoSize = true;
            this.ResolutionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResolutionLabel.Location = new System.Drawing.Point(3, 63);
            this.ResolutionLabel.Margin = new System.Windows.Forms.Padding(3);
            this.ResolutionLabel.Name = "ResolutionLabel";
            this.ResolutionLabel.Size = new System.Drawing.Size(94, 24);
            this.ResolutionLabel.TabIndex = 0;
            this.ResolutionLabel.Text = "Resolutioin";
            this.ResolutionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // VersionComboBox
            //
            this.VersionComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VersionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VersionComboBox.FormattingEnabled = true;
            this.VersionComboBox.Location = new System.Drawing.Point(103, 33);
            this.VersionComboBox.Name = "VersionComboBox";
            this.VersionComboBox.Size = new System.Drawing.Size(328, 23);
            this.VersionComboBox.TabIndex = 1;
            //
            // VersionLabel
            //
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VersionLabel.Location = new System.Drawing.Point(3, 33);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(3);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(94, 24);
            this.VersionLabel.TabIndex = 0;
            this.VersionLabel.Text = "PDF version";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // FormatLabel
            //
            this.FormatLabel.AutoSize = true;
            this.FormatLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormatLabel.Location = new System.Drawing.Point(3, 3);
            this.FormatLabel.Margin = new System.Windows.Forms.Padding(3);
            this.FormatLabel.Name = "FormatLabel";
            this.FormatLabel.Size = new System.Drawing.Size(94, 24);
            this.FormatLabel.TabIndex = 0;
            this.FormatLabel.Text = "Format";
            this.FormatLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // FormatComboBox
            //
            this.FormatComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatComboBox.FormattingEnabled = true;
            this.FormatComboBox.Location = new System.Drawing.Point(103, 3);
            this.FormatComboBox.Name = "FormatComboBox";
            this.FormatComboBox.Size = new System.Drawing.Size(328, 23);
            this.FormatComboBox.TabIndex = 0;
            //
            // ResolutionControl
            //
            this.ResolutionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResolutionControl.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ResolutionControl.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ResolutionControl.Location = new System.Drawing.Point(103, 63);
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
            this.ResolutionControl.Size = new System.Drawing.Size(328, 23);
            this.ResolutionControl.TabIndex = 2;
            this.ResolutionControl.ThousandsSeparator = true;
            this.ResolutionControl.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            //
            // DestinationPanel
            //
            this.DestinationPanel.ColumnCount = 3;
            this.DestinationPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.DestinationPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DestinationPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.DestinationPanel.Controls.Add(this.DestinationButton, 2, 0);
            this.DestinationPanel.Controls.Add(this.DestinationTextBox, 1, 0);
            this.DestinationPanel.Controls.Add(this.DestinationComboBox, 0, 0);
            this.DestinationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationPanel.Location = new System.Drawing.Point(100, 90);
            this.DestinationPanel.Margin = new System.Windows.Forms.Padding(0);
            this.DestinationPanel.Name = "DestinationPanel";
            this.DestinationPanel.RowCount = 1;
            this.DestinationPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DestinationPanel.Size = new System.Drawing.Size(334, 30);
            this.DestinationPanel.TabIndex = 3;
            //
            // DestinationButton
            //
            this.DestinationButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationButton.Location = new System.Drawing.Point(291, 3);
            this.DestinationButton.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.DestinationButton.Name = "DestinationButton";
            this.DestinationButton.Size = new System.Drawing.Size(40, 24);
            this.DestinationButton.TabIndex = 2;
            this.DestinationButton.Text = "...";
            this.DestinationButton.UseVisualStyleBackColor = true;
            //
            // DestinationTextBox
            //
            this.DestinationTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationTextBox.Location = new System.Drawing.Point(91, 3);
            this.DestinationTextBox.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.DestinationTextBox.Name = "DestinationTextBox";
            this.DestinationTextBox.Size = new System.Drawing.Size(198, 23);
            this.DestinationTextBox.TabIndex = 1;
            //
            // DestinationComboBox
            //
            this.DestinationComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestinationComboBox.FormattingEnabled = true;
            this.DestinationComboBox.Location = new System.Drawing.Point(3, 3);
            this.DestinationComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.DestinationComboBox.Name = "DestinationComboBox";
            this.DestinationComboBox.Size = new System.Drawing.Size(86, 23);
            this.DestinationComboBox.TabIndex = 0;
            //
            // DocumentPage
            //
            this.DocumentPage.AutoScroll = true;
            this.DocumentPage.Controls.Add(this.DocumentPanel);
            this.DocumentPage.Location = new System.Drawing.Point(4, 24);
            this.DocumentPage.Name = "DocumentPage";
            this.DocumentPage.Padding = new System.Windows.Forms.Padding(9, 18, 9, 9);
            this.DocumentPage.Size = new System.Drawing.Size(452, 362);
            this.DocumentPage.TabIndex = 1;
            this.DocumentPage.Text = "Document";
            this.DocumentPage.UseVisualStyleBackColor = true;
            //
            // DocumentPanel
            //
            this.DocumentPanel.ColumnCount = 2;
            this.DocumentPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.DocumentPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DocumentPanel.Controls.Add(this.ViewOptionComboBox, 1, 5);
            this.DocumentPanel.Controls.Add(this.ViewOptionLabel, 0, 5);
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
            this.DocumentPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.DocumentPanel.Location = new System.Drawing.Point(9, 18);
            this.DocumentPanel.Name = "DocumentPanel";
            this.DocumentPanel.RowCount = 6;
            this.DocumentPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.DocumentPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.DocumentPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.DocumentPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.DocumentPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.DocumentPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.DocumentPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.DocumentPanel.Size = new System.Drawing.Size(434, 180);
            this.DocumentPanel.TabIndex = 0;
            //
            // ViewOptionComboBox
            //
            this.ViewOptionComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewOptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ViewOptionComboBox.FormattingEnabled = true;
            this.ViewOptionComboBox.Location = new System.Drawing.Point(103, 153);
            this.ViewOptionComboBox.Name = "ViewOptionComboBox";
            this.ViewOptionComboBox.Size = new System.Drawing.Size(328, 23);
            this.ViewOptionComboBox.TabIndex = 5;
            //
            // ViewOptionLabel
            //
            this.ViewOptionLabel.AutoSize = true;
            this.ViewOptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewOptionLabel.Location = new System.Drawing.Point(3, 153);
            this.ViewOptionLabel.Margin = new System.Windows.Forms.Padding(3);
            this.ViewOptionLabel.Name = "ViewOptionLabel";
            this.ViewOptionLabel.Size = new System.Drawing.Size(94, 24);
            this.ViewOptionLabel.TabIndex = 0;
            this.ViewOptionLabel.Text = "View option";
            this.ViewOptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // CreatorTextBox
            //
            this.CreatorTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CreatorTextBox.Location = new System.Drawing.Point(103, 123);
            this.CreatorTextBox.Name = "CreatorTextBox";
            this.CreatorTextBox.Size = new System.Drawing.Size(328, 23);
            this.CreatorTextBox.TabIndex = 4;
            //
            // CreatorLabel
            //
            this.CreatorLabel.AutoSize = true;
            this.CreatorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CreatorLabel.Location = new System.Drawing.Point(3, 123);
            this.CreatorLabel.Margin = new System.Windows.Forms.Padding(3);
            this.CreatorLabel.Name = "CreatorLabel";
            this.CreatorLabel.Size = new System.Drawing.Size(94, 24);
            this.CreatorLabel.TabIndex = 0;
            this.CreatorLabel.Text = "Creator";
            this.CreatorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // KeywordsTextBox
            //
            this.KeywordsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KeywordsTextBox.Location = new System.Drawing.Point(103, 93);
            this.KeywordsTextBox.Name = "KeywordsTextBox";
            this.KeywordsTextBox.Size = new System.Drawing.Size(328, 23);
            this.KeywordsTextBox.TabIndex = 3;
            //
            // KeywordsLabel
            //
            this.KeywordsLabel.AutoSize = true;
            this.KeywordsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KeywordsLabel.Location = new System.Drawing.Point(3, 93);
            this.KeywordsLabel.Margin = new System.Windows.Forms.Padding(3);
            this.KeywordsLabel.Name = "KeywordsLabel";
            this.KeywordsLabel.Size = new System.Drawing.Size(94, 24);
            this.KeywordsLabel.TabIndex = 0;
            this.KeywordsLabel.Text = "Keywords";
            this.KeywordsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // SubjectTextBox
            //
            this.SubjectTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubjectTextBox.Location = new System.Drawing.Point(103, 63);
            this.SubjectTextBox.Name = "SubjectTextBox";
            this.SubjectTextBox.Size = new System.Drawing.Size(328, 23);
            this.SubjectTextBox.TabIndex = 2;
            //
            // SubjectLabel
            //
            this.SubjectLabel.AutoSize = true;
            this.SubjectLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubjectLabel.Location = new System.Drawing.Point(3, 63);
            this.SubjectLabel.Margin = new System.Windows.Forms.Padding(3);
            this.SubjectLabel.Name = "SubjectLabel";
            this.SubjectLabel.Size = new System.Drawing.Size(94, 24);
            this.SubjectLabel.TabIndex = 0;
            this.SubjectLabel.Text = "Subject";
            this.SubjectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // AuthorTextBox
            //
            this.AuthorTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuthorTextBox.Location = new System.Drawing.Point(103, 33);
            this.AuthorTextBox.Name = "AuthorTextBox";
            this.AuthorTextBox.Size = new System.Drawing.Size(328, 23);
            this.AuthorTextBox.TabIndex = 1;
            //
            // AuthorLabel
            //
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuthorLabel.Location = new System.Drawing.Point(3, 33);
            this.AuthorLabel.Margin = new System.Windows.Forms.Padding(3);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(94, 24);
            this.AuthorLabel.TabIndex = 0;
            this.AuthorLabel.Text = "Author";
            this.AuthorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // TitleTextBox
            //
            this.TitleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TitleTextBox.Location = new System.Drawing.Point(103, 3);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(328, 23);
            this.TitleTextBox.TabIndex = 0;
            //
            // TitleLabel
            //
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TitleLabel.Location = new System.Drawing.Point(3, 3);
            this.TitleLabel.Margin = new System.Windows.Forms.Padding(3);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(94, 24);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Title";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // EncryptionTabPage
            //
            this.EncryptionTabPage.AutoScroll = true;
            this.EncryptionTabPage.Controls.Add(this.EncryptionOuterPanel);
            this.EncryptionTabPage.Location = new System.Drawing.Point(4, 24);
            this.EncryptionTabPage.Name = "EncryptionTabPage";
            this.EncryptionTabPage.Padding = new System.Windows.Forms.Padding(9, 18, 9, 9);
            this.EncryptionTabPage.Size = new System.Drawing.Size(452, 362);
            this.EncryptionTabPage.TabIndex = 2;
            this.EncryptionTabPage.Text = "Security";
            this.EncryptionTabPage.UseVisualStyleBackColor = true;
            //
            // EncryptionOuterPanel
            //
            this.EncryptionOuterPanel.ColumnCount = 1;
            this.EncryptionOuterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.EncryptionOuterPanel.Controls.Add(this.EnableEncryptionCheckBox, 0, 0);
            this.EncryptionOuterPanel.Controls.Add(this.EncryptionPanel, 0, 1);
            this.EncryptionOuterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.EncryptionOuterPanel.Location = new System.Drawing.Point(9, 18);
            this.EncryptionOuterPanel.Name = "EncryptionOuterPanel";
            this.EncryptionOuterPanel.RowCount = 2;
            this.EncryptionOuterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.EncryptionOuterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.EncryptionOuterPanel.Size = new System.Drawing.Size(434, 292);
            this.EncryptionOuterPanel.TabIndex = 0;
            //
            // EnableEncryptionCheckBox
            //
            this.EnableEncryptionCheckBox.AutoSize = true;
            this.EnableEncryptionCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnableEncryptionCheckBox.Location = new System.Drawing.Point(3, 3);
            this.EnableEncryptionCheckBox.Name = "EnableEncryptionCheckBox";
            this.EnableEncryptionCheckBox.Size = new System.Drawing.Size(428, 19);
            this.EnableEncryptionCheckBox.TabIndex = 0;
            this.EnableEncryptionCheckBox.Text = "Encrypt the PDF with password";
            this.EnableEncryptionCheckBox.UseVisualStyleBackColor = true;
            //
            // EncryptionPanel
            //
            this.EncryptionPanel.ColumnCount = 2;
            this.EncryptionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.EncryptionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.EncryptionPanel.Controls.Add(this.UserPasswordCheckBox, 1, 2);
            this.EncryptionPanel.Controls.Add(this.OperationLabel, 0, 2);
            this.EncryptionPanel.Controls.Add(this.OwnerConfirmTextBox, 1, 1);
            this.EncryptionPanel.Controls.Add(this.OwnerConfirmLabel, 0, 1);
            this.EncryptionPanel.Controls.Add(this.OwnerPasswordTextBox, 1, 0);
            this.EncryptionPanel.Controls.Add(this.OwnerPasswordLabel, 0, 0);
            this.EncryptionPanel.Controls.Add(this.OperationPanel, 1, 3);
            this.EncryptionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EncryptionPanel.Location = new System.Drawing.Point(0, 25);
            this.EncryptionPanel.Margin = new System.Windows.Forms.Padding(0);
            this.EncryptionPanel.Name = "EncryptionPanel";
            this.EncryptionPanel.RowCount = 4;
            this.EncryptionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.EncryptionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.EncryptionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.EncryptionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.EncryptionPanel.Size = new System.Drawing.Size(434, 267);
            this.EncryptionPanel.TabIndex = 1;
            //
            // UserPasswordCheckBox
            //
            this.UserPasswordCheckBox.AutoSize = true;
            this.UserPasswordCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserPasswordCheckBox.Location = new System.Drawing.Point(103, 63);
            this.UserPasswordCheckBox.Name = "UserPasswordCheckBox";
            this.UserPasswordCheckBox.Size = new System.Drawing.Size(328, 24);
            this.UserPasswordCheckBox.TabIndex = 2;
            this.UserPasswordCheckBox.Text = "Open with password";
            this.UserPasswordCheckBox.UseVisualStyleBackColor = true;
            //
            // OperationLabel
            //
            this.OperationLabel.AutoSize = true;
            this.OperationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationLabel.Location = new System.Drawing.Point(3, 63);
            this.OperationLabel.Margin = new System.Windows.Forms.Padding(3);
            this.OperationLabel.Name = "OperationLabel";
            this.OperationLabel.Size = new System.Drawing.Size(94, 24);
            this.OperationLabel.TabIndex = 0;
            this.OperationLabel.Text = "Operations";
            this.OperationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // OwnerConfirmTextBox
            //
            this.OwnerConfirmTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OwnerConfirmTextBox.Location = new System.Drawing.Point(103, 33);
            this.OwnerConfirmTextBox.Name = "OwnerConfirmTextBox";
            this.OwnerConfirmTextBox.Size = new System.Drawing.Size(328, 23);
            this.OwnerConfirmTextBox.TabIndex = 1;
            //
            // OwnerConfirmLabel
            //
            this.OwnerConfirmLabel.AutoSize = true;
            this.OwnerConfirmLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OwnerConfirmLabel.Location = new System.Drawing.Point(3, 33);
            this.OwnerConfirmLabel.Margin = new System.Windows.Forms.Padding(3);
            this.OwnerConfirmLabel.Name = "OwnerConfirmLabel";
            this.OwnerConfirmLabel.Size = new System.Drawing.Size(94, 24);
            this.OwnerConfirmLabel.TabIndex = 0;
            this.OwnerConfirmLabel.Text = "Confirm";
            this.OwnerConfirmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // OwnerPasswordTextBox
            //
            this.OwnerPasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OwnerPasswordTextBox.Location = new System.Drawing.Point(103, 3);
            this.OwnerPasswordTextBox.Name = "OwnerPasswordTextBox";
            this.OwnerPasswordTextBox.Size = new System.Drawing.Size(328, 23);
            this.OwnerPasswordTextBox.TabIndex = 0;
            //
            // OwnerPasswordLabel
            //
            this.OwnerPasswordLabel.AutoSize = true;
            this.OwnerPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OwnerPasswordLabel.Location = new System.Drawing.Point(3, 3);
            this.OwnerPasswordLabel.Margin = new System.Windows.Forms.Padding(3);
            this.OwnerPasswordLabel.Name = "OwnerPasswordLabel";
            this.OwnerPasswordLabel.Size = new System.Drawing.Size(94, 24);
            this.OwnerPasswordLabel.TabIndex = 0;
            this.OwnerPasswordLabel.Text = "Password";
            this.OwnerPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // OperationPanel
            //
            this.OperationPanel.ColumnCount = 1;
            this.OperationPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.OperationPanel.Controls.Add(this.SharePasswordCheckBox, 0, 1);
            this.OperationPanel.Controls.Add(this.UserPasswordPanel, 0, 0);
            this.OperationPanel.Controls.Add(this.PermissionPanel, 0, 2);
            this.OperationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationPanel.Location = new System.Drawing.Point(100, 90);
            this.OperationPanel.Margin = new System.Windows.Forms.Padding(0);
            this.OperationPanel.Name = "OperationPanel";
            this.OperationPanel.RowCount = 3;
            this.OperationPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.OperationPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OperationPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.OperationPanel.Size = new System.Drawing.Size(334, 177);
            this.OperationPanel.TabIndex = 3;
            //
            // SharePasswordCheckBox
            //
            this.SharePasswordCheckBox.AutoSize = true;
            this.SharePasswordCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SharePasswordCheckBox.Location = new System.Drawing.Point(93, 55);
            this.SharePasswordCheckBox.Margin = new System.Windows.Forms.Padding(93, 3, 3, 3);
            this.SharePasswordCheckBox.Name = "SharePasswordCheckBox";
            this.SharePasswordCheckBox.Size = new System.Drawing.Size(238, 19);
            this.SharePasswordCheckBox.TabIndex = 1;
            this.SharePasswordCheckBox.Text = "Use owner password";
            this.SharePasswordCheckBox.UseVisualStyleBackColor = true;
            //
            // UserPasswordPanel
            //
            this.UserPasswordPanel.ColumnCount = 2;
            this.UserPasswordPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.UserPasswordPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.UserPasswordPanel.Controls.Add(this.UserConfirmTextBox, 1, 1);
            this.UserPasswordPanel.Controls.Add(this.UserPasswordTextBox, 1, 0);
            this.UserPasswordPanel.Controls.Add(this.UserConfirmLabel, 0, 1);
            this.UserPasswordPanel.Controls.Add(this.UserPasswordLabel, 0, 0);
            this.UserPasswordPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserPasswordPanel.Location = new System.Drawing.Point(0, 0);
            this.UserPasswordPanel.Margin = new System.Windows.Forms.Padding(0);
            this.UserPasswordPanel.Name = "UserPasswordPanel";
            this.UserPasswordPanel.RowCount = 2;
            this.UserPasswordPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.UserPasswordPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.UserPasswordPanel.Size = new System.Drawing.Size(334, 52);
            this.UserPasswordPanel.TabIndex = 0;
            //
            // UserConfirmTextBox
            //
            this.UserConfirmTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserConfirmTextBox.Location = new System.Drawing.Point(93, 27);
            this.UserConfirmTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.UserConfirmTextBox.Name = "UserConfirmTextBox";
            this.UserConfirmTextBox.Size = new System.Drawing.Size(238, 23);
            this.UserConfirmTextBox.TabIndex = 1;
            //
            // UserPasswordTextBox
            //
            this.UserPasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserPasswordTextBox.Location = new System.Drawing.Point(93, 1);
            this.UserPasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.UserPasswordTextBox.Name = "UserPasswordTextBox";
            this.UserPasswordTextBox.Size = new System.Drawing.Size(238, 23);
            this.UserPasswordTextBox.TabIndex = 0;
            //
            // UserConfirmLabel
            //
            this.UserConfirmLabel.AutoSize = true;
            this.UserConfirmLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserConfirmLabel.Location = new System.Drawing.Point(3, 29);
            this.UserConfirmLabel.Margin = new System.Windows.Forms.Padding(3);
            this.UserConfirmLabel.Name = "UserConfirmLabel";
            this.UserConfirmLabel.Size = new System.Drawing.Size(84, 20);
            this.UserConfirmLabel.TabIndex = 0;
            this.UserConfirmLabel.Text = "Confirm";
            this.UserConfirmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // UserPasswordLabel
            //
            this.UserPasswordLabel.AutoSize = true;
            this.UserPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserPasswordLabel.Location = new System.Drawing.Point(3, 3);
            this.UserPasswordLabel.Margin = new System.Windows.Forms.Padding(3);
            this.UserPasswordLabel.Name = "UserPasswordLabel";
            this.UserPasswordLabel.Size = new System.Drawing.Size(84, 20);
            this.UserPasswordLabel.TabIndex = 0;
            this.UserPasswordLabel.Text = "Password";
            this.UserPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // OthersTabPage
            //
            this.OthersTabPage.AutoScroll = true;
            this.OthersTabPage.Location = new System.Drawing.Point(4, 24);
            this.OthersTabPage.Name = "OthersTabPage";
            this.OthersTabPage.Padding = new System.Windows.Forms.Padding(9, 18, 9, 9);
            this.OthersTabPage.Size = new System.Drawing.Size(452, 362);
            this.OthersTabPage.TabIndex = 3;
            this.OthersTabPage.Text = "Others";
            this.OthersTabPage.UseVisualStyleBackColor = true;
            //
            // FooterPanel
            //
            this.FooterPanel.ColumnCount = 3;
            this.FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.FooterPanel.Controls.Add(this.ToolsPanel, 0, 0);
            this.FooterPanel.Controls.Add(this.ConvertButton, 1, 0);
            this.FooterPanel.Controls.Add(this.ExitButton, 2, 0);
            this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FooterPanel.Location = new System.Drawing.Point(9, 461);
            this.FooterPanel.Margin = new System.Windows.Forms.Padding(9, 3, 9, 9);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.RowCount = 1;
            this.FooterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FooterPanel.Size = new System.Drawing.Size(466, 41);
            this.FooterPanel.TabIndex = 2;
            //
            // ToolsPanel
            //
            this.ToolsPanel.Controls.Add(this.ConvertProgressBar);
            this.ToolsPanel.Controls.Add(this.ApplyButton);
            this.ToolsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolsPanel.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.ToolsPanel.Location = new System.Drawing.Point(0, 0);
            this.ToolsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ToolsPanel.Name = "ToolsPanel";
            this.ToolsPanel.Size = new System.Drawing.Size(206, 41);
            this.ToolsPanel.TabIndex = 0;
            //
            // ConvertProgressBar
            //
            this.ConvertProgressBar.Location = new System.Drawing.Point(3, 23);
            this.ConvertProgressBar.Name = "ConvertProgressBar";
            this.ConvertProgressBar.Size = new System.Drawing.Size(200, 15);
            this.ConvertProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ConvertProgressBar.TabIndex = 0;
            this.ConvertProgressBar.Visible = false;
            //
            // ApplyButton
            //
            this.ApplyButton.Content = "Save settings";
            this.ApplyButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ApplyButton.FlatAppearance.BorderSize = 0;
            this.ApplyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApplyButton.Image = null;
            this.ApplyButton.Location = new System.Drawing.Point(209, 8);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(100, 30);
            this.ApplyButton.Styles.DisabledStyle.BackColor = System.Drawing.Color.Gainsboro;
            this.ApplyButton.Styles.DisabledStyle.ContentColor = System.Drawing.Color.Gray;
            this.ApplyButton.Styles.MouseDownStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ApplyButton.Styles.MouseOverStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ApplyButton.Styles.NormalStyle.BackColor = System.Drawing.Color.White;
            this.ApplyButton.Styles.NormalStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ApplyButton.Styles.NormalStyle.BorderSize = 1;
            this.ApplyButton.Styles.NormalStyle.ContentColor = System.Drawing.Color.Black;
            this.ApplyButton.TabIndex = 1;
            this.ApplyButton.Text = "button1";
            this.ApplyButton.UseVisualStyleBackColor = true;
            //
            // ConvertButton
            //
            this.ConvertButton.Content = "Convert";
            this.ConvertButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConvertButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConvertButton.FlatAppearance.BorderSize = 0;
            this.ConvertButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConvertButton.Image = null;
            this.ConvertButton.Location = new System.Drawing.Point(209, 3);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new System.Drawing.Size(134, 35);
            this.ConvertButton.Styles.MouseDownStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.ConvertButton.Styles.MouseOverStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.ConvertButton.Styles.NormalStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.ConvertButton.Styles.NormalStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ConvertButton.Styles.NormalStyle.BorderSize = 1;
            this.ConvertButton.Styles.NormalStyle.ContentColor = System.Drawing.Color.White;
            this.ConvertButton.TabIndex = 1;
            this.ConvertButton.Text = "button1";
            this.ConvertButton.UseVisualStyleBackColor = true;
            //
            // ExitButton
            //
            this.ExitButton.Content = "Cancel";
            this.ExitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExitButton.FlatAppearance.BorderSize = 0;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.Image = null;
            this.ExitButton.Location = new System.Drawing.Point(349, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(114, 35);
            this.ExitButton.Styles.MouseDownStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.ExitButton.Styles.MouseOverStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.ExitButton.Styles.NormalStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.ExitButton.Styles.NormalStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ExitButton.Styles.NormalStyle.BorderSize = 1;
            this.ExitButton.Styles.NormalStyle.ContentColor = System.Drawing.Color.White;
            this.ExitButton.TabIndex = 2;
            this.ExitButton.Text = "button2";
            this.ExitButton.UseVisualStyleBackColor = true;
            //
            // AllowPrintCheckBox
            //
            this.AllowPrintCheckBox.AutoSize = true;
            this.AllowPrintCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllowPrintCheckBox.Location = new System.Drawing.Point(3, 3);
            this.AllowPrintCheckBox.Name = "AllowPrintCheckBox";
            this.AllowPrintCheckBox.Size = new System.Drawing.Size(328, 19);
            this.AllowPrintCheckBox.TabIndex = 0;
            this.AllowPrintCheckBox.Text = "Allow printing";
            this.AllowPrintCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowCopyCheckBox
            //
            this.AllowCopyCheckBox.AutoSize = true;
            this.AllowCopyCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllowCopyCheckBox.Location = new System.Drawing.Point(3, 28);
            this.AllowCopyCheckBox.Name = "AllowCopyCheckBox";
            this.AllowCopyCheckBox.Size = new System.Drawing.Size(328, 19);
            this.AllowCopyCheckBox.TabIndex = 1;
            this.AllowCopyCheckBox.Text = "Allow copying text and images";
            this.AllowCopyCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowFormCheckBox
            //
            this.AllowFormCheckBox.AutoSize = true;
            this.AllowFormCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllowFormCheckBox.Location = new System.Drawing.Point(3, 53);
            this.AllowFormCheckBox.Name = "AllowFormCheckBox";
            this.AllowFormCheckBox.Size = new System.Drawing.Size(328, 19);
            this.AllowFormCheckBox.TabIndex = 2;
            this.AllowFormCheckBox.Text = "Allow filling in forms";
            this.AllowFormCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowModifyCheckBox
            //
            this.AllowModifyCheckBox.AutoSize = true;
            this.AllowModifyCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllowModifyCheckBox.Location = new System.Drawing.Point(3, 78);
            this.AllowModifyCheckBox.Name = "AllowModifyCheckBox";
            this.AllowModifyCheckBox.Size = new System.Drawing.Size(328, 19);
            this.AllowModifyCheckBox.TabIndex = 3;
            this.AllowModifyCheckBox.Text = "Allow inserting and removing pages";
            this.AllowModifyCheckBox.UseVisualStyleBackColor = true;
            //
            // PermissionPanel
            //
            this.PermissionPanel.ColumnCount = 1;
            this.PermissionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PermissionPanel.Controls.Add(this.AllowModifyCheckBox, 0, 3);
            this.PermissionPanel.Controls.Add(this.AllowFormCheckBox, 0, 2);
            this.PermissionPanel.Controls.Add(this.AllowCopyCheckBox, 0, 1);
            this.PermissionPanel.Controls.Add(this.AllowPrintCheckBox, 0, 0);
            this.PermissionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PermissionPanel.Location = new System.Drawing.Point(0, 77);
            this.PermissionPanel.Margin = new System.Windows.Forms.Padding(0);
            this.PermissionPanel.Name = "PermissionPanel";
            this.PermissionPanel.RowCount = 4;
            this.PermissionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.PermissionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.PermissionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.PermissionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.PermissionPanel.Size = new System.Drawing.Size(334, 100);
            this.PermissionPanel.TabIndex = 2;
            //
            // MainForm
            //
            this.AcceptButton = this.ConvertButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.ExitButton;
            this.ClientSize = new System.Drawing.Size(484, 511);
            this.Controls.Add(this.RootPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 550);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "MainForm";
            this.Text = "CubePDF";
            this.RootPanel.ResumeLayout(false);
            this.SettingsPanel.ResumeLayout(false);
            this.SettingsTabControl.ResumeLayout(false);
            this.GeneralTabPage.ResumeLayout(false);
            this.GeneralPanel.ResumeLayout(false);
            this.GeneralPanel.PerformLayout();
            this.SourcePanel.ResumeLayout(false);
            this.SourcePanel.PerformLayout();
            this.PostProcessPanel.ResumeLayout(false);
            this.PostProcessPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResolutionControl)).EndInit();
            this.DestinationPanel.ResumeLayout(false);
            this.DestinationPanel.PerformLayout();
            this.DocumentPage.ResumeLayout(false);
            this.DocumentPanel.ResumeLayout(false);
            this.DocumentPanel.PerformLayout();
            this.EncryptionTabPage.ResumeLayout(false);
            this.EncryptionOuterPanel.ResumeLayout(false);
            this.EncryptionOuterPanel.PerformLayout();
            this.EncryptionPanel.ResumeLayout(false);
            this.EncryptionPanel.PerformLayout();
            this.OperationPanel.ResumeLayout(false);
            this.OperationPanel.PerformLayout();
            this.UserPasswordPanel.ResumeLayout(false);
            this.UserPasswordPanel.PerformLayout();
            this.FooterPanel.ResumeLayout(false);
            this.ToolsPanel.ResumeLayout(false);
            this.PermissionPanel.ResumeLayout(false);
            this.PermissionPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private System.Windows.Forms.TabControl SettingsTabControl;
        private System.Windows.Forms.TabPage GeneralTabPage;
        private System.Windows.Forms.TabPage DocumentPage;
        private System.Windows.Forms.TabPage EncryptionTabPage;
        private System.Windows.Forms.TabPage OthersTabPage;
        private System.Windows.Forms.FlowLayoutPanel ToolsPanel;
        private System.Windows.Forms.ProgressBar ConvertProgressBar;
        private Cube.Forms.SettingsControl SettingsPanel;
        private Cube.Forms.FlatButton HeaderButton;
        private Cube.Forms.FlatButton ConvertButton;
        private Cube.Forms.FlatButton ExitButton;
        private Cube.Forms.FlatButton ApplyButton;
        private System.Windows.Forms.TableLayoutPanel GeneralPanel;
        private System.Windows.Forms.Label FormatLabel;
        private System.Windows.Forms.ComboBox FormatComboBox;
        private System.Windows.Forms.ComboBox VersionComboBox;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label ResolutionLabel;
        private System.Windows.Forms.NumericUpDown ResolutionControl;
        private System.Windows.Forms.Label DestinationLabel;
        private System.Windows.Forms.TableLayoutPanel DestinationPanel;
        private System.Windows.Forms.Label PostProcessLabel;
        private System.Windows.Forms.TableLayoutPanel PostProcessPanel;
        private System.Windows.Forms.TableLayoutPanel SourcePanel;
        private System.Windows.Forms.Label SourceLabel;
        private System.Windows.Forms.Button PostProcessButton;
        private System.Windows.Forms.TextBox PostProcessTextBox;
        private System.Windows.Forms.ComboBox PostProcessComboBox;
        private System.Windows.Forms.Button SourceButton;
        private System.Windows.Forms.TextBox SourceTextBox;
        private System.Windows.Forms.Button DestinationButton;
        private System.Windows.Forms.TextBox DestinationTextBox;
        private System.Windows.Forms.ComboBox DestinationComboBox;
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
        private System.Windows.Forms.Label ViewOptionLabel;
        private System.Windows.Forms.ComboBox ViewOptionComboBox;
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
    }
}

