namespace Cube.Pdf.App.ImageEx
{
    partial class HeaderView
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.ImagePictureBox = new System.Windows.Forms.PictureBox();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ImagePictureBox
            // 
            this.ImagePictureBox.BackgroundImage = global::Cube.Pdf.App.ImageEx.Properties.Resources.HeaderImage;
            this.ImagePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ImagePictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.ImagePictureBox.Location = new System.Drawing.Point(217, 0);
            this.ImagePictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.ImagePictureBox.Name = "ImagePictureBox";
            this.ImagePictureBox.Size = new System.Drawing.Size(217, 35);
            this.ImagePictureBox.TabIndex = 3;
            this.ImagePictureBox.TabStop = false;
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.LogoPictureBox.Image = global::Cube.Pdf.App.ImageEx.Properties.Resources.HeaderLogo;
            this.LogoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(190, 35);
            this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.LogoPictureBox.TabIndex = 2;
            this.LogoPictureBox.TabStop = false;
            // 
            // HeaderView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.Controls.Add(this.ImagePictureBox);
            this.Controls.Add(this.LogoPictureBox);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "HeaderView";
            this.Size = new System.Drawing.Size(434, 35);
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ImagePictureBox;
        private System.Windows.Forms.PictureBox LogoPictureBox;
    }
}
