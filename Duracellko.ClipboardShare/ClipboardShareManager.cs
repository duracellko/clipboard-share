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
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;

using Duracellko.ClipboardShare.Service;
using Duracellko.ClipboardShare.IdentityManager;
using Duracellko.ClipboardShare.Helpers;

namespace Duracellko.ClipboardShare
{
    /// <summary>
    /// Main clipboard share service. Provides functionality for user interface and communication service.
    /// </summary>
    public class ClipboardShareManager : IClipboardShareManager
    {

        internal const string ClipboardDataNamespace = "urn://duracellko/ClipboardShare/Data";
        private const string clipboardShareServicePath = "ClipboardShare";

        private bool disposed = false;
        private IClipboardMessageProvider messageProvider;

        private byte[] privateKey;
        private ClipboardIdentity identity;
        private ReaderWriterLockSlim identityLock = new ReaderWriterLockSlim();

        private ClipboardShareServiceHost serviceHost;
        private IdentityServiceHost identityHost;
        private IdentityService identityService;
        private object hostLock = new object();

        private ClipboardContentReceivedHandler handleClipboardContentReceived;
        private object handleClipboardContentReceivedLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardShareManager"/> class.
        /// </summary>
        public ClipboardShareManager()
        {
            messageProvider = new ClipboardMessageProvider();
            MaxReceivedMessageSize = Service.ClipboardShareServiceHost.DefaultMaxReceivedMessageSize;
        }

        #region Singleton

        private static IClipboardShareManager instance;
        private static object instanceLock = new object();

        /// <summary>
        /// Gets current instance of <see cref="IClipboardShareManager"/>.
        /// </summary>
        public static IClipboardShareManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new ClipboardShareManager();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        #region IClipboardShareManager Members

        /// <summary>
        /// Notifies that clipboard message has received. The message is verified and authorized.
        /// </summary>
        public event ClipboardContentReceivedHandler ClipboardContentReceived
        {
            add
            {
                lock (handleClipboardContentReceivedLock)
                {
                    handleClipboardContentReceived += value;
                }
            }
            remove
            {
                lock (handleClipboardContentReceivedLock)
                {
                    handleClipboardContentReceived -= value;
                }
            }
        }

        /// <summary>
        /// Gets the message provider.
        /// </summary>
        /// <value>The message provider.</value>
        public IClipboardMessageProvider MessageProvider
        {
            get
            {
                return messageProvider;
            }
        }

        /// <summary>
        /// Gets or sets the authorization manager.
        /// </summary>
        /// <value>The authorization manager.</value>
        public ISenderAuthorizationManager AuthorizationManager { get; set; }

        /// <summary>
        /// Gets or sets the size of the max received message.
        /// </summary>
        /// <value>The size of the max received message.</value>
        public int MaxReceivedMessageSize { get; set; }

        /// <summary>
        /// Gets or sets the TCP port of the service or 0 if eny free port should be used.
        /// </summary>
        /// <value>The service post.</value>
        public int ServicePort { get; set; }

        /// <summary>
        /// Gets the listening TCP post or 0 if service is stopped.
        /// </summary>
        /// <value>The listening post.</value>
        public int ListeningPort
        {
            get
            {
                lock (hostLock)
                {
                    return serviceHost.ListeningPort;
                }
            }
        }

        /// <summary>
        /// Sets the clipboard share identity.
        /// </summary>
        /// <param name="computer">The computer name.</param>
        /// <param name="username">The user name.</param>
        /// <param name="privateKey">The private key.</param>
        public void SetIdentity(string computer, string username, byte[] privateKey)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ClipboardShareManager");
            }

            if (String.IsNullOrEmpty(computer))
            {
                throw new ArgumentNullException("computer");
            }
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("user");
            }
            if (privateKey == null)
            {
                throw new ArgumentNullException("privateKey");
            }

            try
            {
                Monitor.Enter(hostLock);
                identityLock.EnterWriteLock();

                if (serviceHost != null)
                {
                    throw new InvalidOperationException(Resources.Error_SetIdentityWhenServiceIsRunning);
                }

                var id = new ClipboardIdentity();
                id.Id = Guid.NewGuid();
                id.Computer = computer;
                id.User = username;
                id.PublicKey = MessageProvider.GetPublicKey(privateKey);

                this.identity = id;
                this.privateKey = privateKey;
            }
            finally
            {
                identityLock.ExitWriteLock();
                Monitor.Exit(hostLock);
            }
        }

        /// <summary>
        /// Gets the clipboard share identity.
        /// </summary>
        /// <returns></returns>
        public ClipboardIdentity GetIdentity()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ClipboardShareManager");
            }

            try
            {
                identityLock.EnterReadLock();

                return ClipboardShareHelper.CreateCopy(identity);
            }
            finally
            {
                identityLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Starts clipboard share service, advertise own identity to peers and starts enumerating of clipboard share peers.
        /// </summary>
        public void Start()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ClipboardShareManager");
            }

            lock (hostLock)
            {
                if (identity == null)
                {
                    throw new InvalidOperationException(Resources.Error_StartWithoutIdentity);
                }

                // start clipboard share service
                if (serviceHost == null)
                {
                    serviceHost = new ClipboardShareServiceHost(ServicePort);
                    serviceHost.MaxReceivedMessageSize = MaxReceivedMessageSize;
                    serviceHost.Open();

                    try
                    {
                        identityLock.EnterWriteLock();
                        identity.Port = serviceHost.ListeningPort;
                    }
                    finally
                    {
                        identityLock.ExitWriteLock();
                    }
                }

                // start clipboard share identity service
                if (identityHost == null && NetPeerTcpBinding.IsPnrpAvailable)
                {
                    identityService = new IdentityService();
                    identityService.LocalIdentity = ClipboardShareHelper.CreateCopy(identity);
                    identityHost = new IdentityServiceHost(identityService);
                    identityHost.Open();
                }
            }
        }

        /// <summary>
        /// Stops clipboard share service and stop enumerating of clipboard share peers.
        /// </summary>
        public void Stop()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ClipboardShareManager");
            }

            lock (hostLock)
            {
                // notify peers about service stopping
                if (identity != null && identityService != null)
                {
                    try
                    {
                        identityService.Disconnect();
                    }
                    catch (Exception)
                    {
                        //do nothing
                    }
                }

                // stop clipboard share service
                if (serviceHost != null)
                {
                    ClipboardShareHelper.CloseCommunicationObject(serviceHost);
                    serviceHost = null;
                }

                // stop identity service
                if (identityHost != null)
                {
                    ClipboardShareHelper.CloseCommunicationObject(identityHost);
                    identityService.Dispose();
                    identityHost = null;
                    identityService = null;
                }

                try
                {
                    identityLock.EnterWriteLock();
                    if (identity != null)
                    {
                        identity.Port = 0;
                    }
                }
                finally
                {
                    identityLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Sends content of the clipboard to clipboard with specified ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="content">The clipboard content.</param>
        public void SendClipboardContent(Guid id, ClipboardContent content)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ClipboardShareManager");
            }

            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            ClipboardIdentity identity = null;
            byte[] privateKey = null;
            ClipboardIdentity remoteIdentity = null;
            lock (hostLock)
            {
                identity = ClipboardShareHelper.CreateCopy(this.identity);
                privateKey = this.privateKey;
                remoteIdentity = identityService.GetIdentity(id);
            }

            if (identity != null && remoteIdentity != null)
            {
                var message = CreateMessage(identity, remoteIdentity, content, privateKey);

                var remoteUriBuilder = new UriBuilder(Uri.UriSchemeNetTcp, remoteIdentity.Computer, remoteIdentity.Port, clipboardShareServicePath);
                var client = new ClipboardShareClient(remoteUriBuilder.Uri);
                try
                {
                    client.Open();
                    client.SetClipboard(message);
                }
                finally
                {
                    ClipboardShareHelper.CloseCommunicationObject(client);
                }
            }
        }

        /// <summary>
        /// Sends the content of the clipboard to specific computer.
        /// </summary>
        /// <param name="host">The host name and port separated by ":".</param>
        /// <param name="content">The clipboard content.</param>
        public void SendClipboardContent(string host, ClipboardContent content)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ClipboardShareManager");
            }

            if (String.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            ClipboardIdentity identity = null;
            byte[] privateKey = null;
            try
            {
                identityLock.EnterReadLock();
                identity = ClipboardShareHelper.CreateCopy(this.identity);
                privateKey = this.privateKey;
            }
            finally
            {
                identityLock.ExitReadLock();
            }

            if (identity != null)
            {
                var client = new ClipboardShareClient(String.Format("{0}://{1}/{2}", Uri.UriSchemeNetTcp, host, clipboardShareServicePath));
                try
                {
                    client.Open();

                    var remoteIdentity = client.GetIdentity();

                    // register identity in identity service
                    lock (hostLock)
                    {
                        if (identityService != null)
                        {
                            identityService.RegisterIdentity(remoteIdentity, identity.Id);
                        }
                    }

                    var message = CreateMessage(identity, remoteIdentity, content, privateKey);
                    client.SetClipboard(message);
                }
                finally
                {
                    ClipboardShareHelper.CloseCommunicationObject(client);
                }
            }
        }

        /// <summary>
        /// Decrypt message, verifies signature, authorize message and process received clipboard content.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ReceiveMessage(ClipboardMessage message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ClipboardShareManager");
            }

            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            ClipboardIdentity id;
            byte[] privateKey;
            try
            {
                identityLock.EnterReadLock();
                id = ClipboardShareHelper.CreateCopy(this.identity);
                privateKey = this.privateKey;
            }
            finally
            {
                identityLock.ExitReadLock();
            }

            var recipientId = message.Recipient;
            var senderId = message.Sender;

            // verify, if recipient is me
            if (id != null && recipientId != null && id.Id == recipientId.Id && id.PublicKey != null && recipientId.PublicKey != null && ClipboardShareHelper.CompareBytes(id.PublicKey, recipientId.PublicKey))
            {
                // authorize sender
                if (senderId != null && (AuthorizationManager == null || AuthorizationManager.AuthorizeSender(senderId)))
                {
                    if (message.EncryptedClipboardContent != null)
                    {
                        // decrypt message and verify signature
                        MessageProvider.DecryptMessage(message, privateKey);
                        if (MessageProvider.VerifySignature(message))
                        {
                            var content = MessageProvider.DeserializeClipboardContent(message.EncryptedClipboardContent);
                            OnClipboardContentReceived(new ClipboardContentReceivedEventArgs(content));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the remote clipboard names.
        /// </summary>
        /// <returns>List of clipboard share peers.</returns>
        public ClipboardName[] GetRemoteClipboardNames()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ClipboardShareManager");
            }

            IdentityService identityService = this.identityService;
            if (identityService != null)
            {
                return identityService.GetClipboardNames();
            }
            else
            {
                return new ClipboardName[0];
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (!disposed)
            {
                Stop();
                identityLock.Dispose();

                disposed = true;
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Raises the <see cref="E:ClipboardContentReceived"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Duracellko.ClipboardShare.ClipboardContentReceivedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnClipboardContentReceived(ClipboardContentReceivedEventArgs e)
        {
            lock (handleClipboardContentReceivedLock)
            {
                if (handleClipboardContentReceived != null)
                {
                    handleClipboardContentReceived(this, e);
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates the clipboard message with encrypted and signed content.
        /// </summary>
        private ClipboardMessage CreateMessage(ClipboardIdentity sender, ClipboardIdentity recipient, ClipboardContent content, byte[] privateKey)
        {
            var message = new ClipboardMessage();
            message.Sender = sender;
            message.Recipient = recipient;
            message.EncryptedClipboardContent = MessageProvider.SerializeClipboardContent(content);
            MessageProvider.SignMessage(message, privateKey);
            MessageProvider.EncryptMessage(message);
            return message;
        }


        #endregion

    }
}
