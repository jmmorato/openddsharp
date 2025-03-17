/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2022 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="MulticastInst"/> unit test.
    /// </summary>
    [TestClass]
    public class MulticastInstTest
    {
        #region Constants
        private const string TEST_CATEGORY = "MulticastInst";
        private const string TRANSPORT_TYPE = "multicast";
        private const string INSTANCE_NAME = "MulticastInst";
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the properties default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDefaultValues()
        {
            var inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            var mi = new MulticastInst(inst);
            Assert.IsFalse(mi.DefaultToIpv6);
            Assert.AreEqual("224.0.0.128:49152", mi.GroupAddress);
            Assert.AreEqual(string.Empty, mi.LocalAddress);
            Assert.AreEqual(32U, mi.NakDepth);
            Assert.AreEqual(4U, mi.NakDelayIntervals);
            Assert.IsNotNull(mi.NakInterval);
            Assert.AreEqual(0, mi.NakInterval.Seconds);
            Assert.AreEqual(500000, mi.NakInterval.MicroSeconds);
            Assert.AreEqual(3U, mi.NakMax);
            Assert.IsNotNull(mi.NakTimeout);
            Assert.AreEqual(30, mi.NakTimeout.Seconds);
            Assert.AreEqual(0, mi.NakTimeout.MicroSeconds);
            Assert.AreEqual(49152, mi.PortOffset);
            Assert.IsTrue(mi.Reliable);
            Assert.AreEqual(2.0, mi.SynBackoff);
            Assert.IsNotNull(mi.SynInterval);
            Assert.AreEqual(0, mi.SynInterval.Seconds);
            Assert.AreEqual(250000, mi.SynInterval.MicroSeconds);
            Assert.IsNotNull(mi.SynTimeout);
            Assert.AreEqual(30, mi.SynTimeout.Seconds);
            Assert.AreEqual(0, mi.SynTimeout.MicroSeconds);
            Assert.IsFalse(mi.AsyncSend);
            Assert.AreEqual(1, mi.Ttl);
            Assert.AreEqual(32U, mi.DatalinkControlChunks);
            Assert.AreEqual(10000, mi.DatalinkReleaseDelay);
            Assert.IsTrue(mi.IsReliable);
            Assert.AreEqual(2147481599u, mi.MaxPacketSize);
            Assert.AreEqual(10U, mi.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, mi.Name);
            Assert.AreEqual(4096u, mi.OptimumPacketSize);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Assert.AreEqual(65536U, mi.RcvBufferSize);
            }
            else
            {
                Assert.AreEqual(65535U, mi.RcvBufferSize);
            }

            Assert.AreEqual(TRANSPORT_TYPE, mi.TransportType);
            Assert.IsFalse(mi.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(mi);
        }

        /// <summary>
        /// Test the properties non-default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValues()
        {
            var inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            var mi = new MulticastInst(inst)
            {
                DefaultToIpv6 = true,
                GroupAddress = "224.0.0.64:49150",
                LocalAddress = "127.0.0.1:",
                NakDepth = 64U,
                NakDelayIntervals = 8U,
                NakInterval = new TimeValue
                {
                    Seconds = 1,
                    MicroSeconds = 0,
                },
                NakMax = 6U,
                NakTimeout = new TimeValue
                {
                    Seconds = 10,
                    MicroSeconds = 500000,
                },
                PortOffset = 49150,
                Reliable = false,
                SynBackoff = 1.5,
                SynInterval = new TimeValue
                {
                    Seconds = 10,
                    MicroSeconds = 500000,
                },
                SynTimeout = new TimeValue
                {
                    Seconds = 10,
                    MicroSeconds = 500000,
                },
                AsyncSend = true,
                Ttl = 2,
                DatalinkControlChunks = 64U,
                DatalinkReleaseDelay = 20000,
                MaxPacketSize = 2147481500u,
                MaxSamplesPerPacket = 20U,
                OptimumPacketSize = 2048u,
                RcvBufferSize = 65530U,
                ThreadPerConnection = true,
            };

            Assert.IsTrue(mi.DefaultToIpv6);
            Assert.AreEqual("224.0.0.64:49150", mi.GroupAddress);
            Assert.AreEqual("127.0.0.1:", mi.LocalAddress);
            Assert.AreEqual(64U, mi.NakDepth);
            Assert.AreEqual(8U, mi.NakDelayIntervals);
            Assert.IsNotNull(mi.NakInterval);
            Assert.AreEqual(1, mi.NakInterval.Seconds);
            Assert.AreEqual(0, mi.NakInterval.MicroSeconds);
            Assert.AreEqual(6U, mi.NakMax);
            Assert.IsNotNull(mi.NakTimeout);
            Assert.AreEqual(10, mi.NakTimeout.Seconds);
            Assert.AreEqual(500000, mi.NakTimeout.MicroSeconds);
            Assert.AreEqual(49150, mi.PortOffset);
            Assert.IsFalse(mi.Reliable);
            Assert.AreEqual(1.5, mi.SynBackoff);
            Assert.IsNotNull(mi.SynInterval);
            Assert.AreEqual(10, mi.SynInterval.Seconds);
            Assert.AreEqual(500000, mi.SynInterval.MicroSeconds);
            Assert.IsNotNull(mi.SynTimeout);
            Assert.AreEqual(10, mi.SynTimeout.Seconds);
            Assert.AreEqual(500000, mi.SynTimeout.MicroSeconds);
            Assert.IsTrue(mi.AsyncSend);
            Assert.AreEqual(2, mi.Ttl);
            Assert.AreEqual(64U, mi.DatalinkControlChunks);
            Assert.AreEqual(20000, mi.DatalinkReleaseDelay);
            Assert.IsFalse(mi.IsReliable);
            Assert.AreEqual(2147481500u, mi.MaxPacketSize);
            Assert.AreEqual(20U, mi.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, mi.Name);
            Assert.AreEqual(2048u, mi.OptimumPacketSize);
            Assert.AreEqual(65530U, mi.RcvBufferSize);
            Assert.AreEqual(TRANSPORT_TYPE, mi.TransportType);
            Assert.IsTrue(mi.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(mi);
        }
        #endregion
    }
}