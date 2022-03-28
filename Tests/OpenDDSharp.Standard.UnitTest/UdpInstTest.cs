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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.Standard.UnitTest
{
    /// <summary>
    /// <see cref="UdpInst"/> unit test.
    /// </summary>
    [TestClass]
    public class UdpInstTest
    {
        #region Constants
        private const string TEST_CATEGORY = "UdpInst";
        private const string TRANSPORT_TYPE = "udp";
        private const string INSTANCE_NAME = "UdpInst";
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
            UdpInst udpi = new UdpInst(inst);
            Assert.AreEqual("0.0.0.0:0", udpi.LocalAddress);
            Assert.AreEqual(32U, udpi.DatalinkControlChunks);
            Assert.AreEqual(10000, udpi.DatalinkReleaseDelay);
            Assert.IsFalse(udpi.IsReliable);
            Assert.AreEqual(2147481599u, udpi.MaxPacketSize);
            Assert.AreEqual(10U, udpi.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, udpi.Name);
            Assert.AreEqual(4096u, udpi.OptimumPacketSize);
            Assert.AreEqual(5U, udpi.QueueInitialPools);
            Assert.AreEqual(10U, udpi.QueueMessagesPerPool);
            Assert.AreEqual(65535, udpi.RcvBufferSize);
            Assert.AreEqual(65535, udpi.SendBufferSize);
            Assert.AreEqual(TRANSPORT_TYPE, udpi.TransportType);
            Assert.IsFalse(udpi.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(udpi);
        }

        /// <summary>
        /// Test the properties non-default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValues()
        {
            TransportInst inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            UdpInst udpi = new UdpInst(inst)
            {
                LocalAddress = "127.0.0.1:",
                DatalinkControlChunks = 64U,
                DatalinkReleaseDelay = 20000,
                MaxPacketSize = 2147481500u,
                MaxSamplesPerPacket = 20U,
                OptimumPacketSize = 2048u,
                QueueInitialPools = 20U,
                QueueMessagesPerPool = 20U,
                RcvBufferSize = 65530,
                SendBufferSize = 65530,
                ThreadPerConnection = true,
            };

            Assert.AreEqual("127.0.0.1:0", udpi.LocalAddress);
            Assert.AreEqual(64U, udpi.DatalinkControlChunks);
            Assert.AreEqual(20000, udpi.DatalinkReleaseDelay);
            Assert.AreEqual(2147481500u, udpi.MaxPacketSize);
            Assert.AreEqual(20U, udpi.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, udpi.Name);
            Assert.AreEqual(2048u, udpi.OptimumPacketSize);
            Assert.AreEqual(20U, udpi.QueueInitialPools);
            Assert.AreEqual(20U, udpi.QueueMessagesPerPool);
            Assert.AreEqual(65530, udpi.RcvBufferSize);
            Assert.AreEqual(65530, udpi.SendBufferSize);
            Assert.AreEqual(TRANSPORT_TYPE, udpi.TransportType);
            Assert.IsTrue(udpi.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(udpi);
        }
        #endregion
    }
}