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
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Duracellko.ClipboardShare.Service
{
    /// <summary>
    /// WCF client of clipboard share service.
    /// </summary>
    public class ClipboardShareClient : ClientBase<IClipboardShareService>, IClipboardShareService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardShareClient"/> class.
        /// </summary>
        /// <param name="remoteAddress">The remote address.</param>
        public ClipboardShareClient(string remoteAddress)
            : base(GetBinding(), new EndpointAddress(GetPortLessUri(remoteAddress)))
        {
            SetClientViaBehavior(new Uri(remoteAddress));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardShareClient"/> class.
        /// </summary>
        /// <param name="remoteAddress">The remote address.</param>
        public ClipboardShareClient(Uri remoteAddress)
            : base(GetBinding(), new EndpointAddress(GetPortLessUri(remoteAddress)))
        {
            SetClientViaBehavior(remoteAddress);
        }

        #region IClipboardShareService Members

        /// <summary>
        /// Gets own identity with public key.
        /// </summary>
        public ClipboardIdentity GetIdentity()
        {
            return Channel.GetIdentity();
        }

        /// <summary>
        /// Sets clipboard to specified content after verification and authorization.
        /// </summary>
        /// <param name="message">The clipboard message.</param>
        public void SetClipboard(ClipboardMessage message)
        {
            Channel.SetClipboard(message);
        }

        #endregion

        private void SetClientViaBehavior(Uri viaUri)
        {
            Endpoint.Behaviors.Add(new ClientViaBehavior(viaUri));
        }

        private static Binding GetBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.None);
            binding.Namespace = ClipboardShareService.ClipboardServiceNamespace;
            binding.Name = "ClipboardShareServiceBinding";
            return binding;
        }

        private static Uri GetPortLessUri(string uri)
        {
            return GetPortLessUri(new Uri(uri));
        }

        private static Uri GetPortLessUri(Uri uri)
        {
            var uriBuilder = new UriBuilder(uri);
            uriBuilder.Port = -1;
            return uriBuilder.Uri;
        }

    }
}
