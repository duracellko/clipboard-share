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
using System.Xml.Serialization;

namespace Duracellko.ClipboardShare.Configuration
{
    /// <summary>
    /// Sender authorized to receive messages from.
    /// </summary>
    public class AuthorizedSender
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedSender"/> class.
        /// </summary>
        public AuthorizedSender()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedSender"/> class.
        /// </summary>
        /// <param name="computerName">Name of the computer.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="publicKey">The public key.</param>
        public AuthorizedSender(string computerName, string userName, byte[] publicKey)
        {
            ComputerName = computerName;
            UserName = userName;
            PublicKey = publicKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedSender"/> class.
        /// </summary>
        /// <param name="identity">The identity.</param>
        public AuthorizedSender(ClipboardIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            ComputerName = identity.Computer;
            UserName = identity.User;
            if (identity.PublicKey != null)
            {
                PublicKey = new byte[identity.PublicKey.Length];
                Array.Copy(identity.PublicKey, PublicKey, PublicKey.Length);
            }
        }

        /// <summary>
        /// Gets or sets the name of the computer.
        /// </summary>
        [XmlAttribute("computerName")]
        public string ComputerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [XmlAttribute("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        [XmlElement("publicKey")]
        public byte[] PublicKey { get; set; }

    }
}
