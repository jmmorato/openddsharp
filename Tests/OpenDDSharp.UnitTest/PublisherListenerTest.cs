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
    /// <see cref="PublisherListener"/> unit test class.
    /// </summary>
    [TestClass]
    public class PublisherListenerTest
    {
        #region Constants
        private const string TEST_CATEGORY = "PublisherListener";
        #endregion

        #region Fields
        private DomainParticipant _participant;
        private Topic _topic;
        private Subscriber _subscriber;
        private Publisher _publisher;
        private DataWriter _writer;
        private TestStructDataWriter _dataWriter;
        private MyPublisherListener _listener;
        private DataReader _reader;
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
            _subscriber = _participant.CreateSubscriber(sQos);
            Assert.IsNotNull(_subscriber);

            _listener = new MyPublisherListener();
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
            _publisher = _participant.CreatePublisher(pQos, _listener);
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
        /// Test the <see cref="PublisherListener.OnOfferedDeadlineMissed(DataWriter, OfferedDeadlineMissedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnOfferedDeadlineMissed()
        {
            using var evt = new ManualResetEventSlim(false);

            DataWriter writer = null;
            var totalCount = 0;
            var totalCountChange = 0;
            var lastInstanceHandle = InstanceHandle.HandleNil;

            // Attach to the event
            var count = 0;
            _listener.OfferedDeadlineMissed += (w, s) =>
            {
                writer = w;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                lastInstanceHandle = s.LastInstanceHandle;

                count++;

                evt.Set();
            };

            // Prepare QoS for the test
            var dwQos = new DataWriterQos
            {
                Deadline =
                {
                    Period = new Duration { Seconds = 1 },
                },
            };
            var result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery and write an instance
            var found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            var instance = new TestStruct { Id = 1 };
            var instanceHandle = _dataWriter.RegisterInstance(instance);
            _dataWriter.Write(instance, instanceHandle);

            // After half second deadline should not be lost yet
            Assert.IsFalse(evt.Wait(500));
            Assert.AreEqual(0, count);

            // After one second and a half one deadline should be lost
            Assert.IsTrue(evt.Wait(1_500));
            Assert.AreEqual(1, count);
            Assert.AreEqual(_writer, writer);
            Assert.AreEqual(lastInstanceHandle, instanceHandle);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);

            // Remove the listener to avoid extra messages
            foreach (var d in _listener.OfferedDeadlineMissed.GetInvocationList())
            {
                var del = (Action<DataWriter, OfferedDeadlineMissedStatus>)d;
                _listener.OfferedDeadlineMissed -= del;
            }
            result = _publisher.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="PublisherListener.OnOfferedIncompatibleQos(DataWriter, OfferedIncompatibleQosStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnOfferedIncompatibleQos()
        {
            using var evt = new ManualResetEventSlim(false);

            DataWriter dw = null;
            var totalCount = 0;
            var totalCountChange = 0;
            var lastPolicyId = 0;
            ICollection<QosPolicyCount> policies = new List<QosPolicyCount>();

            // Attach to the event
            var count = 0;
            _listener.OfferedIncompatibleQos += (w, s) =>
            {
                dw = w;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                lastPolicyId = s.LastPolicyId;
                policies = s.Policies;

                count++;

                evt.Set();
            };

            // Prepare QoS for the test
            var dwQos = new DataWriterQos
            {
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                },
            };
            var result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            var drQos = new DataReaderQos
            {
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
            Assert.IsTrue(evt.Wait(1_000));
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
            foreach (var d in _listener.OfferedIncompatibleQos.GetInvocationList())
            {
                var del = (Action<DataWriter, OfferedIncompatibleQosStatus>)d;
                _listener.OfferedIncompatibleQos -= del;
            }
            result = _publisher.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="PublisherListener.OnLivelinessLost(DataWriter, LivelinessLostStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnLivelinessLost()
        {
            using var evt = new ManualResetEventSlim(false);

            DataWriter dw = null;
            var totalCount = 0;
            var totalCountChange = 0;

            // Attach to the event
            var count = 0;
            _listener.LivelinessLost += (w, s) =>
            {
                dw = w;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;

                count++;

                evt.Set();
            };

            // Prepare QoS for the test
            var dwQos = new DataWriterQos
            {
                Liveliness =
                {
                    Kind = LivelinessQosPolicyKind.ManualByTopicLivelinessQos,
                    LeaseDuration = new Duration { Seconds = 1 },
                },
            };
            var result = _writer.SetQos(dwQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Enable entities
            result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // After half second liveliness should not be lost yet
            Assert.IsFalse(evt.Wait(500));
            Assert.AreEqual(0, count);

            // After one second and a half one liveliness should be lost
            Assert.IsTrue(evt.Wait(1_500));
            Assert.AreEqual(1, count);
            Assert.AreEqual(_writer, dw);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);

            // Remove the listener to avoid extra messages
            foreach (var d in _listener.LivelinessLost.GetInvocationList())
            {
                var del = (Action<DataWriter, LivelinessLostStatus>)d;
                _listener.LivelinessLost -= del;
            }
            result = _publisher.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="PublisherListener.OnPublicationMatched(DataWriter, PublicationMatchedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnPublicationMatched()
        {
            using var evt = new ManualResetEventSlim(false);

            DataWriter dw = null;
            var currentCount = 0;
            var currentCountChange = 0;
            var totalCount = 0;
            var totalCountChange = 0;
            var handle = InstanceHandle.HandleNil;

            // Attach to the event
            var count = 0;
            _listener.PublicationMatched += (w, s) =>
            {
                dw = w;
                currentCount = s.CurrentCount;
                currentCountChange = s.CurrentCountChange;
                totalCount = s.TotalCount;
                totalCountChange = s.TotalCountChange;
                handle = s.LastSubscriptionHandle;

                count++;

                evt.Set();
            };

            // Enable entities
            var result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            var found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            Assert.IsTrue(evt.Wait(1_000));
            Assert.AreEqual(1, count);
            Assert.AreEqual(_writer, dw);
            Assert.AreEqual(1, currentCount);
            Assert.AreEqual(1, currentCountChange);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreEqual(_reader.InstanceHandle, handle);

            // Remove the listener to avoid extra messages
            foreach (var d in _listener.PublicationMatched.GetInvocationList())
            {
                var del = (Action<DataWriter, PublicationMatchedStatus>)d;
                _listener.PublicationMatched -= del;
            }
            result = _publisher.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);
        }
        #endregion
    }
}
