namespace Cube.Pdf.App.Clip
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
            this.LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FooterPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ExitButton = new Cube.Forms.Button();
            this.SaveButton = new Cube.Forms.Button();
            this.SourcePanel = new System.Windows.Forms.TableLayoutPanel();
            this.SourceLabel = new System.Windows.Forms.Label();
            this.SourceTextBox = new System.Windows.Forms.TextBox();
            this.OpenButton = new Cube.Forms.Button();
            this.ClipPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ToolsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AttachButton = new Cube.Forms.Button();
            this.DetachButton = new Cube.Forms.Button();
            this.ResetButton = new Cube.Forms.Button();
            this.MyClipDataView = new Cube.Pdf.App.Clip.ClipControl();
            this.VersionButton = new System.Windows.Forms.PictureBox();
            this.LayoutPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.SourcePanel.SuspendLayout();
            this.ClipPanel.SuspendLayout();
            this.ToolsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MyClipDataView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VersionButton)).BeginInit();
            this.SuspendLayout();
            //
            // LayoutPanel
            //
            this.LayoutPanel.ColumnCount = 1;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.Controls.Add(this.FooterPanel, 0, 3);
            this.LayoutPanel.Controls.Add(this.SourcePanel, 0, 1);
            this.LayoutPanel.Controls.Add(this.ClipPanel, 0, 2);
            this.LayoutPanel.Controls.Add(this.VersionButton, 0, 0);
            this.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.RowCount = 4;
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.LayoutPanel.Size = new System.Drawing.Size(584, 281);
            this.LayoutPanel.TabIndex = 0;
            //
            // FooterPanel
            //
            this.FooterPanel.AllowDrop = true;
            this.FooterPanel.Controls.Add(this.ExitButton);
            this.FooterPanel.Controls.Add(this.SaveButton);
            this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FooterPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.FooterPanel.Location = new System.Drawing.Point(0, 221);
            this.FooterPanel.Margin = new System.Windows.Forms.Padding(0);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.Padding = new System.Windows.Forms.Padding(0, 10, 10, 0);
            this.FooterPanel.Size = new System.Drawing.Size(584, 60);
            this.FooterPanel.TabIndex = 7;
            //
            // ExitButton
            //
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.ExitButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.ExitButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.ForeColor = System.Drawing.Color.White;
            this.ExitButton.Location = new System.Drawing.Point(471, 13);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 35);
            this.ExitButton.TabIndex = 2;
            this.ExitButton.Text = "終了";
            this.ExitButton.UseVisualStyleBackColor = false;
            //
            // SaveButton
            //
            this.SaveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.SaveButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.SaveButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.ForeColor = System.Drawing.Color.White;
            this.SaveButton.Location = new System.Drawing.Point(330, 13);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(135, 35);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "保存 (&S)";
            this.SaveButton.UseVisualStyleBackColor = false;
            //
            // SourcePanel
            //
            this.SourcePanel.ColumnCount = 3;
            this.SourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.SourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.SourcePanel.Controls.Add(this.SourceLabel, 0, 0);
            this.SourcePanel.Controls.Add(this.SourceTextBox, 1, 0);
            this.SourcePanel.Controls.Add(this.OpenButton, 2, 0);
            this.SourcePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourcePanel.Location = new System.Drawing.Point(0, 35);
            this.SourcePanel.Margin = new System.Windows.Forms.Padding(0);
            this.SourcePanel.Name = "SourcePanel";
            this.SourcePanel.RowCount = 1;
            this.SourcePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SourcePanel.Size = new System.Drawing.Size(584, 35);
            this.SourcePanel.TabIndex = 5;
            //
            // SourceLabel
            //
            this.SourceLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SourceLabel.AutoSize = true;
            this.SourceLabel.Location = new System.Drawing.Point(6, 10);
            this.SourceLabel.Name = "SourceLabel";
            this.SourceLabel.Size = new System.Drawing.Size(68, 15);
            this.SourceLabel.TabIndex = 0;
            this.SourceLabel.Text = "PDF ファイル";
            //
            // SourceTextBox
            //
            this.SourceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceTextBox.Location = new System.Drawing.Point(86, 6);
            this.SourceTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.SourceTextBox.Name = "SourceTextBox";
            this.SourceTextBox.Size = new System.Drawing.Size(404, 23);
            this.SourceTextBox.TabIndex = 1;
            //
            // OpenButton
            //
            this.OpenButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.OpenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.OpenButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.OpenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenButton.ForeColor = System.Drawing.Color.White;
            this.OpenButton.Location = new System.Drawing.Point(496, 6);
            this.OpenButton.Margin = new System.Windows.Forms.Padding(0);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(70, 23);
            this.OpenButton.TabIndex = 2;
            this.OpenButton.Text = "...";
            this.OpenButton.UseVisualStyleBackColor = false;
            //
            // ClipPanel
            //
            this.ClipPanel.ColumnCount = 2;
            this.ClipPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ClipPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.ClipPanel.Controls.Add(this.ToolsPanel, 1, 0);
            this.ClipPanel.Controls.Add(this.MyClipDataView, 0, 0);
            this.ClipPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClipPanel.Location = new System.Drawing.Point(0, 70);
            this.ClipPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ClipPanel.Name = "ClipPanel";
            this.ClipPanel.RowCount = 1;
            this.ClipPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ClipPanel.Size = new System.Drawing.Size(584, 151);
            this.ClipPanel.TabIndex = 6;
            //
            // ToolsPanel
            //
            this.ToolsPanel.AllowDrop = true;
            this.ToolsPanel.Controls.Add(this.AttachButton);
            this.ToolsPanel.Controls.Add(this.DetachButton);
            this.ToolsPanel.Controls.Add(this.ResetButton);
            this.ToolsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ToolsPanel.Location = new System.Drawing.Point(454, 0);
            this.ToolsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ToolsPanel.Name = "ToolsPanel";
            this.ToolsPanel.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.ToolsPanel.Size = new System.Drawing.Size(130, 151);
            this.ToolsPanel.TabIndex = 3;
            //
            // AttachButton
            //
            this.AttachButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.AttachButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.AttachButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AttachButton.ForeColor = System.Drawing.Color.White;
            this.AttachButton.Location = new System.Drawing.Point(12, 0);
            this.AttachButton.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.AttachButton.Name = "AttachButton";
            this.AttachButton.Size = new System.Drawing.Size(100, 30);
            this.AttachButton.TabIndex = 0;
            this.AttachButton.Text = "追加 (&N) ...";
            this.AttachButton.UseVisualStyleBackColor = false;
            //
            // DetachButton
            //
            this.DetachButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.DetachButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DetachButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DetachButton.ForeColor = System.Drawing.Color.White;
            this.DetachButton.Location = new System.Drawing.Point(12, 34);
            this.DetachButton.Margin = new System.Windows.Forms.Padding(2);
            this.DetachButton.Name = "DetachButton";
            this.DetachButton.Size = new System.Drawing.Size(100, 30);
            this.DetachButton.TabIndex = 3;
            this.DetachButton.Text = "削除 (&D)";
            this.DetachButton.UseVisualStyleBackColor = false;
            //
            // ResetButton
            //
            this.ResetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ResetButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.ResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetButton.ForeColor = System.Drawing.Color.White;
            this.ResetButton.Location = new System.Drawing.Point(12, 68);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(2);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(100, 30);
            this.ResetButton.TabIndex = 4;
            this.ResetButton.Text = "リセット (&R)";
            this.ResetButton.UseVisualStyleBackColor = false;
            //
            // MyClipDataView
            //
            this.MyClipDataView.AllowUserToAddRows = false;
            this.MyClipDataView.AllowUserToDeleteRows = false;
            this.MyClipDataView.AllowUserToResizeRows = false;
            this.MyClipDataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MyClipDataView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.MyClipDataView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MyClipDataView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MyClipDataView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.MyClipDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.MyClipDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MyClipDataView.GridColor = System.Drawing.SystemColors.Control;
            this.MyClipDataView.Location = new System.Drawing.Point(3, 3);
            this.MyClipDataView.Name = "MyClipDataView";
            this.MyClipDataView.ReadOnly = true;
            this.MyClipDataView.RowHeadersVisible = false;
            this.MyClipDataView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MyClipDataView.Size = new System.Drawing.Size(448, 145);
            this.MyClipDataView.TabIndex = 4;
            //
            // VersionButton
            //
            this.VersionButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.VersionButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VersionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VersionButton.Image = global::Cube.Pdf.App.Clip.Properties.Resources.HeaderImage;
            this.VersionButton.Location = new System.Drawing.Point(0, 0);
            this.VersionButton.Margin = new System.Windows.Forms.Padding(0);
            this.VersionButton.Name = "VersionButton";
            this.VersionButton.Size = new System.Drawing.Size(584, 35);
            this.VersionButton.TabIndex = 8;
            this.VersionButton.TabStop = false;
            //
            // MainForm
            //
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(584, 281);
            this.Controls.Add(this.LayoutPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(510, 280);
            this.Name = "MainForm";
            this.Text = "CubePDF Clip";
            this.LayoutPanel.ResumeLayout(false);
            this.FooterPanel.ResumeLayout(false);
            this.SourcePanel.ResumeLayout(false);
            this.SourcePanel.PerformLayout();
            this.ClipPanel.ResumeLayout(false);
            this.ToolsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MyClipDataView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VersionButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private System.Windows.Forms.TableLayoutPanel SourcePanel;
        private System.Windows.Forms.Label SourceLabel;
        private System.Windows.Forms.TextBox SourceTextBox;
        private System.Windows.Forms.TableLayoutPanel ClipPanel;
        private ClipControl MyClipDataView;
        private System.Windows.Forms.FlowLayoutPanel ToolsPanel;
        private Forms.Button AttachButton;
        private Forms.Button DetachButton;
        private Forms.Button ResetButton;
        private System.Windows.Forms.FlowLayoutPanel FooterPanel;
        private Forms.Button ExitButton;
        private Forms.Button SaveButton;
        private Forms.Button OpenButton;
        private System.Windows.Forms.PictureBox VersionButton;
    }
}

