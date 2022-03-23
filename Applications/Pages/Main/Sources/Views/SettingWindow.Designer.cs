
namespace Cube.Pdf.Pages
{
    partial class SettingWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingWindow));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MainTabPanel = new System.Windows.Forms.TabControl();
            this.SettingTabPage = new System.Windows.Forms.TabPage();
            this.SettingPanel = new System.Windows.Forms.TableLayoutPanel();
            this.UpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.OtherLabel = new System.Windows.Forms.Label();
            this.TempTextBox = new System.Windows.Forms.TextBox();
            this.TempLabel = new System.Windows.Forms.Label();
            this.LanguageComboBox = new System.Windows.Forms.ComboBox();
            this.LanguageLabel = new System.Windows.Forms.Label();
            this.KeepOutlineCheckBox = new System.Windows.Forms.CheckBox();
            this.ShrinkResourceCheckBox = new System.Windows.Forms.CheckBox();
            this.OptionLabel = new System.Windows.Forms.Label();
            this.TempButton = new System.Windows.Forms.Button();
            this.VersionTabPage = new System.Windows.Forms.TabPage();
            this.VersionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.VersionControl = new Cube.Forms.Controls.VersionControl();
            this.FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ExecButton = new System.Windows.Forms.Button();
            this.RootPanel.SuspendLayout();
            this.MainTabPanel.SuspendLayout();
            this.SettingTabPage.SuspendLayout();
            this.SettingPanel.SuspendLayout();
            this.VersionTabPage.SuspendLayout();
            this.VersionPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootPanel
            //
            resources.ApplyResources(this.RootPanel, "RootPanel");
            this.RootPanel.Controls.Add(this.MainTabPanel, 1, 1);
            this.RootPanel.Controls.Add(this.FooterPanel, 0, 2);
            this.RootPanel.Name = "RootPanel";
            //
            // MainTabPanel
            //
            this.MainTabPanel.Controls.Add(this.SettingTabPage);
            this.MainTabPanel.Controls.Add(this.VersionTabPage);
            resources.ApplyResources(this.MainTabPanel, "MainTabPanel");
            this.MainTabPanel.Name = "MainTabPanel";
            this.MainTabPanel.SelectedIndex = 0;
            this.MainTabPanel.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            //
            // SettingTabPage
            //
            resources.ApplyResources(this.SettingTabPage, "SettingTabPage");
            this.SettingTabPage.Controls.Add(this.SettingPanel);
            this.SettingTabPage.Name = "SettingTabPage";
            this.SettingTabPage.UseVisualStyleBackColor = true;
            //
            // SettingPanel
            //
            resources.ApplyResources(this.SettingPanel, "SettingPanel");
            this.SettingPanel.Controls.Add(this.UpdateCheckBox, 2, 5);
            this.SettingPanel.Controls.Add(this.OtherLabel, 1, 5);
            this.SettingPanel.Controls.Add(this.TempTextBox, 2, 3);
            this.SettingPanel.Controls.Add(this.TempLabel, 1, 3);
            this.SettingPanel.Controls.Add(this.LanguageComboBox, 2, 4);
            this.SettingPanel.Controls.Add(this.LanguageLabel, 1, 4);
            this.SettingPanel.Controls.Add(this.KeepOutlineCheckBox, 2, 2);
            this.SettingPanel.Controls.Add(this.ShrinkResourceCheckBox, 2, 1);
            this.SettingPanel.Controls.Add(this.OptionLabel, 1, 1);
            this.SettingPanel.Controls.Add(this.TempButton, 3, 3);
            this.SettingPanel.Name = "SettingPanel";
            //
            // UpdateCheckBox
            //
            resources.ApplyResources(this.UpdateCheckBox, "UpdateCheckBox");
            this.SettingPanel.SetColumnSpan(this.UpdateCheckBox, 2);
            this.UpdateCheckBox.Name = "UpdateCheckBox";
            this.UpdateCheckBox.UseVisualStyleBackColor = true;
            //
            // OtherLabel
            //
            resources.ApplyResources(this.OtherLabel, "OtherLabel");
            this.OtherLabel.Name = "OtherLabel";
            //
            // TempTextBox
            //
            resources.ApplyResources(this.TempTextBox, "TempTextBox");
            this.TempTextBox.Name = "TempTextBox";
            //
            // TempLabel
            //
            resources.ApplyResources(this.TempLabel, "TempLabel");
            this.TempLabel.Name = "TempLabel";
            //
            // LanguageComboBox
            //
            this.SettingPanel.SetColumnSpan(this.LanguageComboBox, 2);
            resources.ApplyResources(this.LanguageComboBox, "LanguageComboBox");
            this.LanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguageComboBox.FormattingEnabled = true;
            this.LanguageComboBox.Name = "LanguageComboBox";
            //
            // LanguageLabel
            //
            resources.ApplyResources(this.LanguageLabel, "LanguageLabel");
            this.LanguageLabel.Name = "LanguageLabel";
            //
            // KeepOutlineCheckBox
            //
            resources.ApplyResources(this.KeepOutlineCheckBox, "KeepOutlineCheckBox");
            this.SettingPanel.SetColumnSpan(this.KeepOutlineCheckBox, 2);
            this.KeepOutlineCheckBox.Name = "KeepOutlineCheckBox";
            this.KeepOutlineCheckBox.UseVisualStyleBackColor = true;
            //
            // ShrinkResourceCheckBox
            //
            resources.ApplyResources(this.ShrinkResourceCheckBox, "ShrinkResourceCheckBox");
            this.SettingPanel.SetColumnSpan(this.ShrinkResourceCheckBox, 2);
            this.ShrinkResourceCheckBox.Name = "ShrinkResourceCheckBox";
            this.ShrinkResourceCheckBox.UseVisualStyleBackColor = true;
            //
            // OptionLabel
            //
            resources.ApplyResources(this.OptionLabel, "OptionLabel");
            this.OptionLabel.Name = "OptionLabel";
            //
            // TempButton
            //
            resources.ApplyResources(this.TempButton, "TempButton");
            this.TempButton.Name = "TempButton";
            this.TempButton.UseVisualStyleBackColor = true;
            //
            // VersionTabPage
            //
            resources.ApplyResources(this.VersionTabPage, "VersionTabPage");
            this.VersionTabPage.Controls.Add(this.VersionPanel);
            this.VersionTabPage.Name = "VersionTabPage";
            this.VersionTabPage.UseVisualStyleBackColor = true;
            //
            // VersionPanel
            //
            resources.ApplyResources(this.VersionPanel, "VersionPanel");
            this.VersionPanel.Controls.Add(this.VersionControl, 1, 1);
            this.VersionPanel.Name = "VersionPanel";
            //
            // VersionControl
            //
            this.VersionControl.Copyright = "Copyright © 2013 CubeSoft, Inc.";
            this.VersionControl.Description = "";
            resources.ApplyResources(this.VersionControl, "VersionControl");
            this.VersionControl.Image = global::Cube.Pdf.Pages.Properties.Resources.Logo;
            this.VersionControl.Name = "VersionControl";
            this.VersionControl.OneLine = true;
            this.VersionControl.Product = "CubePDF Page";
            this.VersionControl.Uri = null;
            this.VersionControl.Version = "1.0.0";
            //
            // FooterPanel
            //
            resources.ApplyResources(this.FooterPanel, "FooterPanel");
            this.RootPanel.SetColumnSpan(this.FooterPanel, 3);
            this.FooterPanel.Controls.Add(this.ExitButton, 3, 1);
            this.FooterPanel.Controls.Add(this.ExecButton, 2, 1);
            this.FooterPanel.Name = "FooterPanel";
            //
            // ExitButton
            //
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.ExitButton, "ExitButton");
            this.ExitButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.ExitButton.ForeColor = System.Drawing.Color.White;
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.UseVisualStyleBackColor = false;
            //
            // ExecButton
            //
            this.ExecButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            resources.ApplyResources(this.ExecButton, "ExecButton");
            this.ExecButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.ExecButton.ForeColor = System.Drawing.Color.White;
            this.ExecButton.Name = "ExecButton";
            this.ExecButton.UseVisualStyleBackColor = false;
            //
            // SettingWindow
            //
            this.AcceptButton = this.ExecButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.ExitButton;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.RootPanel);
            this.Name = "SettingWindow";
            this.RootPanel.ResumeLayout(false);
            this.MainTabPanel.ResumeLayout(false);
            this.SettingTabPage.ResumeLayout(false);
            this.SettingPanel.ResumeLayout(false);
            this.SettingPanel.PerformLayout();
            this.VersionTabPage.ResumeLayout(false);
            this.VersionPanel.ResumeLayout(false);
            this.FooterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.TabControl MainTabPanel;
        private System.Windows.Forms.TabPage SettingTabPage;
        private System.Windows.Forms.TabPage VersionTabPage;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private System.Windows.Forms.Button ExecButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.TableLayoutPanel SettingPanel;
        private System.Windows.Forms.Label OptionLabel;
        private System.Windows.Forms.CheckBox KeepOutlineCheckBox;
        private System.Windows.Forms.CheckBox ShrinkResourceCheckBox;
        private System.Windows.Forms.Label LanguageLabel;
        private System.Windows.Forms.ComboBox LanguageComboBox;
        private System.Windows.Forms.Label TempLabel;
        private System.Windows.Forms.TextBox TempTextBox;
        private System.Windows.Forms.Button TempButton;
        private System.Windows.Forms.Label OtherLabel;
        private System.Windows.Forms.CheckBox UpdateCheckBox;
        private System.Windows.Forms.TableLayoutPanel VersionPanel;
        private Forms.Controls.VersionControl VersionControl;
    }
}