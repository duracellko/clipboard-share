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
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;

using Duracellko.ClipboardShare.Helpers;

namespace Duracellko.ClipboardShare.IdentityManager
{
    /// <summary>
    /// WCF service host of clipboard sharing identity service.
    /// </summary>
    public class IdentityServiceHost : ServiceHost
    {

        internal static readonly Uri P2pUri = new Uri("net.p2p://clipboardshare/ClipboardShareIdentity");

        private Timer scheduleTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServiceHost"/> class.
        /// </summary>
        public IdentityServiceHost()
            : this(new IdentityService())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServiceHost"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public IdentityServiceHost(IdentityService service)
            : base(service, P2pUri)
        {
            AddServiceEndpoint(typeof(IIdentityService), ClipboardShareHelper.GetNetPeerBinding(), String.Empty);
        }

        /// <summary>
        /// Gets the service credentials and service authorization behavior for the hosted service.
        /// </summary>
        /// <remarks>
        /// Starts scheduled job required by identity service.
        /// </remarks>
        protected override void OnOpened()
        {
            base.OnOpened();

            if (scheduleTimer == null)
            {
                var identityService = SingletonInstance as IdentityService;
                if (identityService != null)
                {
                    identityService.ResetAdvertising();
                }

                scheduleTimer = new Timer(new TimerCallback(ScheduleTimerCallback), null, 15000, 15000);
            }
        }

        /// <summary>
        /// Invoked during the transition of a communication object into the closing state.
        /// </summary>
        /// <remarks>
        /// Stops scheduled job required by identity service.
        /// </remarks>
        protected override void OnClosing()
        {
            if (scheduleTimer != null)
            {
                scheduleTimer.Dispose();
                scheduleTimer = null;
            }

            base.OnClosing();
        }

        /// <summary>
        /// Executes scheduled job required by identity service.
        /// </summary>
        private void ScheduleTimerCallback(object state)
        {
            try
            {
                var identityService = SingletonInstance as IdentityService;
                if (identityService != null)
                {
                    identityService.ExecuteScheduledJob();
                }
            }
            catch (Exception ex)
            {
                Logging.LogManager.Current.LogException(ex);
            }
        }

    }
}
