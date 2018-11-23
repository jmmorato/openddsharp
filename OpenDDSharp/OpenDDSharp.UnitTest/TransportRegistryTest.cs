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
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class TransportRegistryTest
    {
        #region Constants
        private const string TEST_CATEGORY = "TransportRegistry";
        #endregion

        #region Test Methods
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGlobalConfig()
        {
            // By default returns the OpenDDS default config
            TransportConfig config = TransportRegistry.Instance.GlobalConfig;
            Assert.IsNotNull(config);
            Assert.AreEqual("_OPENDDS_DEFAULT_CONFIG", config.Name);
            
            // Create a new config
            config = TransportRegistry.Instance.CreateConfig(nameof(TestGlobalConfig));
            Assert.IsNotNull(config);
            Assert.AreEqual(nameof(TestGlobalConfig), config.Name);

            // Assign the new config and check it
            TransportRegistry.Instance.GlobalConfig = config;
            Assert.IsNotNull(TransportRegistry.Instance.GlobalConfig);
            Assert.AreEqual(nameof(TestGlobalConfig), TransportRegistry.Instance.GlobalConfig.Name);

            // Null config throws an ArgumentNullException
            bool exception = false;
            try
            {
                TransportRegistry.Instance.GlobalConfig = null;
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateInst()
        {
            // Create a RTPS UDP transport instance
            TransportInst inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "rtps_udp");
            Assert.IsNotNull(inst);
            Assert.AreEqual(nameof(TestCreateInst), inst.Name);
            Assert.AreEqual("rtps_udp", inst.TransportType);
            RtpsUdpInst rui = new RtpsUdpInst(inst);
            Assert.IsNotNull(rui);
            TransportRegistry.Instance.RemoveInst(inst);

            // Create a UDP transport instance
            inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "udp");
            Assert.IsNotNull(inst);
            Assert.AreEqual(nameof(TestCreateInst), inst.Name);
            Assert.AreEqual("udp", inst.TransportType);
            UdpInst udpi = new UdpInst(inst);
            Assert.IsNotNull(udpi);
            TransportRegistry.Instance.RemoveInst(inst);

            // Create a Multicast transport instance
            inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "multicast");
            Assert.IsNotNull(inst);
            Assert.AreEqual(nameof(TestCreateInst), inst.Name);
            Assert.AreEqual("multicast", inst.TransportType);
            MulticastInst multicasti = new MulticastInst(inst);
            Assert.IsNotNull(multicasti);
            TransportRegistry.Instance.RemoveInst(inst);

            // Create a Multicast transport instance
            inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "tcp");
            Assert.IsNotNull(inst);
            Assert.AreEqual(nameof(TestCreateInst), inst.Name);
            Assert.AreEqual("tcp", inst.TransportType);
            MulticastInst tcpi = new MulticastInst(inst);
            Assert.IsNotNull(tcpi);
            TransportRegistry.Instance.RemoveInst(inst);

            // Create a SharedMemory transport instance
            inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "shmem");
            Assert.IsNotNull(inst);
            Assert.AreEqual(nameof(TestCreateInst), inst.Name);
            Assert.AreEqual("shmem", inst.TransportType);
            ShmemInst shmemi = new ShmemInst(inst);
            Assert.IsNotNull(shmemi);
            TransportRegistry.Instance.RemoveInst(inst);

            // Create a instance with an invalid transport type
            inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "quantic_teletransport");
            Assert.IsNull(inst);

            // Try to create a transport instance with empty name
            bool exception = false;
            try
            {
                inst = TransportRegistry.Instance.CreateInst(null, "shmem");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                inst = TransportRegistry.Instance.CreateInst("", "shmem");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                inst = TransportRegistry.Instance.CreateInst("  ", "shmem");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetInst()
        {
            // Get a not existing transport instance
            TransportInst inst = TransportRegistry.Instance.GetInst(nameof(TestGetInst));
            Assert.IsNull(inst);

            // Create a new transport instance an try to get it
            TransportInst created = TransportRegistry.Instance.CreateInst(nameof(TestGetInst), "shmem");
            Assert.IsNotNull(created);

            inst = TransportRegistry.Instance.GetInst(nameof(TestGetInst));
            Assert.IsNotNull(inst);
            Assert.AreEqual(created, inst);

            // Test name parameter guards
            inst = TransportRegistry.Instance.GetInst(null);
            Assert.IsNull(inst);

            inst = TransportRegistry.Instance.GetInst("");
            Assert.IsNull(inst);

            inst = TransportRegistry.Instance.GetInst("   ");
            Assert.IsNull(inst);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestRemoveInst()
        {
            // Create a transport instance and remove it later
            TransportInst created = TransportRegistry.Instance.CreateInst(nameof(TestRemoveInst), "shmem");
            Assert.IsNotNull(created);

            TransportInst inst = TransportRegistry.Instance.GetInst(nameof(TestRemoveInst));
            Assert.IsNotNull(inst);
            Assert.AreEqual(created, inst);

            TransportRegistry.Instance.RemoveInst(created);

            created = TransportRegistry.Instance.GetInst(nameof(TestRemoveInst));
            Assert.IsNull(created);

            // Remove a null instance should not throw exception
            bool exception = false;
            try
            {
                TransportRegistry.Instance.RemoveInst(null);
            }
            catch
            {
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateConfig()
        {
            // Create a new config
            TransportConfig config = TransportRegistry.Instance.CreateConfig(nameof(TestCreateConfig));
            Assert.IsNotNull(config);
            Assert.AreEqual(nameof(TestCreateConfig), config.Name);

            // Try to create a config with the same name
            TransportConfig other = TransportRegistry.Instance.CreateConfig(nameof(TestCreateConfig));
            Assert.IsNull(other);

            // Remove the config and create it again with the same name
            TransportRegistry.Instance.RemoveConfig(config);
            other = TransportRegistry.Instance.CreateConfig(nameof(TestCreateConfig));
            Assert.IsNotNull(other);

            // Try to create a transport instance with empty name
            bool exception = false;
            try
            {
                config = TransportRegistry.Instance.CreateConfig(null);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                config = TransportRegistry.Instance.CreateConfig("");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                config = TransportRegistry.Instance.CreateConfig("   ");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetConfig()
        {
            // Get a not existing transport config
            TransportConfig config = TransportRegistry.Instance.GetConfig(nameof(TestGetConfig));
            Assert.IsNull(config);

            // Get the default OpenDDS config
            config = TransportRegistry.Instance.GetConfig("_OPENDDS_DEFAULT_CONFIG");
            Assert.IsNotNull(config);
            Assert.AreEqual("_OPENDDS_DEFAULT_CONFIG", config.Name);

            // Create a new transport config an try to get it
            TransportConfig created = TransportRegistry.Instance.CreateConfig(nameof(TestGetConfig));
            Assert.IsNotNull(created);

            config = TransportRegistry.Instance.GetConfig(nameof(TestGetConfig));
            Assert.IsNotNull(config);
            Assert.AreEqual(created, config);

            // Test name parameter guards
            config = TransportRegistry.Instance.GetConfig(null);
            Assert.IsNull(config);

            config = TransportRegistry.Instance.GetConfig("");
            Assert.IsNull(config);

            config = TransportRegistry.Instance.GetConfig("   ");
            Assert.IsNull(config);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestRemoveConfig()
        {
            // Create a transport config and remove it later
            TransportConfig created = TransportRegistry.Instance.CreateConfig(nameof(TestRemoveConfig));
            Assert.IsNotNull(created);

            TransportConfig config = TransportRegistry.Instance.GetConfig(nameof(TestRemoveConfig));
            Assert.IsNotNull(config);
            Assert.AreEqual(created, config);

            TransportRegistry.Instance.RemoveConfig(created);

            created = TransportRegistry.Instance.GetConfig(nameof(TestRemoveConfig));
            Assert.IsNull(created);

            // Remove a null config should not throw exception
            bool exception = false;
            try
            {
                TransportRegistry.Instance.RemoveConfig(null);
            }
            catch
            {
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDomainDefaultConfig()
        {
            // Get default config from a domain that is not set
            TransportConfig config = TransportRegistry.Instance.GetDomainDefaultConfig(69);
            Assert.IsNull(config);

            // Create a config and assign it to a domain
            TransportConfig created = TransportRegistry.Instance.CreateConfig(nameof(TestGetDomainDefaultConfig));
            TransportRegistry.Instance.SetDomainDefaultConfig(69, created);

            // Get the default config and check it
            config = TransportRegistry.Instance.GetDomainDefaultConfig(69);
            Assert.IsNotNull(config);
            Assert.AreEqual(created, config);
            Assert.AreEqual(nameof(TestGetDomainDefaultConfig), config.Name);

            // Test with an incorrect domain id
            config = TransportRegistry.Instance.GetDomainDefaultConfig(-1);
            Assert.IsNull(config);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDomainDefaultConfig()
        {
            // Create a new config
            TransportConfig created = TransportRegistry.Instance.CreateConfig(nameof(TestSetDomainDefaultConfig));
            Assert.IsNotNull(created);

            // Set the created config to a domain
            TransportRegistry.Instance.SetDomainDefaultConfig(69, created);

            // Get the config and check it
            TransportConfig config = TransportRegistry.Instance.GetDomainDefaultConfig(69);
            Assert.IsNotNull(config);
            Assert.AreEqual(created, config);
            Assert.AreEqual(nameof(TestSetDomainDefaultConfig), config.Name);

            // Test parameters guards
            bool exception = false;
            try
            {
                TransportRegistry.Instance.SetDomainDefaultConfig(-1, created);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                TransportRegistry.Instance.SetDomainDefaultConfig(0, null);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestBindConfig()
        {
            // Create a domain participant
            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory();
            Assert.IsNotNull(dpf);
            DomainParticipant participant = dpf.CreateParticipant(69);
            Assert.IsNotNull(participant);

            // Create a transport config
            TransportConfig config = TransportRegistry.Instance.CreateConfig(nameof(TestBindConfig));
            Assert.IsNotNull(config);

            // Bind the participant using the name
            bool exception = false;
            try
            {
                TransportRegistry.Instance.BindConfig(nameof(TestBindConfig), participant);
            }
            catch
            {
                exception = true;
            }
            Assert.IsFalse(exception);

            // Bind the participant using the transport config
            exception = false;
            try
            {
                TransportRegistry.Instance.BindConfig(config, participant);
            }
            catch
            {
                exception = true;
            }
            Assert.IsFalse(exception);

            // Test parameters guards
            exception = false;
            try
            {
                TransportRegistry.Instance.BindConfig((string)null, participant);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                TransportRegistry.Instance.BindConfig("", participant);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                TransportRegistry.Instance.BindConfig("  ", participant);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                TransportRegistry.Instance.BindConfig(nameof(TestBindConfig), null);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                TransportRegistry.Instance.BindConfig((TransportConfig)null, participant);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                TransportRegistry.Instance.BindConfig(config, null);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);
        }
        #endregion
    }
}
