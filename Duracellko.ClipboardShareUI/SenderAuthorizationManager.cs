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

using Duracellko.ClipboardShare;
using Duracellko.ClipboardShare.Configuration;

namespace Duracellko.ClipboardShareUI
{
    /// <summary>
    /// Implements authorization of senders. Authorization request is shown to user. When accepted, then all future requests are authorized.
    /// </summary>
    public class SenderAuthorizationManager : ISenderAuthorizationManager
    {

        private HashSet<ClipboardIdentity> authorizedSenders = new HashSet<ClipboardIdentity>(new IdentityComparer());
        private object authorizedSendersLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="SenderAuthorizationManager"/> class.
        /// </summary>
        /// <param name="mainForm">The application main form.</param>
        public SenderAuthorizationManager(MainForm mainForm)
        {
            if (mainForm == null)
            {
                throw new ArgumentNullException("mainForm");
            }

            MainForm = mainForm;
        }

        #region Properties

        /// <summary>
        /// Gets the application main form.
        /// </summary>
        public MainForm MainForm { get; private set; }

        #endregion

        #region ISenderAuthorizationManager Members

        /// <summary>
        /// Authorizes the sender to accept his clipboard messages.
        /// </summary>
        /// <param name="identity">The sender's identity.</param>
        /// <returns>
        /// 	<c>true</c>, if it is allowed to receive messages from specified sender; otherwise <c>false</c>.
        /// </returns>
        public bool AuthorizeSender(ClipboardIdentity identity)
        {
            if (identity == null)
            {
                return false;
            }

            // if identity is in authorized list, then authorize.
            lock (authorizedSendersLock)
            {
                if (authorizedSenders.Contains(identity))
                {
                    return true;
                }
            }

            // show window to user and wait until user accepts or rejects the identity.
            bool result = false;
            using (var authorizationCompletedEvent = new ManualResetEvent(false))
            {
                var authorizeForm = new AuthorizeSenderForm();
                authorizeForm.AuthorizationManager = this;
                authorizeForm.AuthorizationCompletedEvent = authorizationCompletedEvent;
                authorizeForm.Identity = identity;
                MainForm.NotificationWindowManager.ShowForm(authorizeForm, MainForm);

                authorizationCompletedEvent.WaitOne();

                result = authorizeForm.Authorized;
            }

            if (result)
            {
                lock (authorizedSendersLock)
                {
                    authorizedSenders.Add(identity);
                }
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds new authorized senders to automatically authorize.
        /// </summary>
        /// <param name="senders">The senders collection.</param>
        public void AddAuthorizedSenders(IEnumerable<AuthorizedSender> senders)
        {
            if (authorizedSenders == null)
            {
                throw new ArgumentNullException("senders");
            }

            lock (authorizedSendersLock)
            {
                foreach (var sender in senders)
                {
                    var identity = new ClipboardIdentity();
                    identity.Computer = sender.ComputerName;
                    identity.User = sender.UserName;
                    identity.PublicKey = sender.PublicKey;
                    authorizedSenders.Add(identity);
                }
            }
        }

        /// <summary>
        /// Removes authorized sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public void RemoveAuthorizedSender(AuthorizedSender sender)
        {
            if (authorizedSenders == null)
            {
                throw new ArgumentNullException("senders");
            }

            lock (authorizedSendersLock)
            {
                var identity = new ClipboardIdentity();
                identity.Computer = sender.ComputerName;
                identity.User = sender.UserName;
                identity.PublicKey = sender.PublicKey;
                authorizedSenders.Remove(identity);
            }
        }

        #endregion

        #region Inner types

        /// <summary>
        /// Clipboard identity comparer. Compares computer name, user name and public key.
        /// </summary>
        private class IdentityComparer : IEqualityComparer<ClipboardIdentity>
        {

            #region IEqualityComparer<ClipboardIdentity> Members

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first <see cref="ClipboardIdentity"/> to compare.</param>
            /// <param name="y">The second <see cref="ClipboardIdentity"/> to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(ClipboardIdentity x, ClipboardIdentity y)
            {
                if (x == null && y == null)
                {
                    return true;
                }
                else if (x == null || y == null)
                {
                    return false;
                }
                else
                {
                    if (String.Compare(x.Computer, y.Computer, StringComparison.InvariantCultureIgnoreCase) != 0)
                    {
                        return false;
                    }
                    if (String.Compare(x.User, y.User, StringComparison.InvariantCultureIgnoreCase) != 0)
                    {
                        return false;
                    }
                    return CompareBytes(x.PublicKey, y.PublicKey);
                }
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The clipboard identity.</param>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            /// <exception cref="T:System.ArgumentNullException">
            /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
            /// </exception>
            public int GetHashCode(ClipboardIdentity obj)
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    var stringComparer = StringComparer.InvariantCultureIgnoreCase;
                    int result = stringComparer.GetHashCode(obj.Computer);
                    result ^= stringComparer.GetHashCode(obj.User);
                    result ^= GetBytesHashCode(obj.PublicKey);
                    return result;
                }
            }

            #endregion
            
            private bool CompareBytes(byte[] arr1, byte[] arr2)
            {
                if (arr1 == arr2)
                {
                    return true;
                }

                if (arr1 == null || arr2 == null || arr1.Length != arr2.Length)
                {
                    return false;
                }

                for (int i = 0; i < arr1.Length; i++)
                {
                    if (arr1[i] != arr2[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            private int GetBytesHashCode(byte[] arr)
            {
                if (arr == null)
                {
                    return 0;
                }

                int result = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    int val = arr[i];
                    val <<= ((i % 4) * 8);
                    result ^= val;
                }

                return result;
            }

        }

        #endregion

    }
}
