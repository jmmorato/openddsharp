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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="Publisher"/> unit test class.
    /// </summary>
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
        /// <summary>
        /// The test initializer method.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _participant?.DeleteContainedEntities();
            AssemblyInitializer.Factory?.DeleteParticipant(_participant);

            _participant = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="Publisher.Participant"/> property.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetParticipant()
        {
            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);
            Assert.AreSame(_participant, publisher.Participant);
        }

        /// <summary>
        /// Test the <see cref="PublisherQos"/> default constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Included in the calling method.")]
        public void TestNewPublisherQos()
        {
            var qos = new PublisherQos();
            TestHelper.TestDefaultPublisherQos(qos);
        }

        /// <summary>
        /// Test the <see cref="Publisher.GetQos(PublisherQos)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a publisher with it
            var qos = TestHelper.CreateNonDefaultPublisherQos();

            var publisher = _participant.CreatePublisher(qos);
            Assert.IsNotNull(publisher);

            // Call GetQos and check the values received
            qos = new PublisherQos();
            var result = publisher.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultPublisherQos(qos);

            // Test with null parameter
            result = publisher.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Publisher.SetQos(PublisherQos)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Create a new Publisher using the default QoS
            var publisher = _participant.CreatePublisher();

            // Get the qos to ensure that is using the default properties
            var qos = new PublisherQos();
            var result = publisher.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultPublisherQos(qos);

            // Try to change an immutable property
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            result = publisher.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them
            qos = new PublisherQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
                GroupData =
                {
                    Value = new List<byte> { 0x42 },
                },
                Partition =
                {
                    Name = new List<string> { "TestPartition" },
                },
            };
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
            Assert.AreEqual(0x42, qos.GroupData.Value[0]);
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count);
            Assert.AreEqual("TestPartition", qos.Partition.Name[0]);
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);

            // Try to set immutable QoS properties before enable the publisher
            var pQos = new DomainParticipantQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
            };
            result = _participant.SetQos(pQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            var otherPublisher = _participant.CreatePublisher();
            qos = new PublisherQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
                GroupData =
                {
                    Value = new List<byte> { 0x42 },
                },
                Partition =
                {
                    Name = new List<string> { "TestPartition" },
                },
                Presentation =
                {
                    CoherentAccess = true,
                    OrderedAccess = true,
                    AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos,
                },
            };
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
            Assert.AreEqual(0x42, qos.GroupData.Value[0]);
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count);
            Assert.AreEqual("TestPartition", qos.Partition.Name[0]);
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);

            // Test with null parameter
            result = publisher.SetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Publisher.GetListener"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetListener()
        {
            // Create a new Publisher with a listener
            using var listener = new MyPublisherListener();
            var publisher = _participant.CreatePublisher(null, listener);
            Assert.IsNotNull(publisher);

            // Call to GetListener and check the listener received
#pragma warning disable CS0618 // Type or member is obsolete
            var received = (MyPublisherListener)publisher.GetListener();
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);
        }

        /// <summary>
        /// Test the <see cref="Publisher.SetListener(PublisherListener, StatusMask)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetListener()
        {
            // Create a new Publisher without listener
            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var listener = (MyPublisherListener)publisher.Listener;
            Assert.IsNull(listener);

            // Create a listener, set it and check that is correctly set
            listener = new MyPublisherListener();
            var result = publisher.SetListener(listener, StatusMask.AllStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            var received = (MyPublisherListener)publisher.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            // Remove the listener calling SetListener with null and check it
            result = publisher.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            received = (MyPublisherListener)publisher.Listener;
            Assert.IsNull(received);

            listener.Dispose();
        }

        /// <summary>
        /// Test the <see cref="DataWriterQos"/> default constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Included in the calling method.")]
        public void TestNewDataWriterQos()
        {
            var qos = new DataWriterQos();
            TestHelper.TestDefaultDataWriterQos(qos);
        }

        /// <summary>
        /// Test the <see cref="Publisher.CreateDataWriter(Topic, DataWriterQos, DataWriterListener, StatusMask)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestCreateDataWriter()
        {
            // Initialize entities
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestCreateDataWriter), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestCreateDataWriter), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            // Test simplest overload
            var datawriter1 = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(datawriter1);
            Assert.AreEqual(publisher, datawriter1.Publisher);
            Assert.IsNull(datawriter1.Listener);

            var qos = new DataWriterQos();
            result = datawriter1.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Test overload with QoS parameter
            qos = TestHelper.CreateNonDefaultDataWriterQos();
            var datawriter2 = publisher.CreateDataWriter(topic, qos);
            Assert.IsNotNull(datawriter2);
            Assert.AreEqual(publisher, datawriter2.Publisher);
            Assert.IsNull(datawriter2.Listener);

            qos = new DataWriterQos();
            result = datawriter2.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            // Test overload with listener parameter
            var listener = new MyDataWriterListener();
            var datawriter3 = publisher.CreateDataWriter(topic, null, listener);
            Assert.IsNotNull(datawriter3);
            Assert.AreEqual(publisher, datawriter3.Publisher);
            var received = (MyDataWriterListener)datawriter3.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataWriterQos();
            result = datawriter3.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            datawriter3.SetListener(null);
            listener.Dispose();

            // Test overload with listener and StatusMask parameters
            listener = new MyDataWriterListener();
            var datawriter4 = publisher.CreateDataWriter(topic, null, listener, StatusMask.AllStatusMask);
            Assert.IsNotNull(datawriter4);
            Assert.AreEqual(publisher, datawriter4.Publisher);
            received = (MyDataWriterListener)datawriter4.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataWriterQos();
            result = datawriter4.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            datawriter4.SetListener(null);
            listener.Dispose();

            // Test overload with QoS and listener parameters
            qos = TestHelper.CreateNonDefaultDataWriterQos();
            listener = new MyDataWriterListener();
            var datawriter5 = publisher.CreateDataWriter(topic, qos, listener);
            Assert.IsNotNull(datawriter5);
            Assert.AreEqual(publisher, datawriter5.Publisher);
            received = (MyDataWriterListener)datawriter5.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataWriterQos();
            result = datawriter5.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            datawriter5.SetListener(null);
            listener.Dispose();

            // Test full call overload
            qos = TestHelper.CreateNonDefaultDataWriterQos();
            listener = new MyDataWriterListener();
            var datawriter6 = publisher.CreateDataWriter(topic, qos, listener, StatusMask.AllStatusMask);
            Assert.IsNotNull(datawriter6);
            Assert.AreEqual(publisher, datawriter6.Publisher);
            received = (MyDataWriterListener)datawriter6.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataWriterQos();
            result = datawriter6.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            datawriter6.SetListener(null);
            listener.Dispose();

            // Test with null parameter
            Assert.ThrowsException<ArgumentNullException>(() => publisher.CreateDataWriter(null));

            // Test with wrong qos
            var dwQos = new DataWriterQos
            {
                ResourceLimits =
                {
                    MaxSamples = 1,
                    MaxSamplesPerInstance = 2,
                },
            };
            var nullDataWriter = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNull(nullDataWriter);

            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteDataWriter(datawriter1));
            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteDataWriter(datawriter2));
            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteDataWriter(datawriter3));
            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteDataWriter(datawriter4));
            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteDataWriter(datawriter5));
            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteDataWriter(datawriter6));
            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _participant.DeletePublisher(publisher));
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteTopic(topic));
        }

        /// <summary>
        /// Test the <see cref="Publisher.DeleteDataWriter(DataWriter)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteDataWriter()
        {
            // Initialize entities
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestDeleteDataWriter), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestDeleteDataWriter), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var otherPublisher = _participant.CreatePublisher();
            Assert.IsNotNull(otherPublisher);

            // Create a DataWriter and try to delete it with another publisher
            var datawriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(datawriter);
            Assert.AreEqual(publisher, datawriter.Publisher);
            Assert.IsNull(datawriter.Listener);

            var qos = new DataWriterQos();
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

        /// <summary>
        /// Test the <see cref="Publisher.LookupDataWriter(string)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupDataWriter()
        {
            // Initialize entities
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestLookupDataWriter), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestLookupDataWriter), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var otherPublisher = _participant.CreatePublisher();
            Assert.IsNotNull(otherPublisher);

            // Create a DataWriter and lookup in the publishers
            var datawriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(datawriter);
            Assert.AreEqual(publisher, datawriter.Publisher);
            Assert.IsNull(datawriter.Listener);

            var received = publisher.LookupDataWriter(nameof(TestLookupDataWriter));
            Assert.IsNotNull(received);
            Assert.AreEqual(datawriter, received);

            received = otherPublisher.LookupDataWriter(nameof(TestLookupDataWriter));
            Assert.IsNull(received);

            // Create other DataWriter in the same topic and lookup again
            var otherDatawriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(otherDatawriter);
            Assert.AreEqual(publisher, otherDatawriter.Publisher);
            Assert.IsNull(otherDatawriter.Listener);

            received = publisher.LookupDataWriter(nameof(TestLookupDataWriter));
            Assert.IsNotNull(received);
            Assert.IsTrue(datawriter == received || otherDatawriter == received);

            received = otherPublisher.LookupDataWriter(nameof(TestLookupDataWriter));
            Assert.IsNull(received);
        }

        /// <summary>
        /// Test the <see cref="Publisher.DeleteContainedEntities"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDeleteContainedEntities()
        {
            // Initialize entities
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestDeleteContainedEntities), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestDeleteContainedEntities), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            // Call DeleteContainedEntities in an empty publisher
            result = publisher.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a DataWriter in the publisher
            var dataWriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(dataWriter);

            // Try to delete the publisher without delete the datawriter
            result = _participant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Call DeleteContainedEntities and remove the publisher again
            result = publisher.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="Publisher.GetDefaultDataWriterQos(DataWriterQos)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetDefaultDataWriterQos()
        {
            // Initialize entities.
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestGetDefaultDataWriterQos), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestGetDefaultDataWriterQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            // Create a non-default DataWriter Qos, call GetDefaultDataWriterQos and check the default values.
            var qos = TestHelper.CreateNonDefaultDataWriterQos();
            result = publisher.GetDefaultDataWriterQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Test with null parameter.
            result = publisher.GetDefaultDataWriterQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Publisher.SetDefaultDataWriterQos(DataWriterQos)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetDefaultDataWriterQos()
        {
            // Initialize entities.
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestSetDefaultDataWriterQos), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestSetDefaultDataWriterQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            // Creates a non-default QoS, set it an check it
            var qos = TestHelper.CreateNonDefaultDataWriterQos();
            result = publisher.SetDefaultDataWriterQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataWriterQos();
            result = publisher.GetDefaultDataWriterQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            var writer = publisher.CreateDataWriter(topic);
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

            var otherWriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(otherWriter);

            qos = TestHelper.CreateNonDefaultDataWriterQos();
            result = otherWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Test with null parameter.
            result = publisher.SetDefaultDataWriterQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        /// <summary>
        /// Test the <see cref="Publisher.WaitForAcknowledgments(Duration)"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestWaitForAcknowledgments()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestWaitForAcknowledgments), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestWaitForAcknowledgments), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            // Call WaitForAcknowledgments without DataReaders
            result = dataWriter.Write(new TestStruct
            {
                Id = 1,
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = publisher.WaitForAcknowledgments(new Duration
            {
                Seconds = 5,
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a Reliable DataReader, write a new sample and Call WaitForAcknowledgments
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            result = dataWriter.Write(new TestStruct
            {
                Id = 2,
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = publisher.WaitForAcknowledgments(new Duration
            {
                Seconds = 5,
            });
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(5_000));
        }

        /// <summary>
        /// Test the <see cref="Publisher.SuspendPublications"/> and the <see cref="Publisher.ResumePublications"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSuspendResumePublications()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = _participant.CreateTopic(nameof(TestSuspendResumePublications), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestSuspendResumePublications), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            var publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);
            var dataReader = new TestStructDataReader(reader);

            var statusCondition = dataReader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            Assert.IsTrue(reader.WaitForPublications(1, 5_000));
            Assert.IsTrue(writer.WaitForSubscriptions(1, 5_000));

            // Call ResumePublications without calling first SuspendPublications
            result = publisher.ResumePublications();
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Suspend publications and write samples
            result = publisher.SuspendPublications();
            Assert.AreEqual(ReturnCode.Ok, result);

            for (var i = 1; i <= 5; i++)
            {
                // OpenDDS issue: cannot register more than one instance during SuspendPublications.
                // Looks like that the control messages are never delivered and the controlTracker never get free during delete_datawriter
                var sample = new TestStruct
                {
                    Id = 1,
                    ShortField = (short)i,
                };

                var handle = dataWriter.RegisterInstance(sample);
                Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

                result = dataWriter.Write(sample, handle);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Assert.IsFalse(evt.Wait(1_500));

            // Check that not samples arrived
            var data = new List<TestStruct>();
            var sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.NoData, result);

            // Resume publication and check the samples
            result = publisher.ResumePublications();
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(5_000));

            data = new List<TestStruct>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsTrue(data.Count > 0);
            Assert.AreEqual(data.Count, sampleInfos.Count);

            for (var i = 0; i < data.Count; i++)
            {
                Assert.IsTrue(sampleInfos[i].ValidData);
                Assert.AreEqual(i + 1, data[i].ShortField);
            }
        }

        /// <summary>
        /// Test the <see cref="Publisher.BeginCoherentChanges"/> and the <see cref="Publisher.EndCoherentChanges"/> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        [Ignore("OpenDDS Issue: Coherent sets for PRESENTATION QoS not currently implemented")]
        public void TestBeginEndCoherentChanges()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(participant);
            participant.BindTcpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var topic = participant.CreateTopic(nameof(TestBeginEndCoherentChanges), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.Listener);
            Assert.AreEqual(nameof(TestBeginEndCoherentChanges), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            var pQos = new PublisherQos
            {
                Presentation =
                {
                    CoherentAccess = true,
                    OrderedAccess = true,
                    AccessScope = PresentationQosPolicyAccessScopeKind.TopicPresentationQos,
                },
            };
            var publisher = participant.CreatePublisher(pQos);
            Assert.IsNotNull(publisher);

            var sQos = new SubscriberQos
            {
                Presentation =
                {
                    CoherentAccess = true,
                    OrderedAccess = true,
                    AccessScope = PresentationQosPolicyAccessScopeKind.TopicPresentationQos,
                },
            };
            var subscriber = participant.CreateSubscriber(sQos);
            Assert.IsNotNull(subscriber);

            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var writer = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
            };
            var reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);
            var dataReader = new TestStructDataReader(reader);

            var statusCondition = subscriber.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataOnReadersStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            Assert.IsTrue(reader.WaitForPublications(1, 5_000));
            Assert.IsTrue(writer.WaitForSubscriptions(1, 5_000));

            // Call EndCoherentChanges without calling first SuspendPublications
            result = publisher.EndCoherentChanges();
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Begin coherent access and write samples
            result = publisher.BeginCoherentChanges();
            Assert.AreEqual(ReturnCode.Ok, result);

            for (var i = 1; i <= 5; i++)
            {
                var sample = new TestStruct
                {
                    Id = i,
                    ShortField = (short)i,
                };

                var handle = dataWriter.RegisterInstance(sample);
                Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

                result = dataWriter.Write(sample, handle);
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Assert.IsFalse(evt.Wait(1_500));

            // Check that not samples arrived
            var data = new List<TestStruct>();
            var sampleInfos = new List<SampleInfo>();

            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.NoData, result);

            // End coherent access and check the samples
            result = publisher.EndCoherentChanges();
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));

            data = new List<TestStruct>();
            sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(data, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsTrue(data.Count > 0);
            Assert.AreEqual(data.Count, sampleInfos.Count);

            for (var i = 0; i < data.Count; i++)
            {
                Assert.IsTrue(sampleInfos[i].ValidData);
                Assert.AreEqual(i + 1, data[i].Id);
                Assert.AreEqual(i + 1, data[i].ShortField);
            }

            Assert.AreEqual(ReturnCode.Ok, reader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(reader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteDataWriter(writer));
            Assert.AreEqual(ReturnCode.Ok, publisher.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, participant.DeleteTopic(topic));
            Assert.AreEqual(ReturnCode.Ok, participant.DeletePublisher(publisher));
            Assert.AreEqual(ReturnCode.Ok, participant.DeleteSubscriber(subscriber));
            Assert.AreEqual(ReturnCode.Ok, participant.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, AssemblyInitializer.Factory.DeleteParticipant(participant));
        }
        #endregion
    }
}
