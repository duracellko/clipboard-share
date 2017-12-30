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

namespace Duracellko.ClipboardShare
{
    /// <summary>
    /// Represents clipboard share peer name.
    /// </summary>
    public class ClipboardName
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardName"/> class.
        /// </summary>
        public ClipboardName()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardName"/> class.
        /// </summary>
        /// <param name="identity">The clipboard share identity.</param>
        public ClipboardName(ClipboardIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            Id = identity.Id;
            Computer = identity.Computer;
            User = identity.User;
            Port = identity.Port;
        }

        /// <summary>
        /// Gets or sets peer ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the computer name.
        /// </summary>
        public string Computer { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the service TCP port.
        /// </summary>
        /// <value>The TCP port.</value>
        public int Port { get; set; }

        public string Address
        {
            get
            {
                return String.Format("{0}:{1}", Computer, Port);
            }
        }

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
