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
using Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.Standard.UnitTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace OpenDDSharp.Standard.UnitTest
{
    [TestClass]
    public class PublisherTest
    {
        #region Constants
        private const string TEST_CATEGORY = "Publisher";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        #endregion

        #region Initialization/Cleanup
        [TestInitialize]
        public void TestInitialize()
        {
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();
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
        public void TestNewPublisherQos()
        {
            PublisherQos qos = new PublisherQos();
            TestHelper.TestDefaultPublisherQos(qos);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a publisher with it
            PublisherQos qos = TestHelper.CreateNonDefaultPublisherQos();

            Publisher publisher = _participant.CreatePublisher(qos);
            Assert.IsNotNull(publisher);

            // Call GetQos and check the values received
            qos = new PublisherQos();
            ReturnCode result = publisher.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultPublisherQos(qos);

            // Test with null parameter
            result = publisher.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Create a new Publisher using the default QoS
            Publisher publisher = _participant.CreatePublisher();

            // Get the qos to ensure that is using the default properties
            PublisherQos qos = new PublisherQos();
            ReturnCode result = publisher.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultPublisherQos(qos);

            // Try to change an immutable property
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            result = publisher.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them
            qos = new PublisherQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            result = publisher.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new PublisherQos();
            result = publisher.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count);
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count);
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);

            // Try to set immutable QoS properties before enable the publisher
            DomainParticipantQos pQos = new DomainParticipantQos();
            pQos.EntityFactory.AutoenableCreatedEntities = false;
            result = _participant.SetQos(pQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            Publisher otherPublisher = _participant.CreatePublisher();
            qos = new PublisherQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            result = otherPublisher.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new PublisherQos();
            result = otherPublisher.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count);
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count);
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);

            // Test with null parameter
            result = publisher.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDefaultDataWriterQos()
        {
            // Initialize entities.
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestGetDefaultDataWriterQos), typeName);
            Assert.IsNotNull(topic);
            // TODO: Uncomment when properties implemented.
            //Assert.IsNull(topic.GetListener());
            //Assert.AreEqual(nameof(TestGetDefaultDataWriterQos), topic.Name);
            //Assert.AreEqual(typeName, topic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            // Create a non-default DataWriter Qos, call GetDefaultDataWriterQos and check the default values.
            DataWriterQos qos = TestHelper.CreateNonDefaultDataWriterQos();
            result = publisher.GetDefaultDataWriterQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Test with null parameter.
            result = publisher.GetDefaultDataWriterQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDefaultDataWriterQos()
        {
            // Initialize entities.
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestSetDefaultDataWriterQos), typeName);
            Assert.IsNotNull(topic);
            // TODO: Uncomment when properties implemented.
            //Assert.IsNull(topic.GetListener());
            //Assert.AreEqual(nameof(TestSetDefaultDataWriterQos), topic.Name);
            //Assert.AreEqual(typeName, topic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            // Creates a non-default QoS, set it an check it
            DataWriterQos qos = TestHelper.CreateNonDefaultDataWriterQos();
            result = publisher.SetDefaultDataWriterQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataWriterQos();
            result = publisher.GetDefaultDataWriterQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);

            qos = new DataWriterQos();
            result = writer.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            // Put back the default QoS and check it.
            qos = new DataWriterQos();
            result = publisher.SetDefaultDataWriterQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = TestHelper.CreateNonDefaultDataWriterQos();
            result = publisher.GetDefaultDataWriterQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            DataWriter otherWriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(otherWriter);

            qos = TestHelper.CreateNonDefaultDataWriterQos();
            result = otherWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Test with null parameter.
            result = publisher.SetDefaultDataWriterQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }
        #endregion
    }
}
