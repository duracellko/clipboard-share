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
using Config = Duracellko.ClipboardShare.Configuration;
using System.Runtime.InteropServices;

namespace Duracellko.ClipboardShareUI
{
    /// <summary>
    /// Main application form. Only notification tray icon is visible
    /// </summary>
    public partial class MainForm : Form
    {

        private const string HelpFileName = "Clipboard Share.pdf";

        private IClipboardShareManager clipboardShareManager;
        private Duracellko.ClipboardShare.Serialization.ClipboardSerialization clipboardSerializer;
        private string defaultComputer = null;
        private string defaultUserName = null;
        private SenderAuthorizationManager authorizationManager;
        private NotificationWindowManager notificationManager;

        private LogForm logForm;
        private SettingsForm settingsForm;

        public MainForm()
        {
            InitializeComponent();

            Icon = Resources.Icon_ClipboardShare;
        }

        #region Properties

        public IClipboardShareManager ClipboardManager
        {
            get
            {
                if (clipboardShareManager == null)
                {
                    clipboardShareManager = ClipboardShareManager.Instance;
                }
                return clipboardShareManager;
            }
        }

        public Duracellko.ClipboardShare.Serialization.ClipboardSerialization ClipboardSerializer
        {
            get
            {
                if (clipboardSerializer == null)
                {
                    clipboardSerializer = new Duracellko.ClipboardShare.Serialization.ClipboardSerialization();
                }
                return clipboardSerializer;
            }
        }

        internal NotificationWindowManager NotificationWindowManager
        {
            get
            {
                if (notificationManager == null)
                {
                    notificationManager = new NotificationWindowManager();
                }
                return notificationManager;
            }
        }

        #endregion

        #region Overriden methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            mainIcon.Icon = Resources.Icon_ClipboardShare;

            try
            {
                authorizationManager = new SenderAuthorizationManager(this);
                InitializeConfiguration();
                StartClipboardShare();
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            try
            {
                ClipboardManager.Dispose();
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Visible = false;
        }

        #endregion

        #region Event handlers

        private void ClipboardManager_ClipboardContentReceived(object sender, ClipboardContentReceivedEventArgs e)
        {
            SetClipboard(e.Content);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mainIcon_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                // display list of available clipboard names in network
                if (e.Button == MouseButtons.Left)
                {
                    var clipboardNamesForm = new ClipboardNamesForm();
                    clipboardNamesForm.ClipboardNameSelected += new ClipboardNameSelectedEventHandler(clipboardNamesForm_ClipboardNameSelected);
                    clipboardNamesForm.PermanentRecipientSelected += new PermanentRecipientSelectedEventHandler(clipboardNamesForm_PermanentRecipientSelected);
                    clipboardNamesForm.NotInListSelected += new EventHandler(clipboardNamesForm_NotInListSelected);
                    clipboardNamesForm.Show(this);
                }
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }
        }

        private void clipboardNamesForm_ClipboardNameSelected(object sender, ClipboardNameSelectedEventArgs e)
        {
            try
            {
                // send clipboard to selected peer
                var clipboardContent = ClipboardSerializer.GetClipboardContent();
                if (clipboardContent != null)
                {
                    AsyncHelper.ExecuteAsync((id, content) => ClipboardShareManager.Instance.SendClipboardContent(id, content), e.ClipboardName.Id, clipboardContent);
                }
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }
        }

        private void clipboardNamesForm_PermanentRecipientSelected(object sender, PermanentRecipientSelectedEventArgs e)
        {
            try
            {
                // send clipboard to selected peer
                var clipboardContent = ClipboardSerializer.GetClipboardContent();
                if (clipboardContent != null)
                {
                    AsyncHelper.ExecuteAsync((host, content) => ClipboardShareManager.Instance.SendClipboardContent(host, content), e.PermanentRecipient.RecipientAddress, clipboardContent);
                }
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }            
        }

        private void clipboardNamesForm_NotInListSelected(object sender, EventArgs e)
        {
            // display form to enter computer name
            var sendClipboardForm = new SendClipboardForm(this);
            NotificationWindowManager.ShowForm(sendClipboardForm, this);
            sendClipboardForm.Activate();
        }

        private void mainIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    // send clipboard content to default peer
                    if (!String.IsNullOrEmpty(defaultComputer) && !String.IsNullOrEmpty(defaultUserName))
                    {
                        var clipboardContent = ClipboardSerializer.GetClipboardContent();
                        if (clipboardContent != null)
                        {
                            var clipboardNames = ClipboardManager.GetRemoteClipboardNames();
                            var defaultClipboardName = clipboardNames.FirstOrDefault(cn => String.Compare(cn.Computer, defaultComputer, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                                String.Compare(cn.User, defaultUserName, StringComparison.InvariantCultureIgnoreCase) == 0);

                            if (defaultClipboardName != null)
                            {
                                AsyncHelper.ExecuteAsync((id, content) => ClipboardShareManager.Instance.SendClipboardContent(id, content), defaultClipboardName.Id, clipboardContent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLogForm();
        }

        private void logForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            logForm = null;
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettingsForm();
        }

        private void settingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            settingsForm = null;
        }

        private void iconMenu_Opening(object sender, CancelEventArgs e)
        {
            // create list of available clipboard names to allow select default one.
            CreateSetDefaultSubmenuItems();
        }

        private void setDefaultSubmenuItem_Click(object sender, EventArgs e)
        {
            // save selected default recipient to configuration
            var menuItem = (ToolStripMenuItem)sender;
            var clipboardName = menuItem.Tag as ClipboardName;
            if (clipboardName != null)
            {
                defaultComputer = clipboardName.Computer;
                defaultUserName = clipboardName.User;

                try
                {
                    var config = Config.ConfigurationManager.Current;
                    config.DefaultComputerName = defaultComputer;
                    config.DefaultUserName = defaultUserName;
                    config.Save();
                }
                catch (Exception ex)
                {
                    LogManager.Current.LogException(ex);
                }
            }
        }

        private void mainIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            // show log, when notification icon baloon is clicked
            ShowLogForm();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Restarts the clipboard sharing service.
        /// </summary>
        public void RestartService()
        {
            var stopDelegate = new Action(() =>
                {
                    ClipboardShareManager.Instance.Stop();
                });

            var stopHandler = new AsyncCallback(ar =>
                {
                    try
                    {
                        stopDelegate.EndInvoke(ar);
                    }
                    catch (Exception ex)
                    {
                        LogManager.Current.LogException(ex);
                    }

                    StartClipboardShare();
                });

            stopDelegate.BeginInvoke(stopHandler, null);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Sets clipboard data to data in <see cref="ClipboardContent"/> object.
        /// </summary>
        /// <param name="content">The content.</param>
        protected void SetClipboard(ClipboardContent content)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ClipboardContent>(SetClipboard), content);
            }
            else
            {
                ClipboardSerializer.SetClipboard(content);
            }
        }

        #endregion

        #region Private methods

        // load configuration or create new identity
        private void InitializeConfiguration()
        {
            var config = Config.ConfigurationManager.Current;
            config.Load();
            
            bool configChanged = false;
            if (String.IsNullOrEmpty(config.UserName))
            {
                config.UserName = Environment.UserName;
                configChanged = true;
            }
            if (config.PrivateKey == null || config.PrivateKey.Length == 0)
            {
                config.PrivateKey = ClipboardManager.MessageProvider.GeneratePrivateKey();
                configChanged = true;
            }

            if (configChanged)
            {
                config.Save();
            }

            defaultComputer = config.DefaultComputerName;
            defaultUserName = config.DefaultUserName;

            authorizationManager.AddAuthorizedSenders(config.AuthorizedSenders);
        }
        
        // configure and start clipboard sharing service
        private void StartClipboardShare()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(StartClipboardShare));
            }
            else
            {
                var config = Config.ConfigurationManager.Current;

                // display warning if PRNP is not installed
                if (!System.ServiceModel.NetPeerTcpBinding.IsPnrpAvailable)
                {
                    LogManager.Current.LogWarning(Resources.Warning_PnrpNotInstalled);
                    mainIcon.ShowBalloonTip(15000, Text, Resources.Warning_PnrpNotInstalled, ToolTipIcon.Warning);
                }

                // configure service
                ClipboardManager.SetIdentity(Environment.MachineName, config.UserName, config.PrivateKey);
                ClipboardManager.ClipboardContentReceived += new ClipboardContentReceivedHandler(ClipboardManager_ClipboardContentReceived);
                ClipboardManager.MaxReceivedMessageSize = config.MaxReceivedMessageSize;
                ClipboardManager.ServicePort = config.ServicePort;
                ClipboardManager.AuthorizationManager = authorizationManager;

                // start service
                var startDelegate = new Action(ClipboardManager.Start);
                startDelegate.BeginInvoke(ar =>
                {
                    try
                    {
                        startDelegate.EndInvoke(ar);
                    }
                    catch (Exception ex)
                    {
                        LogManager.Current.LogException(ex);
                        ShowErrorStartingServiceNotification();
                    }
                }, null);
            }
        }

        // display error when service starting failed
        private void ShowErrorStartingServiceNotification()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ShowErrorStartingServiceNotification));
            }
            else
            {
                mainIcon.ShowBalloonTip(15000, Resources.Error_Title, Resources.Error_StartingService, ToolTipIcon.Error);
            }
        }

        private void ShowLogForm()
        {
            if (logForm == null)
            {
                logForm = new LogForm();
                logForm.FormClosed += new FormClosedEventHandler(logForm_FormClosed);
                logForm.Show(this);
            }
            else
            {
                logForm.Activate();
            }
        }

        private void ShowSettingsForm()
        {
            if (settingsForm == null)
            {
                settingsForm = new SettingsForm();
                settingsForm.MainForm = this;
                settingsForm.AuthorizationManager = authorizationManager;
                settingsForm.FormClosed += new FormClosedEventHandler(settingsForm_FormClosed);
                settingsForm.Show(this);
            }
            else
            {
                settingsForm.Activate();
            }
        }

        // create submenu items for available clipboard sharing peers to allow select default one
        private void CreateSetDefaultSubmenuItems()
        {
            setDefaultMenuItem.DropDownItems.Clear();

            var clipboardNames = ClipboardManager.GetRemoteClipboardNames();
            bool selectedClipboardName = false;
            foreach (var clipboardName in clipboardNames.OrderBy(cn => cn.Computer).ThenBy(cn => cn.User))
            {
                var menuItem = new ToolStripMenuItem(clipboardName.ToString());
                menuItem.Tag = clipboardName;
                if (!selectedClipboardName && String.Compare(clipboardName.Computer, defaultComputer, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                    String.Compare(clipboardName.User, defaultUserName, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    selectedClipboardName = true;
                    menuItem.Checked = true;
                }
                menuItem.Click += new EventHandler(setDefaultSubmenuItem_Click);
                setDefaultMenuItem.DropDownItems.Add(menuItem);
            }

            setDefaultMenuItem.Enabled = clipboardNames.Length > 0;
        }

        // open "Clipboard Share.pdf" file
        private void ShowHelp()
        {
            string helpFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HelpFileName);

            try
            {
                var helpStartInfo = new System.Diagnostics.ProcessStartInfo(helpFilePath);
                helpStartInfo.UseShellExecute = true;
                System.Diagnostics.Process.Start(helpStartInfo);
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.Error_HelpNotFound, Resources.Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

    }
}
