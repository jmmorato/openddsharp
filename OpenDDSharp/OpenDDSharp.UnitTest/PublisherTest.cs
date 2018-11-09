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
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class PublisherTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;        
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
        private DomainParticipant _participant;
        #endregion

        #region Initialization/Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _dpf = ParticipantService.Instance.GetDomainParticipantFactory();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _participant = _dpf.CreateParticipant(DOMAIN_ID);
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

            if (_dpf != null)
            {
                ReturnCode result = _dpf.DeleteParticipant(_participant);
                Assert.AreEqual(ReturnCode.Ok, result);
            }
        }
        #endregion

        #region Test Methods
        [TestMethod]
        [TestCategory("Publisher")]
        public void TestGetParticipant()
        {
            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);
            Assert.AreEqual(_participant, publisher.Participant);            
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a publisher with it
            PublisherQos qos = TestHelper.CreateNonDefaultPublisherQos();

            Publisher publisher = _participant.CreatePublisher(qos);
            Assert.IsNotNull(publisher);

            // Call GetQos and check the values received
            qos = new PublisherQos();
            ReturnCode result = publisher.GetQos(qos);
            TestHelper.TestNonDefaultPublisherQos(qos);

            // Test with null parameter
            result = publisher.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("Publisher")]
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
            Assert.AreEqual(1, qos.GroupData.Value.Count());
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count());
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
            Assert.AreEqual(1, qos.GroupData.Value.Count());
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count());
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);

            // Test with null parameter
            result = publisher.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestGetListener()
        {
            // Create a new Publisher with a listener
            MyPublisherListener listener = new MyPublisherListener();
            Publisher publisher = _participant.CreatePublisher(listener);
            Assert.IsNotNull(publisher);

            // Call to GetListener and check the listener received
            MyPublisherListener received = (MyPublisherListener)publisher.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestSetListener()
        {
            // Create a new Publisher without listener
            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            MyPublisherListener listener = (MyPublisherListener)publisher.GetListener();
            Assert.IsNull(listener);

            // Create a listener, set it and check that is correctly setted
            listener = new MyPublisherListener();
            ReturnCode result = publisher.SetListener(listener, StatusMask.AllStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            MyPublisherListener received = (MyPublisherListener)publisher.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            // Remove the listener calling SetListener with null and check it
            result = publisher.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            received = (MyPublisherListener)publisher.GetListener();
            Assert.IsNull(received);
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestNewDataWriterQos()
        {
            DataWriterQos qos = new DataWriterQos();
            TestHelper.TestDefaultDataWriterQos(qos);            
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestCreateDataWriter()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);
            
            Topic topic = _participant.CreateTopic(nameof(TestCreateDataWriter), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestCreateDataWriter), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            // Test simplest overload
            DataWriter datawriter1 = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(datawriter1);
            Assert.AreEqual(publisher, datawriter1.Publisher);
            Assert.IsNull(datawriter1.GetListener());

            DataWriterQos qos = new DataWriterQos();
            result = datawriter1.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Test overload with QoS parameter
            qos = TestHelper.CreateNonDefaultDataWriterQos();
            DataWriter datawriter2 = publisher.CreateDataWriter(topic, qos);
            Assert.IsNotNull(datawriter2);
            Assert.AreEqual(publisher, datawriter2.Publisher);
            Assert.IsNull(datawriter2.GetListener());

            qos = new DataWriterQos();
            result = datawriter2.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            // Test overload with listener parameter
            MyDataWriterListener listener = new MyDataWriterListener();
            DataWriter datawriter3 = publisher.CreateDataWriter(topic, listener);
            Assert.IsNotNull(datawriter3);
            Assert.AreEqual(publisher, datawriter3.Publisher);
            MyDataWriterListener received = (MyDataWriterListener)datawriter3.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataWriterQos();
            result = datawriter3.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Test overload with listener and StatusMask parameters
            listener = new MyDataWriterListener();
            DataWriter datawriter4 = publisher.CreateDataWriter(topic, listener, StatusMask.AllStatusMask);
            Assert.IsNotNull(datawriter4);
            Assert.AreEqual(publisher, datawriter4.Publisher);
            received = (MyDataWriterListener)datawriter4.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataWriterQos();
            result = datawriter4.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Test overload with QoS and listener parameters
            qos = TestHelper.CreateNonDefaultDataWriterQos();
            listener = new MyDataWriterListener();
            DataWriter datawriter5 = publisher.CreateDataWriter(topic, qos, listener);
            Assert.IsNotNull(datawriter5);
            Assert.AreEqual(publisher, datawriter5.Publisher);
            received = (MyDataWriterListener)datawriter5.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataWriterQos();
            result = datawriter5.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            // Test full call overload
            qos = TestHelper.CreateNonDefaultDataWriterQos();
            listener = new MyDataWriterListener();
            DataWriter datawriter6 = publisher.CreateDataWriter(topic, qos, listener, StatusMask.AllStatusMask);
            Assert.IsNotNull(datawriter6);
            Assert.AreEqual(publisher, datawriter6.Publisher);
            received = (MyDataWriterListener)datawriter6.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataWriterQos();
            result = datawriter6.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            // Test with null parameter
            DataWriter nullDataWriter = publisher.CreateDataWriter(null);
            Assert.IsNull(nullDataWriter);

            // Test with wrong qos
            DataWriterQos dwQos = new DataWriterQos();
            dwQos.ResourceLimits.MaxSamples = 1;
            dwQos.ResourceLimits.MaxSamplesPerInstance = 2;
            nullDataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNull(nullDataWriter);
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestDeleteDataWriter()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteDataWriter), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestDeleteDataWriter), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            Publisher otherPublisher = _participant.CreatePublisher();
            Assert.IsNotNull(otherPublisher);

            // Create a DataWriter and try to delete it with another publisher
            DataWriter datawriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(datawriter);
            Assert.AreEqual(publisher, datawriter.Publisher);
            Assert.IsNull(datawriter.GetListener());

            DataWriterQos qos = new DataWriterQos();
            result = datawriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            result = otherPublisher.DeleteDataWriter(datawriter);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Delete the datawriter with the correct publisher
            result = publisher.DeleteDataWriter(datawriter);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Try to remove it again
            result = publisher.DeleteDataWriter(datawriter);
            Assert.AreEqual(ReturnCode.Error, result);

            // Test with null parameter
            result = publisher.DeleteDataWriter(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestLookupDataWriter()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestLookupDataWriter), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestLookupDataWriter), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            Publisher otherPublisher = _participant.CreatePublisher();
            Assert.IsNotNull(otherPublisher);

            // Create a DataWriter and lookup in the publishers
            DataWriter datawriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(datawriter);
            Assert.AreEqual(publisher, datawriter.Publisher);
            Assert.IsNull(datawriter.GetListener());

            DataWriter received = publisher.LookupDataWriter(nameof(TestLookupDataWriter));
            Assert.IsNotNull(received);
            Assert.AreEqual(datawriter, received);

            received = otherPublisher.LookupDataWriter(nameof(TestLookupDataWriter));
            Assert.IsNull(received);

            // Create other DataWriter in the same topic and lookup again
            DataWriter otherDatawriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(otherDatawriter);
            Assert.AreEqual(publisher, otherDatawriter.Publisher);
            Assert.IsNull(otherDatawriter.GetListener());

            received = publisher.LookupDataWriter(nameof(TestLookupDataWriter));
            Assert.IsNotNull(received);
            Assert.IsTrue(datawriter == received || otherDatawriter == received);

            received = otherPublisher.LookupDataWriter(nameof(TestLookupDataWriter));
            Assert.IsNull(received);            
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestDeleteContainedEntities()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteContainedEntities), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestDeleteContainedEntities), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            // Call DeleteContainedEntities in an empty publisher
            result = publisher.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a DataWriter in the publisher
            DataWriter dataWriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(publisher);

            // Try to delete the publisher without delete the datawriter
            result = _participant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Call DeleteContainedEntities and remove the publisher again
            result = publisher.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestGetDefaultDataWriterQos()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestGetDefaultDataWriterQos), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestGetDefaultDataWriterQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            // Create a non-default DataWriter Qos, call GetDefaultDataWriterQos and check the default values 
            DataWriterQos qos = TestHelper.CreateNonDefaultDataWriterQos();
            result = publisher.GetDefaultDataWriterQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Test with null parameter
            result = publisher.GetDefaultDataWriterQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestSetDefaultDataWriterQos()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestSetDefaultDataWriterQos), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestSetDefaultDataWriterQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

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

            // Put back the default QoS and check it
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

            // Test with null parameter
            result = publisher.SetDefaultDataWriterQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestWaitForAcknowledgments()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestWaitForAcknowledgments), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestWaitForAcknowledgments), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Call WaitForAcknowledgments without DataReaders
            result = dataWriter.Write(new TestStruct
            {
                Id = 1
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = publisher.WaitForAcknowledgments(new Duration
            {
                Seconds = 5
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a Reliable DataReader, write a new sample and Call WaitForAcknowledgments
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);

            System.Threading.Thread.Sleep(500);

            result = dataWriter.Write(new TestStruct
            {
                Id = 2
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = publisher.WaitForAcknowledgments(new Duration
            {
                Seconds = 5
            });
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestSuspendResumePublications()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestSuspendResumePublications), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestSuspendResumePublications), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);
            
            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            DataReader reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);

            TestStructDataReader dataReader = new TestStructDataReader(reader);

            // Call ResumePublications without calling first SuspendPublications
            result = publisher.ResumePublications();
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Suspend publications and write samples
            result = publisher.SuspendPublications();
            Assert.AreEqual(ReturnCode.Ok, result);

            for (int i = 1; i <= 5; i++)
            {
                // OpenDDS issue: cannot register more than one instance during SuspendPublications. 
                // Looks like that the control messages are never delivered and the controlTracker never get free during delete_datawriter
                TestStruct sample = new TestStruct
                {
                    Id = 1,
                    ShortType = (short)i
                };

                InstanceHandle handle = dataWriter.RegisterInstance(sample);
                Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

                result = dataWriter.Write(sample, handle);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(500);

            // Check that not samples arrived
            List<TestStruct> data = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.NoData, result);

            // Resume publication and check the samples
            result = publisher.ResumePublications();
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(500);

            data = new List<TestStruct>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);

            for (int i = 0; i < data.Count; i++)
            {
                Assert.IsTrue(sampleInfos[i].ValidData);
                Assert.AreEqual(i + 1, data[i].ShortType);
            }
        }

        [TestMethod]
        [TestCategory("Publisher")]
        public void TestBeginEndCoherentChanges()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestBeginEndCoherentChanges), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestBeginEndCoherentChanges), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            PublisherQos pQos = new PublisherQos();
            pQos.Presentation.CoherentAccess = true;
            pQos.Presentation.OrderedAccess = true;
            pQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.TopicPresentationQos;
            Publisher publisher = _participant.CreatePublisher(pQos);
            Assert.IsNotNull(publisher);

            SubscriberQos sQos = new SubscriberQos();
            sQos.Presentation.CoherentAccess = true;
            sQos.Presentation.OrderedAccess = true;
            sQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.TopicPresentationQos;
            Subscriber subscriber = _participant.CreateSubscriber(sQos);
            Assert.IsNotNull(subscriber);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            DataReader reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);

            TestStructDataReader dataReader = new TestStructDataReader(reader);

            // Call EndCoherentChanges without calling first SuspendPublications
            result = publisher.EndCoherentChanges();
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Begin coherent access and write samples 
            result = publisher.BeginCoherentChanges();
            Assert.AreEqual(ReturnCode.Ok, result);

            for (int i = 1; i <= 5; i++)
            {                
                TestStruct sample = new TestStruct
                {
                    Id = i,
                    ShortType = (short)i
                };

                InstanceHandle handle = dataWriter.RegisterInstance(sample);
                Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

                result = dataWriter.Write(sample, handle);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(500);

            // Check that not samples arrived
            List<TestStruct> data = new List<TestStruct>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();

            #region OpenDDS Issue
            // Coherent sets for PRESENTATION QoS not Currently implemented on RTPS
            //result = dataReader.Read(data, sampleInfos);
            //Assert.AreEqual(ReturnCode.NoData, result);
            #endregion

            // End coherent access and check the samples
            result = publisher.EndCoherentChanges();
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(500);

            data = new List<TestStruct>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);

            for (int i = 0; i < data.Count; i++)
            {
                Assert.IsTrue(sampleInfos[i].ValidData);
                Assert.AreEqual(i + 1, data[i].Id);
                Assert.AreEqual(i + 1, data[i].ShortType);
            }
        }
        #endregion        
    }
}
