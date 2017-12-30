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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Duracellko.ClipboardShare.Serialization
{
    /// <summary>
    /// Provides serialization/deserialization of clipboard content in specified image format.
    /// </summary>
    public abstract class ClipboardImageSerializerBase : IClipboardDataSerializer
    {
        #region IClipboardDataSerializer Members

        /// <summary>
        /// Gets format of clipboard content supported by the serializer.
        /// </summary>
        public abstract string Format { get; }

        /// <summary>
        /// Gets a value indicating whether serialization is binary or text.
        /// </summary>
        /// <value><c>true</c> if serialization is binary; otherwise, <c>false</c>.</value>
        public bool IsBinary
        {
            get { return true; }
        }

        /// <summary>
        /// Serializes clipboard object to byte array.
        /// </summary>
        /// <param name="obj">The clipboard object.</param>
        /// <returns>
        /// Byte array containing serialized clipboard object.
        /// </returns>
        public virtual byte[] SerializeToBytes(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var image = (Image)obj;
            using (var stream = new MemoryStream(image.Width * image.Height * 4))
            {
                image.Save(stream, ImageFormat);
                stream.Flush();
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Deserializes clipboard object from the specified byte array.
        /// </summary>
        /// <param name="buffer">The byte array.</param>
        /// <returns>Deserialized clipboard object.</returns>
        public virtual object Deserialize(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            using (var stream = new MemoryStream(buffer))
            {
                return Image.FromStream(stream);
            }
        }

        /// <summary>
        /// Serializes clipboard object to text.
        /// </summary>
        /// <param name="obj">The clipboard object.</param>
        /// <returns>
        /// String containing serialized clipboard object.
        /// </returns>
        string IClipboardDataSerializer.SerializeToText(object obj)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Deserializes clipboard object from the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Deserialized clipboard object.</returns>
        object IClipboardDataSerializer.Deserialize(string text)
        {
            throw new NotSupportedException();
        }

        #endregion

        /// <summary>
        /// Gets the image format used to serialize/deserialize image.
        /// </summary>
        protected abstract ImageFormat ImageFormat { get; }

    }
}
