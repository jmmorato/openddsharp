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
using System.Threading;
using System.Diagnostics;
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Helpers;
using OpenDDSharp.UnitTest.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class DataWriterListenerTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;
        private const string TEST_CATEGORY = "DataWriterListener";
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
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
        public TestContext TestContext { get; set; }
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

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);
            Assert.IsNull(_topic.GetListener());
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
            _writer = _publisher.CreateDataWriter(_topic, _listener);
            Assert.IsNotNull(_writer);
            _dataWriter = new TestStructDataWriter(_writer);
            
            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            _reader = _subscriber.CreateDataReader(_topic, qos);
            Assert.IsNotNull(_reader);
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
        [TestCategory(TEST_CATEGORY)]
        public void TestOnOfferedDeadlineMissed()
        {
            // Attach to the event
            int count = 0;
            _listener.OfferedDeadlineMissed += (w, s) =>
            {
                Assert.AreEqual(_writer, w);
                Assert.AreEqual(1, s.TotalCount);
                Assert.AreEqual(1, s.TotalCountChange);
                Assert.AreNotEqual(InstanceHandle.HandleNil, s.LastInstanceHandle);
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

            // Remove the listener to avoid extra messages
            result = _writer.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnOfferedIncompatibleQos()
        {
            // Attach to the event
            int count = 0;
            _listener.OfferedIncompatibleQos += (w, s) =>
            {
                Assert.AreEqual(_writer, w);
                Assert.AreEqual(1, s.TotalCount);
                Assert.AreEqual(1, s.TotalCountChange);
                Assert.AreEqual(11, s.LastPolicyId);
                Assert.IsNotNull(s.Policies);
                Assert.AreEqual(1, s.Policies.Count());
                Assert.AreEqual(1, s.Policies.First().Count);
                Assert.AreEqual(11, s.Policies.First().PolicyId);

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
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnLivelinessLost()
        {
            // Attach to the event
            int count = 0;
            _listener.LivelinessLost += (w, s) =>
            {
                Assert.AreEqual(_writer, w);
                Assert.AreEqual(1, s.TotalCount);
                Assert.AreEqual(1, s.TotalCountChange);                
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
            result = _writer.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnPublicationMatched()
        {
            // Attach to the event
            int count = 0;
            _listener.PublicationMatched += (w, s) =>
            {
                Assert.AreEqual(_writer, w);
                Assert.AreEqual(1, s.CurrentCount);
                Assert.AreEqual(1, s.CurrentCountChange);
                Assert.AreEqual(1, s.TotalCount);
                Assert.AreEqual(1, s.TotalCountChange);
                Assert.AreEqual(_reader.InstanceHandle, s.LastSubscriptionHandle);
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

            // Remove the listener to avoid extra messages
            result = _writer.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnPublicationDisconnected()
        {
            ManualResetEventSlim evt = new ManualResetEventSlim(false);
            DomainParticipant domainParticipant = _dpf.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(domainParticipant);
            domainParticipant.BindTcpTransportConfig();

            Publisher publisher = domainParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(domainParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = domainParticipant.CreateTopic("TestOnPublicationLostDisconnected", typeName);
            Assert.IsNotNull(topic);

            MyDataWriterListener listener = new MyDataWriterListener();
            DataWriter writer = publisher.CreateDataWriter(topic, listener);

            int count = 0;
            listener.PublicationDisconnected += (w, s) =>
            {
                Assert.AreEqual(writer, w);
                Assert.AreEqual(1, s.SubscriptionHandles.Count());
                Assert.AreNotEqual(InstanceHandle.HandleNil, s.SubscriptionHandles.First());
                count++;
                evt.Set();
            };

            SupportProcessHelper supportProcess = new SupportProcessHelper(TestContext);
            Process process = supportProcess.SpawnSupportProcess(SupportTestKind.PublicationDisconnectedTest);
            try
            {
                // Wait for discovery
                bool found = writer.WaitForSubscriptions(1, 5000);
                Assert.IsTrue(found);
            }
            finally
            {
                supportProcess.KillProcess(process);
            }

            bool resp = evt.Wait(10000);
            Assert.IsTrue(resp);
            Assert.AreEqual(1, count);

            // Remove the listener to avoid extra messages
            result = writer.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);

            domainParticipant.DeleteContainedEntities();
            _dpf.DeleteParticipant(domainParticipant);                       
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnPublicationReconnected()
        {
            // TODO: How to simulate reconnect?
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnPublicationLost()
        {
            ManualResetEventSlim evt = new ManualResetEventSlim(false);
            DomainParticipant domainParticipant = _dpf.CreateParticipant(AssemblyInitializer.INFOREPO_DOMAIN);
            Assert.IsNotNull(domainParticipant);
            domainParticipant.BindTcpTransportConfig();

            Publisher publisher = domainParticipant.CreatePublisher();
            Assert.IsNotNull(publisher);

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(domainParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = domainParticipant.CreateTopic("TestOnPublicationLostDisconnected", typeName);
            Assert.IsNotNull(topic);

            MyDataWriterListener listener = new MyDataWriterListener();
            DataWriter writer = publisher.CreateDataWriter(topic, listener);

            int count = 0;
            listener.PublicationLost += (w, s) =>
            {
                Assert.AreEqual(writer, w);
                Assert.AreEqual(1, s.SubscriptionHandles.Count());
                Assert.AreNotEqual(InstanceHandle.HandleNil, s.SubscriptionHandles.First());
                count++;
                evt.Set();
            };

            SupportProcessHelper supportProcess = new SupportProcessHelper(TestContext);
            Process process = supportProcess.SpawnSupportProcess(SupportTestKind.PublicationLostTest);
            try
            {
                // Wait for discovery
                bool found = writer.WaitForSubscriptions(1, 5000);
                Assert.IsTrue(found);
            }
            finally
            {
                supportProcess.KillProcess(process);
            }

            bool resp = evt.Wait(20000);
            Assert.IsTrue(resp);
            Assert.AreEqual(1, count);

            // Remove the listener to avoid extra messages
            result = writer.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);

            domainParticipant.DeleteContainedEntities();
            _dpf.DeleteParticipant(domainParticipant);

            evt.Dispose();
        }
        #endregion
    }
}
