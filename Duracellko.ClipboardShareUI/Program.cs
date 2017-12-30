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
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using Duracellko.ClipboardShare.Logging;

namespace Duracellko.ClipboardShareUI
{
    static class Program
    {

        private const string singleInstanceMutexName = "Duracellko.ClipboardShare_50130331-9F96-47CE-A265-5EF529A465E9";

        private static MemoryTraceListener traceLog;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // lock mutex to allow only one instance of application
            using (var singleInstanceLock = new Mutex(false, singleInstanceMutexName))
            {
                if (singleInstanceLock.WaitOne(500))
                {
                    // initialize tracing
                    using (traceLog = new MemoryTraceListener("ClipboardShareLog"))
                    {
                        LogManager.Current.TraceSource.Listeners.Add(traceLog);

                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new MainForm());

                        LogManager.Current.TraceSource.Listeners.Remove(traceLog);
                    }

                    singleInstanceLock.ReleaseMutex();
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="MemoryTraceListener"/> instance.
        /// </summary>
        /// <value>The trace log.</value>
        public static MemoryTraceListener TraceLog
        {
            get { return traceLog; }
        }

    }
}
