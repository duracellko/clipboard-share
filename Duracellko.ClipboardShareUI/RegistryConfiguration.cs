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
using System.Windows.Forms;
using Microsoft.Win32;

namespace Duracellko.ClipboardShareUI
{
    /// <summary>
    /// Functions to load save configuration in registry
    /// </summary>
    public class RegistryConfiguration
    {

        private const string RunClipboardShareRegistryPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run";
        private const string RunClipboardShareRegistrySubKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string RunClipboardShareRegistryName = @"ClipboardShare";

        /// <summary>
        /// Gets or sets a value indicating whether to run application on startup.
        /// </summary>
        /// <value><c>true</c> if run application on startup; otherwise, <c>false</c>.</value>
        public static bool RunOnStartup
        {
            get
            {
                object value = Registry.GetValue(RunClipboardShareRegistryPath, RunClipboardShareRegistryName, null);
                return value != null && !String.IsNullOrEmpty(value.ToString());
            }
            set
            {
                if (value)
                {
                    string path = String.Format("\"{0}\"", Application.ExecutablePath);
                    Registry.SetValue(RunClipboardShareRegistryPath, RunClipboardShareRegistryName, path, RegistryValueKind.String);
                }
                else
                {
                    using (var regKey = Registry.CurrentUser.OpenSubKey(RunClipboardShareRegistrySubKeyPath, true))
                    {
                        regKey.DeleteValue(RunClipboardShareRegistryName, false);
                    }
                }
            }
        }

    }
}
