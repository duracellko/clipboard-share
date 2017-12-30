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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Duracellko.ClipboardShare;
using Duracellko.ClipboardShare.Helpers;
using Duracellko.ClipboardShare.Logging;
using Config = Duracellko.ClipboardShare.Configuration;

namespace Duracellko.ClipboardShareUI
{
    /// <summary>
    /// Display menu of clipboard names in clipboard sharing network.
    /// </summary>
    public partial class ClipboardNamesForm : Form
    {

        private List<Label> items;
        private int selectedIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardNamesForm"/> class.
        /// </summary>
        public ClipboardNamesForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when clipboard name is selected.
        /// </summary>
        public event ClipboardNameSelectedEventHandler ClipboardNameSelected;

        /// <summary>
        /// Occurs when permanent recipient is selected.
        /// </summary>
        public event PermanentRecipientSelectedEventHandler PermanentRecipientSelected;

        /// <summary>
        /// Occurs when "not in list" menu item selected.
        /// </summary>
        public event EventHandler NotInListSelected;

        #region Overriden methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // create list of available clipboard names
            try
            {
                CreateItems();
                PositionForm();
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Activate();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            Close();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            // draw border
            using (var pen = new Pen(SystemColors.ControlDark))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            Label unselectItem = null;
            Label selectItem = null;
            switch (e.KeyCode)
            {
                case Keys.Down:
                    // select next item
                    if (items != null && selectedIndex < items.Count - 1)
                    {
                        if (selectedIndex != -1)
                        {
                            unselectItem = items[selectedIndex];
                        }
                        selectedIndex++;
                        selectItem = items[selectedIndex];
                    }
                    break;
                case Keys.Up:
                    // select previous item
                    if (items != null && selectedIndex != 0)
                    {
                        if (selectedIndex != -1)
                        {
                            unselectItem = items[selectedIndex];
                        }
                        else
                        {
                            selectedIndex = items.Count;
                        }
                        selectedIndex--;
                        selectItem = items[selectedIndex];
                    }
                    break;
                case Keys.Enter:
                case Keys.Space:
                    // confirm selection
                    if (items != null)
                    {
                        if (selectedIndex >= 0)
                        {
                            SelectClipboardItem(items[selectedIndex]);
                        }
                    }
                    else
                    {
                        Close();
                    }
                    break;
                case Keys.Escape:
                    // close menu
                    Close();
                    break;
            }

            if (unselectItem != null)
            {
                unselectItem.BackColor = SystemColors.Menu;
            }
            if (selectItem != null)
            {
                selectItem.BackColor = SystemColors.MenuHighlight;
            }
        }

        #endregion

        #region Event handlers

        private void notFoundLabel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void clipboardNameButton_Click(object sender, EventArgs e)
        {
            SelectClipboardItem((Label)sender);
        }

        private void clipboardNameButton_MouseEnter(object sender, EventArgs e)
        {
            // highlight item
            var item = (Label)sender;
            if (selectedIndex >= 0)
            {
                var unselectItem = items[selectedIndex];
                unselectItem.BackColor = SystemColors.Menu;
            }
            selectedIndex = items.IndexOf(item);
            item.BackColor = SystemColors.MenuHighlight;
        }

        private void clipboardNameButton_MouseLeave(object sender, EventArgs e)
        {
            // un-highlight item
            var item = (Label)sender;
            if (selectedIndex == items.IndexOf(item))
            {
                selectedIndex = -1;
            }
            item.BackColor = SystemColors.Menu;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Raises the <see cref="E:ClipboardNameSelected"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Duracellko.ClipboardShareUI.ClipboardNameSelectedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnClipboardNameSelected(ClipboardNameSelectedEventArgs e)
        {
            if (ClipboardNameSelected != null)
            {
                ClipboardNameSelected(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:PermanentRecipientSelected"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Duracellko.ClipboardShareUI.PermanentRecipientSelectedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPermanentRecipientSelected(PermanentRecipientSelectedEventArgs e)
        {
            if (PermanentRecipientSelected != null)
            {
                PermanentRecipientSelected(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:NotInListSelected"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnNotInListSelected(EventArgs e)
        {
            if (NotInListSelected != null)
            {
                NotInListSelected(this, e);
            }
        }

        #endregion

        #region Private methods

        private void CreateItems()
        {
            // create label for each clipboard name

            var clipboardNames = ClipboardShareManager.Instance.GetRemoteClipboardNames().OrderBy(cn => cn.Computer).ThenBy(cn => cn.User).ToArray();
            var clipboardAddresses = clipboardNames.Select(cn => cn.Address).ToArray();

            var menuItems = new List<object>(clipboardNames.Length);
            menuItems.AddRange(clipboardNames);

            // add permanent recipients from configuration
            var config = Config.ConfigurationManager.Current;
            foreach (var recipient in config.PermanentRecipients)
            {
                if (!clipboardAddresses.Contains(recipient.RecipientAddress, StringComparer.InvariantCultureIgnoreCase))
                {
                    menuItems.Add(recipient);
                }
            }

            // add empty name representing "not in list" menu item
            menuItems.Add(null);

            var labels = new List<Label>(menuItems.Count);

            // calculate max text size of menu items
            Size labelSize = new Size();

            using (var g = CreateGraphics())
            {
                foreach (var menuItem in menuItems)
                {
                    var textSize = g.MeasureString(menuItem != null ? menuItem.ToString() : Resources.Text_NotInList, Font).ToSize();
                    if (labelSize.Width < textSize.Width)
                    {
                        labelSize.Width = textSize.Width;
                    }
                    if (labelSize.Height < textSize.Height)
                    {
                        labelSize.Height = textSize.Height;
                    }
                }
            }

            labelSize.Height += 8;
            labelSize.Width += 16;

            // create menu items
            int top = 0;
            foreach (var menuItem in menuItems)
            {
                var label = new Label();
                label.AutoSize = false;
                label.Text = menuItem != null ? menuItem.ToString() : Resources.Text_NotInList;
                label.Tag = menuItem;
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.BackColor = SystemColors.Menu;
                label.Size = labelSize;
                label.Left = 0;
                label.Top = top;
                label.Click += new EventHandler(clipboardNameButton_Click);
                label.MouseEnter += new EventHandler(clipboardNameButton_MouseEnter);
                label.MouseLeave += new EventHandler(clipboardNameButton_MouseLeave);
                borderPanel.Controls.Add(label);
                labels.Add(label);

                top += labelSize.Height;
            }

            items = labels;
            selectedIndex = -1;
            Width = labelSize.Width + 2;
            Height = top + 2;
        }

        private void PositionForm()
        {
            // position menu near mouse cursor
            var position = Cursor.Position;
            var screenRectangle = Screen.GetBounds(position);
            var formRectangle = new Rectangle(position, Size);

            if (formRectangle.Right > screenRectangle.Right)
            {
                formRectangle.Offset(-formRectangle.Width, 0);
            }
            if (formRectangle.Left < screenRectangle.Left)
            {
                formRectangle.X = screenRectangle.Left;
            }

            if (formRectangle.Bottom > screenRectangle.Bottom)
            {
                formRectangle.Offset(0, -formRectangle.Height);
            }
            if (formRectangle.Top < screenRectangle.Top)
            {
                formRectangle.Y = screenRectangle.Top;
            }

            this.Location = formRectangle.Location;
        }

        private void SelectClipboardItem(Label item)
        {
            // notify about selected clipboard name
            try
            {
                if (item.Tag != null)
                {
                    if (item.Tag is ClipboardName)
                    {
                        var clipboardName = (ClipboardName)item.Tag;
                        OnClipboardNameSelected(new ClipboardNameSelectedEventArgs(clipboardName));
                    }
                    else if (item.Tag is Config.PermanentRecipient)
                    {
                        var recipient = (Config.PermanentRecipient)item.Tag;
                        OnPermanentRecipientSelected(new PermanentRecipientSelectedEventArgs(recipient));
                    }
                }
                else
                {
                    OnNotInListSelected(EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }

            Close();
        }

        #endregion

    }
}
