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

namespace OpenDDSharp.Standard.UnitTest
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
            TransportInst inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            TcpInst tcpi = new TcpInst(inst);
            Assert.AreEqual(2000, tcpi.PassiveReconnectDuration);
            Assert.AreEqual(-1, tcpi.MaxOutputPausePeriod);
            Assert.AreEqual(3, tcpi.ConnRetryAttempts);
            Assert.AreEqual(2.0, tcpi.ConnRetryBackoffMultiplier);
            Assert.AreEqual(500, tcpi.ConnRetryInitialDelay);
            Assert.IsFalse(tcpi.EnableNagleAlgorithm);
            Assert.IsTrue(tcpi.IsReliable);
            Assert.AreEqual(string.Empty, tcpi.PublicAddress);
            Assert.AreEqual(string.Empty, tcpi.LocalAddress);
            Assert.AreEqual(32U, tcpi.DatalinkControlChunks);
            Assert.AreEqual(10000, tcpi.DatalinkReleaseDelay);
            Assert.IsTrue(tcpi.IsReliable);
            Assert.AreEqual(2147481599u, tcpi.MaxPacketSize);
            Assert.AreEqual(10U, tcpi.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, tcpi.Name);
            Assert.AreEqual(4096u, tcpi.OptimumPacketSize);
            Assert.AreEqual(5U, tcpi.QueueInitialPools);
            Assert.AreEqual(10U, tcpi.QueueMessagesPerPool);
            Assert.AreEqual(TRANSPORT_TYPE, tcpi.TransportType);
            Assert.IsFalse(tcpi.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(tcpi);
        }

        /// <summary>
        /// Test the properties non-default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValues()
        {
            TransportInst inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            TcpInst tcpi = new TcpInst(inst)
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
                QueueInitialPools = 10U,
                QueueMessagesPerPool = 20U,
                ThreadPerConnection = true,
            };

            Assert.AreEqual(1000, tcpi.PassiveReconnectDuration);
            Assert.AreEqual(5000, tcpi.MaxOutputPausePeriod);
            Assert.AreEqual(5, tcpi.ConnRetryAttempts);
            Assert.AreEqual(1.5, tcpi.ConnRetryBackoffMultiplier);
            Assert.AreEqual(1000, tcpi.ConnRetryInitialDelay);
            Assert.IsTrue(tcpi.EnableNagleAlgorithm);
            Assert.IsTrue(tcpi.IsReliable);
            Assert.AreEqual("127.0.0.1:", tcpi.PublicAddress);
            Assert.AreEqual("127.0.0.1:", tcpi.LocalAddress);
            Assert.AreEqual(64U, tcpi.DatalinkControlChunks);
            Assert.AreEqual(20000, tcpi.DatalinkReleaseDelay);
            Assert.AreEqual(2147481500u, tcpi.MaxPacketSize);
            Assert.AreEqual(20U, tcpi.MaxSamplesPerPacket);
            Assert.AreEqual(2048u, tcpi.OptimumPacketSize);
            Assert.AreEqual(10U, tcpi.QueueInitialPools);
            Assert.AreEqual(20U, tcpi.QueueMessagesPerPool);
            Assert.IsTrue(tcpi.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(tcpi);
        }
        #endregion
    }
}