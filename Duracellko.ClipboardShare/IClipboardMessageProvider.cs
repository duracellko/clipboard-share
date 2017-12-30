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

namespace Duracellko.ClipboardShare
{
    /// <summary>
    /// Provides functions for encryption/decryption and signing/verification of clipboard messages.
    /// </summary>
    public interface IClipboardMessageProvider
    {

        /// <summary>
        /// Encrypts the message with recipient's public key.
        /// </summary>
        /// <param name="message">The clipboard message.</param>
        void EncryptMessage(ClipboardMessage message);

        /// <summary>
        /// Decrypts the message with specified private key.
        /// </summary>
        /// <param name="message">The clipboard message.</param>
        /// <param name="privateKey">The private key.</param>
        void DecryptMessage(ClipboardMessage message, byte[] privateKey);

        /// <summary>
        /// Signs the message with specified private key.
        /// </summary>
        /// <param name="message">The clipboard message.</param>
        /// <param name="privateKey">The private key.</param>
        void SignMessage(ClipboardMessage message, byte[] privateKey);

        /// <summary>
        /// Verifies message signature using sender's public key..
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        bool VerifySignature(ClipboardMessage message);

        /// <summary>
        /// Serializes the content of the clipboard to byte array.
        /// </summary>
        /// <param name="clipboardContent">Content of the clipboard.</param>
        /// <returns>Byte array containing serialized clipboard content.</returns>
        byte[] SerializeClipboardContent(ClipboardContent clipboardContent);

        /// <summary>
        /// Deserializes the content of the clipboard from byte array.
        /// </summary>
        /// <param name="buffer">Serialized clipboard content.</param>
        /// <returns>The clipboard content.</returns>
        ClipboardContent DeserializeClipboardContent(byte[] buffer);

        /// <summary>
        /// Extracts public key from private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <returns>The public key.</returns>
        byte[] GetPublicKey(byte[] privateKey);

        /// <summary>
        /// Generates new private key.
        /// </summary>
        /// <returns>The private key.</returns>
        byte[] GeneratePrivateKey();

    }
}
