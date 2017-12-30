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
    /// Provides methods for logging.
    /// </summary>
    public class LogManager : ILogManager
    {

        private TraceSource traceSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        public LogManager()
        {
            traceSource = new TraceSource("ClipboardShare");
            traceSource.Switch.Level = SourceLevels.All;
        }

        #region Singleton

        private static ILogManager current;
        private static object singletonLock = new object();

        /// <summary>
        /// Gets the current instance of <see cref="ILogManager"/>.
        /// </summary>
        public static ILogManager Current
        {
            get
            {
                if (current == null)
                {
                    lock (singletonLock)
                    {
                        if (current == null)
                        {
                            current = new LogManager();
                        }
                    }
                }

                return current;
            }
        }

        #endregion

        #region ILogManager Members

        /// <summary>
        /// Gets the <see cref="T:System.Diagnostics.TraceSource"/> associated to the log.
        /// </summary>
        public TraceSource TraceSource
        {
            get { return traceSource; }
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogInformation(string message)
        {
            TraceSource.TraceInformation(message);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void LogInformation(string format, params object[] args)
        {
            TraceSource.TraceInformation(format, args);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogWarning(string message)
        {
            TraceSource.TraceEvent(TraceEventType.Warning, 0, message);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void LogWarning(string format, params object[] args)
        {
            TraceSource.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogError(string message)
        {
            TraceSource.TraceEvent(TraceEventType.Error, 0, message);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void LogError(string format, params object[] args)
        {
            TraceSource.TraceEvent(TraceEventType.Error, 0, format, args);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void LogException(Exception ex)
        {
            TraceSource.TraceData(TraceEventType.Error, 0, ex);
        }

        #endregion
    }
}
