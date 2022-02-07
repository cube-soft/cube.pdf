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
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MainVersionControl = new Cube.Forms.Controls.VersionControl();
            this.VersionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.UpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExecButton = new System.Windows.Forms.Button();
            this.LanguageComboBox = new System.Windows.Forms.ComboBox();
            this.LogoBox = new System.Windows.Forms.PictureBox();
            this.RootPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VersionBindingSource)).BeginInit();
            this.FooterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoBox)).BeginInit();
            this.SuspendLayout();
            //
            // RootPanel
            //
            resources.ApplyResources(this.RootPanel, "RootPanel");
            this.RootPanel.BackColor = System.Drawing.Color.White;
            this.RootPanel.Controls.Add(this.MainVersionControl, 1, 1);
            this.RootPanel.Controls.Add(this.UpdateCheckBox, 1, 2);
            this.RootPanel.Controls.Add(this.FooterPanel, 0, 5);
            this.RootPanel.Controls.Add(this.LanguageComboBox, 1, 3);
            this.RootPanel.Controls.Add(this.LogoBox, 1, 0);
            this.RootPanel.Name = "RootPanel";
            //
            // MainVersionControl
            //
            resources.ApplyResources(this.MainVersionControl, "MainVersionControl");
            this.MainVersionControl.Copyright = "Copyright © 2013 CubeSoft, Inc.";
            this.MainVersionControl.DataBindings.Add(new System.Windows.Forms.Binding("Version", this.VersionBindingSource, "Version", true));
            this.MainVersionControl.Description = "";
            this.MainVersionControl.Image = null;
            this.MainVersionControl.Name = "MainVersionControl";
            this.MainVersionControl.OneLine = true;
            this.MainVersionControl.Product = "CubePDF Page";
            this.MainVersionControl.Uri = null;
            this.MainVersionControl.Version = "7.0.0.0";
            //
            // VersionBindingSource
            //
            this.VersionBindingSource.DataSource = typeof(Cube.Pdf.Pages.VersionViewModel);
            //
            // UpdateCheckBox
            //
            resources.ApplyResources(this.UpdateCheckBox, "UpdateCheckBox");
            this.UpdateCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.VersionBindingSource, "CheckUpdate", true));
            this.UpdateCheckBox.Name = "UpdateCheckBox";
            this.UpdateCheckBox.UseVisualStyleBackColor = true;
            //
            // FooterPanel
            //
            resources.ApplyResources(this.FooterPanel, "FooterPanel");
            this.FooterPanel.BackColor = System.Drawing.SystemColors.Control;
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
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.RootPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VersionWindow";
            this.RootPanel.ResumeLayout(false);
            this.RootPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VersionBindingSource)).EndInit();
            this.FooterPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LogoBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private Cube.Forms.Controls.VersionControl MainVersionControl;
        private System.Windows.Forms.CheckBox UpdateCheckBox;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private System.Windows.Forms.Button ExecButton;
        private System.Windows.Forms.BindingSource VersionBindingSource;
        private System.Windows.Forms.ComboBox LanguageComboBox;
        private System.Windows.Forms.PictureBox LogoBox;
    }
}