namespace Cube.Pdf.ImageEx
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
            this.MessageLabel = new System.Windows.Forms.Label();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ExitButton = new Cube.Forms.Button();
            this.SaveButton = new Cube.Forms.Button();
            this.PreviewButton = new Cube.Forms.Button();
            this.HeaderPanel = new Cube.Pdf.ImageEx.HeaderView();
            this.SuspendLayout();
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoEllipsis = true;
            this.MessageLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MessageLabel.Location = new System.Drawing.Point(0, 35);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Padding = new System.Windows.Forms.Padding(12, 4, 12, 0);
            this.MessageLabel.Size = new System.Drawing.Size(434, 50);
            this.MessageLabel.TabIndex = 999;
            this.MessageLabel.Text = "ファイルを解析しています...";
            this.MessageLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(12, 90);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(410, 20);
            this.ProgressBar.TabIndex = 999;
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
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(434, 35);
            this.HeaderPanel.TabIndex = 999;
            this.HeaderPanel.TabStop = false;
            // 
            // ProgressForm
            // 
            this.ClientSize = new System.Drawing.Size(434, 181);
            this.Controls.Add(this.PreviewButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.HeaderPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.Text = "ProgressForm";
            this.ResumeLayout(false);

        }

        #endregion

        private HeaderView HeaderPanel;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private Cube.Forms.Button ExitButton;
        private Cube.Forms.Button SaveButton;
        private Cube.Forms.Button PreviewButton;
    }
}