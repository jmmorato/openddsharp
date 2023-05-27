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
    /// <see cref="ShmemInst"/> unit test.
    /// </summary>
    [TestClass]
    public class ShmemInstTest
    {
        #region Constants
        private const string TEST_CATEGORY = "ShmemInst";
        private const string TRANSPORT_TYPE = "shmem";
        private const string INSTANCE_NAME = "ShmemInst";
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
            ShmemInst shmemi = new ShmemInst(inst);

            Assert.IsFalse(string.IsNullOrWhiteSpace(shmemi.HostName));
            Assert.AreEqual(16777216U, shmemi.PoolSize);
            Assert.IsFalse(string.IsNullOrWhiteSpace(shmemi.PoolName));
            Assert.AreEqual(4096U, shmemi.DatalinkControlSize);
            Assert.AreEqual(32U, shmemi.DatalinkControlChunks);
            Assert.AreEqual(10000, shmemi.DatalinkReleaseDelay);
            Assert.IsTrue(shmemi.IsReliable);
            Assert.AreEqual(2147481599u, shmemi.MaxPacketSize);
            Assert.AreEqual(10U, shmemi.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, shmemi.Name);
            Assert.AreEqual(4096u, shmemi.OptimumPacketSize);
            Assert.AreEqual(5U, shmemi.QueueInitialPools);
            Assert.AreEqual(10U, shmemi.QueueMessagesPerPool);
            Assert.AreEqual(TRANSPORT_TYPE, shmemi.TransportType);
            Assert.IsFalse(shmemi.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(shmemi);
        }

        /// <summary>
        /// Test the properties non-default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValues()
        {
            TransportInst inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            ShmemInst shmemi = new ShmemInst(inst)
            {
                PoolSize = 16000000U,
                DatalinkControlSize = 2048U,
                DatalinkControlChunks = 64U,
                DatalinkReleaseDelay = 20000,
                MaxPacketSize = 2147481500u,
                MaxSamplesPerPacket = 20U,
                OptimumPacketSize = 2048u,
                QueueInitialPools = 20U,
                QueueMessagesPerPool = 20U,
                ThreadPerConnection = true
            };

            Assert.AreEqual(16000000U, shmemi.PoolSize);
            Assert.AreEqual(2048U, shmemi.DatalinkControlSize);
            Assert.AreEqual(64U, shmemi.DatalinkControlChunks);
            Assert.AreEqual(20000, shmemi.DatalinkReleaseDelay);
            Assert.AreEqual(2147481500u, shmemi.MaxPacketSize);
            Assert.AreEqual(20U, shmemi.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, shmemi.Name);
            Assert.AreEqual(2048u, shmemi.OptimumPacketSize);
            Assert.AreEqual(20U, shmemi.QueueInitialPools);
            Assert.AreEqual(20U, shmemi.QueueMessagesPerPool);
            Assert.IsTrue(shmemi.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(shmemi);
        }
        #endregion
    }
}