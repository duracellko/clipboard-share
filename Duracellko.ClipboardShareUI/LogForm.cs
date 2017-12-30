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
using System.Diagnostics;

using Duracellko.ClipboardShare.Logging;

namespace Duracellko.ClipboardShareUI
{
    /// <summary>
    /// Display application log
    /// </summary>
    public partial class LogForm : Form
    {

        private static Rectangle previousPosition;
        private static int previousTopPanelHeight = -1;

        private Bitmap informationImage;
        private Bitmap warningImage;
        private Bitmap errorImage;

        public LogForm()
        {
            InitializeComponent();

            Icon = Resources.Icon_ClipboardShare;
        }

        #region Properties

        public Image InformationImage
        {
            get { return informationImage; }
        }

        public Image WarningImage
        {
            get { return warningImage; }
        }

        public Image ErrorImage
        {
            get { return errorImage; }
        }

        #endregion

        #region Overriden methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadImages();
            clearLogToolButton.Image = Resources.Image_DeleteAll;

            // display form in previous position
            if (!previousPosition.IsEmpty)
            {
                Bounds = previousPosition;
            }
            if (previousTopPanelHeight >= 0)
            {
                mainSplitContainer.SplitterDistance = previousTopPanelHeight;
            }

            logGridView.AutoGenerateColumns = false;
            Program.TraceLog.RecordAdded += new EventHandler(TraceLog_RecordAdded);

            try
            {
                DataBindLog(false);
            }
            catch (Exception ex)
            {
                LogManager.Current.LogException(ex);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // save current position
            previousPosition = Bounds;
            previousTopPanelHeight = mainSplitContainer.SplitterDistance;

            InformationImage.Dispose();
            WarningImage.Dispose();
            ErrorImage.Dispose();
        }

        #endregion

        #region Event handlers

        private void logGridView_SelectionChanged(object sender, EventArgs e)
        {
            // display detail text in bottom section
            TraceRecordView selectedTraceRecord = null;
            if (logGridView.SelectedRows.Count > 0)
            {
                selectedTraceRecord = logGridView.SelectedRows[0].DataBoundItem as TraceRecordView;
            }

            if (selectedTraceRecord != null)
            {
                detailTextBox.Text = !String.IsNullOrEmpty(selectedTraceRecord.TraceRecord.Text) ? selectedTraceRecord.TraceRecord.Text : selectedTraceRecord.Title;
            }
        }

        private void clearLogToolButton_Click(object sender, EventArgs e)
        {
            // clear log and rebind
            Program.TraceLog.Clear();
            DataBindLog(false);
        }

        private void TraceLog_RecordAdded(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(TraceLog_RecordAdded), sender, e);
            }
            else
            {
                // rebind grid
                DataBindLog(true);
            }
        }

        #endregion

        #region Private methods

        private void LoadImages()
        {
            informationImage = Resources.Image_Information;
            //informationImage.MakeTransparent();
            warningImage = Resources.Image_Warning;
            //warningImage.MakeTransparent(Color.FromArgb(255, 0, 255));
            errorImage = Resources.Image_Error;
            //errorImage.MakeTransparent();
        }

        private void DataBindLog(bool restoreSelection)
        {
            var traceRecords = Program.TraceLog.GetTraceRecords();
            var traceRecordViews = traceRecords.Select(tr => new TraceRecordView(tr, this)).ToList();

            int selectedRowIndex = -1;
            if (restoreSelection)
            {
                // save current selection
                if (logGridView.CurrentRow != null)
                {
                    selectedRowIndex = logGridView.CurrentRow.Index;
                }
            }
            else
            {
                detailTextBox.Text = String.Empty;
            }

            logGridView.DataSource = traceRecordViews;

            // restore selection
            if (restoreSelection && selectedRowIndex >= 0 && selectedRowIndex < logGridView.RowCount)
            {
                logGridView.CurrentCell = logGridView.Rows[selectedRowIndex].Cells[0];
            }
        }

        #endregion

        #region Inner types

        /// <summary>
        /// Data to bind to log DataGridView.
        /// </summary>
        private class TraceRecordView
        {

            public TraceRecordView(MemoryTraceRecord traceRecord, LogForm logForm)
            {
                if (traceRecord == null)
                {
                    throw new ArgumentNullException("traceRecord");
                }
                if (logForm == null)
                {
                    throw new ArgumentNullException("logForm");
                }

                TraceRecord = traceRecord;
                LogForm = logForm;
            }

            public MemoryTraceRecord TraceRecord { get; private set; }

            public LogForm LogForm { get; private set; }

            public Image Icon
            {
                get
                {
                    switch (TraceRecord.Type)
                    {
                        case TraceEventType.Information:
                            return LogForm.InformationImage;
                        case TraceEventType.Warning:
                            return LogForm.WarningImage;
                        case TraceEventType.Error:
                        case TraceEventType.Critical:
                            return LogForm.ErrorImage;
                        default:
                            return null;
                    }
                }
            }

            public DateTime Time
            {
                get
                {
                    return TraceRecord.DateTime;
                }
            }

            public string Title
            {
                get
                {
                    return TraceRecord.Title;
                }
            }

        }

        #endregion

    }
}
