/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 Jose Morato

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
using OpenDDSharp.OpenDDS.DCPS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class RtpsUdpInstTest
    {
        #region Constants        
        private const string TEST_CATEGORY = "RtpsUdpInst";
        private const string TRANSPORT_TYPE = "rtps_udp";
        private const string INSTANCE_NAME = "RtpsUdpInst";
        #endregion

        #region Test Method
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDefaultValues()
        {
            TransportInst inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            RtpsUdpInst rui = new RtpsUdpInst(inst);
            Assert.IsTrue(rui.UseMulticast);
            Assert.AreEqual("239.255.0.2:7401", rui.MulticastGroupAddress);
            Assert.AreEqual(string.Empty, rui.MulticastInterface);
            Assert.AreEqual(string.Empty, rui.LocalAddress);
            Assert.AreEqual(32U, rui.NakDepth);
            Assert.IsNotNull(rui.NakResponseDelay);
            Assert.AreEqual(0, rui.NakResponseDelay.Seconds);
            Assert.AreEqual(200000, rui.NakResponseDelay.MicroSeconds);
            Assert.IsNotNull(rui.HeartbeatPeriod);
            Assert.AreEqual(1, rui.HeartbeatPeriod.Seconds);
            Assert.AreEqual(0, rui.HeartbeatPeriod.MicroSeconds);
            Assert.IsNotNull(rui.HeartbeatResponseDelay);
            Assert.AreEqual(0, rui.HeartbeatResponseDelay.Seconds);
            Assert.AreEqual(500000, rui.HeartbeatResponseDelay.MicroSeconds);
            Assert.IsNotNull(rui.HandshakeTimeout);
            Assert.AreEqual(30, rui.HandshakeTimeout.Seconds);
            Assert.AreEqual(0, rui.HandshakeTimeout.MicroSeconds);
            Assert.AreEqual(1, rui.Ttl);
            Assert.AreEqual(32U, rui.DatalinkControlChunks);
            Assert.AreEqual(10000, rui.DatalinkReleaseDelay);
            Assert.IsNotNull(rui.DurableDataTimeout);
            Assert.AreEqual(60, rui.DurableDataTimeout.Seconds);
            Assert.AreEqual(0, rui.DurableDataTimeout.MicroSeconds);
            Assert.IsTrue(rui.IsReliable);
            Assert.AreEqual(2147481599u, rui.MaxPacketSize);
            Assert.AreEqual(10U, rui.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, rui.Name);
            Assert.AreEqual(4096u, rui.OptimumPacketSize);
            Assert.AreEqual(5U, rui.QueueInitialPools);
            Assert.AreEqual(10U, rui.QueueMessagesPerPool);
            Assert.AreEqual(65535, rui.RcvBufferSize);
            Assert.AreEqual(65535, rui.SendBufferSize);
            Assert.AreEqual(TRANSPORT_TYPE, rui.TransportType);
            Assert.IsTrue(rui.RequiresCdr);
            Assert.IsFalse(rui.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(rui);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValues()
        {
            TransportInst inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            RtpsUdpInst rui = new RtpsUdpInst(inst)
            {
                UseMulticast = false,
                MulticastGroupAddress = "239.255.0.1:7402",
                LocalAddress = "127.0.0.1:",
                MulticastInterface = "eth0",
                NakDepth = 64U,
                NakResponseDelay = new TimeValue
                {
                    Seconds = 1,
                    MicroSeconds = 400000
                },
                HeartbeatPeriod = new TimeValue
                {
                    Seconds = 2,
                    MicroSeconds = 100000
                },
                HeartbeatResponseDelay = new TimeValue
                {
                    Seconds = 2,
                    MicroSeconds = 100000
                },
                HandshakeTimeout = new TimeValue
                {
                    Seconds = 60,
                    MicroSeconds = 100000
                },
                Ttl = 2,
                DatalinkControlChunks = 64U,
                DatalinkReleaseDelay = 20000,
                DurableDataTimeout = new TimeValue
                {
                    Seconds = 20,
                    MicroSeconds = 100000
                },
                MaxPacketSize = 2147481500u,
                MaxSamplesPerPacket = 20U,
                OptimumPacketSize = 2048u,
                QueueInitialPools = 20U,
                QueueMessagesPerPool = 20U,
                RcvBufferSize = 65530,
                SendBufferSize = 65530,
                ThreadPerConnection = true
            };

            Assert.IsFalse(rui.UseMulticast);
            Assert.AreEqual("239.255.0.1:7402", rui.MulticastGroupAddress);
            Assert.AreEqual("eth0", rui.MulticastInterface);
            Assert.AreEqual("127.0.0.1:", rui.LocalAddress);
            Assert.AreEqual(64U, rui.NakDepth);
            Assert.IsNotNull(rui.NakResponseDelay);
            Assert.AreEqual(1, rui.NakResponseDelay.Seconds);
            Assert.AreEqual(400000, rui.NakResponseDelay.MicroSeconds);
            Assert.IsNotNull(rui.HeartbeatPeriod);
            Assert.AreEqual(2, rui.HeartbeatPeriod.Seconds);
            Assert.AreEqual(100000, rui.HeartbeatPeriod.MicroSeconds);
            Assert.IsNotNull(rui.HeartbeatResponseDelay);
            Assert.AreEqual(2, rui.HeartbeatResponseDelay.Seconds);
            Assert.AreEqual(100000, rui.HeartbeatResponseDelay.MicroSeconds);
            Assert.IsNotNull(rui.HandshakeTimeout);
            Assert.AreEqual(60, rui.HandshakeTimeout.Seconds);
            Assert.AreEqual(100000, rui.HandshakeTimeout.MicroSeconds);
            Assert.AreEqual(2, rui.Ttl);
            Assert.AreEqual(64U, rui.DatalinkControlChunks);
            Assert.AreEqual(20000, rui.DatalinkReleaseDelay);
            Assert.IsNotNull(rui.DurableDataTimeout);
            Assert.AreEqual(20, rui.DurableDataTimeout.Seconds);
            Assert.AreEqual(100000, rui.DurableDataTimeout.MicroSeconds);
            Assert.IsTrue(rui.IsReliable);
            Assert.AreEqual(2147481500u, rui.MaxPacketSize);
            Assert.AreEqual(20U, rui.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, rui.Name);
            Assert.AreEqual(2048u, rui.OptimumPacketSize);
            Assert.AreEqual(20U, rui.QueueInitialPools);
            Assert.AreEqual(20U, rui.QueueMessagesPerPool);
            Assert.AreEqual(65530, rui.RcvBufferSize);
            Assert.AreEqual(65530, rui.SendBufferSize);
            Assert.AreEqual(TRANSPORT_TYPE, rui.TransportType);
            Assert.IsTrue(rui.RequiresCdr);
            Assert.IsTrue(rui.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(rui);
        }
        #endregion
    }
}
