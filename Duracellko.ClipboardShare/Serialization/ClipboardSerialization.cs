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

namespace Duracellko.ClipboardShare.Serialization
{
    /// <summary>
    /// Provides functions for loading <see cref="ClipboardContent"/> object from data in clipboard and setting clipboard data from <see cref="ClipboardContent"/> object.
    /// </summary>
    public class ClipboardSerialization
    {

        Dictionary<string, IClipboardDataSerializer> serializers;

        /// <summary>
        /// Gets list of clipboard serializers indexed by format.
        /// </summary>
        protected Dictionary<string, IClipboardDataSerializer> Serializers
        {
            get
            {
                if (serializers == null)
                {
                    serializers = CreateSerializers();
                }
                return serializers;
            }
        }

        /// <summary>
        /// Gets <see cref="ClipboardContent"/> object from data stored in clipboard.
        /// </summary>
        /// <returns>ClipboardContent object.</returns>
        public ClipboardContent GetClipboardContent()
        {
            ClipboardContent result = null;

            var data = Clipboard.GetDataObject();
            if (data != null)
            {
                var resultItems = new List<ClipboardContentItem>();

                foreach (string format in data.GetFormats(false))
                {
                    IClipboardDataSerializer serializer = null;
                    if (Serializers.TryGetValue(format, out serializer))
                    {
                        object dataObject = null;
                        try
                        {
                            dataObject = data.GetData(format, false);

                            var contentItem = new ClipboardContentItem();
                            contentItem.Format = format;
                            if (serializer.IsBinary)
                            {
                                contentItem.BinaryData = serializer.SerializeToBytes(dataObject);
                            }
                            else
                            {
                                contentItem.TextData = serializer.SerializeToText(dataObject);
                            }

                            if (contentItem.BinaryData != null || contentItem.TextData != null)
                            {
                                resultItems.Add(contentItem);
                            }
                        }
                        catch (Exception)
                        {
                            // do nothing
                        }
                        finally
                        {
                            var disposableDataObject = dataObject as IDisposable;
                            if (disposableDataObject != null)
                            {
                                disposableDataObject.Dispose();
                            }
                        }
                    }
                }

                if (resultItems.Count > 0)
                {
                    result = new ClipboardContent();
                    result.Items = resultItems;
                }
            }

            return result;
        }

        /// <summary>
        /// Sets clipboard data to specified <see cref="ClipboardContent"/> data object.
        /// </summary>
        /// <param name="content">The clipboard content.</param>
        public void SetClipboard(ClipboardContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (content.Items != null && content.Items.Count > 0)
            {
                var data = new DataObject();
                bool containsData = false;

                foreach (var contentItem in content.Items)
                {
                    IClipboardDataSerializer serializer = null;
                    if (Serializers.TryGetValue(contentItem.Format, out serializer))
                    {
                        object dataObject = null;
                        try
                        {
                            if (serializer.IsBinary)
                            {
                                if (contentItem.BinaryData != null)
                                {
                                    dataObject = serializer.Deserialize(contentItem.BinaryData);
                                }
                            }
                            else
                            {
                                if (contentItem.TextData != null)
                                {
                                    dataObject = serializer.Deserialize(contentItem.TextData);
                                }
                            }

                            if (dataObject != null)
                            {
                                data.SetData(contentItem.Format, dataObject);
                                containsData = true;
                            }
                        }
                        catch (Exception)
                        {
                            // do nothing
                        }
                        finally
                        {
                            var disposableDataObject = dataObject as IDisposable;
                            if (disposableDataObject != null)
                            {
                                disposableDataObject.Dispose();
                            }
                        }
                    }
                }

                if (containsData)
                {
                    Clipboard.SetDataObject(data, true);
                }
            }
        }

        #region Private methods

        private Dictionary<string, IClipboardDataSerializer> CreateSerializers()
        {
            var serializers = new IClipboardDataSerializer[] {
                new ClipboardTextSerializer(),
                new ClipboardUnicodeTextSerializer(),
                new ClipboardRtfSerializer(),
                new ClipboardHtmlSerializer(),
                new ClipboardBitmapSerializer(),
                new ClipboardDibSerializer(),
                new ClipboardMetafilePictSerializer(),
                new ClipboardWaveAudioSerializer()
            };

            return serializers.ToDictionary(s => s.Format);
        }

        #endregion

    }
}
