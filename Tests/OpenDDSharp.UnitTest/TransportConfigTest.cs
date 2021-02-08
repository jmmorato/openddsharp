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
using System;
using System.Linq;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class TransportConfigTest
    {
        #region Constants
        private const string TEST_CATEGORY = "TransportConfig";
        #endregion

        #region Test Methods        
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDefaultValue()
        {
            TransportConfig config = TransportRegistry.Instance.CreateConfig(nameof(TestDefaultValue));
            Assert.IsNotNull(config);
            Assert.AreEqual(nameof(TestDefaultValue), config.Name);
            Assert.AreEqual(60000u, config.PassiveConnectDuration);
            Assert.IsFalse(config.SwapBytes);
            Assert.IsNotNull(config.Transports);
            Assert.AreEqual(0, config.Transports.Count);

            TransportRegistry.Instance.RemoveConfig(config);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValue()
        {
            TransportConfig config = TransportRegistry.Instance.CreateConfig(nameof(TestNonDefaultValue));
            Assert.IsNotNull(config);
            config.SwapBytes = true;
            config.PassiveConnectDuration = 30000u;
            TransportInst inst = TransportRegistry.Instance.CreateInst(nameof(TestNonDefaultValue), "shmem");
            Assert.IsNotNull(inst);
            config.Insert(inst);
            
            Assert.IsNotNull(config);
            Assert.AreEqual(nameof(TestNonDefaultValue), config.Name);
            Assert.AreEqual(30000u, config.PassiveConnectDuration);
            Assert.IsTrue(config.SwapBytes);
            Assert.IsNotNull(config.Transports);
            Assert.AreEqual(1, config.Transports.Count);
            Assert.AreEqual(inst.Name, config.Transports.First().Name);

            TransportRegistry.Instance.RemoveConfig(config);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestInsert()
        {
            // Test successfully insert
            TransportConfig config = TransportRegistry.Instance.CreateConfig(nameof(TestInsert));
            Assert.IsNotNull(config);
            TransportInst inst = TransportRegistry.Instance.CreateInst(nameof(TestInsert), "shmem");
            config.Insert(inst);
            Assert.IsNotNull(inst);

            Assert.IsNotNull(config.Transports);
            Assert.AreEqual(1, config.Transports.Count);
            Assert.AreEqual(inst.Name, config.Transports.First().Name);
            Assert.AreEqual(inst.TransportType, "shmem");

            // Test parameter guard
            bool exception = false;
            try
            {
                config.Insert(null);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            TransportRegistry.Instance.RemoveConfig(config);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSortedInsert()
        {
            // Test successfully insert
            TransportConfig config = TransportRegistry.Instance.CreateConfig(nameof(TestSortedInsert));
            Assert.IsNotNull(config);

            // Create transport instance B
            TransportInst instB = TransportRegistry.Instance.CreateInst(nameof(TestSortedInsert) + "B", "shmem");
            config.SortedInsert(instB);
            Assert.IsNotNull(instB);

            Assert.IsNotNull(config.Transports);
            Assert.AreEqual(1, config.Transports.Count);
            Assert.AreEqual(instB.Name, config.Transports.First().Name);
            Assert.AreEqual(instB.TransportType, "shmem");

            // Create transport instance A
            TransportInst instA = TransportRegistry.Instance.CreateInst(nameof(TestSortedInsert) + "A", "udp");
            config.SortedInsert(instA);
            Assert.IsNotNull(instA);

            // Check the transport list
            Assert.IsNotNull(config.Transports);
            Assert.AreEqual(2, config.Transports.Count);
            Assert.AreEqual(instA.Name, config.Transports.ElementAt(0).Name);
            Assert.AreEqual(instA.TransportType, "udp");
            Assert.AreEqual(instB.Name, config.Transports.ElementAt(1).Name);
            Assert.AreEqual(instB.TransportType, "shmem");

            // Test parameter guard
            bool exception = false;
            try
            {
                config.SortedInsert(null);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            TransportRegistry.Instance.RemoveConfig(config);
        }
        #endregion
    }
}
