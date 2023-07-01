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
using System.Diagnostics;
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
    /// <see cref="DomainParticipantListener"/> unit test class.
    /// </summary>
    [TestClass]
    public class DomainParticipantListenerTest
    {
        #region Constants
        private const string TEST_CATEGORY = "DomainParticipantListener";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Topic _topic;
        private Subscriber _subscriber;
        private Publisher _publisher;
        private DataWriter _writer;
        private TestStructDataWriter _dataWriter;
        private MyParticipantListener _listener;
        private DataReader _reader;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets test context object.
        /// </summary>
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization/Cleanup
        /// <summary>
        /// The test initializer method.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _listener = new MyParticipantListener();
            _participant = AssemblyInitializer.Factory.CreateParticipant(AssemblyInitializer.RTPS_DOMAIN, null, _listener);
            Assert.IsNotNull(_participant);
            _participant.BindRtpsUdpTransportConfig();

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);
            Assert.IsNull(_topic.Listener);
            Assert.AreEqual(TestContext.TestName, _topic.Name);
            Assert.AreEqual(typeName, _topic.TypeName);

            SubscriberQos sQos = new SubscriberQos();
            sQos.EntityFactory.AutoenableCreatedEntities = false;
            // sQos.Presentation.OrderedAccess = true;
            // sQos.Presentation.CoherentAccess = true;
            // sQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.InstancePresentationQos;
            _subscriber = _participant.CreateSubscriber(sQos);
            Assert.IsNotNull(_subscriber);

            PublisherQos pQos = new PublisherQos();
            pQos.EntityFactory.AutoenableCreatedEntities = false;
            // pQos.Presentation.OrderedAccess = true;
            // pQos.Presentation.CoherentAccess = true;
            // pQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.InstancePresentationQos;
            _publisher = _participant.CreatePublisher(pQos);
            Assert.IsNotNull(_publisher);

            _writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(_writer);
            _dataWriter = new TestStructDataWriter(_writer);

            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            _reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(_reader);
        }

        /// <summary>
        /// The test cleanup method.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _publisher?.DeleteDataWriter(_writer);
            _reader?.DeleteContainedEntities();
            _publisher?.DeleteContainedEntities();
            _subscriber?.DeleteDataReader(_reader);
            _subscriber?.DeleteContainedEntities();
            _participant?.DeletePublisher(_publisher);
            _participant?.DeleteSubscriber(_subscriber);
            _participant?.DeleteTopic(_topic);
            _participant?.DeleteContainedEntities();

            AssemblyInitializer.Factory?.DeleteParticipant(_participant);

            _listener.Dispose();

            _participant = null;
            _publisher = null;
            _subscriber = null;
            _topic = null;
            _writer = null;
            _reader = null;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnDataOnReaders(Subscriber)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnDataOnReaders()
        {
            // Attach to the event
            int count = 0;
            Subscriber subscriber = null;
            _listener.DataOnReaders += (s) =>
            {
                subscriber = s;
                count++;
            };

            //// Enable entities
            ReturnCode result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Write some instances
            int total = 5;
            for (int i = 1; i <= total; i++)
            {
                result = _dataWriter.Write(new TestStruct
                {
                    Id = i,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Thread.Sleep(100);

            Assert.AreEqual(total, count);
            Assert.IsNotNull(subscriber);
            Assert.AreEqual(_subscriber, subscriber);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnDataAvailable(DataReader)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnDataAvailable()
        {
            // Prepare status mask:
            // If a ParticipantListener has both on_data_on_readers() and on_data_available() callbacks enabled
            // (by turning on both status bits), only on_data_on_readers() is called.
            ReturnCode result = _participant.SetListener(_listener, StatusKind.DataAvailableStatus);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Attach to the event
            int count = 0;
            DataReader reader = null;
            _listener.DataAvailable += (r) =>
            {
                reader = r;
                count++;
            };

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Write some instances
            int total = 5;
            for (int i = 1; i <= total; i++)
            {
                result = _dataWriter.Write(new TestStruct
                {
                    Id = i,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Thread.Sleep(100);

            Assert.AreEqual(total, count);
            Assert.IsNotNull(reader);
            Assert.AreEqual(_reader, reader);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnRequestedDeadlineMissed(DataReader, RequestedDeadlineMissedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnRequestedDeadlineMissed()
        {
            // Prepare qos for the test
            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Deadline.Period = new Duration { Seconds = 1 };
            ReturnCode result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Deadline.Period = new Duration { Seconds = 1 };
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            result = _reader.SetQos(drQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Attach to the event
            int count = 0;
            DataReader reader = null;
            int totalCount = 0;
            int totalCountChange = 0;
            InstanceHandle lastInstanceHandle = InstanceHandle.HandleNil;
            _listener.RequestedDeadlineMissed += (r, s) =>
            {
                reader = r;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                lastInstanceHandle = s.LastInstanceHandle;
                count++;
            };

            // Write an instance
            _dataWriter.Write(new TestStruct { Id = 1 });

            // After half second deadline should not be lost yet
            Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one deadline should be lost
            Thread.Sleep(1000);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_reader, reader);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreNotEqual(InstanceHandle.HandleNil, lastInstanceHandle);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnRequestedIncompatibleQos(DataReader, RequestedIncompatibleQosStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnRequestedIncompatibleQos()
        {
            int count = 0;
            DataReader dr = null;
            int totalCount = 0;
            int totalCountChange = 0;
            int lastPolicyId = 0;
            ICollection<QosPolicyCount> policies = new List<QosPolicyCount>();

            // Attach to the event
            _listener.RequestedIncompatibleQos += (r, s) =>
            {
                dr = r;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                lastPolicyId = s.LastPolicyId;
                policies = s.Policies;

                count++;
            };

            // Create a incompatible DataWriter
            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            DataWriter otherDataWriter = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(otherDataWriter);

            // Enable entities
            ReturnCode result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = otherDataWriter.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Check the number of incompatible DataWriter
            Thread.Sleep(100);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_reader, dr);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreEqual(11, lastPolicyId);
            Assert.IsNotNull(policies);
            Assert.AreEqual(1, policies.Count);
            Assert.AreEqual(1, policies.First().Count);
            Assert.AreEqual(11, policies.First().PolicyId);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnSampleRejected(DataReader, SampleRejectedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnSampleRejected()
        {
            int count = 0;
            DataReader dataReader = null;
            int totalCount = 0;
            int totalCountChange = 0;
            InstanceHandle lastInstanceHandle = InstanceHandle.HandleNil;
            SampleRejectedStatusKind lastReason = SampleRejectedStatusKind.NotRejected;

            // Attach to the event
            _listener.SampleRejected += (r, s) =>
            {
                dataReader = r;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                lastInstanceHandle = s.LastInstanceHandle;
                lastReason = s.LastReason;

                count++;
            };

            // Prepare QoS for the test
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            drQos.ResourceLimits.MaxInstances = 1;
            drQos.ResourceLimits.MaxSamples = 1;
            drQos.ResourceLimits.MaxSamplesPerInstance = 1;
            ReturnCode result = _reader.SetQos(drQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Write two samples of the same instances
            for (int i = 1; i <= 2; i++)
            {
                result = _dataWriter.Write(new TestStruct
                {
                    Id = 1,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Thread.Sleep(100);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_reader, dataReader);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreNotEqual(InstanceHandle.HandleNil, lastInstanceHandle);
            Assert.AreEqual(SampleRejectedStatusKind.RejectedBySamplesPerInstanceLimit, lastReason);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnLivelinessChanged(DataReader, LivelinessChangedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnLivelinessChanged()
        {
            int count = 0;

            DataReader firstDataReader = null;
            int firstAliveCount = 0;
            int firstAliveCountChange = 0;
            int firstNotAliveCount = 1;
            int firstNotAliveCountChange = 1;
            InstanceHandle firstLastPublicationHandle = InstanceHandle.HandleNil;

            DataReader secondDataReader = null;
            int secondAliveCount = 1;
            int secondAliveCountChange = 1;
            int secondNotAliveCount = 0;
            int secondNotAliveCountChange = 0;
            InstanceHandle secondLastPublicationHandle = InstanceHandle.HandleNil;

            // Attach to the event
            _listener.LivelinessChanged += (r, s) =>
            {
                if (count == 0)
                {
                    // Liveliness alive
                    firstDataReader = r;
                    firstAliveCount = s.AliveCount;
                    firstAliveCountChange = s.AliveCountChange;
                    firstNotAliveCount = s.NotAliveCount;
                    firstNotAliveCountChange = s.NotAliveCountChange;
                    firstLastPublicationHandle = s.LastPublicationHandle;
                }
                else
                {
                    // Liveliness not alive
                    secondDataReader = r;
                    secondAliveCount = s.AliveCount;
                    secondAliveCountChange = s.AliveCountChange;
                    secondNotAliveCount = s.NotAliveCount;
                    secondNotAliveCountChange = s.NotAliveCountChange;
                    secondLastPublicationHandle = s.LastPublicationHandle;
                }

                count++;
            };

            // Prepare the QoS for the test
            DataReaderQos drQos = new DataReaderQos();
            drQos.Liveliness.LeaseDuration = new Duration { Seconds = 1 };
            ReturnCode result = _reader.SetQos(drQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Liveliness.Kind = LivelinessQosPolicyKind.ManualByTopicLivelinessQos;
            dwQos.Liveliness.LeaseDuration = new Duration { Seconds = 1 };
            result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            // Assert liveliness in the writer
            result = _writer.AssertLiveliness();
            Assert.AreEqual(ReturnCode.Ok, result);

            // After half second liveliness should not be lost yet
            Thread.Sleep(500);
            Assert.AreEqual(1, count);
            Assert.IsNotNull(firstDataReader);
            Assert.AreEqual(_reader, firstDataReader);
            Assert.AreEqual(1, firstAliveCount);
            Assert.AreEqual(1, firstAliveCountChange);
            Assert.AreEqual(0, firstNotAliveCount);
            Assert.AreEqual(0, firstNotAliveCountChange);
            Assert.AreEqual(_writer.InstanceHandle, firstLastPublicationHandle);

            // After one second and a half one liveliness should be lost
            Thread.Sleep(1000);
            Assert.AreEqual(2, count);
            Assert.IsNotNull(secondDataReader);
            Assert.AreEqual(_reader, secondDataReader);
            Assert.AreEqual(0, secondAliveCount);
            Assert.AreEqual(-1, secondAliveCountChange);
            Assert.AreEqual(1, secondNotAliveCount);
            Assert.AreEqual(1, secondNotAliveCountChange);
            Assert.AreEqual(_writer.InstanceHandle, secondLastPublicationHandle);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnSubscriptionMatched(DataReader, SubscriptionMatchedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnSubscriptionMatched()
        {
            int count = 0;

            DataReader firstDataReader = null;
            int firstTotalCount = 0;
            int firstTotalCountChange = 0;
            int firstCurrentCount = 1;
            int firstCurrentCountChange = 1;
            InstanceHandle firstHandle = InstanceHandle.HandleNil;

            DataReader secondDataReader = null;
            int secondTotalCount = 1;
            int secondTotalCountChange = 1;
            int secondCurrentCount = 0;
            int secondCurrentCountChange = 0;
            InstanceHandle secondHandle = InstanceHandle.HandleNil;

            // Attach to the event
            _listener.SubscriptionMatched += (r, s) =>
            {
                Assert.AreEqual(_reader, r);

                if (count == 0)
                {
                    // Liveliness alive
                    firstDataReader = r;
                    firstTotalCount = s.TotalCount;
                    firstTotalCountChange = s.TotalCountChange;
                    firstCurrentCount = s.CurrentCount;
                    firstCurrentCountChange = s.CurrentCountChange;
                    firstHandle = s.LastPublicationHandle;
                }
                else
                {
                    // Liveliness not alive
                    secondDataReader = r;
                    secondTotalCount = s.TotalCount;
                    secondTotalCountChange = s.TotalCountChange;
                    secondCurrentCount = s.CurrentCount;
                    secondCurrentCountChange = s.CurrentCountChange;
                    secondHandle = s.LastPublicationHandle;
                }

                count++;
            };

            // Enable entities
            ReturnCode result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Check subscription matched call
            Thread.Sleep(1500);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_reader, firstDataReader);
            Assert.AreEqual(1, firstTotalCount);
            Assert.AreEqual(1, firstTotalCountChange);
            Assert.AreEqual(1, firstCurrentCount);
            Assert.AreEqual(1, firstCurrentCountChange);
            Assert.AreEqual(_writer.InstanceHandle, firstHandle);

            // Delete the writer
            result = _publisher.DeleteDataWriter(_writer);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Check subscription matched call
            Thread.Sleep(500);
            Assert.AreEqual(2, count);
            Assert.AreEqual(_reader, secondDataReader);
            Assert.AreEqual(1, secondTotalCount);
            Assert.AreEqual(0, secondTotalCountChange);
            Assert.AreEqual(0, secondCurrentCount);
            Assert.AreEqual(-1, secondCurrentCountChange);
            Assert.AreEqual(_writer.InstanceHandle, secondHandle);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnSampleLost(DataReader, SampleLostStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnSampleLost()
        {
            using ManualResetEventSlim evt = new ManualResetEventSlim(false);

            DataReader reader = null;
            int count = 0;
            int totalCount = 0;
            int totalCountChange = 0;

            // Attach to the event.
            _listener.SampleLost += (r, s) =>
            {
                reader = r;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;

                count++;

                evt.Set();
            };

            // Prepare QoS for the test.
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            drQos.DestinationOrder.Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepLastHistoryQos;
            drQos.History.Depth = 1;
            ReturnCode result = _reader.SetQos(drQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities.
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery.
            Assert.IsTrue(_reader.WaitForPublications(1, 5_000));
            Assert.IsTrue(_writer.WaitForSubscriptions(1, 5_000));

            // Write two samples of the same instances.
            InstanceHandle handle = _dataWriter.RegisterInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            Timestamp time = DateTime.Now.ToTimestamp();
            result = _dataWriter.Write(new TestStruct { Id = 1 }, handle, time);
            Assert.AreEqual(ReturnCode.Ok, result);

            Thread.Sleep(100);

            time = DateTime.Now.Subtract(TimeSpan.FromSeconds(10)).ToTimestamp();
            result = _dataWriter.Write(new TestStruct { Id = 1 }, handle, time);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(20_000));
            Assert.AreEqual(1, count);
            Assert.AreEqual(_reader, reader);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);

            // Remove the listener to avoid extra messages.
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnOfferedDeadlineMissed(DataWriter, OfferedDeadlineMissedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnOfferedDeadlineMissed()
        {
            DataWriter writer = null;
            int totalCount = 0;
            int totalCountChange = 0;
            var lastInstanceHandle = InstanceHandle.HandleNil;

            // Attach to the event
            int count = 0;
            _listener.OfferedDeadlineMissed += (w, s) =>
            {
                writer = w;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                lastInstanceHandle = s.LastInstanceHandle;
                count++;
            };

            // Prepare QoS for the test
            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Deadline.Period = new Duration { Seconds = 1 };
            ReturnCode result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery and write an instance
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            _dataWriter.Write(new TestStruct { Id = 1 });

            // After half second deadline should not be lost yet
            Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one deadline should be lost
            Thread.Sleep(1000);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_writer, writer);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnOfferedIncompatibleQos(DataWriter, OfferedIncompatibleQosStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnOfferedIncompatibleQos()
        {
            DataWriter dw = null;
            int totalCount = 0;
            int totalCountChange = 0;
            int lastPolicyId = 0;
            ICollection<QosPolicyCount> policies = new List<QosPolicyCount>();

            // Attach to the event
            int count = 0;
            _listener.OfferedIncompatibleQos += (w, s) =>
            {
                dw = w;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                lastPolicyId = s.LastPolicyId;
                policies = s.Policies;

                count++;
            };

            // Prepare QoS for the test
            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            ReturnCode result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            result = _reader.SetQos(drQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            Thread.Sleep(100);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_writer, dw);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreEqual(11, lastPolicyId);
            Assert.IsNotNull(policies);
            Assert.AreEqual(1, policies.Count);
            Assert.AreEqual(1, policies.First().Count);
            Assert.AreEqual(11, policies.First().PolicyId);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnLivelinessLost(DataWriter, LivelinessLostStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        [Ignore("It hangs in Windows. Looking for a solution...")]
        public void TestOnLivelinessLost()
        {
            DataWriter dw = null;
            int totalCount = 0;
            int totalCountChange = 0;

            // Attach to the event
            int count = 0;
            _listener.LivelinessLost += (w, s) =>
            {
                dw = w;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;

                count++;
            };

            // Prepare QoS for the test
            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Liveliness.Kind = LivelinessQosPolicyKind.ManualByTopicLivelinessQos;
            dwQos.Liveliness.LeaseDuration = new Duration { Seconds = 1 };
            ReturnCode result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // After half second liveliness should not be lost yet
            Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one liveliness should be lost
            Thread.Sleep(1000);
            Assert.AreEqual(1, count);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnPublicationMatched(DataWriter, PublicationMatchedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnPublicationMatched()
        {
            DataWriter dw = null;
            int currentCount = 0;
            int currentCountChange = 0;
            int totalCount = 0;
            int totalCountChange = 0;
            InstanceHandle handle = InstanceHandle.HandleNil;

            // Attach to the event
            int count = 0;
            _listener.PublicationMatched += (w, s) =>
            {
                dw = w;
                currentCount = s.CurrentCount;
                currentCountChange = s.CurrentCountChange;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                handle = s.LastSubscriptionHandle;

                count++;
            };

            // Enable entities
            ReturnCode result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            bool found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            Thread.Sleep(100);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_writer, dw);
            Assert.AreEqual(1, currentCount);
            Assert.AreEqual(1, currentCountChange);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreEqual(_reader.InstanceHandle, handle);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DomainParticipantListener.OnInconsistentTopic(Topic, InconsistentTopicStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnInconsistentTopic()
        {
            using ManualResetEventSlim evt = new ManualResetEventSlim(false);

            Topic topic = null;
            int totalCount = 0;
            int totalCountChange = 0;

            // Attach to the event
            int count = 0;
            _listener.InconsistentTopic += (t, s) =>
            {
                topic = t;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;

                count++;
                evt.Set();
            };

            // Enable entities
            ReturnCode result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            SupportProcessHelper supportProcess = new SupportProcessHelper(TestContext);
            Process process = supportProcess.SpawnSupportProcess(SupportTestKind.InconsistentTopicTest);

            // Wait the signal
            bool wait = evt.Wait(20000);
            Assert.IsTrue(wait);
            Assert.AreSame(_topic, topic);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);

            // Kill the process
            supportProcess.KillProcess(process);

            Assert.AreEqual(1, count);

            // Remove listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }
        #endregion
    }
}
