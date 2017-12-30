namespace Duracellko.ClipboardShareUI
{
    partial class SendClipboardForm
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
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.addPermanentlyCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // addressTextBox
            // 
            this.addressTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.addressTextBox.Location = new System.Drawing.Point(0, 0);
            this.addressTextBox.MaxLength = 150;
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(188, 21);
            this.addressTextBox.TabIndex = 0;
            this.mainToolTip.SetToolTip(this.addressTextBox, "Address to send clipboard to including port (computer:port).");
            this.addressTextBox.TextChanged += new System.EventHandler(this.addressTextBox_TextChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(132, 23);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(25, 23);
            this.okButton.TabIndex = 2;
            this.mainToolTip.SetToolTip(this.okButton, "OK");
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(163, 23);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(25, 23);
            this.cancelButton.TabIndex = 3;
            this.mainToolTip.SetToolTip(this.cancelButton, "Cancel");
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // mainToolTip
            // 
            this.mainToolTip.AutoPopDelay = 10000;
            this.mainToolTip.InitialDelay = 500;
            this.mainToolTip.ReshowDelay = 100;
            // 
            // addPermanentlyCheckBox
            // 
            this.addPermanentlyCheckBox.AutoSize = true;
            this.addPermanentlyCheckBox.Location = new System.Drawing.Point(12, 23);
            this.addPermanentlyCheckBox.Name = "addPermanentlyCheckBox";
            this.addPermanentlyCheckBox.Size = new System.Drawing.Size(108, 17);
            this.addPermanentlyCheckBox.TabIndex = 1;
            this.addPermanentlyCheckBox.Text = "Add permanently";
            this.addPermanentlyCheckBox.UseVisualStyleBackColor = true;
            // 
            // SendClipboardForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(188, 47);
            this.Controls.Add(this.addPermanentlyCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.addressTextBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendClipboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Send Clipboard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ToolTip mainToolTip;
        private System.Windows.Forms.CheckBox addPermanentlyCheckBox;
    }
}