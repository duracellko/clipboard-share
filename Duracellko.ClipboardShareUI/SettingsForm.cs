/*
The MIT License

Copyright (c) 2010 Rasto Novotny

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Reflection;

using Duracellko.ClipboardShare;
using Config = Duracellko.ClipboardShare.Configuration;
using Duracellko.ClipboardShare.Logging;
using Duracellko.ClipboardShare.Helpers;

namespace Duracellko.ClipboardShareUI
{
    /// <summary>
    /// Form that allows user to edit application settings.
    /// </summary>
    public partial class SettingsForm : Form
    {

        private AuthorizedSendersCollection authorizedSenders;
        private PermanentRecipientsCollection permanentRecipients;

        public SettingsForm()
        {
            InitializeComponent();

            Icon = Resources.Icon_ClipboardShare;
        }

        public MainForm MainForm { get; set; }

        public SenderAuthorizationManager AuthorizationManager { get; set; }

        #region Overriden methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            authorizedSendersGrid.AutoGenerateColumns = false;
            permanentRecipientsGrid.AutoGenerateColumns = false;
            deleteColumn.Image = Resources.Image_Delete;
            deleteRecipientColumn.Image = Resources.Image_Delete;

            applicationNameLabel.Text = Application.ProductName;
            versionLabel.Text = Application.ProductVersion;
            copyrightLabel.Text = AssemblyCopyright;

            try
            {
                // display user settings
                LoadData();
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }
        }

        #endregion

        #region Event handlers

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                // validate and save user settings
                if (ValidateData())
                {
                    SaveData();
                    Close();
                }
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
                MessageBox.Show(Resources.Error_ConfigurationSaveFailed, Resources.Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void explicitPortRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            explicitPortTextBox.Enabled = explicitPortRadioButton.Checked;
        }

        private void authorizedSendersGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == deleteColumn.Index)
            {
                authorizedSendersGrid.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void permanentRecipientsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == deleteRecipientColumn.Index)
            {
                permanentRecipientsGrid.Rows.RemoveAt(e.RowIndex);
            }
        }

        #endregion

        #region Private methods

        // display user settings
        private void LoadData()
        {
            var config = Config.ConfigurationManager.Current;
            
            userNameTextBox.Text = config.UserName;
            maxReceivedMessageSizeTextBox.Text = (config.MaxReceivedMessageSize / 1024).ToString("N0");

            bool explicitPort = config.ServicePort != 0;
            freePortRadioButton.Checked = !explicitPort;
            explicitPortRadioButton.Checked = explicitPort;
            explicitPortTextBox.Enabled = explicitPort;
            explicitPortTextBox.Text = config.ServicePort.ToString();
            portTextBox.Text = ClipboardShareManager.Instance.ListeningPort.ToString();

            authorizedSenders = new AuthorizedSendersCollection(config.AuthorizedSenders);
            authorizedSendersBindingSource.DataSource = authorizedSenders;

            permanentRecipients = new PermanentRecipientsCollection(config.PermanentRecipients);
            permanentRecipientsBindingSource.DataSource = permanentRecipients;

            runOnStartupCheckBox.Checked = RegistryConfiguration.RunOnStartup;
        }

        // save user settings
        private void SaveData()
        {
            bool configurationChanged = false;
            bool restartNeeded = false;

            var config = Config.ConfigurationManager.Current;
            if (userNameTextBox.Text != config.UserName)
            {
                config.UserName = userNameTextBox.Text;
                configurationChanged = true;
                restartNeeded = true;
            }

            int maxReceivedMessageSize = int.Parse(maxReceivedMessageSizeTextBox.Text, NumberStyles.Number) * 1024;
            if (maxReceivedMessageSize != config.MaxReceivedMessageSize)
            {
                config.MaxReceivedMessageSize = maxReceivedMessageSize;
                configurationChanged = true;
                restartNeeded = true;
            }

            int servicePort = 0;
            if (explicitPortRadioButton.Checked)
            {
                servicePort = int.Parse(explicitPortTextBox.Text, NumberStyles.Number);
            }
            if (servicePort != config.ServicePort)
            {
                config.ServicePort = servicePort;
                configurationChanged = true;
                restartNeeded = true;
            }

            foreach (var sender in authorizedSenders.RemovedItems)
            {
                config.AuthorizedSenders.Remove(sender);
                configurationChanged = true;

                if (AuthorizationManager != null)
                {
                    AuthorizationManager.RemoveAuthorizedSender(sender);
                }
            }

            foreach (var recipient in permanentRecipients.RemovedItems)
            {
                config.PermanentRecipients.Remove(recipient);
                configurationChanged = true;
            }

            if (configurationChanged)
            {
                config.Save();
            }

            RegistryConfiguration.RunOnStartup = runOnStartupCheckBox.Checked;

            if (restartNeeded)
            {
                MainForm.RestartService();
            }
        }

        // validate user settings
        private bool ValidateData()
        {
            if (userNameTextBox.Text == String.Empty)
            {
                MessageBox.Show(Resources.Error_EmptyUserName, Resources.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int val = 0;
            if (!int.TryParse(maxReceivedMessageSizeTextBox.Text, NumberStyles.Number, null, out val) || val <= 0)
            {
                MessageBox.Show(Resources.Error_InvalidMaxReceivedMessageSize, Resources.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (explicitPortRadioButton.Checked)
            {
                if (!int.TryParse(explicitPortTextBox.Text, NumberStyles.Number, null, out val) || val <= 0)
                {
                    MessageBox.Show(Resources.Error_InvalidServicePort, Resources.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        #endregion

        #region Inner types

        /// <summary>
        /// Collection of authorized senders tracking removed items.
        /// </summary>
        private class AuthorizedSendersCollection : Collection<Config.AuthorizedSender>
        {

            private List<Config.AuthorizedSender> removedItems = new List<Config.AuthorizedSender>();
            public AuthorizedSendersCollection()
                : base()
            {
            }

            public AuthorizedSendersCollection(IEnumerable<Config.AuthorizedSender> items)
                : base(items.ToList())
            {
            }

            public IEnumerable<Config.AuthorizedSender> RemovedItems
            {
                get
                {
                    return removedItems;
                }
            }

            protected override void RemoveItem(int index)
            {
                removedItems.Add(this[index]);
                base.RemoveItem(index);
            }

            protected override void ClearItems()
            {
                removedItems.AddRange(this);
                base.ClearItems();
            }

        }

        /// <summary>
        /// Collection of permanent recipients tracking removed items.
        /// </summary>
        private class PermanentRecipientsCollection : Collection<Config.PermanentRecipient>
        {

            private List<Config.PermanentRecipient> removedItems = new List<Config.PermanentRecipient>();
            public PermanentRecipientsCollection()
                : base()
            {
            }

            public PermanentRecipientsCollection(IEnumerable<Config.PermanentRecipient> items)
                : base(items.ToList())
            {
            }

            public IEnumerable<Config.PermanentRecipient> RemovedItems
            {
                get
                {
                    return removedItems;
                }
            }

            protected override void RemoveItem(int index)
            {
                removedItems.Add(this[index]);
                base.RemoveItem(index);
            }

            protected override void ClearItems()
            {
                removedItems.AddRange(this);
                base.ClearItems();
            }

        }

        #endregion

    }
}
