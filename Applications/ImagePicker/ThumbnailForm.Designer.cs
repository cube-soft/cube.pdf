namespace Cube.Pdf.ImageEx
{
    partial class ThumbnailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThumbnailForm));
            this.LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ImageListView = new System.Windows.Forms.ListView();
            this.FooterPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ExitButton = new Cube.Forms.Button();
            this.SaveAllButton = new Cube.Forms.Button();
            this.SaveButton = new Cube.Forms.Button();
            this.HeaderPanel = new Cube.Pdf.ImageEx.HeaderView();
            this.LayoutPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.ColumnCount = 1;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.Controls.Add(this.ImageListView, 0, 1);
            this.LayoutPanel.Controls.Add(this.FooterPanel, 0, 2);
            this.LayoutPanel.Controls.Add(this.HeaderPanel, 0, 0);
            this.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.RowCount = 3;
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.LayoutPanel.Size = new System.Drawing.Size(634, 361);
            this.LayoutPanel.TabIndex = 0;
            // 
            // ImageListView
            // 
            this.ImageListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ImageListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageListView.Location = new System.Drawing.Point(0, 35);
            this.ImageListView.Margin = new System.Windows.Forms.Padding(0);
            this.ImageListView.Name = "ImageListView";
            this.ImageListView.Size = new System.Drawing.Size(634, 266);
            this.ImageListView.TabIndex = 5;
            this.ImageListView.UseCompatibleStateImageBehavior = false;
            // 
            // FooterPanel
            // 
            this.FooterPanel.Controls.Add(this.ExitButton);
            this.FooterPanel.Controls.Add(this.SaveAllButton);
            this.FooterPanel.Controls.Add(this.SaveButton);
            this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FooterPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.FooterPanel.Location = new System.Drawing.Point(0, 301);
            this.FooterPanel.Margin = new System.Windows.Forms.Padding(0);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.Padding = new System.Windows.Forms.Padding(8, 10, 0, 10);
            this.FooterPanel.Size = new System.Drawing.Size(634, 60);
            this.FooterPanel.TabIndex = 2;
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.ExitButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.ForeColor = System.Drawing.Color.White;
            this.ExitButton.Location = new System.Drawing.Point(523, 13);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 35);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = false;
            // 
            // SaveAllButton
            // 
            this.SaveAllButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.SaveAllButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.SaveAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveAllButton.ForeColor = System.Drawing.Color.White;
            this.SaveAllButton.Location = new System.Drawing.Point(387, 13);
            this.SaveAllButton.Name = "SaveAllButton";
            this.SaveAllButton.Size = new System.Drawing.Size(130, 35);
            this.SaveAllButton.TabIndex = 1;
            this.SaveAllButton.Text = "全て保存";
            this.SaveAllButton.UseVisualStyleBackColor = false;
            // 
            // SaveButton
            // 
            this.SaveButton.BackColor = System.Drawing.SystemColors.Highlight;
            this.SaveButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Highlight;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.ForeColor = System.Drawing.Color.White;
            this.SaveButton.Location = new System.Drawing.Point(251, 13);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(130, 35);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "選択画像の保存";
            this.SaveButton.UseVisualStyleBackColor = false;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPanel.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(634, 35);
            this.HeaderPanel.TabIndex = 1;
            // 
            // ThumbnailForm
            // 
            this.ClientSize = new System.Drawing.Size(634, 361);
            this.Controls.Add(this.LayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(450, 200);
            this.Name = "ThumbnailForm";
            this.Text = "Thumbnail List";
            this.LayoutPanel.ResumeLayout(false);
            this.FooterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private HeaderView HeaderPanel;
        private System.Windows.Forms.FlowLayoutPanel FooterPanel;
        private Cube.Forms.Button ExitButton;
        private Cube.Forms.Button SaveAllButton;
        private Cube.Forms.Button SaveButton;
        private System.Windows.Forms.ListView ImageListView;
    }
}