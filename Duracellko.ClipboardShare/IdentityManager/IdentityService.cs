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

using Duracellko.ClipboardShare.Helpers;

namespace Duracellko.ClipboardShare.IdentityManager
{

    /// <summary>
    /// WCF clipboard sharing identity service.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class IdentityService : IIdentityService, IDisposable
    {

        internal const string IdentityServiceNamespace = "urn://duracellko/ClipboardShare/IdentityService";
        private const int shortAdvertisementsCount = 4;
        private readonly TimeSpan shortAdvertisementDelay = new TimeSpan(0, 0, 15);
        private readonly TimeSpan regularAdvertisementDelay = new TimeSpan(0, 5, 0);
        private readonly TimeSpan identityExpirationTime = new TimeSpan(0, 15, 0);

        private bool disposed = false;

        private Dictionary<Guid, IdentityRecord> identities = new Dictionary<Guid, IdentityRecord>();
        private object identitiesLock = new object();

        private IIdentityService clientChannel;
        private object clientChannelLock = new object();

        private int advertisementsCount;
        private DateTime lastAdvertisementTime;

        /// <summary>
        /// Gets the client identity service channel.
        /// </summary>
        protected IIdentityService Channel
        {
            get
            {
                if (clientChannel == null)
                {
                    lock (clientChannelLock)
                    {
                        if (clientChannel == null)
                        {
                            var binding = ClipboardShareHelper.GetNetPeerBinding();
                            var peerAddress = new EndpointAddress(IdentityServiceHost.P2pUri);
                            clientChannel = ChannelFactory<IIdentityService>.CreateChannel(binding, peerAddress);
                        }
                    }
                }
                return clientChannel;
            }
        }

        /// <summary>
        /// Gets or sets the local identity.
        /// </summary>
        public ClipboardIdentity LocalIdentity { get; set; }

        #region IIdentityService Members

        /// <summary>
        /// Registers the identity in clipboard sharing network.
        /// </summary>
        /// <param name="identity">New identity.</param>
        /// <param name="responseTo">Identity, whose registration executed this re-registration.</param>
        public void RegisterIdentity(ClipboardIdentity identity, Guid? responseTo)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (identity != null)
                {
                    bool newRecord = false;
                    lock (identitiesLock)
                    {
                        // create new identity record or update timestamp
                        IdentityRecord identityRecord;
                        if (identities.TryGetValue(identity.Id, out identityRecord))
                        {
                            identityRecord.Identity = identity;
                            identityRecord.Timestamp = DateTime.UtcNow;
                        }
                        else
                        {
                            newRecord = true;
                            identities[identity.Id] = new IdentityRecord(identity);
                        }
                    }

                    // if new record is created, then sender does not know me and must be notified of me
                    // however, only if not response of my own registration to avoid message cycling.
                    if (newRecord && LocalIdentity != null && LocalIdentity.Id != responseTo)
                    {
                        Channel.RegisterIdentity(LocalIdentity, identity.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogManager.Current.LogException(ex);
            }

        }

        /// <summary>
        /// Unregisters the identity from clipboard sharing network.
        /// </summary>
        /// <param name="id">ID to unregister.</param>
        public void UnregisterIdentity(Guid id)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                lock (identitiesLock)
                {
                    identities.Remove(id);
                }
            }
            catch (Exception ex)
            {
                Logging.LogManager.Current.LogException(ex);
            }
        }

        #endregion

        /// <summary>
        /// Gets the list clipboard names in clipboard sharing network.
        /// </summary>
        /// <returns></returns>
        public ClipboardName[] GetClipboardNames()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("IdentityService");
            }

            lock (identitiesLock)
            {
                IEnumerable<IdentityRecord> ids = identities.Values;
                if (LocalIdentity != null)
                {
                    ids = ids.Where(ir => ir.Identity.Id != LocalIdentity.Id);
                }
                return ids.Select(ir => new ClipboardName(ir.Identity)).ToArray();
            }
        }

        /// <summary>
        /// Gets clipboard identity with specified ID.
        /// </summary>
        /// <param name="id">The identity ID.</param>
        /// <returns>The clipboard identity.</returns>
        public ClipboardIdentity GetIdentity(Guid id)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("IdentityService");
            }

            lock (identitiesLock)
            {
                IdentityRecord result = null;
                if (identities.TryGetValue(id, out result))
                {
                    return ClipboardShareHelper.CreateCopy(result.Identity);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Unregister local identity from network.
        /// </summary>
        internal void Disconnect()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("IdentityService");
            }

            Channel.UnregisterIdentity(LocalIdentity.Id);
        }

        /// <summary>
        /// Resets the advertising time and counter.
        /// </summary>
        internal void ResetAdvertising()
        {
            advertisementsCount = 0;
            lastAdvertisementTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Executes the scheduled job required by identity service.
        /// </summary>
        internal void ExecuteScheduledJob()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("IdentityService");
            }

            RepairIdentitiesCollection();
            DoAdvertising();
        }

        #region Private methods

        // advertise my own identity in network
        // 4 times in 1 minute and then every 5 minutes
        private void DoAdvertising()
        {
            var now = DateTime.UtcNow;

            TimeSpan advertisementDelay = advertisementsCount <= shortAdvertisementsCount ? shortAdvertisementDelay : regularAdvertisementDelay;
            if (now - lastAdvertisementTime >= advertisementDelay)
            {
                Channel.RegisterIdentity(LocalIdentity, null);
                lastAdvertisementTime = DateTime.UtcNow;
                if (advertisementsCount <= shortAdvertisementsCount)
                {
                    advertisementsCount++;
                }
            }
        }

        // remove identities older than 10 minutes
        private void RepairIdentitiesCollection()
        {
            lock (identitiesLock)
            {
                var now = DateTime.UtcNow;

                foreach (var identityRecord in identities.Values.ToArray())
                {
                    if (now - identityRecord.Timestamp > identityExpirationTime)
                    {
                        identities.Remove(identityRecord.Identity.Id);
                    }
                }
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;

                lock (clientChannelLock)
                {
                    var clientCommunicationObject = clientChannel as ICommunicationObject;
                    if (clientCommunicationObject != null)
                    {
                        ClipboardShareHelper.CloseCommunicationObject(clientCommunicationObject);
                    }
                }
            }
        }

        #endregion
    }
}
