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
    /// Delegate of function, which is executed when new clipboard content is received.
    /// </summary>
    public delegate void ClipboardContentReceivedHandler(object sender, ClipboardContentReceivedEventArgs e);

    /// <summary>
    /// Arguments of <see cref="ClipboardContentReceivedHandler"/> delegate.
    /// </summary>
    public class ClipboardContentReceivedEventArgs : EventArgs
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardContentReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="content">The clipboard content.</param>
        public ClipboardContentReceivedEventArgs(ClipboardContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            Content = content;
        }

        /// <summary>
        /// Gets received clipboard content.
        /// </summary>
        public ClipboardContent Content { get; private set; }

    }
}
