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
using System.Linq;
using System.Collections.Generic;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class DomainParticipantFactoryTest
    {
        #region Fields
        static DomainParticipantFactory _dpf;
        #endregion

        #region Initialization/Cleanup
        [TestInitialize]
        public void TestInitialize()
        {
            _dpf = ParticipantService.Instance.GetDomainParticipantFactory();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dpf = null;
            System.GC.Collect();
        }
        #endregion

        #region Test Methods
        [TestMethod]
        [TestCategory("DomainParticipantFactory")]
        public void TestNewParticipantFactoryQos()
        {
            DomainParticipantFactoryQos qos = new DomainParticipantFactoryQos();

            Assert.IsNotNull(qos.EntityFactory);            
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);            
        }

        [TestMethod]
        [TestCategory("DomainParticipantFactory")]
        public void TestGetQos()
        {
            DomainParticipantFactoryQos qos = new DomainParticipantFactoryQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;            

            ReturnCode result = _dpf.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);           
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);

            // Test with null parameter
            result = _dpf.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("DomainParticipantFactory")]
        public void TestSetQos()
        {
            // Creates a non-default QoS, set it an check it
            DomainParticipantFactoryQos qos = new DomainParticipantFactoryQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;            

            ReturnCode result = _dpf.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DomainParticipantFactoryQos();
            result = _dpf.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);            
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);

            // Put back the default QoS and check it
            qos = new DomainParticipantFactoryQos();
            result = _dpf.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);            
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);

            // Test with null parameter
            result = _dpf.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("DomainParticipantFactory")]
        public void TestGetDefaultDomainParticipantQos()
        {
            DomainParticipantQos qos = TestHelper.CreateNonDefaultDomainParticipantQos();

            ReturnCode result = _dpf.GetDefaultDomainParticipantQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test with null parameter
            result = _dpf.GetDefaultDomainParticipantQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("DomainParticipantFactory")]
        public void TestSetDefaultDomainParticipantQos()
        {
            // Creates a non-default QoS, set it an check it
            DomainParticipantQos qos = TestHelper.CreateNonDefaultDomainParticipantQos();
            ReturnCode result = _dpf.SetDefaultDomainParticipantQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DomainParticipantQos();
            result = _dpf.GetDefaultDomainParticipantQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDomainParticipantQos(qos);            

            // Put back the default QoS and check it
            qos = new DomainParticipantQos();
            result = _dpf.SetDefaultDomainParticipantQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.GetDefaultDomainParticipantQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test with null parameter
            result = _dpf.SetDefaultDomainParticipantQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("DomainParticipantFactory")]
        public void TestCreateParticipant()
        {
            // Test simplest overload
            DomainParticipant domainParticipant0 = _dpf.CreateParticipant(0);
            Assert.IsNotNull(domainParticipant0);
            Assert.AreEqual(0, domainParticipant0.DomainId);
            Assert.IsNull(domainParticipant0.GetListener());

            DomainParticipantQos qos = new DomainParticipantQos();
            ReturnCode result = domainParticipant0.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test overload with QoS parameter
            qos = TestHelper.CreateNonDefaultDomainParticipantQos();
            DomainParticipant domainParticipant1 = _dpf.CreateParticipant(1, qos);
            Assert.IsNotNull(domainParticipant1);
            Assert.AreEqual(1, domainParticipant1.DomainId);
            Assert.IsNull(domainParticipant1.GetListener());

            qos = new DomainParticipantQos();
            result = domainParticipant1.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDomainParticipantQos(qos);

            // Test overload with listener parameter
            MyParticipantListener listener = new MyParticipantListener();
            DomainParticipant domainParticipant2 = _dpf.CreateParticipant(2, listener);
            Assert.IsNotNull(domainParticipant2);
            Assert.AreEqual(2, domainParticipant2.DomainId);
            Assert.IsNotNull(domainParticipant2.GetListener());
            
            result = domainParticipant2.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test overload with listener and StatusMask parameters
            DomainParticipant domainParticipant3 = _dpf.CreateParticipant(3, listener, StatusMask.NoStatusMask);
            Assert.IsNotNull(domainParticipant3);
            Assert.AreEqual(3, domainParticipant3.DomainId);
            Assert.IsNotNull(domainParticipant3.GetListener());

            result = domainParticipant2.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDomainParticipantQos(qos);

            // Test overload with QoS and listener parameters
            qos = new DomainParticipantQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.UserData.Value = new List<byte> { 0x42 };
            DomainParticipant domainParticipant4 = _dpf.CreateParticipant(4, qos, listener);
            Assert.IsNotNull(domainParticipant4);
            Assert.AreEqual(4, domainParticipant4.DomainId);

            result = domainParticipant4.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDomainParticipantQos(qos);

            // Test full call overload
            qos = new DomainParticipantQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.UserData.Value = new List<byte> { 0x42 };
            DomainParticipant domainParticipant5 = _dpf.CreateParticipant(5, qos, listener, StatusMask.NoStatusMask);
            Assert.IsNotNull(domainParticipant5);
            Assert.AreEqual(5, domainParticipant5.DomainId);

            result = domainParticipant5.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDomainParticipantQos(qos);

            // Delete all participants
            result = _dpf.DeleteParticipant(domainParticipant0);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.DeleteParticipant(domainParticipant1);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.DeleteParticipant(domainParticipant2);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.DeleteParticipant(domainParticipant3);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.DeleteParticipant(domainParticipant4);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.DeleteParticipant(domainParticipant5);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DomainParticipantFactory")]
        public void TestLookupParticipant()
        {
            DomainParticipant participant = _dpf.CreateParticipant(42);
            Assert.IsNotNull(participant);

            DomainParticipant lookupParticipant = _dpf.LookupParticipant(42);
            Assert.IsNotNull(participant);
            Assert.AreEqual(participant, lookupParticipant);

            DomainParticipant otherParticipant = _dpf.CreateParticipant(42);
            Assert.IsNotNull(otherParticipant);

            lookupParticipant = _dpf.LookupParticipant(42);
            Assert.IsNotNull(lookupParticipant);
            Assert.IsTrue(lookupParticipant == participant || lookupParticipant == otherParticipant);

            lookupParticipant = _dpf.LookupParticipant(23);
            Assert.IsNull(lookupParticipant);

            ReturnCode result = _dpf.DeleteParticipant(participant);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DomainParticipantFactory")]
        public void TestDeleteParticipant()
        {
            DomainParticipant participant = _dpf.CreateParticipant(42);
            Assert.IsNotNull(participant);

            Subscriber subscriber = participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            ReturnCode result = _dpf.DeleteParticipant(participant);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            result = participant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.DeleteParticipant(participant);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.DeleteParticipant(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }
        #endregion
    }
}
