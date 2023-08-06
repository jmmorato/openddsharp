/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

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

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="TcpInst"/> unit test.
    /// </summary>
    [TestClass]
    public class TcpInstTest
    {
        #region Constants
        private const string TEST_CATEGORY = "TcpInst";
        private const string TRANSPORT_TYPE = "tcp";
        private const string INSTANCE_NAME = "TcpInst";
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
            var tcpInst = new TcpInst(inst);
            Assert.AreEqual(2000, tcpInst.PassiveReconnectDuration);
            Assert.AreEqual(-1, tcpInst.MaxOutputPausePeriod);
            Assert.AreEqual(3, tcpInst.ConnRetryAttempts);
            Assert.AreEqual(2.0, tcpInst.ConnRetryBackoffMultiplier);
            Assert.AreEqual(500, tcpInst.ConnRetryInitialDelay);
            Assert.IsFalse(tcpInst.EnableNagleAlgorithm);
            Assert.IsTrue(tcpInst.IsReliable);
            Assert.AreEqual(string.Empty, tcpInst.PublicAddress);
            Assert.AreEqual(string.Empty, tcpInst.LocalAddress);
            Assert.AreEqual(32U, tcpInst.DatalinkControlChunks);
            Assert.AreEqual(10000, tcpInst.DatalinkReleaseDelay);
            Assert.IsTrue(tcpInst.IsReliable);
            Assert.AreEqual(2147481599u, tcpInst.MaxPacketSize);
            Assert.AreEqual(10U, tcpInst.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, tcpInst.Name);
            Assert.AreEqual(4096u, tcpInst.OptimumPacketSize);
            Assert.AreEqual(TRANSPORT_TYPE, tcpInst.TransportType);
            Assert.IsFalse(tcpInst.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(tcpInst);
        }

        /// <summary>
        /// Test the properties non-default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValues()
        {
            var inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            var tcpInst = new TcpInst(inst)
            {
                PassiveReconnectDuration = 1000,
                MaxOutputPausePeriod = 5000,
                ConnRetryAttempts = 5,
                ConnRetryBackoffMultiplier = 1.5,
                ConnRetryInitialDelay = 1000,
                EnableNagleAlgorithm = true,
                PublicAddress = "127.0.0.1:",
                LocalAddress = "127.0.0.1:",
                DatalinkControlChunks = 64U,
                DatalinkReleaseDelay = 20000,
                MaxPacketSize = 2147481500u,
                MaxSamplesPerPacket = 20U,
                OptimumPacketSize = 2048u,
                ThreadPerConnection = true,
            };

            Assert.AreEqual(1000, tcpInst.PassiveReconnectDuration);
            Assert.AreEqual(5000, tcpInst.MaxOutputPausePeriod);
            Assert.AreEqual(5, tcpInst.ConnRetryAttempts);
            Assert.AreEqual(1.5, tcpInst.ConnRetryBackoffMultiplier);
            Assert.AreEqual(1000, tcpInst.ConnRetryInitialDelay);
            Assert.IsTrue(tcpInst.EnableNagleAlgorithm);
            Assert.IsTrue(tcpInst.IsReliable);
            Assert.AreEqual("127.0.0.1:", tcpInst.PublicAddress);
            Assert.AreEqual("127.0.0.1:", tcpInst.LocalAddress);
            Assert.AreEqual(64U, tcpInst.DatalinkControlChunks);
            Assert.AreEqual(20000, tcpInst.DatalinkReleaseDelay);
            Assert.AreEqual(2147481500u, tcpInst.MaxPacketSize);
            Assert.AreEqual(20U, tcpInst.MaxSamplesPerPacket);
            Assert.AreEqual(2048u, tcpInst.OptimumPacketSize);
            Assert.IsTrue(tcpInst.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(tcpInst);
        }
        #endregion
    }
}