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
            this.DocumentPage = new System.Windows.Forms.TabPage();
            this.SecurityTabPage = new System.Windows.Forms.TabPage();
            this.OthersTabPage = new System.Windows.Forms.TabPage();
            this.FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ToolsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ConvertProgressBar = new System.Windows.Forms.ProgressBar();
            this.ApplyButton = new Cube.Forms.FlatButton();
            this.ConvertButton = new Cube.Forms.FlatButton();
            this.ExitButton = new Cube.Forms.FlatButton();
            this.RootPanel.SuspendLayout();
            this.SettingsPanel.SuspendLayout();
            this.SettingsTabControl.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.ToolsPanel.SuspendLayout();
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
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
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
            this.SettingsPanel.Size = new System.Drawing.Size(466, 389);
            this.SettingsPanel.TabIndex = 1;
            //
            // SettingsTabControl
            //
            this.SettingsTabControl.Controls.Add(this.GeneralTabPage);
            this.SettingsTabControl.Controls.Add(this.DocumentPage);
            this.SettingsTabControl.Controls.Add(this.SecurityTabPage);
            this.SettingsTabControl.Controls.Add(this.OthersTabPage);
            this.SettingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsTabControl.ItemSize = new System.Drawing.Size(100, 20);
            this.SettingsTabControl.Location = new System.Drawing.Point(3, 3);
            this.SettingsTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsTabControl.Name = "SettingsTabControl";
            this.SettingsTabControl.SelectedIndex = 0;
            this.SettingsTabControl.Size = new System.Drawing.Size(460, 383);
            this.SettingsTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.SettingsTabControl.TabIndex = 0;
            //
            // GeneralTabPage
            //
            this.GeneralTabPage.Location = new System.Drawing.Point(4, 24);
            this.GeneralTabPage.Name = "GeneralTabPage";
            this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralTabPage.Size = new System.Drawing.Size(452, 355);
            this.GeneralTabPage.TabIndex = 0;
            this.GeneralTabPage.Text = "General";
            this.GeneralTabPage.UseVisualStyleBackColor = true;
            //
            // DocumentPage
            //
            this.DocumentPage.Location = new System.Drawing.Point(4, 24);
            this.DocumentPage.Name = "DocumentPage";
            this.DocumentPage.Padding = new System.Windows.Forms.Padding(3);
            this.DocumentPage.Size = new System.Drawing.Size(452, 355);
            this.DocumentPage.TabIndex = 1;
            this.DocumentPage.Text = "Document";
            this.DocumentPage.UseVisualStyleBackColor = true;
            //
            // SecurityTabPage
            //
            this.SecurityTabPage.Location = new System.Drawing.Point(4, 24);
            this.SecurityTabPage.Name = "SecurityTabPage";
            this.SecurityTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.SecurityTabPage.Size = new System.Drawing.Size(452, 355);
            this.SecurityTabPage.TabIndex = 2;
            this.SecurityTabPage.Text = "Security";
            this.SecurityTabPage.UseVisualStyleBackColor = true;
            //
            // OthersTabPage
            //
            this.OthersTabPage.Location = new System.Drawing.Point(4, 24);
            this.OthersTabPage.Name = "OthersTabPage";
            this.OthersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.OthersTabPage.Size = new System.Drawing.Size(452, 355);
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
            this.FooterPanel.Location = new System.Drawing.Point(9, 454);
            this.FooterPanel.Margin = new System.Windows.Forms.Padding(9, 3, 9, 9);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.RowCount = 1;
            this.FooterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FooterPanel.Size = new System.Drawing.Size(466, 48);
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
            this.ToolsPanel.Size = new System.Drawing.Size(206, 48);
            this.ToolsPanel.TabIndex = 0;
            //
            // ConvertProgressBar
            //
            this.ConvertProgressBar.Location = new System.Drawing.Point(3, 30);
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
            this.ApplyButton.Location = new System.Drawing.Point(209, 15);
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
            this.ConvertButton.Size = new System.Drawing.Size(134, 42);
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
            this.ExitButton.Size = new System.Drawing.Size(114, 42);
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
            this.FooterPanel.ResumeLayout(false);
            this.ToolsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private System.Windows.Forms.TabControl SettingsTabControl;
        private System.Windows.Forms.TabPage GeneralTabPage;
        private System.Windows.Forms.TabPage DocumentPage;
        private System.Windows.Forms.TabPage SecurityTabPage;
        private System.Windows.Forms.TabPage OthersTabPage;
        private System.Windows.Forms.FlowLayoutPanel ToolsPanel;
        private System.Windows.Forms.ProgressBar ConvertProgressBar;
        private Cube.Forms.SettingsControl SettingsPanel;
        private Cube.Forms.FlatButton HeaderButton;
        private Cube.Forms.FlatButton ConvertButton;
        private Cube.Forms.FlatButton ExitButton;
        private Cube.Forms.FlatButton ApplyButton;
    }
}

