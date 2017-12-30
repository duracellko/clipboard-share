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
using System.Threading;

using Duracellko.ClipboardShare;
using Config = Duracellko.ClipboardShare.Configuration;

namespace Duracellko.ClipboardShareUI
{
    /// <summary>
    /// Window requesting user to authorize sender.
    /// </summary>
    public partial class AuthorizeSenderForm : Form
    {

        private const int movingStep = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeSenderForm"/> class.
        /// </summary>
        public AuthorizeSenderForm()
        {
            InitializeComponent();
        }

        private Bitmap backgroundBitmap;

        #region Properties

        /// <summary>
        /// Gets or sets the identity to authorize.
        /// </summary>
        public ClipboardIdentity Identity { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="P:Identity"/> is authorized.
        /// </summary>
        /// <value><c>true</c> if authorized; otherwise, <c>false</c>.</value>
        public bool Authorized { get; private set; }

        /// <summary>
        /// Gets or sets the authorization manager.
        /// </summary>
        internal SenderAuthorizationManager AuthorizationManager { get; set; }

        /// <summary>
        /// Gets or sets the authorization completed event to notify, that user completed action.
        /// </summary>
        internal EventWaitHandle AuthorizationCompletedEvent { get; set; }

        /// <summary>
        /// Gets or sets the required position, that should be window sliding to.
        /// </summary>
        public Rectangle RequiredPosition { get; set; }

        #endregion

        #region Overriden methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // display identity text
            if (Identity != null)
            {
                nameLabel.Text = Identity.ToString();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // notify, that user completed action
            try
            {
                if (AuthorizationCompletedEvent != null)
                {
                    AuthorizationCompletedEvent.Set();
                }
            }
            catch (Exception)
            {
                // do nothing
            }

            if (backgroundBitmap != null)
            {
                backgroundBitmap.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // paint background
            if (backgroundBitmap == null)
            {
                backgroundBitmap = Resources.Image_AuthorizationBackground;
            }
            e.Graphics.DrawImage(backgroundBitmap, ClientRectangle);
        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                // do not activate window, when shown
                return true;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams result = base.CreateParams;
                // window without titlebar
                result.Style |= WinApi.WS_POPUP;
                // do not activate window, when shown
                result.ExStyle |= WinApi.WS_EX_NOACTIVATE;
                return result;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Shows this authorization request form.
        /// </summary>
        /// <param name="ownerForm">The owner form.</param>
        public void ShowAuthorizationRequest(Form ownerForm)
        {
            if (ownerForm.InvokeRequired)
            {
                ownerForm.Invoke(new Action<Form>(ShowAuthorizationRequest), ownerForm);
            }
            else
            {
                Show(ownerForm);
            }
        }

        /// <summary>
        /// Starts sliding animation to <see cref="P:RequiredPosition"/>.
        /// </summary>
        public void StartAnimation()
        {
            if (Bounds != RequiredPosition)
            {
                visibilityTimer.Enabled = true;
            }
        }

        #endregion

        #region Event handlers

        private void acceptLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // set sender authorized
            Authorized = true;

            // save to configuration file
            var config = Config.ConfigurationManager.Current;
            var authorizedSender = new Config.AuthorizedSender(Identity);
            config.AuthorizedSenders.Add(authorizedSender);
            config.Save();

            Close();
        }

        private void rejectLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // set sender rejected

            Authorized = false;
            Close();
        }

        private void visibilityTimer_Tick(object sender, EventArgs e)
        {
            // move slowly to required position

            bool isFinalPosition = true;

            if (Height < RequiredPosition.Height)
            {
                Height += movingStep;
                if (Height >= RequiredPosition.Height)
                {
                    Height = RequiredPosition.Height;
                }
                else
                {
                    isFinalPosition = false;
                }
            }

            if (Left < RequiredPosition.X)
            {
                Left += movingStep;
                if (Left >= RequiredPosition.X)
                {
                    Left = RequiredPosition.X;
                }
                else
                {
                    isFinalPosition = false;
                }
            }
            else if (Left > RequiredPosition.X)
            {
                Left -= movingStep;
                if (Left <= RequiredPosition.X)
                {
                    Left = RequiredPosition.X;
                }
                else
                {
                    isFinalPosition = false;
                }
            }

            if (Top < RequiredPosition.Y)
            {
                Top += movingStep;
                if (Top >= RequiredPosition.Y)
                {
                    Top = RequiredPosition.Y;
                }
                else
                {
                    isFinalPosition = false;
                }
            }
            else if (Top > RequiredPosition.Y)
            {
                Top -= movingStep;
                if (Top <= RequiredPosition.Y)
                {
                    Top = RequiredPosition.Y;
                }
                else
                {
                    isFinalPosition = false;
                }
            }

            if (isFinalPosition)
            {
                visibilityTimer.Enabled = false;
            }
        }

        #endregion

    }
}
