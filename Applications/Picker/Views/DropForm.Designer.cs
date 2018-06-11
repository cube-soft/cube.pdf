namespace Cube.Pdf.App.Picker
{
    partial class DropForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DropForm));
            this.DropPanel = new Cube.Forms.Panel();
            this.ExitButton = new Cube.Forms.Button();
            this.DropPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // DropPanel
            //
            this.DropPanel.BackgroundImage = global::Cube.Pdf.App.Picker.Properties.Resources.DragDrop;
            this.DropPanel.Controls.Add(this.ExitButton);
            this.DropPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DropPanel.Location = new System.Drawing.Point(1, 1);
            this.DropPanel.Margin = new System.Windows.Forms.Padding(0);
            this.DropPanel.Name = "DropPanel";
            this.DropPanel.Size = new System.Drawing.Size(98, 98);
            this.DropPanel.TabIndex = 0;
            //
            // ExitButton
            //
            this.ExitButton.BackColor = System.Drawing.Color.Transparent;
            this.ExitButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.ExitButton.FlatAppearance.BorderSize = 0;
            this.ExitButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ExitButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.ForeColor = System.Drawing.Color.Transparent;
            this.ExitButton.Image = global::Cube.Pdf.App.Picker.Properties.Resources.CloseButton;
            this.ExitButton.Location = new System.Drawing.Point(84, 0);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(16, 16);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.UseVisualStyleBackColor = false;
            //
            // DropForm
            //
            this.AllowDrop = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(100, 100);
            this.Controls.Add(this.DropPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1160);
            this.Name = "DropForm";
            this.ShowInTaskbar = false;
            this.Sizable = false;
            this.SystemMenu = false;
            this.Text = "CubePDF ImagePicker";
            this.TopMost = true;
            this.DropPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Cube.Forms.Panel DropPanel;
        private Cube.Forms.Button ExitButton;
    }
}

