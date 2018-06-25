namespace Cube.Pdf.App.Picker
{
    partial class ProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ExitButton = new Cube.Forms.Button();
            this.SaveButton = new Cube.Forms.Button();
            this.PreviewButton = new Cube.Forms.Button();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.ImagePictureBox = new System.Windows.Forms.PictureBox();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.SuspendLayout();
            //
            // ProgressBar
            //
            this.ProgressBar.Location = new System.Drawing.Point(12, 90);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(410, 20);
            this.ProgressBar.TabIndex = 1000;
            //
            // ExitButton
            //
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.ExitButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.ForeColor = System.Drawing.Color.White;
            this.ExitButton.Location = new System.Drawing.Point(322, 134);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 35);
            this.ExitButton.TabIndex = 2;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = false;
            //
            // SaveButton
            //
            this.SaveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.SaveButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.ForeColor = System.Drawing.Color.White;
            this.SaveButton.Location = new System.Drawing.Point(186, 134);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(130, 35);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "全て保存";
            this.SaveButton.UseVisualStyleBackColor = false;
            //
            // PreviewButton
            //
            this.PreviewButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.PreviewButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.PreviewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PreviewButton.ForeColor = System.Drawing.Color.White;
            this.PreviewButton.Location = new System.Drawing.Point(50, 134);
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Size = new System.Drawing.Size(130, 35);
            this.PreviewButton.TabIndex = 0;
            this.PreviewButton.Text = "プレビュー";
            this.PreviewButton.UseVisualStyleBackColor = false;
            //
            // HeaderPanel
            //
            this.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.HeaderPanel.Controls.Add(this.ImagePictureBox);
            this.HeaderPanel.Controls.Add(this.LogoPictureBox);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(434, 35);
            this.HeaderPanel.TabIndex = 1000;
            //
            // ImagePictureBox
            //
            this.ImagePictureBox.BackgroundImage = global::Cube.Pdf.App.Picker.Properties.Resources.HeaderImage;
            this.ImagePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ImagePictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.ImagePictureBox.Location = new System.Drawing.Point(217, 0);
            this.ImagePictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.ImagePictureBox.Name = "ImagePictureBox";
            this.ImagePictureBox.Size = new System.Drawing.Size(217, 35);
            this.ImagePictureBox.TabIndex = 5;
            this.ImagePictureBox.TabStop = false;
            //
            // LogoPictureBox
            //
            this.LogoPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.LogoPictureBox.Image = global::Cube.Pdf.App.Picker.Properties.Resources.HeaderLogo;
            this.LogoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(190, 35);
            this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.LogoPictureBox.TabIndex = 4;
            this.LogoPictureBox.TabStop = false;
            //
            // MessageLabel
            //
            this.MessageLabel.AutoEllipsis = true;
            this.MessageLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MessageLabel.Location = new System.Drawing.Point(0, 35);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Padding = new System.Windows.Forms.Padding(12, 4, 12, 0);
            this.MessageLabel.Size = new System.Drawing.Size(434, 50);
            this.MessageLabel.TabIndex = 1000;
            this.MessageLabel.Text = "ファイルを解析しています...";
            this.MessageLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // ProgressForm
            //
            this.ClientSize = new System.Drawing.Size(434, 181);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.HeaderPanel);
            this.Controls.Add(this.PreviewButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.Text = "ProgressForm";
            this.HeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar ProgressBar;
        private Cube.Forms.Button ExitButton;
        private Cube.Forms.Button SaveButton;
        private Cube.Forms.Button PreviewButton;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.PictureBox ImagePictureBox;
        private System.Windows.Forms.PictureBox LogoPictureBox;
        private System.Windows.Forms.Label MessageLabel;
    }
}