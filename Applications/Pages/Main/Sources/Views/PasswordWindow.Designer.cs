namespace Cube.Pdf.Pages
{
    partial class PasswordWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordWindow));
            this.LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.ImagePictureBox = new System.Windows.Forms.PictureBox();
            this.TitleButton = new System.Windows.Forms.PictureBox();
            this.ButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ExitButton = new Cube.Forms.Button();
            this.ExecButton = new Cube.Forms.Button();
            this.ContentsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.PasswordKeyLabel = new System.Windows.Forms.Label();
            this.ShowPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.PasswordBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.LayoutPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleButton)).BeginInit();
            this.ButtonsPanel.SuspendLayout();
            this.ContentsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordBindingSource)).BeginInit();
            this.SuspendLayout();
            //
            // LayoutPanel
            //
            this.LayoutPanel.ColumnCount = 1;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.Controls.Add(this.PasswordLabel, 0, 1);
            this.LayoutPanel.Controls.Add(this.HeaderPanel, 0, 0);
            this.LayoutPanel.Controls.Add(this.ButtonsPanel, 0, 3);
            this.LayoutPanel.Controls.Add(this.ContentsPanel, 0, 2);
            this.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.RowCount = 4;
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.LayoutPanel.Size = new System.Drawing.Size(482, 223);
            this.LayoutPanel.TabIndex = 4;
            //
            // PasswordLabel
            //
            this.PasswordLabel.AutoEllipsis = true;
            this.PasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PasswordLabel.Location = new System.Drawing.Point(0, 35);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(0);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.PasswordLabel.Size = new System.Drawing.Size(482, 60);
            this.PasswordLabel.TabIndex = 1001;
            this.PasswordLabel.Text = "オーナーパスワードを入力してください。";
            this.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // HeaderPanel
            //
            this.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.HeaderPanel.Controls.Add(this.ImagePictureBox);
            this.HeaderPanel.Controls.Add(this.TitleButton);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(482, 35);
            this.HeaderPanel.TabIndex = 999;
            //
            // ImagePictureBox
            //
            this.ImagePictureBox.BackgroundImage = global::Cube.Pdf.Pages.Properties.Resources.HeaderImage;
            this.ImagePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ImagePictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.ImagePictureBox.Location = new System.Drawing.Point(265, 0);
            this.ImagePictureBox.Name = "ImagePictureBox";
            this.ImagePictureBox.Size = new System.Drawing.Size(217, 35);
            this.ImagePictureBox.TabIndex = 1;
            this.ImagePictureBox.TabStop = false;
            //
            // TitleButton
            //
            this.TitleButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TitleButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.TitleButton.Image = global::Cube.Pdf.Pages.Properties.Resources.HeaderTitle;
            this.TitleButton.Location = new System.Drawing.Point(0, 0);
            this.TitleButton.Margin = new System.Windows.Forms.Padding(0);
            this.TitleButton.Name = "TitleButton";
            this.TitleButton.Size = new System.Drawing.Size(170, 35);
            this.TitleButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.TitleButton.TabIndex = 0;
            this.TitleButton.TabStop = false;
            //
            // ButtonsPanel
            //
            this.ButtonsPanel.Controls.Add(this.ExitButton);
            this.ButtonsPanel.Controls.Add(this.ExecButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 163);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 10, 10, 0);
            this.ButtonsPanel.Size = new System.Drawing.Size(482, 60);
            this.ButtonsPanel.TabIndex = 6;
            //
            // ExitButton
            //
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.ForeColor = System.Drawing.Color.White;
            this.ExitButton.Location = new System.Drawing.Point(370, 12);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(2);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 35);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = false;
            //
            // ExecButton
            //
            this.ExecButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.ExecButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ExecButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.ExecButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExecButton.ForeColor = System.Drawing.Color.White;
            this.ExecButton.Location = new System.Drawing.Point(236, 12);
            this.ExecButton.Margin = new System.Windows.Forms.Padding(2);
            this.ExecButton.Name = "ExecButton";
            this.ExecButton.Size = new System.Drawing.Size(130, 35);
            this.ExecButton.TabIndex = 1;
            this.ExecButton.Text = "OK";
            this.ExecButton.UseVisualStyleBackColor = false;
            //
            // ContentsPanel
            //
            this.ContentsPanel.Controls.Add(this.PasswordTextBox);
            this.ContentsPanel.Controls.Add(this.PasswordKeyLabel);
            this.ContentsPanel.Controls.Add(this.ShowPasswordCheckBox);
            this.ContentsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ContentsPanel.Location = new System.Drawing.Point(0, 95);
            this.ContentsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ContentsPanel.Name = "ContentsPanel";
            this.ContentsPanel.Size = new System.Drawing.Size(482, 68);
            this.ContentsPanel.TabIndex = 1000;
            //
            // PasswordTextBox
            //
            this.PasswordTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.PasswordBindingSource, "Password", true));
            this.PasswordTextBox.Location = new System.Drawing.Point(170, 4);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(0, 4, 12, 0);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(300, 19);
            this.PasswordTextBox.TabIndex = 6;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            //
            // PasswordKeyLabel
            //
            this.PasswordKeyLabel.AutoSize = true;
            this.ContentsPanel.SetFlowBreak(this.PasswordKeyLabel, true);
            this.PasswordKeyLabel.Location = new System.Drawing.Point(71, 8);
            this.PasswordKeyLabel.Margin = new System.Windows.Forms.Padding(3, 8, 8, 0);
            this.PasswordKeyLabel.Name = "PasswordKeyLabel";
            this.PasswordKeyLabel.Size = new System.Drawing.Size(91, 12);
            this.PasswordKeyLabel.TabIndex = 8;
            this.PasswordKeyLabel.Text = "パスワードを入力：";
            //
            // ShowPasswordCheckBox
            //
            this.ShowPasswordCheckBox.AutoSize = true;
            this.ShowPasswordCheckBox.Location = new System.Drawing.Point(366, 29);
            this.ShowPasswordCheckBox.Margin = new System.Windows.Forms.Padding(0, 6, 12, 0);
            this.ShowPasswordCheckBox.Name = "ShowPasswordCheckBox";
            this.ShowPasswordCheckBox.Size = new System.Drawing.Size(104, 16);
            this.ShowPasswordCheckBox.TabIndex = 7;
            this.ShowPasswordCheckBox.Text = "パスワードを表示";
            this.ShowPasswordCheckBox.UseVisualStyleBackColor = true;
            //
            // PasswordBindingSource
            //
            this.PasswordBindingSource.DataSource = typeof(Cube.Pdf.Pages.PasswordViewModel);
            //
            // PasswordWindow
            //
            this.AcceptButton = this.ExecButton;
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.CancelButton = this.ExitButton;
            this.ClientSize = new System.Drawing.Size(482, 223);
            this.Controls.Add(this.LayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordWindow";
            this.ShowInTaskbar = false;
            this.Text = "パスワードを入力して下さい";
            this.LayoutPanel.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleButton)).EndInit();
            this.ButtonsPanel.ResumeLayout(false);
            this.ContentsPanel.ResumeLayout(false);
            this.ContentsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.PictureBox ImagePictureBox;
        private System.Windows.Forms.PictureBox TitleButton;
        private System.Windows.Forms.FlowLayoutPanel ButtonsPanel;
        private Forms.Button ExitButton;
        private Forms.Button ExecButton;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.FlowLayoutPanel ContentsPanel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label PasswordKeyLabel;
        private System.Windows.Forms.CheckBox ShowPasswordCheckBox;
        private System.Windows.Forms.BindingSource PasswordBindingSource;
    }
}