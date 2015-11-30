namespace Cube.Pdf.Page
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
            this.SplitButton = new System.Windows.Forms.Button();
            this.MergeButton = new System.Windows.Forms.Button();
            this.ContentsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.FileButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.RemoveAllButton = new System.Windows.Forms.Button();
            this.PageListView = new System.Windows.Forms.ListView();
            this.FileColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PageColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SizeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LayoutPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.ContentsPanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.ColumnCount = 1;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.Controls.Add(this.FooterPanel, 0, 1);
            this.LayoutPanel.Controls.Add(this.ContentsPanel, 0, 0);
            this.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.RowCount = 2;
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.LayoutPanel.Size = new System.Drawing.Size(684, 311);
            this.LayoutPanel.TabIndex = 0;
            // 
            // FooterPanel
            // 
            this.FooterPanel.AllowDrop = true;
            this.FooterPanel.Controls.Add(this.SplitButton);
            this.FooterPanel.Controls.Add(this.MergeButton);
            this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FooterPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.FooterPanel.Location = new System.Drawing.Point(0, 251);
            this.FooterPanel.Margin = new System.Windows.Forms.Padding(0);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.Padding = new System.Windows.Forms.Padding(0, 10, 10, 0);
            this.FooterPanel.Size = new System.Drawing.Size(684, 60);
            this.FooterPanel.TabIndex = 1;
            // 
            // SplitButton
            // 
            this.SplitButton.Location = new System.Drawing.Point(542, 12);
            this.SplitButton.Margin = new System.Windows.Forms.Padding(2);
            this.SplitButton.Name = "SplitButton";
            this.SplitButton.Size = new System.Drawing.Size(130, 35);
            this.SplitButton.TabIndex = 0;
            this.SplitButton.Text = "分割";
            this.SplitButton.UseVisualStyleBackColor = true;
            // 
            // MergeButton
            // 
            this.MergeButton.Location = new System.Drawing.Point(408, 12);
            this.MergeButton.Margin = new System.Windows.Forms.Padding(2);
            this.MergeButton.Name = "MergeButton";
            this.MergeButton.Size = new System.Drawing.Size(130, 35);
            this.MergeButton.TabIndex = 1;
            this.MergeButton.Text = "結合";
            this.MergeButton.UseVisualStyleBackColor = true;
            // 
            // ContentsPanel
            // 
            this.ContentsPanel.ColumnCount = 2;
            this.ContentsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ContentsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.ContentsPanel.Controls.Add(this.ButtonsPanel, 0, 0);
            this.ContentsPanel.Controls.Add(this.PageListView, 0, 0);
            this.ContentsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentsPanel.Location = new System.Drawing.Point(0, 0);
            this.ContentsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ContentsPanel.Name = "ContentsPanel";
            this.ContentsPanel.RowCount = 1;
            this.ContentsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ContentsPanel.Size = new System.Drawing.Size(684, 251);
            this.ContentsPanel.TabIndex = 2;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.AllowDrop = true;
            this.ButtonsPanel.Controls.Add(this.FileButton);
            this.ButtonsPanel.Controls.Add(this.UpButton);
            this.ButtonsPanel.Controls.Add(this.DownButton);
            this.ButtonsPanel.Controls.Add(this.RemoveButton);
            this.ButtonsPanel.Controls.Add(this.RemoveAllButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ButtonsPanel.Location = new System.Drawing.Point(560, 0);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Padding = new System.Windows.Forms.Padding(10, 2, 10, 0);
            this.ButtonsPanel.Size = new System.Drawing.Size(124, 251);
            this.ButtonsPanel.TabIndex = 2;
            // 
            // FileButton
            // 
            this.FileButton.Location = new System.Drawing.Point(12, 4);
            this.FileButton.Margin = new System.Windows.Forms.Padding(2);
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(100, 30);
            this.FileButton.TabIndex = 0;
            this.FileButton.Text = "追加...";
            this.FileButton.UseVisualStyleBackColor = true;
            // 
            // UpButton
            // 
            this.UpButton.Location = new System.Drawing.Point(12, 38);
            this.UpButton.Margin = new System.Windows.Forms.Padding(2);
            this.UpButton.Name = "UpButton";
            this.UpButton.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.UpButton.Size = new System.Drawing.Size(100, 30);
            this.UpButton.TabIndex = 1;
            this.UpButton.Text = "上へ";
            this.UpButton.UseVisualStyleBackColor = true;
            // 
            // DownButton
            // 
            this.DownButton.Location = new System.Drawing.Point(12, 72);
            this.DownButton.Margin = new System.Windows.Forms.Padding(2);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(100, 30);
            this.DownButton.TabIndex = 2;
            this.DownButton.Text = "下へ";
            this.DownButton.UseVisualStyleBackColor = true;
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(12, 106);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(2);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(100, 30);
            this.RemoveButton.TabIndex = 3;
            this.RemoveButton.Text = "削除";
            this.RemoveButton.UseVisualStyleBackColor = true;
            // 
            // RemoveAllButton
            // 
            this.RemoveAllButton.Location = new System.Drawing.Point(12, 140);
            this.RemoveAllButton.Margin = new System.Windows.Forms.Padding(2);
            this.RemoveAllButton.Name = "RemoveAllButton";
            this.RemoveAllButton.Size = new System.Drawing.Size(100, 30);
            this.RemoveAllButton.TabIndex = 4;
            this.RemoveAllButton.Text = "すべて削除";
            this.RemoveAllButton.UseVisualStyleBackColor = true;
            // 
            // PageListView
            // 
            this.PageListView.AllowDrop = true;
            this.PageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileColumnHeader,
            this.PageColumnHeader,
            this.SizeColumnHeader,
            this.DateColumnHeader});
            this.PageListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PageListView.FullRowSelect = true;
            this.PageListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.PageListView.Location = new System.Drawing.Point(0, 0);
            this.PageListView.Margin = new System.Windows.Forms.Padding(0);
            this.PageListView.Name = "PageListView";
            this.PageListView.ShowItemToolTips = true;
            this.PageListView.Size = new System.Drawing.Size(560, 251);
            this.PageListView.TabIndex = 3;
            this.PageListView.UseCompatibleStateImageBehavior = false;
            this.PageListView.View = System.Windows.Forms.View.Details;
            // 
            // FileColumnHeader
            // 
            this.FileColumnHeader.Text = "ファイル名";
            this.FileColumnHeader.Width = 200;
            // 
            // PageColumnHeader
            // 
            this.PageColumnHeader.Text = "ページ数";
            this.PageColumnHeader.Width = 120;
            // 
            // SizeColumnHeader
            // 
            this.SizeColumnHeader.Text = "サイズ";
            this.SizeColumnHeader.Width = 120;
            // 
            // DateColumnHeader
            // 
            this.DateColumnHeader.Text = "更新日時";
            this.DateColumnHeader.Width = 120;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(684, 311);
            this.Controls.Add(this.LayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 280);
            this.Name = "MainForm";
            this.Text = "CubePDF Page";
            this.LayoutPanel.ResumeLayout(false);
            this.FooterPanel.ResumeLayout(false);
            this.ContentsPanel.ResumeLayout(false);
            this.ButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel FooterPanel;
        private System.Windows.Forms.Button SplitButton;
        private System.Windows.Forms.Button MergeButton;
        private System.Windows.Forms.TableLayoutPanel ContentsPanel;
        private System.Windows.Forms.FlowLayoutPanel ButtonsPanel;
        private System.Windows.Forms.Button FileButton;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button RemoveAllButton;
        private System.Windows.Forms.ListView PageListView;
        private System.Windows.Forms.ColumnHeader FileColumnHeader;
        private System.Windows.Forms.ColumnHeader PageColumnHeader;
        private System.Windows.Forms.ColumnHeader SizeColumnHeader;
        private System.Windows.Forms.ColumnHeader DateColumnHeader;
    }
}

