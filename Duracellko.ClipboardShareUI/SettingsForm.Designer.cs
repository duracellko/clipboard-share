namespace Duracellko.ClipboardShareUI
{
    partial class SettingsForm
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
            this.settingsTabs = new System.Windows.Forms.TabControl();
            this.generalPage = new System.Windows.Forms.TabPage();
            this.activePortLabel = new System.Windows.Forms.Label();
            this.explicitPortTextBox = new System.Windows.Forms.TextBox();
            this.explicitPortRadioButton = new System.Windows.Forms.RadioButton();
            this.freePortRadioButton = new System.Windows.Forms.RadioButton();
            this.runOnStartupCheckBox = new System.Windows.Forms.CheckBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.maxReceivedMessageSizeKbLabel = new System.Windows.Forms.Label();
            this.maxReceivedMessageSizeTextBox = new System.Windows.Forms.TextBox();
            this.maxReceivedMessageSizeLabel = new System.Windows.Forms.Label();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.authorizedSendersPage = new System.Windows.Forms.TabPage();
            this.authorizedSendersGrid = new System.Windows.Forms.DataGridView();
            this.deleteColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.computerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authorizedSendersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.permanentRecipientsPage = new System.Windows.Forms.TabPage();
            this.permanentRecipientsGrid = new System.Windows.Forms.DataGridView();
            this.deleteRecipientColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.recipientColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.permanentRecipientsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.aboutTab = new System.Windows.Forms.TabPage();
            this.mainTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.applicationNameLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.settingsTabs.SuspendLayout();
            this.generalPage.SuspendLayout();
            this.authorizedSendersPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.authorizedSendersGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.authorizedSendersBindingSource)).BeginInit();
            this.permanentRecipientsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.permanentRecipientsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.permanentRecipientsBindingSource)).BeginInit();
            this.aboutTab.SuspendLayout();
            this.mainTablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsTabs
            // 
            this.settingsTabs.Controls.Add(this.generalPage);
            this.settingsTabs.Controls.Add(this.authorizedSendersPage);
            this.settingsTabs.Controls.Add(this.permanentRecipientsPage);
            this.settingsTabs.Controls.Add(this.aboutTab);
            this.settingsTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsTabs.Location = new System.Drawing.Point(0, 0);
            this.settingsTabs.Name = "settingsTabs";
            this.settingsTabs.SelectedIndex = 0;
            this.settingsTabs.Size = new System.Drawing.Size(487, 294);
            this.settingsTabs.TabIndex = 0;
            // 
            // generalPage
            // 
            this.generalPage.Controls.Add(this.activePortLabel);
            this.generalPage.Controls.Add(this.explicitPortTextBox);
            this.generalPage.Controls.Add(this.explicitPortRadioButton);
            this.generalPage.Controls.Add(this.freePortRadioButton);
            this.generalPage.Controls.Add(this.runOnStartupCheckBox);
            this.generalPage.Controls.Add(this.portTextBox);
            this.generalPage.Controls.Add(this.portLabel);
            this.generalPage.Controls.Add(this.maxReceivedMessageSizeKbLabel);
            this.generalPage.Controls.Add(this.maxReceivedMessageSizeTextBox);
            this.generalPage.Controls.Add(this.maxReceivedMessageSizeLabel);
            this.generalPage.Controls.Add(this.userNameTextBox);
            this.generalPage.Controls.Add(this.userNameLabel);
            this.generalPage.Location = new System.Drawing.Point(4, 22);
            this.generalPage.Name = "generalPage";
            this.generalPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalPage.Size = new System.Drawing.Size(479, 268);
            this.generalPage.TabIndex = 0;
            this.generalPage.Text = "General";
            this.generalPage.UseVisualStyleBackColor = true;
            // 
            // activePortLabel
            // 
            this.activePortLabel.AutoSize = true;
            this.activePortLabel.Location = new System.Drawing.Point(8, 86);
            this.activePortLabel.Name = "activePortLabel";
            this.activePortLabel.Size = new System.Drawing.Size(60, 13);
            this.activePortLabel.TabIndex = 9;
            this.activePortLabel.Text = "Active port";
            // 
            // explicitPortTextBox
            // 
            this.explicitPortTextBox.Location = new System.Drawing.Point(268, 59);
            this.explicitPortTextBox.Name = "explicitPortTextBox";
            this.explicitPortTextBox.Size = new System.Drawing.Size(64, 21);
            this.explicitPortTextBox.TabIndex = 8;
            // 
            // explicitPortRadioButton
            // 
            this.explicitPortRadioButton.AutoSize = true;
            this.explicitPortRadioButton.Location = new System.Drawing.Point(204, 60);
            this.explicitPortRadioButton.Name = "explicitPortRadioButton";
            this.explicitPortRadioButton.Size = new System.Drawing.Size(58, 17);
            this.explicitPortRadioButton.TabIndex = 7;
            this.explicitPortRadioButton.TabStop = true;
            this.explicitPortRadioButton.Text = "Explicit";
            this.explicitPortRadioButton.UseVisualStyleBackColor = true;
            this.explicitPortRadioButton.CheckedChanged += new System.EventHandler(this.explicitPortRadioButton_CheckedChanged);
            // 
            // freePortRadioButton
            // 
            this.freePortRadioButton.AutoSize = true;
            this.freePortRadioButton.Checked = true;
            this.freePortRadioButton.Location = new System.Drawing.Point(151, 60);
            this.freePortRadioButton.Name = "freePortRadioButton";
            this.freePortRadioButton.Size = new System.Drawing.Size(47, 17);
            this.freePortRadioButton.TabIndex = 6;
            this.freePortRadioButton.TabStop = true;
            this.freePortRadioButton.Text = "Free";
            this.freePortRadioButton.UseVisualStyleBackColor = true;
            // 
            // runOnStartupCheckBox
            // 
            this.runOnStartupCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.runOnStartupCheckBox.Location = new System.Drawing.Point(8, 110);
            this.runOnStartupCheckBox.Name = "runOnStartupCheckBox";
            this.runOnStartupCheckBox.Size = new System.Drawing.Size(158, 24);
            this.runOnStartupCheckBox.TabIndex = 11;
            this.runOnStartupCheckBox.Text = "Run on startup";
            this.runOnStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(151, 83);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.ReadOnly = true;
            this.portTextBox.Size = new System.Drawing.Size(64, 21);
            this.portTextBox.TabIndex = 10;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(8, 62);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(27, 13);
            this.portLabel.TabIndex = 5;
            this.portLabel.Text = "Port";
            // 
            // maxReceivedMessageSizeKbLabel
            // 
            this.maxReceivedMessageSizeKbLabel.AutoSize = true;
            this.maxReceivedMessageSizeKbLabel.Location = new System.Drawing.Point(338, 36);
            this.maxReceivedMessageSizeKbLabel.Name = "maxReceivedMessageSizeKbLabel";
            this.maxReceivedMessageSizeKbLabel.Size = new System.Drawing.Size(19, 13);
            this.maxReceivedMessageSizeKbLabel.TabIndex = 4;
            this.maxReceivedMessageSizeKbLabel.Text = "KB";
            // 
            // maxReceivedMessageSizeTextBox
            // 
            this.maxReceivedMessageSizeTextBox.Location = new System.Drawing.Point(151, 33);
            this.maxReceivedMessageSizeTextBox.Name = "maxReceivedMessageSizeTextBox";
            this.maxReceivedMessageSizeTextBox.Size = new System.Drawing.Size(181, 21);
            this.maxReceivedMessageSizeTextBox.TabIndex = 3;
            this.maxReceivedMessageSizeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // maxReceivedMessageSizeLabel
            // 
            this.maxReceivedMessageSizeLabel.AutoSize = true;
            this.maxReceivedMessageSizeLabel.Location = new System.Drawing.Point(8, 36);
            this.maxReceivedMessageSizeLabel.Name = "maxReceivedMessageSizeLabel";
            this.maxReceivedMessageSizeLabel.Size = new System.Drawing.Size(137, 13);
            this.maxReceivedMessageSizeLabel.TabIndex = 2;
            this.maxReceivedMessageSizeLabel.Text = "Max received message size";
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(151, 6);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(181, 21);
            this.userNameTextBox.TabIndex = 1;
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Location = new System.Drawing.Point(8, 9);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(58, 13);
            this.userNameLabel.TabIndex = 0;
            this.userNameLabel.Text = "User name";
            // 
            // authorizedSendersPage
            // 
            this.authorizedSendersPage.Controls.Add(this.authorizedSendersGrid);
            this.authorizedSendersPage.Location = new System.Drawing.Point(4, 22);
            this.authorizedSendersPage.Name = "authorizedSendersPage";
            this.authorizedSendersPage.Padding = new System.Windows.Forms.Padding(3);
            this.authorizedSendersPage.Size = new System.Drawing.Size(479, 268);
            this.authorizedSendersPage.TabIndex = 1;
            this.authorizedSendersPage.Text = "Authorized Senders";
            this.authorizedSendersPage.UseVisualStyleBackColor = true;
            // 
            // authorizedSendersGrid
            // 
            this.authorizedSendersGrid.AllowUserToAddRows = false;
            this.authorizedSendersGrid.AllowUserToResizeRows = false;
            this.authorizedSendersGrid.AutoGenerateColumns = false;
            this.authorizedSendersGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.authorizedSendersGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.authorizedSendersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.authorizedSendersGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deleteColumn,
            this.computerColumn,
            this.userNameColumn});
            this.authorizedSendersGrid.DataSource = this.authorizedSendersBindingSource;
            this.authorizedSendersGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.authorizedSendersGrid.Location = new System.Drawing.Point(3, 3);
            this.authorizedSendersGrid.MultiSelect = false;
            this.authorizedSendersGrid.Name = "authorizedSendersGrid";
            this.authorizedSendersGrid.ReadOnly = true;
            this.authorizedSendersGrid.RowHeadersVisible = false;
            this.authorizedSendersGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.authorizedSendersGrid.Size = new System.Drawing.Size(473, 262);
            this.authorizedSendersGrid.TabIndex = 0;
            this.authorizedSendersGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.authorizedSendersGrid_CellContentClick);
            // 
            // deleteColumn
            // 
            this.deleteColumn.HeaderText = "";
            this.deleteColumn.Name = "deleteColumn";
            this.deleteColumn.ReadOnly = true;
            this.deleteColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.deleteColumn.ToolTipText = "Delete";
            this.deleteColumn.Width = 30;
            // 
            // computerColumn
            // 
            this.computerColumn.DataPropertyName = "ComputerName";
            this.computerColumn.HeaderText = "Computer";
            this.computerColumn.Name = "computerColumn";
            this.computerColumn.ReadOnly = true;
            this.computerColumn.Width = 200;
            // 
            // userNameColumn
            // 
            this.userNameColumn.DataPropertyName = "UserName";
            this.userNameColumn.HeaderText = "User Name";
            this.userNameColumn.Name = "userNameColumn";
            this.userNameColumn.ReadOnly = true;
            this.userNameColumn.Width = 200;
            // 
            // authorizedSendersBindingSource
            // 
            this.authorizedSendersBindingSource.AllowNew = false;
            // 
            // permanentRecipientsPage
            // 
            this.permanentRecipientsPage.Controls.Add(this.permanentRecipientsGrid);
            this.permanentRecipientsPage.Location = new System.Drawing.Point(4, 22);
            this.permanentRecipientsPage.Name = "permanentRecipientsPage";
            this.permanentRecipientsPage.Padding = new System.Windows.Forms.Padding(3);
            this.permanentRecipientsPage.Size = new System.Drawing.Size(479, 268);
            this.permanentRecipientsPage.TabIndex = 3;
            this.permanentRecipientsPage.Text = "Permanent Recipients";
            this.permanentRecipientsPage.UseVisualStyleBackColor = true;
            // 
            // permanentRecipientsGrid
            // 
            this.permanentRecipientsGrid.AllowUserToAddRows = false;
            this.permanentRecipientsGrid.AllowUserToResizeRows = false;
            this.permanentRecipientsGrid.AutoGenerateColumns = false;
            this.permanentRecipientsGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.permanentRecipientsGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.permanentRecipientsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.permanentRecipientsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deleteRecipientColumn,
            this.recipientColumn});
            this.permanentRecipientsGrid.DataSource = this.permanentRecipientsBindingSource;
            this.permanentRecipientsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.permanentRecipientsGrid.Location = new System.Drawing.Point(3, 3);
            this.permanentRecipientsGrid.MultiSelect = false;
            this.permanentRecipientsGrid.Name = "permanentRecipientsGrid";
            this.permanentRecipientsGrid.ReadOnly = true;
            this.permanentRecipientsGrid.RowHeadersVisible = false;
            this.permanentRecipientsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.permanentRecipientsGrid.Size = new System.Drawing.Size(473, 262);
            this.permanentRecipientsGrid.TabIndex = 0;
            this.permanentRecipientsGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.permanentRecipientsGrid_CellContentClick);
            // 
            // deleteRecipientColumn
            // 
            this.deleteRecipientColumn.HeaderText = "";
            this.deleteRecipientColumn.Name = "deleteRecipientColumn";
            this.deleteRecipientColumn.ReadOnly = true;
            this.deleteRecipientColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.deleteRecipientColumn.ToolTipText = "Delete";
            this.deleteRecipientColumn.Width = 30;
            // 
            // recipientColumn
            // 
            this.recipientColumn.DataPropertyName = "RecipientAddress";
            this.recipientColumn.HeaderText = "Recipient";
            this.recipientColumn.Name = "recipientColumn";
            this.recipientColumn.ReadOnly = true;
            this.recipientColumn.Width = 300;
            // 
            // permanentRecipientsBindingSource
            // 
            this.permanentRecipientsBindingSource.AllowNew = false;
            // 
            // aboutTab
            // 
            this.aboutTab.Controls.Add(this.mainTablePanel);
            this.aboutTab.Location = new System.Drawing.Point(4, 22);
            this.aboutTab.Name = "aboutTab";
            this.aboutTab.Padding = new System.Windows.Forms.Padding(3);
            this.aboutTab.Size = new System.Drawing.Size(479, 268);
            this.aboutTab.TabIndex = 2;
            this.aboutTab.Text = "About";
            this.aboutTab.UseVisualStyleBackColor = true;
            // 
            // mainTablePanel
            // 
            this.mainTablePanel.ColumnCount = 2;
            this.mainTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.mainTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTablePanel.Controls.Add(this.logoPictureBox, 0, 0);
            this.mainTablePanel.Controls.Add(this.applicationNameLabel, 1, 0);
            this.mainTablePanel.Controls.Add(this.versionLabel, 1, 1);
            this.mainTablePanel.Controls.Add(this.copyrightLabel, 1, 2);
            this.mainTablePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainTablePanel.Location = new System.Drawing.Point(3, 3);
            this.mainTablePanel.Name = "mainTablePanel";
            this.mainTablePanel.Padding = new System.Windows.Forms.Padding(10);
            this.mainTablePanel.RowCount = 4;
            this.mainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTablePanel.Size = new System.Drawing.Size(473, 140);
            this.mainTablePanel.TabIndex = 1;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPictureBox.Image = global::Duracellko.ClipboardShareUI.Resources.Image_Logo;
            this.logoPictureBox.Location = new System.Drawing.Point(13, 13);
            this.logoPictureBox.Name = "logoPictureBox";
            this.mainTablePanel.SetRowSpan(this.logoPictureBox, 4);
            this.logoPictureBox.Size = new System.Drawing.Size(74, 114);
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // applicationNameLabel
            // 
            this.applicationNameLabel.AutoSize = true;
            this.applicationNameLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.applicationNameLabel.Location = new System.Drawing.Point(93, 10);
            this.applicationNameLabel.Name = "applicationNameLabel";
            this.applicationNameLabel.Size = new System.Drawing.Size(0, 14);
            this.applicationNameLabel.TabIndex = 1;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(93, 40);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(0, 13);
            this.versionLabel.TabIndex = 2;
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(93, 70);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(0, 13);
            this.copyrightLabel.TabIndex = 3;
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.Transparent;
            this.bottomPanel.Controls.Add(this.cancelButton);
            this.bottomPanel.Controls.Add(this.okButton);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 294);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(487, 46);
            this.bottomPanel.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(405, 11);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(324, 11);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(487, 340);
            this.Controls.Add(this.settingsTabs);
            this.Controls.Add(this.bottomPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clipboard Share Settings";
            this.settingsTabs.ResumeLayout(false);
            this.generalPage.ResumeLayout(false);
            this.generalPage.PerformLayout();
            this.authorizedSendersPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.authorizedSendersGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.authorizedSendersBindingSource)).EndInit();
            this.permanentRecipientsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.permanentRecipientsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.permanentRecipientsBindingSource)).EndInit();
            this.aboutTab.ResumeLayout(false);
            this.mainTablePanel.ResumeLayout(false);
            this.mainTablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl settingsTabs;
        private System.Windows.Forms.TabPage generalPage;
        private System.Windows.Forms.TabPage authorizedSendersPage;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.DataGridView authorizedSendersGrid;
        private System.Windows.Forms.BindingSource authorizedSendersBindingSource;
        private System.Windows.Forms.TextBox maxReceivedMessageSizeTextBox;
        private System.Windows.Forms.Label maxReceivedMessageSizeLabel;
        private System.Windows.Forms.Label maxReceivedMessageSizeKbLabel;
        private System.Windows.Forms.CheckBox runOnStartupCheckBox;
        private System.Windows.Forms.TabPage aboutTab;
        private System.Windows.Forms.TableLayoutPanel mainTablePanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label applicationNameLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.DataGridViewImageColumn deleteColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn computerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameColumn;
        private System.Windows.Forms.Label activePortLabel;
        private System.Windows.Forms.TextBox explicitPortTextBox;
        private System.Windows.Forms.RadioButton explicitPortRadioButton;
        private System.Windows.Forms.RadioButton freePortRadioButton;
        private System.Windows.Forms.TabPage permanentRecipientsPage;
        private System.Windows.Forms.DataGridView permanentRecipientsGrid;
        private System.Windows.Forms.DataGridViewImageColumn deleteRecipientColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recipientColumn;
        private System.Windows.Forms.BindingSource permanentRecipientsBindingSource;
    }
}