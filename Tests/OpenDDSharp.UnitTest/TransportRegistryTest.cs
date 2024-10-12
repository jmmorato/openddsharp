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
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Helpers;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="TransportRegistry"/> unit test.
    /// </summary>
    [TestClass]
    public class TransportRegistryTest
    {
        #region Constants
        private const string TEST_CATEGORY = "TransportRegistry";
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="TransportRegistry.GetConfig(string)" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGlobalConfig()
        {
            // By default returns the OpenDDS default config
            var config = TransportRegistry.Instance.GlobalConfig;
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
            var exception = false;
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

        /// <summary>
        /// Test the <see cref="TransportRegistry.CreateInst(string, string)" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateInst()
        {
            // Create a RTPS UDP transport instance
            var inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "rtps_udp");
            Assert.IsNotNull(inst);
            Assert.AreEqual(nameof(TestCreateInst), inst.Name);
            Assert.AreEqual("rtps_udp", inst.TransportType);
            var rui = new RtpsUdpInst(inst);
            Assert.IsNotNull(rui);
            TransportRegistry.Instance.RemoveInst(inst);

            // Create a UDP transport instance
            inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "udp");
            Assert.IsNotNull(inst);
            Assert.AreEqual(nameof(TestCreateInst), inst.Name);
            Assert.AreEqual("udp", inst.TransportType);
            var udpInst = new UdpInst(inst);
            Assert.IsNotNull(udpInst);
            TransportRegistry.Instance.RemoveInst(inst);

            // Create a Multicast transport instance
            inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "multicast");
            Assert.IsNotNull(inst);
            Assert.AreEqual(nameof(TestCreateInst), inst.Name);
            Assert.AreEqual("multicast", inst.TransportType);
            var multicastInst = new MulticastInst(inst);
            Assert.IsNotNull(multicastInst);
            TransportRegistry.Instance.RemoveInst(inst);

            // Create a TCP transport instance
            inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "tcp");
            Assert.IsNotNull(inst);
            Assert.AreEqual(nameof(TestCreateInst), inst.Name);
            Assert.AreEqual("tcp", inst.TransportType);
            var tcpInst = new TcpInst(inst);
            Assert.IsNotNull(tcpInst);
            TransportRegistry.Instance.RemoveInst(inst);

            // Create a SharedMemory transport instance
            inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "shmem");
            Assert.IsNotNull(inst);
            Assert.AreEqual(nameof(TestCreateInst), inst.Name);
            Assert.AreEqual("shmem", inst.TransportType);
            var shmemInst = new ShmemInst(inst);
            Assert.IsNotNull(shmemInst);
            TransportRegistry.Instance.RemoveInst(inst);

            // Create an instance with an invalid transport type
            inst = TransportRegistry.Instance.CreateInst(nameof(TestCreateInst), "quantic_teletransportation");
            Assert.IsNull(inst);

            // Try to create a transport instance with empty name
            var exception = false;
            try
            {
                TransportRegistry.Instance.CreateInst(null, "shmem");
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
                TransportRegistry.Instance.CreateInst(string.Empty, "shmem");
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
                TransportRegistry.Instance.CreateInst("  ", "shmem");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);
        }

        /// <summary>
        /// Test the <see cref="TransportRegistry.GetInst(string)" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetInst()
        {
            // Get not existing transport instance
            var inst = TransportRegistry.Instance.GetInst(nameof(TestGetInst));
            Assert.IsNull(inst);

            // Create a new transport instance and try to get it
            var created = TransportRegistry.Instance.CreateInst(nameof(TestGetInst), "shmem");
            Assert.IsNotNull(created);

            inst = TransportRegistry.Instance.GetInst(nameof(TestGetInst));
            Assert.IsNotNull(inst);
            Assert.AreEqual(created, inst);

            // Test name parameter guards
            inst = TransportRegistry.Instance.GetInst(null);
            Assert.IsNull(inst);

            inst = TransportRegistry.Instance.GetInst(string.Empty);
            Assert.IsNull(inst);

            inst = TransportRegistry.Instance.GetInst("   ");
            Assert.IsNull(inst);
        }

        /// <summary>
        /// Test the <see cref="TransportRegistry.RemoveInst(TransportInst)" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestRemoveInst()
        {
            // Create a transport instance and remove it later
            var created = TransportRegistry.Instance.CreateInst(nameof(TestRemoveInst), "shmem");
            Assert.IsNotNull(created);

            var inst = TransportRegistry.Instance.GetInst(nameof(TestRemoveInst));
            Assert.IsNotNull(inst);
            Assert.AreEqual(created, inst);

            TransportRegistry.Instance.RemoveInst(created);

            created = TransportRegistry.Instance.GetInst(nameof(TestRemoveInst));
            Assert.IsNull(created);

            // Remove a null instance should not throw exception
            var exception = false;
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

        /// <summary>
        /// Test the <see cref="TransportRegistry.CreateConfig(string)" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateConfig()
        {
            // Create a new config
            var config = TransportRegistry.Instance.CreateConfig(nameof(TestCreateConfig));
            Assert.IsNotNull(config);
            Assert.AreEqual(nameof(TestCreateConfig), config.Name);

            // Try to create a config with the same name
            var other = TransportRegistry.Instance.CreateConfig(nameof(TestCreateConfig));
            Assert.IsNull(other);

            // Remove the config and create it again with the same name
            TransportRegistry.Instance.RemoveConfig(config);
            other = TransportRegistry.Instance.CreateConfig(nameof(TestCreateConfig));
            Assert.IsNotNull(other);

            // Try to create a transport instance with empty name
            var exception = false;
            try
            {
                TransportRegistry.Instance.CreateConfig(null);
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
                TransportRegistry.Instance.CreateConfig(string.Empty);
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
                TransportRegistry.Instance.CreateConfig("   ");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);
        }

        /// <summary>
        /// Test the <see cref="TransportRegistry.GetConfig(string)" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetConfig()
        {
            // Get not existing transport config
            var config = TransportRegistry.Instance.GetConfig(nameof(TestGetConfig));
            Assert.IsNull(config);

            // Get the default OpenDDS config
            config = TransportRegistry.Instance.GetConfig("_OPENDDS_DEFAULT_CONFIG");
            Assert.IsNotNull(config);
            Assert.AreEqual("_OPENDDS_DEFAULT_CONFIG", config.Name);

            // Create a new transport config and try to get it
            var created = TransportRegistry.Instance.CreateConfig(nameof(TestGetConfig));
            Assert.IsNotNull(created);

            config = TransportRegistry.Instance.GetConfig(nameof(TestGetConfig));
            Assert.IsNotNull(config);
            Assert.AreEqual(created, config);

            // Test name parameter guards
            config = TransportRegistry.Instance.GetConfig(null);
            Assert.IsNull(config);

            config = TransportRegistry.Instance.GetConfig(string.Empty);
            Assert.IsNull(config);

            config = TransportRegistry.Instance.GetConfig("   ");
            Assert.IsNull(config);
        }

        /// <summary>
        /// Test the <see cref="TransportRegistry.RemoveConfig(TransportConfig)" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestRemoveConfig()
        {
            // Create a transport config and remove it later
            var created = TransportRegistry.Instance.CreateConfig(nameof(TestRemoveConfig));
            Assert.IsNotNull(created);

            var config = TransportRegistry.Instance.GetConfig(nameof(TestRemoveConfig));
            Assert.IsNotNull(config);
            Assert.AreEqual(created, config);

            TransportRegistry.Instance.RemoveConfig(created);

            created = TransportRegistry.Instance.GetConfig(nameof(TestRemoveConfig));
            Assert.IsNull(created);

            // Remove a null config should not throw exception
            var exception = false;
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

        /// <summary>
        /// Test the <see cref="TransportRegistry.GetDomainDefaultConfig(int)" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDomainDefaultConfig()
        {
            // Get default config from a domain that is not set
            var config = TransportRegistry.Instance.GetDomainDefaultConfig(69);
            Assert.IsNull(config);

            // Create a config and assign it to a domain
            var created = TransportRegistry.Instance.CreateConfig(nameof(TestGetDomainDefaultConfig));
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

        /// <summary>
        /// Test the <see cref="TransportRegistry.SetDomainDefaultConfig(int, TransportConfig)" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDomainDefaultConfig()
        {
            // Create a new config
            var created = TransportRegistry.Instance.CreateConfig(nameof(TestSetDomainDefaultConfig));
            Assert.IsNotNull(created);

            // Set the created config to a domain
            TransportRegistry.Instance.SetDomainDefaultConfig(69, created);

            // Get the config and check it
            var config = TransportRegistry.Instance.GetDomainDefaultConfig(69);
            Assert.IsNotNull(config);
            Assert.AreEqual(created, config);
            Assert.AreEqual(nameof(TestSetDomainDefaultConfig), config.Name);

            // Test parameters guards
            var exception = false;
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

        /// <summary>
        /// Test the <see cref="TransportRegistry.BindConfig(string, Entity)" /> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestBindConfig()
        {
            // Create a domain participant
            var participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_OTHER_DOMAIN);
            Assert.IsNotNull(participant);
            participant.BindRtpsUdpTransportConfig();

            // Create a transport config
            var config = TransportRegistry.Instance.CreateConfig(nameof(TestBindConfig));
            Assert.IsNotNull(config);

            // Bind the participant using the name
            var exception = false;
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
                TransportRegistry.Instance.BindConfig(string.Empty, participant);
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

            var ret = AssemblyInitializer.Factory.DeleteParticipant(participant);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }
        #endregion
    }
}