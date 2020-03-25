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
using System.Collections.Generic;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.Standard.UnitTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;
using System.Diagnostics.CodeAnalysis;

namespace OpenDDSharp.Standard.UnitTest
{
    [TestClass]
    public class DataWriterTest
    {
        #region Constants
        private const string TEST_CATEGORY = "DataWriter";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Topic _topic;
        private Publisher _publisher;
        #endregion

        #region Properties
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization/Cleanup
        [TestInitialize]
        public void TestInitialize()
        {
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);
            // TODO: Uncomment when implemented.
            //Assert.IsNull(_topic.GetListener());
            //Assert.AreEqual(TestContext.TestName, _topic.Name);
            //Assert.AreEqual(typeName, _topic.TypeName);

            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_participant != null)
            {
                ReturnCode result = _participant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            if (AssemblyInitializer.Factory != null)
            {
                ReturnCode result = AssemblyInitializer.Factory.DeleteParticipant(_participant);
                Assert.AreEqual(ReturnCode.Ok, result);
            }
        }
        #endregion

        #region Test Methods
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Included in the calling method.")]
        public void TestNewDataWriterQos()
        {
            DataWriterQos qos = new DataWriterQos();
            TestHelper.TestDefaultDataWriterQos(qos);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a datawriter with it
            DataWriterQos qos = TestHelper.CreateNonDefaultDataWriterQos();

            DataWriter dataWriter = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(dataWriter);

            // Call GetQos and check the values received
            qos = new DataWriterQos();
            ReturnCode result = dataWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            // Test GetQos with null parameter
            result = dataWriter.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Create a new DataWriter using the default QoS
            DataWriter dataWriter = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(dataWriter);

            // Get the qos to ensure that is using the default properties
            DataWriterQos qos = new DataWriterQos();
            ReturnCode result = dataWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Try to change an immutable property
            qos.Ownership.Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos;
            result = dataWriter.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them
            qos = new DataWriterQos();
            qos.OwnershipStrength.Value = 100;

            result = dataWriter.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataWriterQos();
            result = dataWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(100, qos.OwnershipStrength.Value);

            // Try to set immutable QoS properties before enable the datawriter
            PublisherQos pubQos = new PublisherQos();
            pubQos.EntityFactory.AutoenableCreatedEntities = false;
            result = _publisher.SetQos(pubQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            DataWriter otherDataWriter = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(otherDataWriter);

            qos = new DataWriterQos();
            qos.Ownership.Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos;
            result = otherDataWriter.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataWriterQos();
            result = otherDataWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, qos.Ownership.Kind);

            result = otherDataWriter.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Set back the default publisher QoS
            pubQos = new PublisherQos();
            _publisher.SetQos(pubQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Test SetQos with null parameter
            result = dataWriter.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }
        #endregion
    }
}
