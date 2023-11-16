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
using System.Linq;
using System.Threading;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="DataWriter"/> unit test class.
    /// </summary>
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
        /// <summary>
        /// Gets or sets the <see cref="TestContext"/> property.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by MSTest")]
        public TestContext TestContext { get; set; }
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

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);
            Assert.IsNull(_topic.Listener);
            Assert.AreEqual(TestContext.TestName, _topic.Name);
            Assert.AreEqual(typeName, _topic.TypeName);

            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _publisher?.DeleteContainedEntities();
            _participant?.DeletePublisher(_publisher);
            _participant?.DeleteTopic(_topic);
            _participant?.DeleteContainedEntities();

            AssemblyInitializer.Factory?.DeleteParticipant(_participant);

            _participant = null;
            _publisher = null;
            _topic = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="DataWriter" /> properties.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestProperties()
        {
            // Create a DataWriter and check the Topic and Participant properties
            var writer = _publisher.CreateDataWriter(_topic);

            Assert.IsNotNull(writer);
            Assert.IsNotNull(writer.Topic);
            Assert.IsNotNull(writer.Publisher);
            Assert.AreSame(_topic, writer.Topic);
            Assert.AreSame(_publisher, writer.Publisher);
            Assert.IsNull(writer.Listener);

            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="DataWriterQos" /> constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNewDataWriterQos()
        {
            var qos = new DataWriterQos();
            TestHelper.TestDefaultDataWriterQos(qos);
        }

        /// <summary>
        /// Test the <see cref="DataWriter.GetQos(DataWriterQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a DataWriter with it
            var qos = TestHelper.CreateNonDefaultDataWriterQos();

            var dataWriter = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(dataWriter);

            // Call GetQos and check the values received
            qos = new DataWriterQos();
            var result = dataWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            // Test GetQos with null parameter
            result = dataWriter.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            _publisher.DeleteDataWriter(dataWriter);
        }

        /// <summary>
        /// Test the <see cref="DataWriter.SetQos(DataWriterQos)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetQos()
        {
            // Create a new DataWriter using the default QoS
            var dataWriter = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(dataWriter);

            // Get the qos to ensure that is using the default properties
            var qos = new DataWriterQos();
            var result = dataWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Try to change an immutable property
            qos.Ownership.Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos;
            result = dataWriter.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them
            qos = new DataWriterQos
            {
                OwnershipStrength =
                {
                    Value = 100,
                },
            };

            result = dataWriter.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataWriterQos();
            result = dataWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(100, qos.OwnershipStrength.Value);

            // Try to set immutable QoS properties before enable the DataWriter
            var pubQos = new PublisherQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
            };
            result = _publisher.SetQos(pubQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            var otherDataWriter = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(otherDataWriter);

            qos = new DataWriterQos
            {
                Ownership =
                {
                    Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos,
                },
            };
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

            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(dataWriter));
            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(otherDataWriter));
        }

        /// <summary>
        /// Test the <see cref="DataWriter.GetListener" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetListener()
        {
            // Create a new DataWriter with a listener
            var listener = new MyDataWriterListener();
            var dataWriter = _publisher.CreateDataWriter(_topic, null, listener);
            Assert.IsNotNull(dataWriter);

            // Call to GetListener and check the listener received
#pragma warning disable CS0618 // Type or member is obsolete
            var received = (MyDataWriterListener)dataWriter.GetListener();
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.IsNotNull(received);
            Assert.AreSame(listener, received);

            Assert.AreEqual(ReturnCode.Ok, dataWriter.SetListener(null));
            listener.Dispose();

            _publisher.DeleteDataWriter(dataWriter);
        }

        /// <summary>
        /// Test the <see cref="DataWriter.SetListener(DataWriterListener, StatusMask)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestSetListener()
        {
            // Create a new DataWriter without listener
            var dataWriter = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(dataWriter);

            var listener = (MyDataWriterListener)dataWriter.Listener;
            Assert.IsNull(listener);

            // Create a listener, set it and check that is correctly set
            listener = new MyDataWriterListener();
            var result = dataWriter.SetListener(listener, StatusMask.AllStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            var received = (MyDataWriterListener)dataWriter.Listener;
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            // Remove the listener calling SetListener with null and check it
            result = dataWriter.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            listener.Dispose();

            received = (MyDataWriterListener)dataWriter.Listener;
            Assert.IsNull(received);

            _publisher.DeleteDataWriter(dataWriter);
        }

        /// <summary>
        /// Test the <see cref="DataWriter.WaitForAcknowledgments(Duration)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestWaitForAcknowledgments()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            var statusCondition = reader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;

            // Write some instances and wait for acknowledgments
            for (var i = 0; i < 10; i++)
            {
                evt.Reset();
                TestHelper.CreateWaitSetThread(evt, statusCondition);

                var result = dataWriter.Write(new TestStruct
                {
                    Id = i,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);

                Assert.IsTrue(evt.Wait(5_000));
            }

            Assert.AreEqual(ReturnCode.Ok, reader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(reader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteSubscriber(subscriber));
        }

        /// <summary>
        /// Test the <see cref="DataWriter.GetLivelinessLostStatus(ref LivelinessLostStatus)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetLivelinessLostStatus()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var dwQos = new DataWriterQos
            {
                Liveliness =
                {
                    Kind = LivelinessQosPolicyKind.ManualByTopicLivelinessQos,
                    LeaseDuration = new Duration { Seconds = 1 },
                },
            };

            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);
            Assert.IsNotNull(dataWriter);

            var result = writer.AssertLiveliness();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a reader
            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var reader = subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            var statusCondition = writer.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.LivelinessLostStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            Assert.IsTrue(reader.WaitForPublications(1, 1_500));
            Assert.IsTrue(writer.WaitForSubscriptions(1, 1_500));

            result = writer.AssertLiveliness();
            Assert.AreEqual(ReturnCode.Ok, result);

            // After half second liveliness should not be lost yet
            Assert.IsFalse(evt.Wait(500));

            LivelinessLostStatus status = default;
            result = writer.GetLivelinessLostStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);

            // After one second and a half one liveliness should be lost
            Assert.IsTrue(evt.Wait(1_500));

            result = writer.GetLivelinessLostStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);

            Assert.AreEqual(ReturnCode.Ok, reader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(reader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteSubscriber(subscriber));
        }

        /// <summary>
        /// Test the <see cref="DataWriter.GetOfferedDeadlineMissedStatus(ref OfferedDeadlineMissedStatus)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetOfferedDeadlineMissedStatus()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var qos = new DataWriterQos
            {
                Deadline =
                {
                    Period = new Duration
                    {
                        Seconds = 1,
                    },
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            var statusCondition = writer.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.OfferedDeadlineMissedStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var reader = subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            // Wait for discovery and write an instance
            var found = writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            found = reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            dataWriter.Write(new TestStruct
            {
                Id = 1,
            });

            // After half second deadline should not be lost yet
            Assert.IsFalse(evt.Wait(500));

            OfferedDeadlineMissedStatus status = default;
            var result = writer.GetOfferedDeadlineMissedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);

            // After one second and a half one deadline should be lost
            Assert.IsTrue(evt.Wait(1_500));

            result = writer.GetOfferedDeadlineMissedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreNotEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);

            Assert.AreEqual(ReturnCode.Ok, reader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(reader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteSubscriber(subscriber));
        }

        /// <summary>
        /// Test the <see cref="DataWriter.GetOfferedIncompatibleQosStatus(ref OfferedIncompatibleQosStatus)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetOfferedIncompatibleQosStatus()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var qos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);

            var statusCondition = writer.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.OfferedIncompatibleQosStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            // If not matched readers should return the default status
            OfferedIncompatibleQosStatus status = default;
            var result = writer.GetOfferedIncompatibleQosStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.IsNotNull(status.Policies);
            Assert.AreEqual(0, status.Policies.Count);
            Assert.AreEqual(0, status.LastPolicyId);

            // Create a not compatible reader
            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            // Wait for discovery and check the status
            Assert.IsTrue(evt.Wait(1_500));

            status = default;
            result = writer.GetOfferedIncompatibleQosStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreEqual(11, status.LastPolicyId);
            Assert.IsNotNull(status.Policies);
            Assert.AreEqual(1, status.Policies.Count);
            Assert.AreEqual(1, status.Policies.First().Count);
            Assert.AreEqual(11, status.Policies.First().PolicyId);

            Assert.AreEqual(ReturnCode.Ok, reader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(reader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteSubscriber(subscriber));
            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
        }

        /// <summary>
        /// Test the <see cref="DataWriter.GetPublicationMatchedStatus(ref PublicationMatchedStatus)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetPublicationMatchedStatus()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var qos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);

            var statusCondition = writer.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.PublicationMatchedStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            // If not DataReaders are created should return the default status
            PublicationMatchedStatus status = default;
            var result = writer.GetPublicationMatchedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.CurrentCount);
            Assert.AreEqual(0, status.CurrentCountChange);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastSubscriptionHandle);

            // Create a not compatible reader
            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            // Wait for discovery and check the status
            Assert.IsFalse(evt.Wait(1_500));

            result = writer.GetPublicationMatchedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.CurrentCount);
            Assert.AreEqual(0, status.CurrentCountChange);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastSubscriptionHandle);

            // Create a compatible reader
            var otherReader = subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(otherReader);

            // Wait for discovery and check the status
            Assert.IsTrue(evt.Wait(1_500));

            result = writer.GetPublicationMatchedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.CurrentCount);
            Assert.AreEqual(1, status.CurrentCountChange);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreEqual(otherReader.InstanceHandle, status.LastSubscriptionHandle);

            Assert.AreEqual(ReturnCode.Ok, reader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, otherReader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(reader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(otherReader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteSubscriber(subscriber));
            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
        }

        /// <summary>
        /// Test the <see cref="DataWriter.AssertLiveliness()" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestAssertLiveliness()
        {
            // Initialize entities
            var qos = new DataWriterQos
            {
                Liveliness =
                {
                    Kind = LivelinessQosPolicyKind.ManualByTopicLivelinessQos,
                    LeaseDuration = new Duration
                    {
                        Seconds = 1,
                    },
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);

            // Manually assert liveliness
            for (var i = 0; i < 5; i++)
            {
                var assertResult = writer.AssertLiveliness();
                Assert.AreEqual(ReturnCode.Ok, assertResult);
            }

            // Check that no liveliness has been lost
            LivelinessLostStatus status = default;
            var result = writer.GetLivelinessLostStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);

            Assert.AreEqual(ReturnCode.Ok, writer.AssertLiveliness());
            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
        }

        /// <summary>
        /// Test the <see cref="DataWriter.GetMatchedSubscriptions(ICollection{InstanceHandle})" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetMatchedSubscriptions()
        {
            using var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var qos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);

            var statusCondition = writer.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.PublicationMatchedStatus;
            TestHelper.CreateWaitSetThread(evt, statusCondition);

            // Test matched subscriptions without any match
            var list = new List<InstanceHandle>
            {
                InstanceHandle.HandleNil,
            };
            var result = writer.GetMatchedSubscriptions(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, list.Count);

            // Create a not compatible reader
            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var reader = subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            // Wait for discovery and check the matched subscriptions
            Assert.IsFalse(evt.Wait(1_500));

            result = writer.GetMatchedSubscriptions(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, list.Count);

            // Create a compatible reader
            var otherReader = subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(otherReader);

            // Wait for discovery and check the matched subscriptions
            Assert.IsTrue(evt.Wait(1_500));

            result = writer.GetMatchedSubscriptions(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(otherReader.InstanceHandle, list[0]);

            // Test with null parameter
            result = writer.GetMatchedSubscriptions(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            _publisher.DeleteDataWriter(writer);
            reader.DeleteContainedEntities();
            otherReader.DeleteContainedEntities();
            subscriber.DeleteDataReader(reader);
            subscriber.DeleteDataReader(otherReader);
            subscriber.DeleteContainedEntities();
            _participant.DeleteSubscriber(subscriber);
        }

        /// <summary>
        /// Test the <see cref="DataWriter.GetMatchedSubscriptionData(InstanceHandle, ref SubscriptionBuiltinTopicData)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetMatchedSubscriptionData()
        {
            // Initialize entities
            var dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            var writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);

            // DCPSInfoRepo-based discovery generates Built-In Topic data once (inside the
            // info repo process) and therefore all known entities in the domain are
            // reflected in the Built-In Topics. RTPS discovery, on the other hand, follows
            // the DDS specification and omits "local" entities from the Built-In Topics.
            // The definition of "local" means those entities belonging to the same Domain
            // Participant as the given Built-In Topic Subscriber.
            // https://github.com/OpenDDS/OpenDDS/blob/master/docs/design/RTPS

            // OPENDDS ISSUE: GetMatchedSubscriptions returns local entities but GetMatchedSubscriptionData doesn't
            // because is looking in the Built-in topic. If not found in the built-in, shouldn't try to look locally?
            // WORKAROUND: Create another participant for the DataReader.
            var otherParticipant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN);
            Assert.IsNotNull(otherParticipant);
            otherParticipant.BindRtpsUdpTransportConfig();

            var support = new TestStructTypeSupport();
            var typeName = support.GetTypeName();
            var result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            var otherTopic = otherParticipant.CreateTopic(nameof(TestGetMatchedSubscriptionData), typeName);
            Assert.IsNotNull(otherTopic);

            var subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var drQos = TestHelper.CreateNonDefaultDataReaderQos();
            var reader = subscriber.CreateDataReader(otherTopic, drQos);
            Assert.IsNotNull(reader);

            // Wait for subscriptions/publications
            var found = writer.WaitForSubscriptions(1, 5000);
            Assert.IsTrue(found);
            found = reader.WaitForPublications(1, 5000);
            Assert.IsTrue(found);

            // Get the matched subscriptions
            var list = new List<InstanceHandle>();
            result = writer.GetMatchedSubscriptions(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, list.Count);

            // Get the matched subscription data
            SubscriptionBuiltinTopicData data = default;
            result = writer.GetMatchedSubscriptionData(list[0], ref data);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestNonDefaultSubscriptionData(data);

            // Destroy entities
            Assert.AreEqual(ReturnCode.Ok, reader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(reader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, otherParticipant.DeleteSubscriber(subscriber));
            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
            Assert.AreEqual(ReturnCode.Ok, otherParticipant.DeleteTopic(otherTopic));
            Assert.AreEqual(ReturnCode.Ok, AssemblyInitializer.Factory.DeleteParticipant(otherParticipant));
        }

        /// <summary>
        /// Test the <see cref="TestStructDataWriter.RegisterInstance(TestStruct, Timestamp)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestRegisterInstance()
        {
            // Initialize entities
            var writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            // Register an instance
            var handle = dataWriter.RegisterInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            // Register an instance with timestamp
            var otherHandle = dataWriter.RegisterInstance(new TestStruct { Id = 2 }, DateTime.Now.ToTimestamp());
            Assert.AreNotEqual(InstanceHandle.HandleNil, otherHandle);
            Assert.AreNotEqual(handle, otherHandle);

            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
        }

        /// <summary>
        /// Test the <see cref="TestStructDataWriter.UnregisterInstance(TestStruct, InstanceHandle, Timestamp)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestUnregisterInstance()
        {
            // Initialize entities
            var writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            // Unregister not registered instance
            var result = dataWriter.UnregisterInstance(new TestStruct { Id = 1 });
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Register an instance
            var instance1 = new TestStruct { Id = 1 };
            var handle1 = dataWriter.RegisterInstance(instance1);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle1);

            // Unregister the previous registered instance with the simplest overload
            result = dataWriter.UnregisterInstance(new TestStruct { Id = 1 });
            Assert.AreEqual(ReturnCode.Ok, result);

            // Register a new instance
            var instance2 = new TestStruct { Id = 2 };
            var handle2 = dataWriter.RegisterInstance(instance2);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle2);

            // Unregister the previous registered instance with the handle
            result = dataWriter.UnregisterInstance(new TestStruct { Id = 2 }, handle2);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Register a new instance
            var instance3 = new TestStruct { Id = 3 };
            var handle3 = dataWriter.RegisterInstance(instance3);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle3);

            // Unregister the previous registered instance with the handle and the timestamp
            result = dataWriter.UnregisterInstance(new TestStruct { Id = 3 }, handle3, DateTime.Now.ToTimestamp());
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
        }

        /// <summary>
        /// Test the <see cref="TestStructDataWriter.Write(TestStruct, InstanceHandle, Timestamp)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestWrite()
        {
            var evt = new ManualResetEventSlim(false);

            // Initialize entities
            var duration = new Duration { Seconds = 5 };

            var writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            var listener = new MyDataReaderListener();
            var dataReader = subscriber.CreateDataReader(_topic, qos, listener);
            Assert.IsNotNull(dataReader);

            var count = 0;
            Timestamp timestamp = default;
            var samples = new List<TestStruct>();
            var infos = new List<SampleInfo>();
            DataReader receivedReader = null;

            listener.DataAvailable += (reader) =>
            {
                count++;
                receivedReader = reader;

                evt.Set();
            };

            // Wait for discovery
            var found = writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            found = dataReader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write an instance with the simplest overload
            var result = dataWriter.Write(new TestStruct { Id = 1 });
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));
            Assert.AreEqual(1, count);

            evt.Reset();

            // Write an instance with the handle parameter as HandleNil
            result = dataWriter.Write(new TestStruct { Id = 2 }, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));
            Assert.AreEqual(2, count);
            evt.Reset();

            // Write an instance with the handle parameter with a previously registered instance
            var instance = new TestStruct { Id = 3 };
            var handle = dataWriter.RegisterInstance(instance);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = dataWriter.Write(instance, handle);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));
            Assert.AreEqual(3, count);
            evt.Reset();

            // Write an instance with the handle parameter and the timestamp
            var now = DateTime.Now.ToTimestamp();
            var instance1 = new TestStruct { Id = 4 };
            var handle1 = dataWriter.RegisterInstance(instance1);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle1);

            result = dataWriter.Write(instance1, handle1, now);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(1_500));
            Assert.AreEqual(4, count);
            Assert.IsNotNull(receivedReader);

            var dr = new TestStructDataReader(receivedReader);

            var lookupHandle = dr.LookupInstance(new TestStruct { Id = count });
            var retReadInstance = dr.ReadInstance(samples, infos, lookupHandle);
            if (retReadInstance == ReturnCode.Ok && infos.Count > 0)
            {
                timestamp = infos[0].SourceTimestamp;
            }

            Assert.AreNotEqual(InstanceHandle.HandleNil, lookupHandle);
            Assert.AreEqual(ReturnCode.Ok, retReadInstance);
            Assert.IsNotNull(infos);
            Assert.AreEqual(1, infos.Count);
            Assert.AreEqual(now.Seconds, timestamp.Seconds);
            Assert.AreEqual(now.NanoSeconds, timestamp.NanoSeconds);

            foreach (var d in listener.DataAvailable.GetInvocationList())
            {
                var del = (Action<DataReader>)d;
                listener.DataAvailable -= del;
            }
            Assert.AreEqual(ReturnCode.Ok, dataReader.SetListener(null, StatusMask.NoStatusMask));
            listener.Dispose();

            evt.Dispose();

            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
            Assert.AreEqual(ReturnCode.Ok, dataReader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(dataReader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteSubscriber(subscriber));
        }

        /// <summary>
        /// Test the <see cref="TestStructDataWriter.Dispose(TestStruct, InstanceHandle, Timestamp)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDispose()
        {
            // Initialize entities
            var qos = new DataWriterQos
            {
                WriterDataLifecycle =
                {
                    AutodisposeUnregisteredInstances = false,
                },
            };
            var writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            var subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            var drQos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            var dataReader = subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(dataReader);
            var statusCondition = dataReader.StatusCondition;
            statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;

            var dr = new TestStructDataReader(dataReader);

            var evtDisposed = new ManualResetEventSlim(false);
            var evtAlive = new ManualResetEventSlim(false);

            var countDisposed = 0;
            Timestamp timestamp = default;
            var waitSet = new WaitSet();
            waitSet.AttachCondition(statusCondition);
            var thread = new Thread(() =>
            {
                var isSet = false;
                while (!isSet)
                {
                    ICollection<Condition> conditions = new List<Condition>();
                    waitSet.Wait(conditions);

                    if (!conditions.Any(cond => cond == statusCondition && cond.TriggerValue))
                    {
                        continue;
                    }

                    var samples = new List<TestStruct>();
                    var infos = new List<SampleInfo>();
                    var ret = dr.Take(samples, infos);
                    if (ret != ReturnCode.Ok || !infos.Any())
                    {
                        continue;
                    }

                    foreach (var info in infos)
                    {
                        if (info.InstanceState == InstanceStateKind.NotAliveDisposedInstanceState)
                        {
                            countDisposed++;
                            if (countDisposed == 3)
                            {
                                timestamp = info.SourceTimestamp;
                                isSet = true;
                            }

                            evtDisposed.Set();
                        }
                        else if (info.InstanceState == InstanceStateKind.AliveInstanceState)
                        {
                            evtAlive.Set();
                        }
                    }
                }
            })
            {
                IsBackground = true,
            };
            thread.Start();

            // Wait for discovery
            Assert.IsTrue(writer.WaitForSubscriptions(1, 10_000));
            Assert.IsTrue(dataReader.WaitForPublications(1, 10_000));

            // Dispose an instance that does not exist
            var result = dataWriter.Dispose(new TestStruct { Id = 1 }, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Error, result);

            // Call dispose with the simplest overload
            var instance1 = new TestStruct { Id = 1 };
            result = dataWriter.Write(instance1);
            Assert.AreEqual(ReturnCode.Ok, result);

            var duration = new Duration { Seconds = 5 };
            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evtAlive.Wait(2_500));
            evtAlive.Reset();

            result = dataWriter.Dispose(instance1);
            Assert.AreEqual(ReturnCode.Ok, result);

            duration = new Duration { Seconds = 5 };
            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evtDisposed.Wait(2_500));
            Assert.AreEqual(1, countDisposed);
            evtDisposed.Reset();

            // Call dispose with the handle parameter
            var instance2 = new TestStruct { Id = 2 };
            var handle2 = dataWriter.RegisterInstance(instance2);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle2);

            result = dataWriter.Write(instance2, handle2);
            Assert.AreEqual(ReturnCode.Ok, result);

            duration = new Duration { Seconds = 5 };
            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evtAlive.Wait(2_500));
            evtAlive.Reset();

            result = dataWriter.Dispose(instance2, handle2);
            Assert.AreEqual(ReturnCode.Ok, result);

            duration = new Duration { Seconds = 5 };
            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evtDisposed.Wait(2_500));
            Assert.AreEqual(2, countDisposed);
            evtDisposed.Reset();

            // Call dispose with the handle parameter and specific timestamp
            var now = DateTime.Now.ToTimestamp();
            var instance3 = new TestStruct { Id = 3 };
            var handle3 = dataWriter.RegisterInstance(instance3);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle3);

            result = dataWriter.Write(instance3, handle3);
            Assert.AreEqual(ReturnCode.Ok, result);

            duration = new Duration { Seconds = 5 };
            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evtAlive.Wait(2_500));
            Assert.AreEqual(2, countDisposed);
            evtAlive.Reset();

            result = dataWriter.Dispose(instance3, handle3, now);
            Assert.AreEqual(ReturnCode.Ok, result);

            duration = new Duration { Seconds = 5 };
            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evtDisposed.Wait(2_500));
            Assert.AreEqual(3, countDisposed);
            Assert.AreEqual(now.Seconds, timestamp.Seconds);
            Assert.AreEqual(now.NanoSeconds, timestamp.NanoSeconds);

            evtDisposed.Dispose();
            evtAlive.Dispose();

            Assert.AreEqual(ReturnCode.Ok, dataReader.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteDataReader(dataReader));
            Assert.AreEqual(ReturnCode.Ok, subscriber.DeleteContainedEntities());
            Assert.AreEqual(ReturnCode.Ok, _publisher.DeleteDataWriter(writer));
            Assert.AreEqual(ReturnCode.Ok, _participant.DeleteSubscriber(subscriber));
        }

        /// <summary>
        /// Test the <see cref="TestStructDataWriter.GetKeyValue(TestStruct, InstanceHandle)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestGetKeyValue()
        {
            // Initialize entities
            var writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            // Call GetKeyValue with HandleNil
            var data = new TestStruct();
            var result = dataWriter.GetKeyValue(data, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Register an instance
            var instance = new TestStruct { Id = 1 };
            var handle = dataWriter.RegisterInstance(instance);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            // Call GetKeyValue
            data = new TestStruct();
            result = dataWriter.GetKeyValue(data, handle);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, data.Id);

            _publisher.DeleteDataWriter(writer);
        }

        /// <summary>
        /// Test the <see cref="TestStructDataWriter.LookupInstance(TestStruct)" /> method.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestLookupInstance()
        {
            // Initialize entities
            var writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            var dataWriter = new TestStructDataWriter(writer);

            // Lookup for a non-existing instance
            var handle = dataWriter.LookupInstance(new TestStruct { Id = 1 });
            Assert.AreEqual(InstanceHandle.HandleNil, handle);

            // Register an instance
            var handle1 = dataWriter.RegisterInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle1);

            // Lookup for an existing instance
            handle = dataWriter.LookupInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);
            Assert.AreEqual(handle1, handle);

            _publisher.DeleteDataWriter(writer);
        }
        #endregion
    }
}
