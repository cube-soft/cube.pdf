namespace Cube.Pdf.Pages
{
    partial class VersionWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionWindow));
            this.RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MainVersionControl = new Cube.Forms.Controls.VersionControl();
            this.UpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.DummyPanel = new System.Windows.Forms.Panel();
            this.ButtonsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExecButton = new System.Windows.Forms.Button();
            this.VersionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.RootLayoutPanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VersionBindingSource)).BeginInit();
            this.SuspendLayout();
            //
            // RootLayoutPanel
            //
            this.RootLayoutPanel.BackColor = System.Drawing.Color.White;
            this.RootLayoutPanel.ColumnCount = 1;
            this.RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootLayoutPanel.Controls.Add(this.MainVersionControl, 0, 0);
            this.RootLayoutPanel.Controls.Add(this.UpdateCheckBox, 0, 1);
            this.RootLayoutPanel.Controls.Add(this.DummyPanel, 0, 2);
            this.RootLayoutPanel.Controls.Add(this.ButtonsPanel, 0, 3);
            this.RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.RootLayoutPanel.Name = "RootLayoutPanel";
            this.RootLayoutPanel.RowCount = 4;
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.RootLayoutPanel.Size = new System.Drawing.Size(384, 241);
            this.RootLayoutPanel.TabIndex = 0;
            //
            // MainVersionControl
            //
            this.MainVersionControl.Copyright = "Copyright © 2013 CubeSoft, Inc.";
            this.MainVersionControl.DataBindings.Add(new System.Windows.Forms.Binding("Version", this.VersionBindingSource, "Version", true));
            this.MainVersionControl.Description = "";
            this.MainVersionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainVersionControl.Image = global::Cube.Pdf.Pages.Properties.Resources.Logo;
            this.MainVersionControl.Location = new System.Drawing.Point(20, 20);
            this.MainVersionControl.Margin = new System.Windows.Forms.Padding(20, 20, 3, 0);
            this.MainVersionControl.Name = "MainVersionControl";
            this.MainVersionControl.Product = "CubePDF Page";
            this.MainVersionControl.Size = new System.Drawing.Size(361, 110);
            this.MainVersionControl.TabIndex = 0;
            this.MainVersionControl.Uri = null;
            this.MainVersionControl.Version = "Version 5.0.0.0";
            //
            // UpdateCheckBox
            //
            this.UpdateCheckBox.AutoSize = true;
            this.UpdateCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.VersionBindingSource, "CheckUpdate", true));
            this.UpdateCheckBox.Location = new System.Drawing.Point(78, 133);
            this.UpdateCheckBox.Margin = new System.Windows.Forms.Padding(78, 3, 3, 3);
            this.UpdateCheckBox.Name = "UpdateCheckBox";
            this.UpdateCheckBox.Size = new System.Drawing.Size(176, 19);
            this.UpdateCheckBox.TabIndex = 1;
            this.UpdateCheckBox.Text = "起動時にアップデートを確認する";
            this.UpdateCheckBox.UseVisualStyleBackColor = true;
            //
            // DummyPanel
            //
            this.DummyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DummyPanel.Location = new System.Drawing.Point(3, 158);
            this.DummyPanel.Name = "DummyPanel";
            this.DummyPanel.Size = new System.Drawing.Size(378, 20);
            this.DummyPanel.TabIndex = 2;
            //
            // ButtonsPanel
            //
            this.ButtonsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ButtonsPanel.ColumnCount = 1;
            this.ButtonsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ButtonsPanel.Controls.Add(this.ExecButton, 0, 0);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 181);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.RowCount = 1;
            this.ButtonsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ButtonsPanel.Size = new System.Drawing.Size(384, 60);
            this.ButtonsPanel.TabIndex = 3;
            //
            // ExecButton
            //
            this.ExecButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ExecButton.Location = new System.Drawing.Point(129, 15);
            this.ExecButton.Name = "ExecButton";
            this.ExecButton.Size = new System.Drawing.Size(125, 30);
            this.ExecButton.TabIndex = 0;
            this.ExecButton.Text = "OK";
            this.ExecButton.UseVisualStyleBackColor = true;
            //
            // VersionBindingSource
            //
            this.VersionBindingSource.DataSource = typeof(Cube.Pdf.Pages.VersionViewModel);
            //
            // VersionWindow
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(384, 241);
            this.Controls.Add(this.RootLayoutPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VersionWindow";
            this.Text = "CubePDF Page について";
            this.RootLayoutPanel.ResumeLayout(false);
            this.RootLayoutPanel.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VersionBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        private Cube.Forms.Controls.VersionControl MainVersionControl;
        private System.Windows.Forms.CheckBox UpdateCheckBox;
        private System.Windows.Forms.Panel DummyPanel;
        private System.Windows.Forms.TableLayoutPanel ButtonsPanel;
        private System.Windows.Forms.Button ExecButton;
        private System.Windows.Forms.BindingSource VersionBindingSource;
    }
}