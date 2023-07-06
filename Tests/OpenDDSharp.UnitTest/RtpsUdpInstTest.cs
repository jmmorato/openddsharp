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
    /// <see cref="RtpsUdpInst"/> unit test.
    /// </summary>
    [TestClass]
    public class RtpsUdpInstTest
    {
        #region Constants
        private const string TEST_CATEGORY = "RtpsUdpInst";
        private const string TRANSPORT_TYPE = "rtps_udp";
        private const string INSTANCE_NAME = "RtpsUdpInst";
        #endregion

        #region Test Method
        /// <summary>
        /// Test the properties default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDefaultValues()
        {
            TransportInst inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            RtpsUdpInst rui = new RtpsUdpInst(inst);
            Assert.IsTrue(rui.UseMulticast);
            Assert.AreEqual("239.255.0.2:7401", rui.MulticastGroupAddress);
            Assert.AreEqual(string.Empty, rui.MulticastInterface);
            Assert.AreEqual("0.0.0.0:0", rui.LocalAddress);
            Assert.AreEqual(0U, rui.NakDepth);
            Assert.IsNotNull(rui.NakResponseDelay);
            Assert.AreEqual(0, rui.NakResponseDelay.Seconds);
            Assert.AreEqual(200000, rui.NakResponseDelay.MicroSeconds);
            Assert.IsNotNull(rui.HeartbeatPeriod);
            Assert.AreEqual(1, rui.HeartbeatPeriod.Seconds);
            Assert.AreEqual(0, rui.HeartbeatPeriod.MicroSeconds);
            Assert.AreEqual(1, rui.Ttl);
            Assert.AreEqual(32U, rui.DatalinkControlChunks);
            Assert.AreEqual(10000, rui.DatalinkReleaseDelay);
            Assert.IsTrue(rui.IsReliable);
            Assert.AreEqual(2147481599u, rui.MaxPacketSize);
            Assert.AreEqual(10U, rui.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, rui.Name);
            Assert.AreEqual(4096u, rui.OptimumPacketSize);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Assert.AreEqual(65536, rui.RcvBufferSize);
                Assert.AreEqual(65536, rui.SendBufferSize);
            }
            else
            {
                Assert.AreEqual(65535, rui.RcvBufferSize);
                Assert.AreEqual(65535, rui.SendBufferSize);
            }
            Assert.AreEqual(TRANSPORT_TYPE, rui.TransportType);
            Assert.IsTrue(rui.RequiresCdr);
            Assert.IsFalse(rui.ThreadPerConnection);
            Assert.AreEqual(5, rui.ReceiveAddressDuration.Seconds);
            Assert.AreEqual(0, rui.ReceiveAddressDuration.MicroSeconds);

            TransportRegistry.Instance.RemoveInst(rui);
        }

        /// <summary>
        /// Test the properties non-default values after calling the constructor.
        /// </summary>
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
                    MicroSeconds = 400000,
                },
                HeartbeatPeriod = new TimeValue
                {
                    Seconds = 2,
                    MicroSeconds = 100000,
                },
                Ttl = 2,
                DatalinkControlChunks = 64U,
                DatalinkReleaseDelay = 20000,
                MaxPacketSize = 2147481500u,
                MaxSamplesPerPacket = 20U,
                OptimumPacketSize = 2048u,
                RcvBufferSize = 65530,
                SendBufferSize = 65530,
                ThreadPerConnection = true,
                ReceiveAddressDuration = new TimeValue
                {
                    Seconds = 2,
                    MicroSeconds = 100000,
                },
            };

            Assert.IsFalse(rui.UseMulticast);
            Assert.AreEqual("239.255.0.1:7402", rui.MulticastGroupAddress);
            Assert.AreEqual("eth0", rui.MulticastInterface);
            Assert.AreEqual("127.0.0.1:0", rui.LocalAddress);
            Assert.AreEqual(64U, rui.NakDepth);
            Assert.IsNotNull(rui.NakResponseDelay);
            Assert.AreEqual(1, rui.NakResponseDelay.Seconds);
            Assert.AreEqual(400000, rui.NakResponseDelay.MicroSeconds);
            Assert.IsNotNull(rui.HeartbeatPeriod);
            Assert.AreEqual(2, rui.HeartbeatPeriod.Seconds);
            Assert.AreEqual(100000, rui.HeartbeatPeriod.MicroSeconds);
            Assert.AreEqual(2, rui.Ttl);
            Assert.AreEqual(64U, rui.DatalinkControlChunks);
            Assert.AreEqual(20000, rui.DatalinkReleaseDelay);
            Assert.IsTrue(rui.IsReliable);
            Assert.AreEqual(2147481500u, rui.MaxPacketSize);
            Assert.AreEqual(20U, rui.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, rui.Name);
            Assert.AreEqual(2048u, rui.OptimumPacketSize);
            Assert.AreEqual(65530, rui.RcvBufferSize);
            Assert.AreEqual(65530, rui.SendBufferSize);
            Assert.AreEqual(TRANSPORT_TYPE, rui.TransportType);
            Assert.IsTrue(rui.RequiresCdr);
            Assert.IsTrue(rui.ThreadPerConnection);
            Assert.AreEqual(2, rui.ReceiveAddressDuration.Seconds);
            Assert.AreEqual(100000, rui.ReceiveAddressDuration.MicroSeconds);

            TransportRegistry.Instance.RemoveInst(rui);
        }
        #endregion
    }
}
