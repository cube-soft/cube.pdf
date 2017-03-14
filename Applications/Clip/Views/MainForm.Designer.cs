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
            this.AttachButton = new Cube.Forms.Button();
            this.SourcePanel = new System.Windows.Forms.TableLayoutPanel();
            this.SourceLabel = new System.Windows.Forms.Label();
            this.SourceTextBox = new System.Windows.Forms.TextBox();
            this.SourceButton = new Cube.Forms.Button();
            this.AttachPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ToolsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AddButton = new Cube.Forms.Button();
            this.RemoveButton = new Cube.Forms.Button();
            this.ClearButton = new Cube.Forms.Button();
            this.AttachListView = new Cube.Pdf.App.Clip.ClipListView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HeaderButton = new System.Windows.Forms.PictureBox();
            this.LayoutPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.SourcePanel.SuspendLayout();
            this.AttachPanel.SuspendLayout();
            this.ToolsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AttachListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderButton)).BeginInit();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.ColumnCount = 1;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.Controls.Add(this.FooterPanel, 0, 3);
            this.LayoutPanel.Controls.Add(this.SourcePanel, 0, 1);
            this.LayoutPanel.Controls.Add(this.AttachPanel, 0, 2);
            this.LayoutPanel.Controls.Add(this.HeaderButton, 0, 0);
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
            this.FooterPanel.Controls.Add(this.AttachButton);
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
            // AttachButton
            // 
            this.AttachButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.AttachButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.AttachButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.AttachButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AttachButton.ForeColor = System.Drawing.Color.White;
            this.AttachButton.Location = new System.Drawing.Point(330, 13);
            this.AttachButton.Name = "AttachButton";
            this.AttachButton.Size = new System.Drawing.Size(135, 35);
            this.AttachButton.TabIndex = 3;
            this.AttachButton.Text = "添付";
            this.AttachButton.UseVisualStyleBackColor = false;
            // 
            // SourcePanel
            // 
            this.SourcePanel.ColumnCount = 3;
            this.SourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.SourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.SourcePanel.Controls.Add(this.SourceLabel, 0, 0);
            this.SourcePanel.Controls.Add(this.SourceTextBox, 1, 0);
            this.SourcePanel.Controls.Add(this.SourceButton, 2, 0);
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
            // SourceButton
            // 
            this.SourceButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SourceButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.SourceButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.SourceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SourceButton.ForeColor = System.Drawing.Color.White;
            this.SourceButton.Location = new System.Drawing.Point(496, 6);
            this.SourceButton.Margin = new System.Windows.Forms.Padding(0);
            this.SourceButton.Name = "SourceButton";
            this.SourceButton.Size = new System.Drawing.Size(70, 23);
            this.SourceButton.TabIndex = 2;
            this.SourceButton.Text = "...";
            this.SourceButton.UseVisualStyleBackColor = false;
            // 
            // AttachPanel
            // 
            this.AttachPanel.ColumnCount = 2;
            this.AttachPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AttachPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.AttachPanel.Controls.Add(this.ToolsPanel, 1, 0);
            this.AttachPanel.Controls.Add(this.AttachListView, 0, 0);
            this.AttachPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttachPanel.Location = new System.Drawing.Point(0, 70);
            this.AttachPanel.Margin = new System.Windows.Forms.Padding(0);
            this.AttachPanel.Name = "AttachPanel";
            this.AttachPanel.RowCount = 1;
            this.AttachPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AttachPanel.Size = new System.Drawing.Size(584, 151);
            this.AttachPanel.TabIndex = 6;
            // 
            // ToolsPanel
            // 
            this.ToolsPanel.AllowDrop = true;
            this.ToolsPanel.Controls.Add(this.AddButton);
            this.ToolsPanel.Controls.Add(this.RemoveButton);
            this.ToolsPanel.Controls.Add(this.ClearButton);
            this.ToolsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ToolsPanel.Location = new System.Drawing.Point(454, 0);
            this.ToolsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ToolsPanel.Name = "ToolsPanel";
            this.ToolsPanel.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.ToolsPanel.Size = new System.Drawing.Size(130, 151);
            this.ToolsPanel.TabIndex = 3;
            // 
            // AddButton
            // 
            this.AddButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.AddButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.AddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddButton.ForeColor = System.Drawing.Color.White;
            this.AddButton.Location = new System.Drawing.Point(12, 0);
            this.AddButton.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(100, 30);
            this.AddButton.TabIndex = 0;
            this.AddButton.Text = "追加 ...";
            this.AddButton.UseVisualStyleBackColor = false;
            // 
            // RemoveButton
            // 
            this.RemoveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.RemoveButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.RemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveButton.ForeColor = System.Drawing.Color.White;
            this.RemoveButton.Location = new System.Drawing.Point(12, 34);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(2);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(100, 30);
            this.RemoveButton.TabIndex = 3;
            this.RemoveButton.Text = "削除";
            this.RemoveButton.UseVisualStyleBackColor = false;
            // 
            // ClearButton
            // 
            this.ClearButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClearButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.ClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearButton.ForeColor = System.Drawing.Color.White;
            this.ClearButton.Location = new System.Drawing.Point(12, 68);
            this.ClearButton.Margin = new System.Windows.Forms.Padding(2);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(100, 30);
            this.ClearButton.TabIndex = 4;
            this.ClearButton.Text = "すべて削除";
            this.ClearButton.UseVisualStyleBackColor = false;
            // 
            // AttachListView
            // 
            this.AttachListView.AllowUserToAddRows = false;
            this.AttachListView.AllowUserToDeleteRows = false;
            this.AttachListView.AllowUserToResizeRows = false;
            this.AttachListView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.AttachListView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.AttachListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AttachListView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.AttachListView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.AttachListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AttachListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.AttachListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttachListView.GridColor = System.Drawing.SystemColors.Control;
            this.AttachListView.Location = new System.Drawing.Point(0, 0);
            this.AttachListView.Margin = new System.Windows.Forms.Padding(0);
            this.AttachListView.Name = "AttachListView";
            this.AttachListView.ReadOnly = true;
            this.AttachListView.RowHeadersVisible = false;
            this.AttachListView.RowTemplate.Height = 21;
            this.AttachListView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AttachListView.Size = new System.Drawing.Size(454, 151);
            this.AttachListView.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.FillWeight = 250F;
            this.dataGridViewTextBoxColumn1.HeaderText = "ファイル名";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Condition";
            this.dataGridViewTextBoxColumn2.FillWeight = 43.75F;
            this.dataGridViewTextBoxColumn2.HeaderText = "状態";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 6.25F;
            this.dataGridViewTextBoxColumn3.HeaderText = "";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // HeaderButton
            // 
            this.HeaderButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.HeaderButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderButton.Image = global::Cube.Pdf.App.Clip.Properties.Resources.HeaderImage;
            this.HeaderButton.Location = new System.Drawing.Point(0, 0);
            this.HeaderButton.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderButton.Name = "HeaderButton";
            this.HeaderButton.Size = new System.Drawing.Size(584, 35);
            this.HeaderButton.TabIndex = 8;
            this.HeaderButton.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(584, 281);
            this.Controls.Add(this.LayoutPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimumSize = new System.Drawing.Size(510, 280);
            this.Name = "MainForm";
            this.Text = "CubePDF Clip";
            this.LayoutPanel.ResumeLayout(false);
            this.FooterPanel.ResumeLayout(false);
            this.SourcePanel.ResumeLayout(false);
            this.SourcePanel.PerformLayout();
            this.AttachPanel.ResumeLayout(false);
            this.ToolsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AttachListView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private System.Windows.Forms.TableLayoutPanel SourcePanel;
        private System.Windows.Forms.Label SourceLabel;
        private System.Windows.Forms.TextBox SourceTextBox;
        private System.Windows.Forms.TableLayoutPanel AttachPanel;
        private ClipListView AttachListView;
        private System.Windows.Forms.FlowLayoutPanel ToolsPanel;
        private Forms.Button AddButton;
        private Forms.Button RemoveButton;
        private Forms.Button ClearButton;
        private System.Windows.Forms.FlowLayoutPanel FooterPanel;
        private Forms.Button ExitButton;
        private Forms.Button AttachButton;
        private Forms.Button SourceButton;
        private System.Windows.Forms.PictureBox HeaderButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}

