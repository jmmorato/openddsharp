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
    /// <see cref="SubscriberListener"/> unit test class.
    /// </summary>
    [TestClass]
    public class SubscriberListenerTest
    {
        #region Constants
        private const string TEST_CATEGORY = "SubscriberListener";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Topic _topic;
        private Subscriber _subscriber;
        private Publisher _publisher;
        private DataWriter _writer;
        private TestStructDataWriter _dataWriter;
        private MySubscriberListener _listener;
        private DataReader _reader;
        private TestStructDataReader _dataReader;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets test context object.
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

            _listener = new MySubscriberListener();
            var sQos = new SubscriberQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
                Presentation =
                {
                    OrderedAccess = true,
                    CoherentAccess = true,
                    AccessScope = PresentationQosPolicyAccessScopeKind.InstancePresentationQos,
                },
            };
            _subscriber = _participant.CreateSubscriber(sQos, _listener);
            Assert.IsNotNull(_subscriber);

            var pQos = new PublisherQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
                Presentation =
                {
                    OrderedAccess = true,
                    CoherentAccess = true,
                    AccessScope = PresentationQosPolicyAccessScopeKind.InstancePresentationQos,
                },
            };
            _publisher = _participant.CreatePublisher(pQos);
            Assert.IsNotNull(_publisher);

            _writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(_writer);
            _dataWriter = new TestStructDataWriter(_writer);

            var qos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            _reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(_reader);
            _dataReader = new TestStructDataReader(_reader);
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

            _listener?.Dispose();

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
        /// Test the <see cref="SubscriberListener.OnDataOnReaders(Subscriber)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnDataOnReaders()
        {
            using var evt = new ManualResetEventSlim(false);

            var result = _subscriber.SetListener(_listener, StatusKind.DataOnReadersStatus);
            Assert.AreEqual(ReturnCode.Ok, result);

            Subscriber subscriber = null;

            // Attach to the event
            var count = 0;
            const int total = 5;
            _listener.DataOnReaders += (s) =>
            {
                subscriber = s;
                count++;

                var readers = new List<DataReader>();
                result = _subscriber.GetDataReaders(readers);
                Assert.AreEqual(ReturnCode.Ok, result);

                foreach (var reader in readers)
                {
                    var sample = new List<TestStruct>();
                    var info = new List<SampleInfo>();

                    result = _dataReader.Take(sample, info);
                    Assert.AreEqual(ReturnCode.Ok, result);
                }

                if (count == total)
                {
                    evt.Set();
                }
            };

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            var found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            found = _reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write some instances
            for (var i = 1; i <= total; i++)
            {
                result = _dataWriter.Write(new TestStruct
                {
                    Id = i,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Assert.IsTrue(evt.Wait(1_500));

            Assert.AreEqual(total, count);
            Assert.IsNotNull(subscriber);
            Assert.AreEqual(_subscriber, subscriber);

            // Remove the listener to avoid extra messages
            result = _subscriber.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="SubscriberListener.OnDataAvailable(DataReader)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnDataAvailable()
        {
            using var evt = new ManualResetEventSlim(false);

            // Prepare status mask:
            // If a SubscriberListener has both on_data_on_readers() and on_data_available() callbacks enabled
            // (by turning on both status bits), only on_data_on_readers() is called.
            var result = _subscriber.SetListener(_listener, StatusKind.DataAvailableStatus);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Attach to the event
            var count = 0;
            const int total = 5;
            DataReader reader = null;
            _listener.DataAvailable += (r) =>
            {
                reader = r;
                count++;

                var sample = new List<TestStruct>();
                var info = new List<SampleInfo>();

                result = _dataReader.Take(sample, info);
                Assert.AreEqual(ReturnCode.Ok, result);

                if (count == total)
                {
                    evt.Set();
                }
            };

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            var found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            found = _reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write some instances
            for (var i = 1; i <= total; i++)
            {
                result = _dataWriter.Write(new TestStruct
                {
                    Id = i,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Assert.IsTrue(evt.Wait(1_500));

            Assert.AreEqual(total, count);
            Assert.IsNotNull(reader);
            Assert.AreEqual(_reader, reader);

            // Remove the listener to avoid extra messages
            result = _subscriber.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="SubscriberListener.OnRequestedDeadlineMissed(DataReader, RequestedDeadlineMissedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnRequestedDeadlineMissed()
        {
            using var evt = new ManualResetEventSlim(false);

            // Prepare qos for the test
            var dwQos = new DataWriterQos
            {
                Deadline =
                {
                    Period = new Duration { Seconds = 1 },
                },
            };
            var result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            var drQos = new DataReaderQos
            {
                Deadline =
                {
                    Period = new Duration { Seconds = 1 },
                },
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
            };
            result = _reader.SetQos(drQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            var found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            found = _reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Attach to the event
            var count = 0;
            DataReader reader = null;
            var totalCount = 0;
            var totalCountChange = 0;
            var lastInstanceHandle = InstanceHandle.HandleNil;
            _listener.RequestedDeadlineMissed += (r, s) =>
            {
                reader = r;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                lastInstanceHandle = s.LastInstanceHandle;

                count++;

                evt.Set();
            };

            // Write an instance
            _dataWriter.Write(new TestStruct { Id = 1 });

            // After half second deadline should not be lost yet
            Assert.IsFalse(evt.Wait(500));
            Assert.AreEqual(0, count);

            // After one second and a half one deadline should be lost
            Assert.IsTrue(evt.Wait(1_500));
            Assert.AreEqual(1, count);
            Assert.AreEqual(_reader, reader);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreNotEqual(InstanceHandle.HandleNil, lastInstanceHandle);

            // Remove the listener to avoid extra messages
            result = _subscriber.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="SubscriberListener.OnRequestedIncompatibleQos(DataReader, RequestedIncompatibleQosStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnRequestedIncompatibleQos()
        {
            using var evt = new ManualResetEventSlim(false);

            var count = 0;
            DataReader dr = null;
            var totalCount = 0;
            var totalCountChange = 0;
            var lastPolicyId = 0;
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

                evt.Set();
            };

            // Create a incompatible DataWriter
            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                },
            };
            var otherDataWriter = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(otherDataWriter);

            // Enable entities
            var result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = otherDataWriter.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            var found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            found = _reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Check the number of incompatible DataWriter
            Assert.IsTrue(evt.Wait(1_500));
            Assert.AreEqual(1, count);
            Assert.AreEqual(_reader, dr);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreEqual(11, lastPolicyId);
            Assert.IsNotNull(policies);
            Assert.AreEqual(1, policies.Count);
            Assert.AreEqual(1, policies.First().Count);
            Assert.AreEqual(11, policies.First().PolicyId);

            // Remove the listener to avoid extra messages
            result = _subscriber.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="SubscriberListener.OnSampleRejected(DataReader, SampleRejectedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnSampleRejected()
        {
            using var evt = new ManualResetEventSlim(false);

            var count = 0;
            DataReader dataReader = null;
            var totalCount = 0;
            var totalCountChange = 0;
            var lastInstanceHandle = InstanceHandle.HandleNil;
            var lastReason = SampleRejectedStatusKind.NotRejected;

            // Attach to the event
            _listener.SampleRejected += (r, s) =>
            {
                dataReader = r;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                lastInstanceHandle = s.LastInstanceHandle;
                lastReason = s.LastReason;

                count++;

                evt.Set();
            };

            // Prepare QoS for the test
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                },
                ResourceLimits =
                {
                    MaxInstances = 1,
                    MaxSamples = 1,
                    MaxSamplesPerInstance = 1,
                },
            };
            var result = _reader.SetQos(drQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            var found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            found = _reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Write two samples of the same instances
            for (var i = 1; i <= 2; i++)
            {
                result = _dataWriter.Write(new TestStruct
                {
                    Id = 1,
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Assert.IsTrue(evt.Wait(1_500));

            Assert.AreEqual(1, count);
            Assert.AreEqual(_reader, dataReader);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreNotEqual(InstanceHandle.HandleNil, lastInstanceHandle);
            Assert.AreEqual(SampleRejectedStatusKind.RejectedBySamplesPerInstanceLimit, lastReason);

            // Remove the listener to avoid extra messages
            result = _subscriber.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="SubscriberListener.OnLivelinessChanged(DataReader, LivelinessChangedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnLivelinessChanged()
        {
            var count = 0;

            using var evt1 = new ManualResetEventSlim(false);
            using var evt2 = new ManualResetEventSlim(false);

            DataReader firstDataReader = null;
            var firstAliveCount = 0;
            var firstAliveCountChange = 0;
            var firstNotAliveCount = 1;
            var firstNotAliveCountChange = 1;
            var firstLastPublicationHandle = InstanceHandle.HandleNil;

            DataReader secondDataReader = null;
            var secondAliveCount = 1;
            var secondAliveCountChange = 1;
            var secondNotAliveCount = 0;
            var secondNotAliveCountChange = 0;
            var secondLastPublicationHandle = InstanceHandle.HandleNil;

            // Attach to the event
            _listener.LivelinessChanged += (r, s) =>
            {
                Assert.AreEqual(_reader, r);

                if (count == 0)
                {
                    // Liveliness alive
                    firstDataReader = r;
                    firstAliveCount = s.AliveCount;
                    firstAliveCountChange = s.AliveCountChange;
                    firstNotAliveCount = s.NotAliveCount;
                    firstNotAliveCountChange = s.NotAliveCountChange;
                    firstLastPublicationHandle = s.LastPublicationHandle;

                    count++;

                    evt1.Set();
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

                    count++;

                    evt2.Set();
                }
            };

            // Prepare the QoS for the test
            var drQos = new DataReaderQos
            {
                Liveliness =
                {
                    LeaseDuration = new Duration { Seconds = 1 },
                },
            };
            var result = _reader.SetQos(drQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            var dwQos = new DataWriterQos
            {
                Liveliness =
                {
                    Kind = LivelinessQosPolicyKind.ManualByTopicLivelinessQos,
                    LeaseDuration = new Duration { Seconds = 1 },
                },
            };
            result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            var found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            found = _reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Assert liveliness in the writer
            result = _writer.AssertLiveliness();
            Assert.AreEqual(ReturnCode.Ok, result);

            // After half second liveliness should not be lost yet
            Assert.IsTrue(evt1.Wait(500));
            Assert.AreEqual(1, count);
            Assert.IsNotNull(firstDataReader);
            Assert.AreEqual(_reader, firstDataReader);
            Assert.AreEqual(1, firstAliveCount);
            Assert.AreEqual(1, firstAliveCountChange);
            Assert.AreEqual(0, firstNotAliveCount);
            Assert.AreEqual(0, firstNotAliveCountChange);
            Assert.AreEqual(_writer.InstanceHandle, firstLastPublicationHandle);

            // After one second and a half one liveliness should be lost
            Assert.IsTrue(evt2.Wait(1_500));
            Assert.AreEqual(2, count);
            Assert.IsNotNull(secondDataReader);
            Assert.AreEqual(_reader, secondDataReader);
            Assert.AreEqual(0, secondAliveCount);
            Assert.AreEqual(-1, secondAliveCountChange);
            Assert.AreEqual(1, secondNotAliveCount);
            Assert.AreEqual(1, secondNotAliveCountChange);
            Assert.AreEqual(_writer.InstanceHandle, secondLastPublicationHandle);

            // Remove the listener to avoid extra messages
            result = _subscriber.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);

            firstDataReader.DeleteContainedEntities();
            secondDataReader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(firstDataReader);
            _subscriber.DeleteDataReader(secondDataReader);
        }

        /// <summary>
        /// Test the <see cref="SubscriberListener.OnSubscriptionMatched(DataReader, SubscriptionMatchedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnSubscriptionMatched()
        {
            var count = 0;

            using var evt1 = new ManualResetEventSlim(false);
            using var evt2 = new ManualResetEventSlim(false);

            DataReader firstDataReader = null;
            var firstTotalCount = 0;
            var firstTotalCountChange = 0;
            var firstCurrentCount = 1;
            var firstCurrentCountChange = 1;
            var firstHandle = InstanceHandle.HandleNil;

            DataReader secondDataReader = null;
            var secondTotalCount = 1;
            var secondTotalCountChange = 1;
            var secondCurrentCount = 0;
            var secondCurrentCountChange = 0;
            var secondHandle = InstanceHandle.HandleNil;

            // Attach to the event
            _listener.SubscriptionMatched += (r, s) =>
            {
                if (count == 0)
                {
                    // Liveliness alive
                    firstDataReader = r;
                    firstTotalCount = s.TotalCount;
                    firstTotalCountChange = s.TotalCountChange;
                    firstCurrentCount = s.CurrentCount;
                    firstCurrentCountChange = s.CurrentCountChange;
                    firstHandle = s.LastPublicationHandle;

                    count++;

                    evt1.Set();
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

                    count++;

                    evt2.Set();
                }
            };

            // Enable entities
            var result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            var found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            found = _reader.WaitForPublications(1, 1000);
            Assert.IsTrue(found);

            // Check subscription matched call
            Assert.IsTrue(evt1.Wait(1_500));
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
            Assert.IsTrue(evt2.Wait(1_500));
            Assert.AreEqual(2, count);
            Assert.AreEqual(_reader, secondDataReader);
            Assert.AreEqual(1, secondTotalCount);
            Assert.AreEqual(0, secondTotalCountChange);
            Assert.AreEqual(0, secondCurrentCount);
            Assert.AreEqual(-1, secondCurrentCountChange);
            Assert.AreEqual(_writer.InstanceHandle, secondHandle);

            // Remove the listener to avoid extra messages
            result = _subscriber.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);

            firstDataReader.DeleteContainedEntities();
            secondDataReader.DeleteContainedEntities();
            _subscriber.DeleteDataReader(firstDataReader);
            _subscriber.DeleteDataReader(secondDataReader);
        }

        /// <summary>
        /// Test the <see cref="SubscriberListener.OnSampleLost(DataReader, SampleLostStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnSampleLost()
        {
            using var evt = new ManualResetEventSlim(false);

            DataReader reader = null;
            var count = 0;
            var totalCount = 0;
            var totalCountChange = 0;

            // Attach to the event
            _listener.SampleLost += (r, s) =>
            {
                reader = r;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;

                count++;

                evt.Set();
            };

            // Prepare QoS for the test
            var drQos = new DataReaderQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                },
                DestinationOrder =
                {
                    Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos,
                },
                History =
                {
                    Kind = HistoryQosPolicyKind.KeepLastHistoryQos,
                    Depth = 1,
                },
            };
            var result = _reader.SetQos(drQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            Assert.IsTrue(_reader.WaitForPublications(1, 5_000));
            Assert.IsTrue(_writer.WaitForSubscriptions(1, 5_000));

            // Write two samples of the same instances
            var handle = _dataWriter.RegisterInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            var time = DateTime.Now.ToTimestamp();
            result = _dataWriter.Write(new TestStruct { Id = 1 }, handle, time);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsFalse(evt.Wait(500));

            time = DateTime.Now.Subtract(TimeSpan.FromSeconds(10)).ToTimestamp();
            result = _dataWriter.Write(new TestStruct { Id = 1 }, handle, time);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.IsTrue(evt.Wait(20_000));
            Assert.AreEqual(1, count);
            Assert.AreEqual(_reader, reader);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);

            // Remove the listener to avoid extra messages
            result = _subscriber.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }
        #endregion
    }
}
