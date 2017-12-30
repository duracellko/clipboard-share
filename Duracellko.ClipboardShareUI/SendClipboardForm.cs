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

using Duracellko.ClipboardShare;
using Duracellko.ClipboardShare.Helpers;
using Duracellko.ClipboardShare.Logging;

namespace Duracellko.ClipboardShareUI
{
    /// <summary>
    /// Window to ask user for computer name and send clipboard content
    /// </summary>
    public partial class SendClipboardForm : Form
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SendClipboardForm"/> class.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        public SendClipboardForm(MainForm mainForm)
        {
            if (mainForm == null)
            {
                throw new ArgumentNullException("mainForm");
            }

            InitializeComponent();

            MainForm = mainForm;
        }

        #region Properties

        protected MainForm MainForm { get; private set; }

        #endregion

        #region Overriden methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Icon = Resources.Icon_ClipboardShare;
            okButton.Image = Resources.Image_Ok;
            cancelButton.Image = Resources.Image_Cancel;
        }

        #endregion

        #region Event handlers

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(addressTextBox.Text))
                {
                    if (addPermanentlyCheckBox.Checked)
                    {
                        // save permanently to configuration
                        var config = Duracellko.ClipboardShare.Configuration.ConfigurationManager.Current;
                        config.PermanentRecipients.Add(new ClipboardShare.Configuration.PermanentRecipient(addressTextBox.Text));
                        config.Save();
                    }

                    // send clipboard to specified computer
                    var clipboardContent = MainForm.ClipboardSerializer.GetClipboardContent();
                    if (clipboardContent != null)
                    {
                        AsyncHelper.ExecuteAsync((host, content) => ClipboardShareManager.Instance.SendClipboardContent(host, content), addressTextBox.Text, clipboardContent);
                    }

                    Close();
                }
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addressTextBox_TextChanged(object sender, EventArgs e)
        {
            okButton.Enabled = addressTextBox.Text != String.Empty;
        }

        #endregion

    }
}
