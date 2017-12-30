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
using System.Diagnostics;

namespace Duracellko.ClipboardShare.Logging
{
    /// <summary>
    /// Trace event stored in memory.
    /// </summary>
    public class MemoryTraceRecord
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryTraceRecord"/> class.
        /// </summary>
        /// <param name="type">The event type.</param>
        /// <param name="title">The title.</param>
        public MemoryTraceRecord(TraceEventType type, string title)
            : this(type, title, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryTraceRecord"/> class.
        /// </summary>
        /// <param name="type">The event type.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        public MemoryTraceRecord(TraceEventType type, string title, string text)
        {
            DateTime = DateTime.Now;
            Type = type;
            Title = title;
            Text = text;
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Gets the event type.
        /// </summary>
        public TraceEventType Type { get; private set; }

        /// <summary>
        /// Gets the event title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the event text.
        /// </summary>
        public string Text { get; private set; }

    }
}
