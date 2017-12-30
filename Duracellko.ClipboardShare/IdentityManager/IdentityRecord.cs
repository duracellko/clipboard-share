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

namespace Duracellko.ClipboardShare.IdentityManager
{
    /// <summary>
    /// Record of identity in clipboard sharing network.
    /// </summary>
    public class IdentityRecord
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRecord"/> class.
        /// </summary>
        /// <param name="identity">The identity.</param>
        public IdentityRecord(ClipboardIdentity identity)
            : this(identity, DateTime.UtcNow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRecord"/> class.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="timestamp">The timestamp.</param>
        public IdentityRecord(ClipboardIdentity identity, DateTime timestamp)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            Identity = identity;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Gets or sets the identity.
        /// </summary>
        public ClipboardIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

    }
}
