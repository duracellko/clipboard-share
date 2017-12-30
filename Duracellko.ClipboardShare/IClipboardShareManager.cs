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
using System.ServiceModel;

namespace Duracellko.ClipboardShare
{
    /// <summary>
    /// Main clipboard share service. Provides functionality for user interface and communication service.
    /// </summary>
    public interface IClipboardShareManager : IDisposable
    {

        /// <summary>
        /// Notifies that clipboard message has received. The message is verified and authorized.
        /// </summary>
        event ClipboardContentReceivedHandler ClipboardContentReceived;

        /// <summary>
        /// Gets the message provider.
        /// </summary>
        /// <value>The message provider.</value>
        IClipboardMessageProvider MessageProvider { get; }

        /// <summary>
        /// Gets or sets the authorization manager.
        /// </summary>
        /// <value>The authorization manager.</value>
        ISenderAuthorizationManager AuthorizationManager { get; set; }

        /// <summary>
        /// Gets or sets the size of the max received message.
        /// </summary>
        /// <value>The size of the max received message.</value>
        int MaxReceivedMessageSize { get; set; }

        /// <summary>
        /// Gets or the TCP port of the service.
        /// </summary>
        /// <value>The service post.</value>
        int ServicePort { get; set; }

        /// <summary>
        /// Gets the listening TCP port or 0 if service is stopped.
        /// </summary>
        /// <value>The listening post.</value>
        int ListeningPort { get; }

        /// <summary>
        /// Sets the clipboard share identity.
        /// </summary>
        /// <param name="computer">The computer name.</param>
        /// <param name="username">The user name.</param>
        /// <param name="privateKey">The private key.</param>
        void SetIdentity(string computer, string username, byte[] privateKey);

        /// <summary>
        /// Gets the clipboard share identity.
        /// </summary>
        /// <returns></returns>
        ClipboardIdentity GetIdentity();

        /// <summary>
        /// Starts clipboard share service, advertise own identity to peers and starts enumerating of clipboard share peers.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops clipboard share service and stop enumerating of clipboard share peers.
        /// </summary>
        void Stop();

        /// <summary>
        /// Sends content of the clipboard to clipboard with specified ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="content">The clipboard content.</param>
        void SendClipboardContent(Guid id, ClipboardContent content);

        /// <summary>
        /// Sends the content of the clipboard to specific computer.
        /// </summary>
        /// <param name="host">The host name and port separated by &quot;:&quot;.</param>
        /// <param name="content">The clipboard content.</param>
        void SendClipboardContent(string host, ClipboardContent content);

        /// <summary>
        /// Decrypt message, verifies signature, authorize message and process received clipboard content.
        /// </summary>
        /// <param name="message">The message.</param>
        void ReceiveMessage(ClipboardMessage message);

        /// <summary>
        /// Gets the remote clipboard names.
        /// </summary>
        /// <returns>List of clipboard share peers.</returns>
        ClipboardName[] GetRemoteClipboardNames();

    }
}
