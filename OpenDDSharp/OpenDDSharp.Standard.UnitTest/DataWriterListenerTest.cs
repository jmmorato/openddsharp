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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.Standard.UnitTest.Helpers;
using OpenDDSharp.Standard.UnitTest.Listeners;
using Test;

namespace OpenDDSharp.Standard.UnitTest
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
            sQos.Presentation.OrderedAccess = true;
            sQos.Presentation.CoherentAccess = true;
            sQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.InstancePresentationQos;
            _subscriber = _participant.CreateSubscriber(sQos);
            Assert.IsNotNull(_subscriber);

            PublisherQos pQos = new PublisherQos();
            pQos.EntityFactory.AutoenableCreatedEntities = false;
            pQos.Presentation.OrderedAccess = true;
            pQos.Presentation.CoherentAccess = true;
            pQos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.InstancePresentationQos;
            _publisher = _participant.CreatePublisher(pQos);
            Assert.IsNotNull(_publisher);

            _listener = new MyDataWriterListener();
            _writer = _publisher.CreateDataWriter(_topic, null, _listener);
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
        /// <summary>
        /// Test the <see cref="DataWriterListener.OnOfferedDeadlineMissed(DataWriter, OfferedDeadlineMissedStatus)" /> event.
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
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one deadline should be lost
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(1, count);
            Assert.AreEqual(_writer, writer);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, totalCountChange);

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
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one liveliness should be lost
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(1, count);
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
