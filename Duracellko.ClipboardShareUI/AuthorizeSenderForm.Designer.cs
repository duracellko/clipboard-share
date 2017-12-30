namespace Duracellko.ClipboardShareUI
{
    partial class AuthorizeSenderForm
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.captionLabel = new System.Windows.Forms.Label();
            this.acceptLink = new System.Windows.Forms.LinkLabel();
            this.rejectLink = new System.Windows.Forms.LinkLabel();
            this.visibilityTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nameLabel.Location = new System.Drawing.Point(12, 15);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(248, 20);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // captionLabel
            // 
            this.captionLabel.BackColor = System.Drawing.Color.Transparent;
            this.captionLabel.Location = new System.Drawing.Point(12, 34);
            this.captionLabel.Name = "captionLabel";
            this.captionLabel.Size = new System.Drawing.Size(248, 20);
            this.captionLabel.TabIndex = 1;
            this.captionLabel.Text = "wants to share clipboard with you.";
            this.captionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // acceptLink
            // 
            this.acceptLink.BackColor = System.Drawing.Color.Transparent;
            this.acceptLink.Location = new System.Drawing.Point(46, 54);
            this.acceptLink.Name = "acceptLink";
            this.acceptLink.Size = new System.Drawing.Size(88, 20);
            this.acceptLink.TabIndex = 2;
            this.acceptLink.TabStop = true;
            this.acceptLink.Text = "Accept";
            this.acceptLink.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.acceptLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.acceptLink_LinkClicked);
            // 
            // rejectLink
            // 
            this.rejectLink.BackColor = System.Drawing.Color.Transparent;
            this.rejectLink.Location = new System.Drawing.Point(140, 54);
            this.rejectLink.Name = "rejectLink";
            this.rejectLink.Size = new System.Drawing.Size(88, 20);
            this.rejectLink.TabIndex = 3;
            this.rejectLink.TabStop = true;
            this.rejectLink.Text = "Reject";
            this.rejectLink.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rejectLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.rejectLink_LinkClicked);
            // 
            // visibilityTimer
            // 
            this.visibilityTimer.Interval = 20;
            this.visibilityTimer.Tick += new System.EventHandler(this.visibilityTimer_Tick);
            // 
            // AuthorizeSenderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Magenta;
            this.ClientSize = new System.Drawing.Size(272, 83);
            this.Controls.Add(this.rejectLink);
            this.Controls.Add(this.acceptLink);
            this.Controls.Add(this.captionLabel);
            this.Controls.Add(this.nameLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuthorizeSenderForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Authorize Clipboard Sharing";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label captionLabel;
        private System.Windows.Forms.LinkLabel acceptLink;
        private System.Windows.Forms.LinkLabel rejectLink;
        private System.Windows.Forms.Timer visibilityTimer;
    }
}