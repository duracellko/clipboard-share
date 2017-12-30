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
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Duracellko.ClipboardShare.Service
{
    /// <summary>
    /// WCF service host of clipboard sharing service.
    /// </summary>
    public class ClipboardShareServiceHost : ServiceHost
    {

        public const int DefaultMaxReceivedMessageSize = 1048576;
        private const string ServiceUri = "net.tcp://localhost/ClipboardShare";

        private NetTcpBinding binding;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardShareServiceHost"/> class.
        /// </summary>
        public ClipboardShareServiceHost()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardShareServiceHost"/> class.
        /// </summary>
        /// <param name="port">The service port.</param>
        public ClipboardShareServiceHost(int port)
            : base(new ClipboardShareService(), new Uri(ServiceUri))
        {
            binding = new NetTcpBinding(SecurityMode.None);
            binding.Namespace = ClipboardShareService.ClipboardServiceNamespace;
            binding.Name = "ClipboardShareServiceBinding";
            binding.MaxReceivedMessageSize = DefaultMaxReceivedMessageSize;
            binding.ReaderQuotas.MaxArrayLength = DefaultMaxReceivedMessageSize;
            binding.ReaderQuotas.MaxStringContentLength = DefaultMaxReceivedMessageSize;

            if (port == 0)
            {
                var sp = AddServiceEndpoint(typeof(IClipboardShareService), binding, String.Empty);
                sp.ListenUriMode = ListenUriMode.Unique;
            }
            else
            {
                var sp = AddServiceEndpoint(typeof(IClipboardShareService), binding, String.Empty, GetServiceUri(port));
            }
        }

        /// <summary>
        /// Gets the listening TCP port.
        /// </summary>
        public int ListeningPort
        {
            get
            {
                string bindingName = ClipboardShareService.ClipboardServiceNamespace + ":ClipboardShareServiceBinding";
                var channelDispatcher = ChannelDispatchers.OfType<ChannelDispatcher>().FirstOrDefault(cd => cd.BindingName == bindingName);
                if (channelDispatcher != null)
                {
                    return channelDispatcher.Listener.Uri.Port;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the size of the max received message.
        /// </summary>
        /// <value>The size of the max received message.</value>
        public int MaxReceivedMessageSize
        {
            get
            {
                return (int)binding.MaxReceivedMessageSize;
            }
            set
            {
                binding.MaxReceivedMessageSize = value;
                binding.ReaderQuotas.MaxArrayLength = value;
                binding.ReaderQuotas.MaxStringContentLength = value;
            }
        }

        #region Private methods

        private static Uri GetServiceUri(int port)
        {
            var builder = new UriBuilder(ServiceUri);
            if (port != 0)
            {
                builder.Port = port;
            }
            return builder.Uri;
        }

        #endregion
    }
}
