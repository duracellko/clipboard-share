namespace Duracellko.ClipboardShareUI
{
    partial class MainForm
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
            System.Windows.Forms.ToolStripSeparator exitSeparator;
            this.mainIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.iconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setDefaultMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.iconMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // exitSeparator
            // 
            exitSeparator.Name = "exitSeparator";
            exitSeparator.Size = new System.Drawing.Size(124, 6);
            // 
            // mainIcon
            // 
            this.mainIcon.ContextMenuStrip = this.iconMenu;
            this.mainIcon.Text = "Clipboard Share";
            this.mainIcon.Visible = true;
            this.mainIcon.BalloonTipClicked += new System.EventHandler(this.mainIcon_BalloonTipClicked);
            this.mainIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mainIcon_MouseClick);
            this.mainIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mainIcon_MouseDoubleClick);
            // 
            // iconMenu
            // 
            this.iconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setDefaultMenuItem,
            this.settingsMenuItem,
            this.logToolStripMenuItem,
            this.helpSeparator,
            this.helpToolStripMenuItem,
            exitSeparator,
            this.exitMenuItem});
            this.iconMenu.Name = "iconMenu";
            this.iconMenu.ShowImageMargin = false;
            this.iconMenu.Size = new System.Drawing.Size(128, 148);
            this.iconMenu.Opening += new System.ComponentModel.CancelEventHandler(this.iconMenu_Opening);
            // 
            // setDefaultMenuItem
            // 
            this.setDefaultMenuItem.Name = "setDefaultMenuItem";
            this.setDefaultMenuItem.Size = new System.Drawing.Size(127, 22);
            this.setDefaultMenuItem.Text = "Set &Default";
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(127, 22);
            this.settingsMenuItem.Text = "&Settings...";
            this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.logToolStripMenuItem.Text = "&Log...";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(127, 22);
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // helpSeparator
            // 
            this.helpSeparator.Name = "helpSeparator";
            this.helpSeparator.Size = new System.Drawing.Size(124, 6);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 112);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Clipboard Share";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.iconMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon mainIcon;
        private System.Windows.Forms.ContextMenuStrip iconMenu;
        private System.Windows.Forms.ToolStripMenuItem setDefaultMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripSeparator helpSeparator;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;

    }
}