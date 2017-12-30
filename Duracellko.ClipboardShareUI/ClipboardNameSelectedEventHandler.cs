﻿/*
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

using Duracellko.ClipboardShare;

namespace Duracellko.ClipboardShareUI
{

    /// <summary>
    /// Delegate executing when user selects clipboard name from menu.
    /// </summary>
    public delegate void ClipboardNameSelectedEventHandler(object sender, ClipboardNameSelectedEventArgs e);

    /// <summary>
    /// Event arguments for <see cref="ClipboardNameSelectedEventHandler"/> delegate.
    /// </summary>
    public class ClipboardNameSelectedEventArgs : EventArgs
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardNameSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="clipboardName">The clipboard name.</param>
        public ClipboardNameSelectedEventArgs(ClipboardName clipboardName)
        {
            if (clipboardName == null)
            {
                throw new ArgumentNullException("clipboardName");
            }

            ClipboardName = clipboardName;
        }

        /// <summary>
        /// Gets or sets the clipboard name.
        /// </summary>
        public ClipboardName ClipboardName { get; private set; }

    }

}
