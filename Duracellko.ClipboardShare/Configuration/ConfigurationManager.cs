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
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;

using Duracellko.ClipboardShare.Helpers;

namespace Duracellko.ClipboardShare.Configuration
{
    /// <summary>
    /// Provides services for loading and saving user configuration.
    /// </summary>
    public class ConfigurationManager
    {

        private static readonly byte[] aesKey = new byte[] { 237, 198, 242, 100, 244, 121, 224, 4, 209, 252, 110, 25, 90, 247, 250, 166, 4, 127, 144, 12, 18, 79, 20, 116, 89, 54, 253, 26, 204, 163, 153, 19 };

        private List<AuthorizedSender> authorizedSenders = new List<AuthorizedSender>();
        private List<PermanentRecipient> permanentRecipients = new List<PermanentRecipient>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationManager"/> class.
        /// </summary>
        public ConfigurationManager()
        {
            MaxReceivedMessageSize = Service.ClipboardShareServiceHost.DefaultMaxReceivedMessageSize;
        }

        #region Singleton

        private static ConfigurationManager current;
        private static object singletonLock = new object();

        /// <summary>
        /// Gets the current <see cref="ConfigurationManager"/> instance.
        /// </summary>
        public static ConfigurationManager Current
        {
            get
            {
                if (current == null)
                {
                    lock (singletonLock)
                    {
                        if (current == null)
                        {
                            current = new ConfigurationManager();
                        }
                    }
                }

                return current;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        public byte[] PrivateKey { get; set; }

        /// <summary>
        /// Gets the list of authorized senders.
        /// </summary>
        public IList<AuthorizedSender> AuthorizedSenders
        {
            get { return authorizedSenders; }
        }

        /// <summary>
        /// Gets or sets computer name of default recipient.
        /// </summary>
        public string DefaultComputerName { get; set; }

        /// <summary>
        /// Gets or sets user name of default recipient.
        /// </summary>
        public string DefaultUserName { get; set; }

        /// <summary>
        /// Gets or sets the size of the max received message.
        /// </summary>
        public int MaxReceivedMessageSize { get; set; }

        /// <summary>
        /// Gets or sets service TCP port or 0, if free port should be used.
        /// </summary>
        public int ServicePort { get; set; }

        /// <summary>
        /// Gets the list of permanent recipients.
        /// </summary>
        public IList<PermanentRecipient> PermanentRecipients
        {
            get { return permanentRecipients; }
        }

        #region Load

        /// <summary>
        /// Loads configuration from user's ApplicationData folder.
        /// </summary>
        public void Load()
        {
            string path = GetUserConfigurationFilePath();
            if (File.Exists(path))
            {
                Load(path);
            }
        }

        /// <summary>
        /// Loads configuration from specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Load(string path)
        {
            var serializer = new XmlSerializer(typeof(ConfigurationData));
            ConfigurationData data;
            using (var reader = XmlReader.Create(path))
            {
                data = (ConfigurationData)serializer.Deserialize(reader);
            }

            AuthorizedSenders.Clear();

            UserName = data.UserName;
            if (data.PrivateKey != null && data.PrivateKey.Length > 0)
            {
                PrivateKey = DecryptPrivateKey(data.PrivateKey);
            }
            if (data.AuthorizedSenders != null)
            {
                authorizedSenders.Clear();
                authorizedSenders.AddRange(data.AuthorizedSenders);
            }
            if (data.PermanentRecpipients != null)
            {
                permanentRecipients.Clear();
                permanentRecipients.AddRange(data.PermanentRecpipients);
            }
            DefaultComputerName = data.DefaultComputerName;
            DefaultUserName = data.DefaultUserName;
            MaxReceivedMessageSize = data.MaxReceivedMessageSize != 0 ? data.MaxReceivedMessageSize : Service.ClipboardShareServiceHost.DefaultMaxReceivedMessageSize;
            ServicePort = data.ServicePort;
        }

        #endregion

        #region Save

        /// <summary>
        /// Saves the configuration to user's ApplicationData folder.
        /// </summary>
        public void Save()
        {
            string path = GetUserConfigurationFilePath();
            string folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            Save(path);
        }

        /// <summary>
        /// Saves the configuration to specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Save(string path)
        {
            var data = new ConfigurationData();

            data.UserName = UserName;
            if (PrivateKey != null)
            {
                data.PrivateKey = EncryptPrivateKey(PrivateKey);
            }
            data.AuthorizedSenders = AuthorizedSenders.ToArray();
            data.PermanentRecpipients = PermanentRecipients.ToArray();
            data.DefaultComputerName = DefaultComputerName;
            data.DefaultUserName = DefaultUserName;
            data.MaxReceivedMessageSize = MaxReceivedMessageSize;
            data.ServicePort = ServicePort;

            var serializer = new XmlSerializer(typeof(ConfigurationData));
            var writerSettings = new XmlWriterSettings();
            writerSettings.Indent = true;
            using (var writer = XmlWriter.Create(path, writerSettings))
            {
                serializer.Serialize(writer, data);
            }
        }

        #endregion

        #region Private methods

        private string GetUserConfigurationFilePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, @"ClipboardShare\config.xml");
            return path;
        }

        private byte[] EncryptPrivateKey(byte[] privateKey)
        {
            using (var aes = new AesManaged())
            {
                aes.Key = aesKey;
                byte[] iv = aes.IV;

                using (var buffer = new MemoryStream(privateKey.Length + 2 * iv.Length))
                {
                    buffer.Write(iv, 0, iv.Length);

                    using (var privateKeyStream = new MemoryStream(privateKey))
                    {
                        using (var cryptoStream = new CryptoStream(privateKeyStream, aes.CreateEncryptor(), CryptoStreamMode.Read))
                        {
                            ClipboardShareHelper.CopyStream(cryptoStream, buffer, aes.BlockSize);
                        }
                    }

                    return buffer.ToArray();
                }
            }
        }

        private byte[] DecryptPrivateKey(byte[] encryptedData)
        {
            using (var aes = new AesManaged())
            {
                aes.Key = aesKey;
                byte[] iv = new byte[aes.BlockSize / 8];

                using (var buffer = new MemoryStream(encryptedData.Length))
                {
                    using (var encryptedDataStream = new MemoryStream(encryptedData))
                    {
                        encryptedDataStream.Read(iv, 0, iv.Length);
                        aes.IV = iv;

                        using (var cryptoStream = new CryptoStream(encryptedDataStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            ClipboardShareHelper.CopyStream(cryptoStream, buffer, aes.BlockSize);
                        }
                    }

                    return buffer.ToArray();
                }
            }
        }

        #endregion

    }
}
