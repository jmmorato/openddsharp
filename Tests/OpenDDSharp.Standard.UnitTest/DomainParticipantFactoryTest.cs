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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.Standard.UnitTest.Helpers;
using OpenDDSharp.Standard.UnitTest.Listeners;

namespace OpenDDSharp.Standard.UnitTest
{
    /// <summary>
    /// <see cref="DomainParticipantFactory"/> unit test class.
    /// </summary>
    [TestClass]
    public class DomainParticipantFactoryTest
    {
        #region Constants
        private const string TEST_CATEGORY = "DomainParticipantFactory";
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the default values for a new <see cref="DomainParticipantFactoryQos"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNewParticipantFactoryQos()
        {
            DomainParticipantFactoryQos qos = new DomainParticipantFactoryQos();

            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantFactory.GetQos(DomainParticipantFactoryQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            DomainParticipantFactoryQos qos = new DomainParticipantFactoryQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;

            ReturnCode result = AssemblyInitializer.Factory.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);

            // Test with null parameter
            result = AssemblyInitializer.Factory.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantFactory.SetQos(DomainParticipantFactoryQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Creates a non-default QoS, set it an check it
            DomainParticipantFactoryQos qos = new DomainParticipantFactoryQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;

            ReturnCode result = AssemblyInitializer.Factory.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DomainParticipantFactoryQos();
            result = AssemblyInitializer.Factory.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);

            // Put back the default QoS and check it
            qos = new DomainParticipantFactoryQos();
            result = AssemblyInitializer.Factory.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);

            // Test with null parameter
            result = AssemblyInitializer.Factory.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantFactory.GetDefaultDomainParticipantQos(DomainParticipantQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDefaultDomainParticipantQos()
        {
            DomainParticipantQos qos = TestHelper.CreateNonDefaultDomainParticipantQos();

            ReturnCode result = AssemblyInitializer.Factory.GetDefaultDomainParticipantQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test with null parameter
            result = AssemblyInitializer.Factory.GetDefaultDomainParticipantQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantFactory.SetDefaultDomainParticipantQos(DomainParticipantQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDefaultDomainParticipantQos()
        {
            // Creates a non-default QoS, set it an check it
            DomainParticipantQos qos = TestHelper.CreateNonDefaultDomainParticipantQos();
            ReturnCode result = AssemblyInitializer.Factory.SetDefaultDomainParticipantQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DomainParticipantQos();
            result = AssemblyInitializer.Factory.GetDefaultDomainParticipantQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDomainParticipantQos(qos);

            // Put back the default QoS and check it
            qos = new DomainParticipantQos();
            result = AssemblyInitializer.Factory.SetDefaultDomainParticipantQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.GetDefaultDomainParticipantQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test with null parameter
            result = AssemblyInitializer.Factory.SetDefaultDomainParticipantQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantFactory.CreateParticipant(int)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateParticipant()
        {
            // Test simplest overload
            DomainParticipant domainParticipant0 = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(domainParticipant0);
            Assert.AreEqual(AssemblyInitializer.RTPS_DOMAIN, domainParticipant0.DomainId);
            Assert.IsNull(domainParticipant0.Listener);

            DomainParticipantQos qos = new DomainParticipantQos();
            ReturnCode result = domainParticipant0.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test overload with QoS parameter
            qos = TestHelper.CreateNonDefaultDomainParticipantQos();
            DomainParticipant domainParticipant1 = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN, qos);
            Assert.IsNotNull(domainParticipant1);
            Assert.AreEqual(AssemblyInitializer.RTPS_DOMAIN, domainParticipant1.DomainId);
            Assert.IsNull(domainParticipant1.Listener);

            qos = new DomainParticipantQos();
            result = domainParticipant1.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDomainParticipantQos(qos);

            // Test overload with QoS and listener parameters
            MyParticipantListener listener = new MyParticipantListener();
            qos = new DomainParticipantQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.UserData.Value = new List<byte> { 0x42 };
            DomainParticipant domainParticipant2 = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN, qos, listener);
            Assert.IsNotNull(domainParticipant2);
            Assert.AreEqual(AssemblyInitializer.RTPS_DOMAIN, domainParticipant2.DomainId);

            result = domainParticipant2.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDomainParticipantQos(qos);

            // Test full call overload
            qos = new DomainParticipantQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.UserData.Value = new List<byte> { 0x42 };
            DomainParticipant domainParticipant3 = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN, qos, listener, StatusMask.NoStatusMask);
            Assert.IsNotNull(domainParticipant3);
            Assert.AreEqual(AssemblyInitializer.RTPS_DOMAIN, domainParticipant3.DomainId);

            result = domainParticipant3.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDomainParticipantQos(qos);

            // Delete all participants
            result = AssemblyInitializer.Factory.DeleteParticipant(domainParticipant0);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.DeleteParticipant(domainParticipant1);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.DeleteParticipant(domainParticipant2);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.DeleteParticipant(domainParticipant3);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantFactory.LookupParticipant(int)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupParticipant()
        {
            DomainParticipant participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(participant);
            participant.BindRtpsUdpTransportConfig();

            DomainParticipant lookupParticipant = AssemblyInitializer.Factory.LookupParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(lookupParticipant);
            Assert.AreEqual(participant, lookupParticipant);

            DomainParticipant otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            lookupParticipant = AssemblyInitializer.Factory.LookupParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(lookupParticipant);
            Assert.IsTrue(lookupParticipant == participant || lookupParticipant == otherParticipant);

            lookupParticipant = AssemblyInitializer.Factory.LookupParticipant(AssemblyInitializer.RTPS_OTHER_DOMAIN);
            Assert.IsNull(lookupParticipant);

            ReturnCode result = AssemblyInitializer.Factory.DeleteParticipant(participant);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantFactory.DeleteParticipant(DomainParticipant)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteParticipant()
        {
            DomainParticipant participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(participant);
            participant.BindRtpsUdpTransportConfig();

            Subscriber subscriber = participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            ReturnCode result = AssemblyInitializer.Factory.DeleteParticipant(participant);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            result = participant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.DeleteParticipant(participant);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = AssemblyInitializer.Factory.DeleteParticipant(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }
        #endregion
    }
}
