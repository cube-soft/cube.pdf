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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionWindow));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.VersionPanel = new Cube.Forms.Controls.VersionControl();
            this.UpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExecButton = new System.Windows.Forms.Button();
            this.LanguageComboBox = new System.Windows.Forms.ComboBox();
            this.LogoBox = new System.Windows.Forms.PictureBox();
            this.RootPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoBox)).BeginInit();
            this.SuspendLayout();
            //
            // RootPanel
            //
            this.RootPanel.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.RootPanel, "RootPanel");
            this.RootPanel.Controls.Add(this.VersionPanel, 1, 1);
            this.RootPanel.Controls.Add(this.UpdateCheckBox, 1, 2);
            this.RootPanel.Controls.Add(this.FooterPanel, 0, 5);
            this.RootPanel.Controls.Add(this.LanguageComboBox, 1, 3);
            this.RootPanel.Controls.Add(this.LogoBox, 1, 0);
            this.RootPanel.Name = "RootPanel";
            //
            // VersionPanel
            //
            this.VersionPanel.Copyright = "Copyright © 2013 CubeSoft, Inc.";
            this.VersionPanel.Description = "";
            resources.ApplyResources(this.VersionPanel, "VersionPanel");
            this.VersionPanel.Image = null;
            this.VersionPanel.Name = "VersionPanel";
            this.VersionPanel.OneLine = true;
            this.VersionPanel.Product = "CubePDF Page";
            this.VersionPanel.Uri = null;
            this.VersionPanel.Version = "7.0.0.0";
            //
            // UpdateCheckBox
            //
            resources.ApplyResources(this.UpdateCheckBox, "UpdateCheckBox");
            this.UpdateCheckBox.Name = "UpdateCheckBox";
            this.UpdateCheckBox.UseVisualStyleBackColor = true;
            //
            // FooterPanel
            //
            this.FooterPanel.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.FooterPanel, "FooterPanel");
            this.RootPanel.SetColumnSpan(this.FooterPanel, 3);
            this.FooterPanel.Controls.Add(this.ExecButton, 0, 0);
            this.FooterPanel.Name = "FooterPanel";
            //
            // ExecButton
            //
            resources.ApplyResources(this.ExecButton, "ExecButton");
            this.ExecButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.ExecButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.ExecButton.ForeColor = System.Drawing.Color.White;
            this.ExecButton.Name = "ExecButton";
            this.ExecButton.UseVisualStyleBackColor = false;
            //
            // LanguageComboBox
            //
            resources.ApplyResources(this.LanguageComboBox, "LanguageComboBox");
            this.LanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguageComboBox.FormattingEnabled = true;
            this.LanguageComboBox.Name = "LanguageComboBox";
            //
            // LogoBox
            //
            resources.ApplyResources(this.LogoBox, "LogoBox");
            this.LogoBox.Image = global::Cube.Pdf.Pages.Properties.Resources.Logo;
            this.LogoBox.Name = "LogoBox";
            this.LogoBox.TabStop = false;
            //
            // VersionWindow
            //
            this.AcceptButton = this.ExecButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.RootPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VersionWindow";
            this.RootPanel.ResumeLayout(false);
            this.RootPanel.PerformLayout();
            this.FooterPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LogoBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private Cube.Forms.Controls.VersionControl VersionPanel;
        private System.Windows.Forms.CheckBox UpdateCheckBox;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private System.Windows.Forms.Button ExecButton;
        private System.Windows.Forms.ComboBox LanguageComboBox;
        private System.Windows.Forms.PictureBox LogoBox;
    }
}