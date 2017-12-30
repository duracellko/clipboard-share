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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace Duracellko.ClipboardShareUI
{
    /// <summary>
    /// Manages positioning of windows into stack near notification tray.
    /// </summary>
    internal class NotificationWindowManager
    {

        private IntPtr notificationToolbarWnd;
        private StringBuilder buffer;
        private List<AuthorizeSenderForm> windowStack = new List<AuthorizeSenderForm>();
        private Rectangle notificationBarRectangle = Rectangle.Empty;
        private DateTime notificationBarRectangleTimestamp = DateTime.MinValue;

        #region Public methods

        /// <summary>
        /// Shows form near notification tray in next available stack position.
        /// </summary>
        /// <param name="form">The form do display.</param>
        /// <param name="ownerForm">The owner form.</param>
        public void ShowForm(AuthorizeSenderForm form, Form ownerForm)
        {
            if (ownerForm.InvokeRequired)
            {
                ownerForm.Invoke(new Action<AuthorizeSenderForm, Form>(ShowForm), form, ownerForm);
            }
            else
            {
                windowStack.Add(form);
                int index = windowStack.Count - 1;
                form.FormClosed += new FormClosedEventHandler(formClosed);

                // save size of the window
                form.RequiredPosition = form.Bounds;
                // calculate window position
                PositionWindow(index);
                form.Bounds = form.RequiredPosition;

                // set initial height to 1px
                var trayRect = GetNotificationBarRectangle();
                if (trayRect.Y > form.RequiredPosition.Y)
                {
                    form.Top += form.Height - 1;
                }
                form.Height = 1;

                form.StartAnimation();
                form.ShowAuthorizationRequest(ownerForm);
            }
        }

        /// <summary>
        /// Shows form near notification tray.
        /// </summary>
        /// <param name="form">The form to display.</param>
        /// <param name="ownerForm">The owner form.</param>
        public void ShowForm(Form form, Form ownerForm)
        {
            if (ownerForm.InvokeRequired)
            {
                ownerForm.Invoke(new Action<Form, Form>(ShowForm), form, ownerForm);
            }
            else
            {
                // get notificcation tray senter point
                Rectangle rect = form.Bounds;
                Rectangle notifRect = GetNotificationBarRectangle();
                Point p = new Point(notifRect.X + notifRect.Width / 2, notifRect.Y + notifRect.Height / 2);
                rect.X = p.X - rect.Width / 2;
                rect.Y = p.Y - rect.Height / 2;

                // get working area exclude 5px margin
                Rectangle workArea = Screen.PrimaryScreen.WorkingArea;
                workArea.Inflate(-10, -10);
                workArea.Offset(5, 5);

                // calculate window position in work area closest to notification tray center point.
                if (rect.Right > workArea.Right)
                {
                    rect.X = workArea.Right - rect.Width;
                }
                if (rect.X < workArea.X)
                {
                    rect.X = workArea.X;
                }
                if (rect.Bottom > workArea.Bottom)
                {
                    rect.Y = workArea.Bottom - rect.Height;
                }
                if (rect.Y < workArea.Y)
                {
                    rect.Y = workArea.Y;
                }

                // show the form
                form.Bounds = rect;
                form.Show(ownerForm);
            }
        }

        #endregion

        #region Private methods

        // reposition all windows after closing one.
        private void PositionWindows()
        {
            for (int i = 0; i < windowStack.Count; i++)
            {
                PositionWindow(i);
            }
        }

        // find window position in stack of windows near notification tray
        private void PositionWindow(int index)
        {
            // get notificcation tray senter point
            AuthorizeSenderForm form = windowStack[index];
            Rectangle rect = form.RequiredPosition;
            Rectangle notifRect = GetNotificationBarRectangle();
            Point p = new Point(notifRect.X + notifRect.Width / 2, notifRect.Y + notifRect.Height / 2);
            rect.X = p.X - rect.Width / 2;
            rect.Y = p.Y - rect.Height / 2;

            // get working area exclude 5px margin
            Rectangle workArea = Screen.PrimaryScreen.WorkingArea;
            workArea.Inflate(-10, -10);
            workArea.Offset(5, 5);

            // exclude windows in stack from working area
            for (int i = 0; i < index; i++)
            {
                Rectangle windowRect = windowStack[i].RequiredPosition;
                if (windowRect.Y - workArea.Y > workArea.Bottom - windowRect.Bottom)
                {
                    workArea.Height = windowRect.Y - workArea.Y;
                }
                else
                {
                    workArea.Height = workArea.Bottom - windowRect.Bottom;
                    workArea.Y = windowRect.Bottom;
                }
            }

            // calculate window position in work area closest to notification tray center point.
            if (rect.Right > workArea.Right)
            {
                rect.X = workArea.Right - rect.Width;
            }
            if (rect.X < workArea.X)
            {
                rect.X = workArea.X;
            }
            if (rect.Bottom > workArea.Bottom)
            {
                rect.Y = workArea.Bottom - rect.Height;
            }
            if (rect.Y < workArea.Y)
            {
                rect.Y = workArea.Y;
            }

            // start moving animation
            form.RequiredPosition = rect;
            form.StartAnimation();
        }

        private void formClosed(object sender, EventArgs e)
        {
            // reposition other windows after closing one
            var form = (AuthorizeSenderForm)sender;
            windowStack.Remove(form);
            PositionWindows();
        }

        // get position of notification tray
        private Rectangle GetNotificationBarRectangle()
        {
            if (notificationBarRectangle.IsEmpty || DateTime.UtcNow - notificationBarRectangleTimestamp > new TimeSpan(0, 0, 1))
            {
                var result = Rectangle.Empty;

                var shellTrayWnd = WinApi.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", IntPtr.Zero);
                if (shellTrayWnd == IntPtr.Zero)
                {
                    return result;
                }

                notificationToolbarWnd = IntPtr.Zero;
                var managerHandle = GCHandle.Alloc(this);
                try
                {
                    WinApi.EnumChildWindows(shellTrayWnd, new WinApi.EnumChildProc(EnumTrayWindowProc), GCHandle.ToIntPtr(managerHandle));
                }
                finally
                {
                    if (managerHandle.IsAllocated)
                    {
                        managerHandle.Free();
                    }
                }

                if (notificationToolbarWnd != IntPtr.Zero)
                {
                    WinApi.RECT rect = new WinApi.RECT();
                    if (WinApi.GetWindowRect(notificationToolbarWnd, ref rect))
                    {
                        result = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                    }
                }

                notificationBarRectangle = result;
                notificationBarRectangleTimestamp = DateTime.UtcNow;
            }

            return notificationBarRectangle;
        }

        private static bool EnumTrayWindowProc(IntPtr hwnd, IntPtr pointer)
        {
            try
            {
                var managerHandle = GCHandle.FromIntPtr(pointer);
                var manager = (NotificationWindowManager)managerHandle.Target;
                if (manager.buffer == null)
                {
                    manager.buffer = new StringBuilder(256);
                }
                manager.buffer.Length = 0;
                int length = WinApi.GetClassName(hwnd, manager.buffer, manager.buffer.Capacity);
                if (length <= manager.buffer.Length && manager.buffer.ToString().StartsWith("ToolbarWindow32"))
                {
                    manager.notificationToolbarWnd = hwnd;
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return true;
        }

        #endregion

    }
}
