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

namespace Duracellko.ClipboardShare.Helpers
{
    /// <summary>
    /// Helper methods to execute methods asynchronously and log thrown exceptions.
    /// </summary>
    public static class AsyncHelper
    {

        /// <summary>
        /// Executes method asynchronously.
        /// </summary>
        /// <param name="operation">The operation.</param>
        public static void ExecuteAsync(Action operation)
        {
            var callback = new AsyncCallback(ar =>
                {
                    try
                    {
                        operation.EndInvoke(ar);
                    }
                    catch (Exception ex)
                    {
                        Logging.LogManager.Current.LogException(ex);
                    }
                });

            operation.BeginInvoke(callback, null);
        }

        /// <summary>
        /// Executes method asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of argument.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="obj">The argument.</param>
        public static void ExecuteAsync<T>(Action<T> operation, T obj)
        {
            var callback = new AsyncCallback(ar =>
            {
                try
                {
                    operation.EndInvoke(ar);
                }
                catch (Exception ex)
                {
                    Logging.LogManager.Current.LogException(ex);
                }
            });

            operation.BeginInvoke(obj, callback, null);
        }

        /// <summary>
        /// Executes method asynchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1.</typeparam>
        /// <typeparam name="T2">The type of the argument 2.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="arg1">The argument 1.</param>
        /// <param name="arg2">The argument 2.</param>
        public static void ExecuteAsync<T1, T2>(Action<T1, T2> operation, T1 arg1, T2 arg2)
        {
            var callback = new AsyncCallback(ar =>
            {
                try
                {
                    operation.EndInvoke(ar);
                }
                catch (Exception ex)
                {
                    Logging.LogManager.Current.LogException(ex);
                }
            });

            operation.BeginInvoke(arg1, arg2, callback, null);
        }

        /// <summary>
        /// Executes method asynchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1.</typeparam>
        /// <typeparam name="T2">The type of the argument 2.</typeparam>
        /// <typeparam name="T3">The type of the argument 3.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="arg1">The argument 1.</param>
        /// <param name="arg2">The argument 2.</param>
        /// <param name="arg3">The argument 3.</param>
        public static void ExecuteAsync<T1, T2, T3>(Action<T1, T2, T3> operation, T1 arg1, T2 arg2, T3 arg3)
        {
            var callback = new AsyncCallback(ar =>
            {
                try
                {
                    operation.EndInvoke(ar);
                }
                catch (Exception ex)
                {
                    Logging.LogManager.Current.LogException(ex);
                }
            });

            operation.BeginInvoke(arg1, arg2, arg3, callback, null);
        }

        /// <summary>
        /// Executes method asynchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1.</typeparam>
        /// <typeparam name="T2">The type of the argument 2.</typeparam>
        /// <typeparam name="T3">The type of the argument 3.</typeparam>
        /// <typeparam name="T4">The type of the argument 4.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="arg1">The argument 1.</param>
        /// <param name="arg2">The argument 2.</param>
        /// <param name="arg3">The argument 3.</param>
        /// <param name="arg4">The argument 4.</param>
        public static void ExecuteAsync<T1, T2, T3, T4>(Action<T1, T2, T3, T4> operation, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var callback = new AsyncCallback(ar =>
            {
                try
                {
                    operation.EndInvoke(ar);
                }
                catch (Exception ex)
                {
                    Logging.LogManager.Current.LogException(ex);
                }
            });

            operation.BeginInvoke(arg1, arg2, arg3, arg4, callback, null);
        }

    }
}
