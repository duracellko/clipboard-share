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
using System.Xml.Serialization;
using System.ComponentModel;

namespace Duracellko.ClipboardShare.Configuration
{
    /// <summary>
    /// Configuration data for loading from/saving to XML.
    /// </summary>
    [XmlRoot("clipboardShare")]
    public class ConfigurationData
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationData"/> class.
        /// </summary>
        public ConfigurationData()
        {
            MaxReceivedMessageSize = Service.ClipboardShareServiceHost.DefaultMaxReceivedMessageSize;
        }

        [XmlElement("userName")]
        public string UserName { get; set; }

        [XmlElement("publicKey")]
        public byte[] PrivateKey { get; set; }

        [XmlArray("authorizedSenders")]
        [XmlArrayItem("sender")]
        public AuthorizedSender[] AuthorizedSenders { get; set; }

        [XmlElement("defaultComputerName")]
        public string DefaultComputerName { get; set; }

        [XmlElement("defaultUserName")]
        public string DefaultUserName { get; set; }

        [XmlElement("maxReceivedMessageSize")]
        [DefaultValue(Service.ClipboardShareServiceHost.DefaultMaxReceivedMessageSize)]
        public int MaxReceivedMessageSize { get; set; }

        [XmlElement("servicePort")]
        [DefaultValue(0)]
        public int ServicePort { get; set; }

        [XmlArray("permanentRecipients")]
        [XmlArrayItem("recipient")]
        public PermanentRecipient[] PermanentRecpipients { get; set; }

    }
}
