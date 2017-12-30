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
using System.Runtime.Serialization;

namespace Duracellko.ClipboardShare
{
    /// <summary>
    /// Represents identity in clipboard share network.
    /// </summary>
    [DataContract(Namespace=ClipboardShareManager.ClipboardDataNamespace)]
    public class ClipboardIdentity
    {

        /// <summary>
        /// Gets or sets clipboard ID in clipboard share network.
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the computer name.
        /// </summary>
        [DataMember]
        public string Computer { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [DataMember]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets listening TCP port.
        /// </summary>
        [DataMember]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets public key.
        /// </summary>
        [DataMember]
        public byte[] PublicKey { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}:{1}", Computer, User);
        }

    }
}
