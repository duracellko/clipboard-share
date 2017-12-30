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
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

using Duracellko.ClipboardShare.Helpers;

namespace Duracellko.ClipboardShare
{
    /// <summary>
    /// Provides functions for encryption/decryption and signing/verification of clipboard messages.
    /// </summary>
    public class ClipboardMessageProvider : IClipboardMessageProvider
    {

        /// <summary>
        /// Encrypts the message with recipient's public key.
        /// </summary>
        /// <param name="message">The clipboard message.</param>
        public void EncryptMessage(ClipboardMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (message.EncryptedClipboardContent != null)
            {
                if (message.Recipient == null)
                {
                    throw new InvalidOperationException(Resources.Error_RecipientNotSpecified);
                }
                if (message.Recipient.PublicKey == null)
                {
                    throw new InvalidOperationException(Resources.Error_RecipientsKeyNotSpecified);
                }

                using (var sourceStream = new MemoryStream(message.EncryptedClipboardContent))
                {
                    using (var destinationStream = new MemoryStream(message.EncryptedClipboardContent.Length + 500))
                    {
                        using (var aes = new AesCryptoServiceProvider())
                        {
                            byte[] key = aes.Key;
                            byte[] iv = aes.IV;
                            using (var rsa = new RSACryptoServiceProvider())
                            {
                                rsa.ImportCspBlob(message.Recipient.PublicKey);
                                key = rsa.Encrypt(key, true);
                                iv = rsa.Encrypt(iv, true);
                            }

                            WriteInt16ToStream(destinationStream, (short)key.Length);
                            destinationStream.Write(key, 0, key.Length);
                            WriteInt16ToStream(destinationStream, (short)iv.Length);
                            destinationStream.Write(iv, 0, iv.Length);

                            using (var cryptoStream = new CryptoStream(sourceStream, aes.CreateEncryptor(), CryptoStreamMode.Read))
                            {
                                ClipboardShareHelper.CopyStream(cryptoStream, destinationStream, aes.BlockSize);
                            }
                        }

                        message.EncryptedClipboardContent = destinationStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts the message with specified private key.
        /// </summary>
        /// <param name="message">The clipboard message.</param>
        /// <param name="privateKey">The private key.</param>
        public void DecryptMessage(ClipboardMessage message, byte[] privateKey)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (privateKey == null)
            {
                throw new ArgumentNullException("privateKey");
            }

            using (var sourceStream = new MemoryStream(message.EncryptedClipboardContent))
            {
                using (var destinationStream = new MemoryStream(message.EncryptedClipboardContent.Length))
                {
                    int length = ReadInt16FromStream(sourceStream);
                    byte[] key = new byte[length];
                    if (sourceStream.Read(key, 0, length) < length)
                    {
                        throw new InvalidOperationException(Resources.Error_InvalidMessage);
                    }
                    length = ReadInt16FromStream(sourceStream);
                    byte[] iv = new byte[length];
                    if (sourceStream.Read(iv, 0, length) < length)
                    {
                        throw new InvalidOperationException(Resources.Error_InvalidMessage);
                    }

                    using (var rsa = new RSACryptoServiceProvider())
                    {
                        rsa.ImportCspBlob(privateKey);
                        key = rsa.Decrypt(key, true);
                        iv = rsa.Decrypt(iv, true);
                    }

                    using (var aes = new AesCryptoServiceProvider())
                    {
                        aes.Key = key;
                        aes.IV = iv;

                        using (var cryptoStream = new CryptoStream(sourceStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            ClipboardShareHelper.CopyStream(cryptoStream, destinationStream, aes.BlockSize);
                        }
                    }

                    message.EncryptedClipboardContent = destinationStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Signs the message with specified private key.
        /// </summary>
        /// <param name="message">The clipboard message.</param>
        /// <param name="privateKey">The private key.</param>
        public void SignMessage(ClipboardMessage message, byte[] privateKey)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (privateKey == null)
            {
                throw new ArgumentNullException("privateKey");
            }

            byte[] signature = null;

            if (message.EncryptedClipboardContent != null)
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportCspBlob(privateKey);
                    using (var sha1 = new SHA1CryptoServiceProvider())
                    {
                        signature = rsa.SignData(message.EncryptedClipboardContent, sha1);
                    }
                }
            }

            message.Signature = signature;
        }

        /// <summary>
        /// Verifies message signature using sender's public key..
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public bool VerifySignature(ClipboardMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (message.EncryptedClipboardContent != null)
            {
                if (message.Sender == null)
                {
                    throw new InvalidOperationException(Resources.Error_SenderNotSpecified);
                }
                if (message.Sender.PublicKey == null)
                {
                    throw new InvalidOperationException(Resources.Error_SendersKeyNotSpecified);
                }

                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportCspBlob(message.Sender.PublicKey);
                    using (var sha1 = new SHA1CryptoServiceProvider())
                    {
                        return rsa.VerifyData(message.EncryptedClipboardContent, sha1, message.Signature);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Serializes the content of the clipboard to byte array.
        /// </summary>
        /// <param name="clipboardContent">Content of the clipboard.</param>
        /// <returns>
        /// Byte array containing serialized clipboard content.
        /// </returns>
        public byte[] SerializeClipboardContent(ClipboardContent clipboardContent)
        {
            if (clipboardContent == null)
            {
                throw new ArgumentNullException("clipboardContent");
            }

            var serializer = new DataContractSerializer(typeof(ClipboardContent));

            using (var stream = new MemoryStream(1000))
            {
                using (var writer = XmlDictionaryWriter.CreateBinaryWriter(stream))
                {
                    serializer.WriteObject(writer, clipboardContent);
                    writer.Flush();
                    stream.Flush();
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Deserializes the content of the clipboard from byte array.
        /// </summary>
        /// <param name="buffer">Serialized clipboard content.</param>
        /// <returns>The clipboard content.</returns>
        public ClipboardContent DeserializeClipboardContent(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            var serializer = new DataContractSerializer(typeof(ClipboardContent));

            using (var reader = XmlDictionaryReader.CreateBinaryReader(buffer, XmlDictionaryReaderQuotas.Max))
            {
                return (ClipboardContent)serializer.ReadObject(reader);
            }
        }

        /// <summary>
        /// Extracts public key from private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <returns>The public key.</returns>
        public byte[] GetPublicKey(byte[] privateKey)
        {
            if (privateKey == null)
            {
                throw new ArgumentNullException("privateKey");
            }

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(privateKey);
                return rsa.ExportCspBlob(false);
            }
        }

        /// <summary>
        /// Generates new private key.
        /// </summary>
        /// <returns>The private key.</returns>
        public byte[] GeneratePrivateKey()
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                return rsa.ExportCspBlob(true);
            }
        }

        #region Private methods

        private void WriteInt16ToStream(Stream stream, short value)
        {
            var buffer = new byte[2];
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            stream.Write(buffer, 0, 2);
        }

        private short ReadInt16FromStream(Stream stream)
        {
            var buffer = new byte[2];
            stream.Read(buffer, 0, 2);
            return (short)(buffer[0] | (buffer[1] << 8));
        }

        #endregion

    }
}
