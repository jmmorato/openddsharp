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
using System.Collections.Generic;
using System.Linq;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="DataWriterListener"/> unit test class.
    /// </summary>
    [TestClass]
    public class DataWriterListenerTest
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
        private MyDataWriterListener _listener;
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

            _listener = new MyDataWriterListener();
            _writer = _publisher.CreateDataWriter(_topic, null, _listener);
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
            _listener = null;

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
        /// Test the <see cref="DataWriterListener.OnOfferedDeadlineMissed(DataWriter, OfferedDeadlineMissedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnOfferedDeadlineMissed()
        {
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
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one deadline should be lost
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_writer, writer);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreEqual(instanceHandle, lastInstanceHandle);

            // Remove the listener to avoid extra messages
            result = _dataWriter.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DataWriterListener.OnOfferedIncompatibleQos(DataWriter, OfferedIncompatibleQosStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnOfferedIncompatibleQos()
        {
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
            System.Threading.Thread.Sleep(100);
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
            result = _dataWriter.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DataWriterListener.OnLivelinessLost(DataWriter, LivelinessLostStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        // [Ignore("It hangs in Windows. Looking for a solution...")]
        public void TestOnLivelinessLost()
        {
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

            // After half second liveliness should not be lost yet
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one liveliness should be lost
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_writer, dw);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);

            // Remove the listener to avoid extra messages
            result = _dataWriter.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        /// <summary>
        /// Test the <see cref="DataWriterListener.OnPublicationMatched(DataWriter, PublicationMatchedStatus)" /> event.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnPublicationMatched()
        {
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
            };

            // Enable entities
            var result = _writer.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _reader.Enable();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Wait for discovery
            var found = _writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_writer, dw);
            Assert.AreEqual(1, currentCount);
            Assert.AreEqual(1, currentCountChange);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);
            Assert.AreEqual(_reader.InstanceHandle, handle);

            // Remove the listener to avoid extra messages
            result = _dataWriter.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }
        #endregion
    }
}
