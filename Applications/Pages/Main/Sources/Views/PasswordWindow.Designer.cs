namespace Cube.Pdf.Pages
{
    partial class PasswordWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordWindow));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.ShowPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.TitleButton = new System.Windows.Forms.PictureBox();
            this.FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExitButton = new Cube.Forms.Controls.Button();
            this.ExecButton = new Cube.Forms.Controls.Button();
            this.RootPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TitleButton)).BeginInit();
            this.FooterPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootPanel
            //
            this.RootPanel.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.RootPanel, "RootPanel");
            this.RootPanel.Controls.Add(this.PasswordTextBox, 1, 2);
            this.RootPanel.Controls.Add(this.ShowPasswordCheckBox, 1, 3);
            this.RootPanel.Controls.Add(this.PasswordLabel, 1, 1);
            this.RootPanel.Controls.Add(this.HeaderPanel, 0, 0);
            this.RootPanel.Controls.Add(this.FooterPanel, 0, 5);
            this.RootPanel.Name = "RootPanel";
            //
            // PasswordTextBox
            //
            resources.ApplyResources(this.PasswordTextBox, "PasswordTextBox");
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.UseSystemPasswordChar = true;
            //
            // ShowPasswordCheckBox
            //
            resources.ApplyResources(this.ShowPasswordCheckBox, "ShowPasswordCheckBox");
            this.ShowPasswordCheckBox.Name = "ShowPasswordCheckBox";
            this.ShowPasswordCheckBox.UseVisualStyleBackColor = true;
            //
            // PasswordLabel
            //
            this.PasswordLabel.AutoEllipsis = true;
            resources.ApplyResources(this.PasswordLabel, "PasswordLabel");
            this.PasswordLabel.Name = "PasswordLabel";
            //
            // HeaderPanel
            //
            this.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(39)))), ((int)(((byte)(45)))));
            this.HeaderPanel.BackgroundImage = global::Cube.Pdf.Pages.Properties.Resources.Background;
            this.RootPanel.SetColumnSpan(this.HeaderPanel, 3);
            this.HeaderPanel.Controls.Add(this.TitleButton);
            resources.ApplyResources(this.HeaderPanel, "HeaderPanel");
            this.HeaderPanel.Name = "HeaderPanel";
            //
            // TitleButton
            //
            this.TitleButton.BackgroundImage = global::Cube.Pdf.Pages.Properties.Resources.Background;
            this.TitleButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.TitleButton, "TitleButton");
            this.TitleButton.Image = global::Cube.Pdf.Pages.Properties.Resources.HeaderTitle;
            this.TitleButton.Name = "TitleButton";
            this.TitleButton.TabStop = false;
            //
            // FooterPanel
            //
            this.FooterPanel.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.FooterPanel, "FooterPanel");
            this.RootPanel.SetColumnSpan(this.FooterPanel, 3);
            this.FooterPanel.Controls.Add(this.ExitButton, 1, 1);
            this.FooterPanel.Controls.Add(this.ExecButton, 1, 1);
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
            this.ExecButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.ExecButton, "ExecButton");
            this.ExecButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.ExecButton.ForeColor = System.Drawing.Color.White;
            this.ExecButton.Name = "ExecButton";
            this.ExecButton.UseVisualStyleBackColor = false;
            //
            // PasswordWindow
            //
            this.AcceptButton = this.ExecButton;
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.ExitButton;
            this.Controls.Add(this.RootPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordWindow";
            this.ShowInTaskbar = false;
            this.RootPanel.ResumeLayout(false);
            this.RootPanel.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TitleButton)).EndInit();
            this.FooterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.PictureBox TitleButton;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.CheckBox ShowPasswordCheckBox;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private Forms.Controls.Button ExitButton;
        private Forms.Controls.Button ExecButton;
    }
}