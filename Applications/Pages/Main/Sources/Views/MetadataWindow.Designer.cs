namespace Cube.Pdf.Pages
{
    partial class MetadataWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetadataWindow));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.MetadataTabPage = new System.Windows.Forms.TabPage();
            this.MetadataPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ViewOptionComboBox = new System.Windows.Forms.ComboBox();
            this.ViewOptionLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.CreatorTextBox = new System.Windows.Forms.TextBox();
            this.CreatorLabel = new System.Windows.Forms.Label();
            this.KeywordTextBox = new System.Windows.Forms.TextBox();
            this.KeywordLabel = new System.Windows.Forms.Label();
            this.SubjectTextBox = new System.Windows.Forms.TextBox();
            this.SubjectLabel = new System.Windows.Forms.Label();
            this.AuthorTextBox = new System.Windows.Forms.TextBox();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.VersionComboBox = new System.Windows.Forms.ComboBox();
            this.EncryptionTabPage = new System.Windows.Forms.TabPage();
            this.EncryptionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.EncryptionSubPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SharePasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.OperationLabel = new System.Windows.Forms.Label();
            this.OwnerConfirmTextBox = new System.Windows.Forms.TextBox();
            this.OwnerConfirmLabel = new System.Windows.Forms.Label();
            this.OwnerPasswordLabel = new System.Windows.Forms.Label();
            this.OwnerPasswordTextBox = new System.Windows.Forms.TextBox();
            this.UserPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.UserPasswordPanel = new System.Windows.Forms.TableLayoutPanel();
            this.UserConfirmTextBox = new System.Windows.Forms.TextBox();
            this.UserConfirmLabel = new System.Windows.Forms.Label();
            this.UserPasswordTextBox = new System.Windows.Forms.TextBox();
            this.UserPasswordLabel = new System.Windows.Forms.Label();
            this.PermissionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AllowAnnotationCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowFormCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowAccessibilityCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowModifyCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowCopyCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowPrintCheckBox = new System.Windows.Forms.CheckBox();
            this.OwnerPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExecButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.RootPanel.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.MetadataTabPage.SuspendLayout();
            this.MetadataPanel.SuspendLayout();
            this.EncryptionTabPage.SuspendLayout();
            this.EncryptionPanel.SuspendLayout();
            this.EncryptionSubPanel.SuspendLayout();
            this.UserPasswordPanel.SuspendLayout();
            this.PermissionPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootPanel
            //
            resources.ApplyResources(this.RootPanel, "RootPanel");
            this.RootPanel.Controls.Add(this.MainTabControl, 1, 1);
            this.RootPanel.Controls.Add(this.FooterPanel, 0, 2);
            this.RootPanel.Name = "RootPanel";
            //
            // MainTabControl
            //
            this.MainTabControl.Controls.Add(this.MetadataTabPage);
            this.MainTabControl.Controls.Add(this.EncryptionTabPage);
            resources.ApplyResources(this.MainTabControl, "MainTabControl");
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            //
            // MetadataTabPage
            //
            resources.ApplyResources(this.MetadataTabPage, "MetadataTabPage");
            this.MetadataTabPage.Controls.Add(this.MetadataPanel);
            this.MetadataTabPage.Name = "MetadataTabPage";
            this.MetadataTabPage.UseVisualStyleBackColor = true;
            //
            // MetadataPanel
            //
            resources.ApplyResources(this.MetadataPanel, "MetadataPanel");
            this.MetadataPanel.Controls.Add(this.ViewOptionComboBox, 2, 7);
            this.MetadataPanel.Controls.Add(this.ViewOptionLabel, 1, 7);
            this.MetadataPanel.Controls.Add(this.VersionLabel, 1, 6);
            this.MetadataPanel.Controls.Add(this.CreatorTextBox, 2, 5);
            this.MetadataPanel.Controls.Add(this.CreatorLabel, 1, 5);
            this.MetadataPanel.Controls.Add(this.KeywordTextBox, 2, 4);
            this.MetadataPanel.Controls.Add(this.KeywordLabel, 1, 4);
            this.MetadataPanel.Controls.Add(this.SubjectTextBox, 2, 3);
            this.MetadataPanel.Controls.Add(this.SubjectLabel, 1, 3);
            this.MetadataPanel.Controls.Add(this.AuthorTextBox, 2, 2);
            this.MetadataPanel.Controls.Add(this.AuthorLabel, 1, 2);
            this.MetadataPanel.Controls.Add(this.TitleLabel, 1, 1);
            this.MetadataPanel.Controls.Add(this.TitleTextBox, 2, 1);
            this.MetadataPanel.Controls.Add(this.VersionComboBox, 2, 6);
            this.MetadataPanel.Name = "MetadataPanel";
            //
            // ViewOptionComboBox
            //
            resources.ApplyResources(this.ViewOptionComboBox, "ViewOptionComboBox");
            this.ViewOptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ViewOptionComboBox.FormattingEnabled = true;
            this.ViewOptionComboBox.Name = "ViewOptionComboBox";
            //
            // ViewOptionLabel
            //
            resources.ApplyResources(this.ViewOptionLabel, "ViewOptionLabel");
            this.ViewOptionLabel.Name = "ViewOptionLabel";
            //
            // VersionLabel
            //
            resources.ApplyResources(this.VersionLabel, "VersionLabel");
            this.VersionLabel.Name = "VersionLabel";
            //
            // CreatorTextBox
            //
            resources.ApplyResources(this.CreatorTextBox, "CreatorTextBox");
            this.CreatorTextBox.Name = "CreatorTextBox";
            //
            // CreatorLabel
            //
            resources.ApplyResources(this.CreatorLabel, "CreatorLabel");
            this.CreatorLabel.Name = "CreatorLabel";
            //
            // KeywordTextBox
            //
            resources.ApplyResources(this.KeywordTextBox, "KeywordTextBox");
            this.KeywordTextBox.Name = "KeywordTextBox";
            //
            // KeywordLabel
            //
            resources.ApplyResources(this.KeywordLabel, "KeywordLabel");
            this.KeywordLabel.Name = "KeywordLabel";
            //
            // SubjectTextBox
            //
            resources.ApplyResources(this.SubjectTextBox, "SubjectTextBox");
            this.SubjectTextBox.Name = "SubjectTextBox";
            //
            // SubjectLabel
            //
            resources.ApplyResources(this.SubjectLabel, "SubjectLabel");
            this.SubjectLabel.Name = "SubjectLabel";
            //
            // AuthorTextBox
            //
            resources.ApplyResources(this.AuthorTextBox, "AuthorTextBox");
            this.AuthorTextBox.Name = "AuthorTextBox";
            //
            // AuthorLabel
            //
            resources.ApplyResources(this.AuthorLabel, "AuthorLabel");
            this.AuthorLabel.Name = "AuthorLabel";
            //
            // TitleLabel
            //
            resources.ApplyResources(this.TitleLabel, "TitleLabel");
            this.TitleLabel.Name = "TitleLabel";
            //
            // TitleTextBox
            //
            resources.ApplyResources(this.TitleTextBox, "TitleTextBox");
            this.TitleTextBox.Name = "TitleTextBox";
            //
            // VersionComboBox
            //
            resources.ApplyResources(this.VersionComboBox, "VersionComboBox");
            this.VersionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VersionComboBox.FormattingEnabled = true;
            this.VersionComboBox.Name = "VersionComboBox";
            //
            // EncryptionTabPage
            //
            resources.ApplyResources(this.EncryptionTabPage, "EncryptionTabPage");
            this.EncryptionTabPage.Controls.Add(this.EncryptionPanel);
            this.EncryptionTabPage.Name = "EncryptionTabPage";
            this.EncryptionTabPage.UseVisualStyleBackColor = true;
            //
            // EncryptionPanel
            //
            resources.ApplyResources(this.EncryptionPanel, "EncryptionPanel");
            this.EncryptionPanel.Controls.Add(this.EncryptionSubPanel, 1, 2);
            this.EncryptionPanel.Controls.Add(this.OwnerPasswordCheckBox, 1, 1);
            this.EncryptionPanel.Name = "EncryptionPanel";
            //
            // EncryptionSubPanel
            //
            resources.ApplyResources(this.EncryptionSubPanel, "EncryptionSubPanel");
            this.EncryptionSubPanel.Controls.Add(this.SharePasswordCheckBox, 1, 4);
            this.EncryptionSubPanel.Controls.Add(this.OperationLabel, 0, 2);
            this.EncryptionSubPanel.Controls.Add(this.OwnerConfirmTextBox, 1, 1);
            this.EncryptionSubPanel.Controls.Add(this.OwnerConfirmLabel, 0, 1);
            this.EncryptionSubPanel.Controls.Add(this.OwnerPasswordLabel, 0, 0);
            this.EncryptionSubPanel.Controls.Add(this.OwnerPasswordTextBox, 1, 0);
            this.EncryptionSubPanel.Controls.Add(this.UserPasswordCheckBox, 1, 2);
            this.EncryptionSubPanel.Controls.Add(this.UserPasswordPanel, 1, 3);
            this.EncryptionSubPanel.Controls.Add(this.PermissionPanel, 1, 5);
            this.EncryptionSubPanel.Name = "EncryptionSubPanel";
            //
            // SharePasswordCheckBox
            //
            this.SharePasswordCheckBox.AutoEllipsis = true;
            resources.ApplyResources(this.SharePasswordCheckBox, "SharePasswordCheckBox");
            this.SharePasswordCheckBox.Name = "SharePasswordCheckBox";
            this.SharePasswordCheckBox.UseVisualStyleBackColor = true;
            //
            // OperationLabel
            //
            resources.ApplyResources(this.OperationLabel, "OperationLabel");
            this.OperationLabel.Name = "OperationLabel";
            //
            // OwnerConfirmTextBox
            //
            resources.ApplyResources(this.OwnerConfirmTextBox, "OwnerConfirmTextBox");
            this.OwnerConfirmTextBox.Name = "OwnerConfirmTextBox";
            this.OwnerConfirmTextBox.UseSystemPasswordChar = true;
            //
            // OwnerConfirmLabel
            //
            resources.ApplyResources(this.OwnerConfirmLabel, "OwnerConfirmLabel");
            this.OwnerConfirmLabel.Name = "OwnerConfirmLabel";
            //
            // OwnerPasswordLabel
            //
            resources.ApplyResources(this.OwnerPasswordLabel, "OwnerPasswordLabel");
            this.OwnerPasswordLabel.Name = "OwnerPasswordLabel";
            //
            // OwnerPasswordTextBox
            //
            resources.ApplyResources(this.OwnerPasswordTextBox, "OwnerPasswordTextBox");
            this.OwnerPasswordTextBox.Name = "OwnerPasswordTextBox";
            this.OwnerPasswordTextBox.UseSystemPasswordChar = true;
            //
            // UserPasswordCheckBox
            //
            this.UserPasswordCheckBox.AutoEllipsis = true;
            resources.ApplyResources(this.UserPasswordCheckBox, "UserPasswordCheckBox");
            this.UserPasswordCheckBox.Name = "UserPasswordCheckBox";
            this.UserPasswordCheckBox.UseVisualStyleBackColor = true;
            //
            // UserPasswordPanel
            //
            resources.ApplyResources(this.UserPasswordPanel, "UserPasswordPanel");
            this.UserPasswordPanel.Controls.Add(this.UserConfirmTextBox, 2, 1);
            this.UserPasswordPanel.Controls.Add(this.UserConfirmLabel, 0, 1);
            this.UserPasswordPanel.Controls.Add(this.UserPasswordTextBox, 1, 0);
            this.UserPasswordPanel.Controls.Add(this.UserPasswordLabel, 0, 0);
            this.UserPasswordPanel.Name = "UserPasswordPanel";
            //
            // UserConfirmTextBox
            //
            resources.ApplyResources(this.UserConfirmTextBox, "UserConfirmTextBox");
            this.UserConfirmTextBox.Name = "UserConfirmTextBox";
            this.UserConfirmTextBox.UseSystemPasswordChar = true;
            //
            // UserConfirmLabel
            //
            resources.ApplyResources(this.UserConfirmLabel, "UserConfirmLabel");
            this.UserConfirmLabel.Name = "UserConfirmLabel";
            //
            // UserPasswordTextBox
            //
            resources.ApplyResources(this.UserPasswordTextBox, "UserPasswordTextBox");
            this.UserPasswordTextBox.Name = "UserPasswordTextBox";
            this.UserPasswordTextBox.UseSystemPasswordChar = true;
            //
            // UserPasswordLabel
            //
            resources.ApplyResources(this.UserPasswordLabel, "UserPasswordLabel");
            this.UserPasswordLabel.Name = "UserPasswordLabel";
            //
            // PermissionPanel
            //
            resources.ApplyResources(this.PermissionPanel, "PermissionPanel");
            this.PermissionPanel.Controls.Add(this.AllowAnnotationCheckBox, 0, 5);
            this.PermissionPanel.Controls.Add(this.AllowFormCheckBox, 0, 4);
            this.PermissionPanel.Controls.Add(this.AllowAccessibilityCheckBox, 0, 3);
            this.PermissionPanel.Controls.Add(this.AllowModifyCheckBox, 0, 2);
            this.PermissionPanel.Controls.Add(this.AllowCopyCheckBox, 0, 1);
            this.PermissionPanel.Controls.Add(this.AllowPrintCheckBox, 0, 0);
            this.PermissionPanel.Name = "PermissionPanel";
            //
            // AllowAnnotationCheckBox
            //
            this.AllowAnnotationCheckBox.AutoEllipsis = true;
            resources.ApplyResources(this.AllowAnnotationCheckBox, "AllowAnnotationCheckBox");
            this.AllowAnnotationCheckBox.Name = "AllowAnnotationCheckBox";
            this.AllowAnnotationCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowFormCheckBox
            //
            this.AllowFormCheckBox.AutoEllipsis = true;
            resources.ApplyResources(this.AllowFormCheckBox, "AllowFormCheckBox");
            this.AllowFormCheckBox.Name = "AllowFormCheckBox";
            this.AllowFormCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowAccessibilityCheckBox
            //
            this.AllowAccessibilityCheckBox.AutoEllipsis = true;
            resources.ApplyResources(this.AllowAccessibilityCheckBox, "AllowAccessibilityCheckBox");
            this.AllowAccessibilityCheckBox.Name = "AllowAccessibilityCheckBox";
            this.AllowAccessibilityCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowModifyCheckBox
            //
            this.AllowModifyCheckBox.AutoEllipsis = true;
            resources.ApplyResources(this.AllowModifyCheckBox, "AllowModifyCheckBox");
            this.AllowModifyCheckBox.Name = "AllowModifyCheckBox";
            this.AllowModifyCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowCopyCheckBox
            //
            this.AllowCopyCheckBox.AutoEllipsis = true;
            resources.ApplyResources(this.AllowCopyCheckBox, "AllowCopyCheckBox");
            this.AllowCopyCheckBox.Name = "AllowCopyCheckBox";
            this.AllowCopyCheckBox.UseVisualStyleBackColor = true;
            //
            // AllowPrintCheckBox
            //
            this.AllowPrintCheckBox.AutoEllipsis = true;
            resources.ApplyResources(this.AllowPrintCheckBox, "AllowPrintCheckBox");
            this.AllowPrintCheckBox.Name = "AllowPrintCheckBox";
            this.AllowPrintCheckBox.UseVisualStyleBackColor = true;
            //
            // OwnerPasswordCheckBox
            //
            this.OwnerPasswordCheckBox.AutoEllipsis = true;
            resources.ApplyResources(this.OwnerPasswordCheckBox, "OwnerPasswordCheckBox");
            this.OwnerPasswordCheckBox.Name = "OwnerPasswordCheckBox";
            this.OwnerPasswordCheckBox.UseVisualStyleBackColor = true;
            //
            // FooterPanel
            //
            resources.ApplyResources(this.FooterPanel, "FooterPanel");
            this.RootPanel.SetColumnSpan(this.FooterPanel, 3);
            this.FooterPanel.Controls.Add(this.ExecButton, 1, 1);
            this.FooterPanel.Controls.Add(this.ExitButton, 2, 1);
            this.FooterPanel.Name = "FooterPanel";
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
            // MetadataWindow
            //
            this.AcceptButton = this.ExecButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.ExitButton;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.RootPanel);
            this.Name = "MetadataWindow";
            this.RootPanel.ResumeLayout(false);
            this.MainTabControl.ResumeLayout(false);
            this.MetadataTabPage.ResumeLayout(false);
            this.MetadataPanel.ResumeLayout(false);
            this.MetadataPanel.PerformLayout();
            this.EncryptionTabPage.ResumeLayout(false);
            this.EncryptionPanel.ResumeLayout(false);
            this.EncryptionSubPanel.ResumeLayout(false);
            this.EncryptionSubPanel.PerformLayout();
            this.UserPasswordPanel.ResumeLayout(false);
            this.UserPasswordPanel.PerformLayout();
            this.PermissionPanel.ResumeLayout(false);
            this.FooterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage MetadataTabPage;
        private System.Windows.Forms.TabPage EncryptionTabPage;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private System.Windows.Forms.Button ExecButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.TableLayoutPanel MetadataPanel;
        private System.Windows.Forms.Label AuthorLabel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.TextBox AuthorTextBox;
        private System.Windows.Forms.Label SubjectLabel;
        private System.Windows.Forms.TextBox SubjectTextBox;
        private System.Windows.Forms.TextBox KeywordTextBox;
        private System.Windows.Forms.Label KeywordLabel;
        private System.Windows.Forms.TextBox CreatorTextBox;
        private System.Windows.Forms.Label CreatorLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.ComboBox VersionComboBox;
        private System.Windows.Forms.ComboBox ViewOptionComboBox;
        private System.Windows.Forms.Label ViewOptionLabel;
        private System.Windows.Forms.TableLayoutPanel EncryptionPanel;
        private System.Windows.Forms.TableLayoutPanel EncryptionSubPanel;
        private System.Windows.Forms.Label OperationLabel;
        private System.Windows.Forms.Label OwnerConfirmLabel;
        private System.Windows.Forms.Label OwnerPasswordLabel;
        private System.Windows.Forms.CheckBox OwnerPasswordCheckBox;
        private System.Windows.Forms.TextBox OwnerConfirmTextBox;
        private System.Windows.Forms.TextBox OwnerPasswordTextBox;
        private System.Windows.Forms.CheckBox UserPasswordCheckBox;
        private System.Windows.Forms.TableLayoutPanel UserPasswordPanel;
        private System.Windows.Forms.TextBox UserConfirmTextBox;
        private System.Windows.Forms.Label UserConfirmLabel;
        private System.Windows.Forms.TextBox UserPasswordTextBox;
        private System.Windows.Forms.Label UserPasswordLabel;
        private System.Windows.Forms.TableLayoutPanel PermissionPanel;
        private System.Windows.Forms.CheckBox AllowAnnotationCheckBox;
        private System.Windows.Forms.CheckBox AllowFormCheckBox;
        private System.Windows.Forms.CheckBox AllowAccessibilityCheckBox;
        private System.Windows.Forms.CheckBox AllowModifyCheckBox;
        private System.Windows.Forms.CheckBox AllowCopyCheckBox;
        private System.Windows.Forms.CheckBox AllowPrintCheckBox;
        private System.Windows.Forms.CheckBox SharePasswordCheckBox;
    }
}