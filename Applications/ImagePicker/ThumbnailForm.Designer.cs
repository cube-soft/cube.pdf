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
            this.LayoutContainer = new System.Windows.Forms.SplitContainer();
            this.HeaderContainer = new System.Windows.Forms.SplitContainer();
            this.HeaderView = new Cube.Pdf.ImageEx.HeaderView();
            this.ListView = new System.Windows.Forms.ListView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ExitButton = new NoFocusCueButton();
            this.SaveAllButton = new NoFocusCueButton();
            this.SaveButton = new NoFocusCueButton();
            this.LayoutContainer.Panel1.SuspendLayout();
            this.LayoutContainer.Panel2.SuspendLayout();
            this.LayoutContainer.SuspendLayout();
            this.HeaderContainer.Panel1.SuspendLayout();
            this.HeaderContainer.Panel2.SuspendLayout();
            this.HeaderContainer.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LayoutContainer
            // 
            this.LayoutContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.LayoutContainer.IsSplitterFixed = true;
            this.LayoutContainer.Location = new System.Drawing.Point(0, 0);
            this.LayoutContainer.Name = "LayoutContainer";
            this.LayoutContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // LayoutContainer.Panel1
            // 
            this.LayoutContainer.Panel1.Controls.Add(this.HeaderContainer);
            // 
            // LayoutContainer.Panel2
            // 
            this.LayoutContainer.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.LayoutContainer.Panel2MinSize = 60;
            this.LayoutContainer.Size = new System.Drawing.Size(634, 361);
            this.LayoutContainer.SplitterDistance = 300;
            this.LayoutContainer.SplitterWidth = 1;
            this.LayoutContainer.TabIndex = 0;
            // 
            // HeaderContainer
            // 
            this.HeaderContainer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.HeaderContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.HeaderContainer.IsSplitterFixed = true;
            this.HeaderContainer.Location = new System.Drawing.Point(0, 0);
            this.HeaderContainer.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderContainer.Name = "HeaderContainer";
            this.HeaderContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // HeaderContainer.Panel1
            // 
            this.HeaderContainer.Panel1.Controls.Add(this.HeaderView);
            this.HeaderContainer.Panel1MinSize = 35;
            // 
            // HeaderContainer.Panel2
            // 
            this.HeaderContainer.Panel2.Controls.Add(this.ListView);
            this.HeaderContainer.Size = new System.Drawing.Size(634, 300);
            this.HeaderContainer.SplitterDistance = 35;
            this.HeaderContainer.SplitterWidth = 1;
            this.HeaderContainer.TabIndex = 0;
            // 
            // HeaderView
            // 
            this.HeaderView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.HeaderView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderView.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.HeaderView.Location = new System.Drawing.Point(0, 0);
            this.HeaderView.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderView.Name = "HeaderView";
            this.HeaderView.Size = new System.Drawing.Size(634, 35);
            this.HeaderView.TabIndex = 0;
            // 
            // ListView
            // 
            this.ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListView.Location = new System.Drawing.Point(0, 0);
            this.ListView.Name = "ListView";
            this.ListView.Size = new System.Drawing.Size(634, 264);
            this.ListView.TabIndex = 0;
            this.ListView.UseCompatibleStateImageBehavior = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ExitButton);
            this.flowLayoutPanel1.Controls.Add(this.SaveAllButton);
            this.flowLayoutPanel1.Controls.Add(this.SaveButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(8, 10, 0, 10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(634, 60);
            this.flowLayoutPanel1.TabIndex = 0;
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
            // ThumbnailForm
            // 
            this.ClientSize = new System.Drawing.Size(634, 361);
            this.Controls.Add(this.LayoutContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ThumbnailForm";
            this.Text = "Thumbnail List";
            this.LayoutContainer.Panel1.ResumeLayout(false);
            this.LayoutContainer.Panel2.ResumeLayout(false);
            this.LayoutContainer.ResumeLayout(false);
            this.HeaderContainer.Panel1.ResumeLayout(false);
            this.HeaderContainer.Panel2.ResumeLayout(false);
            this.HeaderContainer.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer LayoutContainer;
        private System.Windows.Forms.SplitContainer HeaderContainer;
        private HeaderView HeaderView;
        private System.Windows.Forms.ListView ListView;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private NoFocusCueButton SaveAllButton;
        private NoFocusCueButton ExitButton;
        private NoFocusCueButton SaveButton;
    }
}