namespace Duracellko.ClipboardShareUI
{
    partial class LogForm
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
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.logGridView = new System.Windows.Forms.DataGridView();
            this.IconColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detailTextBox = new System.Windows.Forms.TextBox();
            this.logToolStrip = new System.Windows.Forms.ToolStrip();
            this.clearLogToolButton = new System.Windows.Forms.ToolStripButton();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logGridView)).BeginInit();
            this.logToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 25);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.logGridView);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.detailTextBox);
            this.mainSplitContainer.Size = new System.Drawing.Size(632, 421);
            this.mainSplitContainer.SplitterDistance = 198;
            this.mainSplitContainer.TabIndex = 1;
            // 
            // logGridView
            // 
            this.logGridView.AllowUserToAddRows = false;
            this.logGridView.AllowUserToDeleteRows = false;
            this.logGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.logGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.logGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IconColumn,
            this.TimeColumn,
            this.TitleColumn});
            this.logGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logGridView.Location = new System.Drawing.Point(0, 0);
            this.logGridView.MultiSelect = false;
            this.logGridView.Name = "logGridView";
            this.logGridView.ReadOnly = true;
            this.logGridView.RowHeadersVisible = false;
            this.logGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.logGridView.Size = new System.Drawing.Size(632, 198);
            this.logGridView.TabIndex = 0;
            this.logGridView.SelectionChanged += new System.EventHandler(this.logGridView_SelectionChanged);
            // 
            // IconColumn
            // 
            this.IconColumn.DataPropertyName = "Icon";
            this.IconColumn.HeaderText = "";
            this.IconColumn.Name = "IconColumn";
            this.IconColumn.ReadOnly = true;
            this.IconColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.IconColumn.Width = 24;
            // 
            // TimeColumn
            // 
            this.TimeColumn.DataPropertyName = "Time";
            this.TimeColumn.HeaderText = "Time";
            this.TimeColumn.Name = "TimeColumn";
            this.TimeColumn.ReadOnly = true;
            this.TimeColumn.Width = 150;
            // 
            // TitleColumn
            // 
            this.TitleColumn.DataPropertyName = "Title";
            this.TitleColumn.HeaderText = "Title";
            this.TitleColumn.Name = "TitleColumn";
            this.TitleColumn.ReadOnly = true;
            this.TitleColumn.Width = 430;
            // 
            // detailTextBox
            // 
            this.detailTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailTextBox.Location = new System.Drawing.Point(0, 0);
            this.detailTextBox.Multiline = true;
            this.detailTextBox.Name = "detailTextBox";
            this.detailTextBox.ReadOnly = true;
            this.detailTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.detailTextBox.Size = new System.Drawing.Size(632, 219);
            this.detailTextBox.TabIndex = 0;
            this.detailTextBox.WordWrap = false;
            // 
            // logToolStrip
            // 
            this.logToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearLogToolButton});
            this.logToolStrip.Location = new System.Drawing.Point(0, 0);
            this.logToolStrip.Name = "logToolStrip";
            this.logToolStrip.Size = new System.Drawing.Size(632, 25);
            this.logToolStrip.TabIndex = 0;
            // 
            // clearLogToolButton
            // 
            this.clearLogToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearLogToolButton.Name = "clearLogToolButton";
            this.clearLogToolButton.Size = new System.Drawing.Size(56, 22);
            this.clearLogToolButton.Text = "Clear Log";
            this.clearLogToolButton.Click += new System.EventHandler(this.clearLogToolButton_Click);
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 446);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.logToolStrip);
            this.Name = "LogForm";
            this.Text = "Log";
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            this.mainSplitContainer.Panel2.PerformLayout();
            this.mainSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logGridView)).EndInit();
            this.logToolStrip.ResumeLayout(false);
            this.logToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.DataGridView logGridView;
        private System.Windows.Forms.TextBox detailTextBox;
        private System.Windows.Forms.DataGridViewImageColumn IconColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleColumn;
        private System.Windows.Forms.ToolStrip logToolStrip;
        private System.Windows.Forms.ToolStripButton clearLogToolButton;
    }
}