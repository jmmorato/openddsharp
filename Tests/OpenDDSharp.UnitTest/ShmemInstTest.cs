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
            var inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            var shmemInst = new ShmemInst(inst);

            Assert.IsFalse(string.IsNullOrWhiteSpace(shmemInst.HostName));
            Assert.AreEqual(16777216U, shmemInst.PoolSize);
            Assert.IsFalse(string.IsNullOrWhiteSpace(shmemInst.PoolName));
            Assert.AreEqual(4096U, shmemInst.DatalinkControlSize);
            Assert.AreEqual(32U, shmemInst.DatalinkControlChunks);
            Assert.AreEqual(10000, shmemInst.DatalinkReleaseDelay);
            Assert.IsTrue(shmemInst.IsReliable);
            Assert.AreEqual(2147481599u, shmemInst.MaxPacketSize);
            Assert.AreEqual(10U, shmemInst.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, shmemInst.Name);
            Assert.AreEqual(4096u, shmemInst.OptimumPacketSize);
            Assert.AreEqual(TRANSPORT_TYPE, shmemInst.TransportType);
            Assert.IsFalse(shmemInst.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(shmemInst);
        }

        /// <summary>
        /// Test the properties non-default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValues()
        {
            var inst = TransportRegistry.Instance.CreateInst(INSTANCE_NAME, TRANSPORT_TYPE);
            var shmemInst = new ShmemInst(inst)
            {
                PoolSize = 16000000U,
                DatalinkControlSize = 2048U,
                DatalinkControlChunks = 64U,
                DatalinkReleaseDelay = 20000,
                MaxPacketSize = 2147481500u,
                MaxSamplesPerPacket = 20U,
                OptimumPacketSize = 2048u,
                ThreadPerConnection = true,
            };

            Assert.AreEqual(16000000U, shmemInst.PoolSize);
            Assert.AreEqual(2048U, shmemInst.DatalinkControlSize);
            Assert.AreEqual(64U, shmemInst.DatalinkControlChunks);
            Assert.AreEqual(20000, shmemInst.DatalinkReleaseDelay);
            Assert.AreEqual(2147481500u, shmemInst.MaxPacketSize);
            Assert.AreEqual(20U, shmemInst.MaxSamplesPerPacket);
            Assert.AreEqual(INSTANCE_NAME, shmemInst.Name);
            Assert.AreEqual(2048u, shmemInst.OptimumPacketSize);
            Assert.IsTrue(shmemInst.ThreadPerConnection);

            TransportRegistry.Instance.RemoveInst(shmemInst);
        }
        #endregion
    }
}