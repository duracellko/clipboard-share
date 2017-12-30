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

namespace Duracellko.ClipboardShare.Service
{
    /// <summary>
    /// WCF service implementing clipboard share service.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext=false)]
    public class ClipboardShareService : IClipboardShareService
    {

        internal const string ClipboardServiceNamespace = "urn://duracellko/ClipboardShare/Service";

        #region IClipboardShareService Members

        /// <summary>
        /// Gets own identity with public key.
        /// </summary>
        public ClipboardIdentity GetIdentity()
        {
            return ClipboardShareManager.Instance.GetIdentity();
        }

        /// <summary>
        /// Sets clipboard to specified content after verification and authorization.
        /// </summary>
        /// <param name="message">The clipboard message.</param>
        public void SetClipboard(ClipboardMessage message)
        {
            try
            {
                if (message != null)
                {
                    var asyncOperation = new Action<ClipboardMessage>(msg =>
                        {
                            ClipboardShareManager.Instance.ReceiveMessage(msg);
                        });

                    Helpers.AsyncHelper.ExecuteAsync(asyncOperation, message);
                }
            }
            catch (Exception ex)
            {
                Logging.LogManager.Current.LogException(ex);
            }
        }

        #endregion
    }
}
