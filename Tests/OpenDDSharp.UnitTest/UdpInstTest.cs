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
            var inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            var udpInst = new UdpInst(inst);
            Assert.AreEqual("0.0.0.0:0", udpInst.LocalAddress);
            Assert.AreEqual(32U, udpInst.DatalinkControlChunks);
            Assert.AreEqual(10000, udpInst.DatalinkReleaseDelay);
            Assert.IsFalse(udpInst.IsReliable);
            Assert.AreEqual(2147481599u, udpInst.MaxPacketSize);
            Assert.AreEqual(10U, udpInst.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, udpInst.Name);
            Assert.AreEqual(4096u, udpInst.OptimumPacketSize);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Assert.AreEqual(65536, udpInst.RcvBufferSize);
                Assert.AreEqual(65536, udpInst.SendBufferSize);
            }
            else
            {
                Assert.AreEqual(65535, udpInst.RcvBufferSize);
                Assert.AreEqual(65535, udpInst.SendBufferSize);
            }
            Assert.AreEqual(TRANSPORT_TYPE, udpInst.TransportType);
            Assert.IsFalse(udpInst.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(udpInst);
        }

        /// <summary>
        /// Test the properties non-default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValues()
        {
            var inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            var udpInst = new UdpInst(inst)
            {
                LocalAddress = "127.0.0.1:",
                DatalinkControlChunks = 64U,
                DatalinkReleaseDelay = 20000,
                MaxPacketSize = 2147481500u,
                MaxSamplesPerPacket = 20U,
                OptimumPacketSize = 2048u,
                RcvBufferSize = 65530,
                SendBufferSize = 65530,
                ThreadPerConnection = true,
            };

            Assert.AreEqual("127.0.0.1:0", udpInst.LocalAddress);
            Assert.AreEqual(64U, udpInst.DatalinkControlChunks);
            Assert.AreEqual(20000, udpInst.DatalinkReleaseDelay);
            Assert.AreEqual(2147481500u, udpInst.MaxPacketSize);
            Assert.AreEqual(20U, udpInst.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, udpInst.Name);
            Assert.AreEqual(2048u, udpInst.OptimumPacketSize);
            Assert.AreEqual(65530, udpInst.RcvBufferSize);
            Assert.AreEqual(65530, udpInst.SendBufferSize);
            Assert.AreEqual(TRANSPORT_TYPE, udpInst.TransportType);
            Assert.IsTrue(udpInst.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(udpInst);
        }
        #endregion
    }
}