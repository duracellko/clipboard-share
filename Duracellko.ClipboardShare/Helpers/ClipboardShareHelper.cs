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
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.PeerResolvers;

namespace Duracellko.ClipboardShare.Helpers
{
    /// <summary>
    /// Internal helper methods.
    /// </summary>
    internal static class ClipboardShareHelper
    {

        /// <summary>
        /// Closes the communication object, if opened, or abort, if faulted.
        /// </summary>
        internal static void CloseCommunicationObject(ICommunicationObject obj)
        {
            if (obj.State == CommunicationState.Opened || obj.State == CommunicationState.Opening)
            {
                obj.Close();
            }
            else if (obj.State == CommunicationState.Faulted)
            {
                obj.Abort();
            }
        }

        /// <summary>
        /// Compares two byte arrays and returns <c>true</c>, if they are equal; otherwise returns <c>false</c>.
        /// </summary>
        internal static bool CompareBytes(byte[] arr1, byte[] arr2)
        {
            if (arr1 == arr2)
            {
                return true;
            }

            if (arr1 == null || arr2 == null || arr1.Length != arr2.Length)
            {
                return false;
            }

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates copy of clipboard identity.
        /// </summary>
        internal static ClipboardIdentity CreateCopy(ClipboardIdentity identity)
        {
            if (identity == null)
            {
                return null;
            }

            var result = new ClipboardIdentity();
            result.Id = identity.Id;
            result.Computer = identity.Computer;
            result.User = identity.User;
            result.Port = identity.Port;
            if (identity.PublicKey != null)
            {
                result.PublicKey = new byte[identity.PublicKey.Length];
                Array.Copy(identity.PublicKey, result.PublicKey, identity.PublicKey.Length);
            }
            return result;
        }

        /// <summary>
        /// Gets the net peer binding for PRNP protocol.
        /// </summary>
        internal static Binding GetNetPeerBinding()
        {
            var binding = new NetPeerTcpBinding();
            binding.Resolver.Mode = PeerResolverMode.Pnrp;
            binding.Security.Mode = SecurityMode.None;
            return binding;
        }

        /// <summary>
        /// Copies content of source stream to destination stream.
        /// </summary>
        internal static void CopyStream(Stream source, Stream destination, int bufferSize)
        {
            var buffer = new byte[bufferSize];

            int bytesRead = source.Read(buffer, 0, bufferSize);
            while (bytesRead > 0)
            {
                destination.Write(buffer, 0, bytesRead);
                bytesRead = source.Read(buffer, 0, bufferSize);
            }
        }

    }
}
