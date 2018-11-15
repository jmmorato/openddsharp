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
    public class DomainParticipantListenerTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;
        private const string TEST_CATEGORY = "DomainParticipantListener";
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
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
        public TestContext TestContext { get; set; }
        #endregion

        #region Initialization/Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _dpf = ParticipantService.Instance.GetDomainParticipantFactory(new string[] { "-DCPSDebugLevel", "10" });
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _listener = new MyParticipantListener();
            _participant = _dpf.CreateParticipant(DOMAIN_ID, _listener);
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

            _writer = _publisher.CreateDataWriter(_topic);
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
        public void TestOnDataOnReaders()
        {
            // Attach to the event
            int count = 0;
            _listener.DataOnReaders += (s) =>
            {
                Assert.AreEqual(_subscriber, s);
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

            // Write some instances
            for (int i = 1; i <= 5; i++)
            {
                result = _dataWriter.Write(new TestStruct
                {
                    Id = i
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(100);

            Assert.AreEqual(5, count);
        }

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
            _listener.DataAvailable += (r) =>
            {
                Assert.AreEqual(_reader, r);
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
            for (int i = 1; i <= 5; i++)
            {
                result = _dataWriter.Write(new TestStruct
                {
                    Id = i
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(100);

            Assert.AreEqual(5, count);
        }

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
            _listener.RequestedDeadlineMissed += (r, s) =>
            {
                Assert.AreEqual(_reader, r);
                Assert.AreEqual(1, s.TotalCount);
                Assert.AreEqual(1, s.TotalCountChange);
                Assert.AreNotEqual(InstanceHandle.HandleNil, s.LastInstanceHandle);
                count++;
            };

            // Write an instance
            _dataWriter.Write(new TestStruct { Id = 1 });

            // After half second deadline should not be lost yet
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one deadline should be lost
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnRequestedIncompatibleQos()
        {
            // Attach to the event
            int count = 0;
            _listener.RequestedIncompatibleQos += (r, s) =>
            {
                Assert.AreEqual(_reader, r);
                Assert.AreEqual(1, s.TotalCount);
                Assert.AreEqual(1, s.TotalCountChange);
                Assert.AreEqual(11, s.LastPolicyId);
                Assert.IsNotNull(s.Policies);
                Assert.AreEqual(1, s.Policies.Count());
                Assert.AreEqual(1, s.Policies.First().Count);
                Assert.AreEqual(11, s.Policies.First().PolicyId);
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
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnSampleRejected()
        {
            // Attach to the event
            int count = 0;
            _listener.SampleRejected += (r, s) =>
            {
                Assert.AreEqual(_reader, r);
                Assert.AreEqual(1, s.TotalCount);
                Assert.AreEqual(1, s.TotalCountChange);
                Assert.AreNotEqual(InstanceHandle.HandleNil, s.LastInstanceHandle);
                Assert.AreEqual(SampleRejectedStatusKind.RejectedBySamplesPerInstanceLimit, s.LastReason);

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
                    Id = 1
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = _dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, count);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnLivelinessChanged()
        {
            // Attach to the event
            int count = 0;
            _listener.LivelinessChanged += (r, s) =>
            {
                Assert.AreEqual(_reader, r);

                if (count == 0)
                {
                    // Liveliness alive
                    Assert.AreEqual(1, s.AliveCount);
                    Assert.AreEqual(1, s.AliveCountChange);
                    Assert.AreEqual(0, s.NotAliveCount);
                    Assert.AreEqual(0, s.NotAliveCountChange);
                    Assert.AreEqual(_writer.InstanceHandle, s.LastPublicationHandle);
                }
                else
                {
                    // Liveliness not alive
                    Assert.AreEqual(0, s.AliveCount);
                    Assert.AreEqual(-1, s.AliveCountChange);
                    Assert.AreEqual(1, s.NotAliveCount);
                    Assert.AreEqual(1, s.NotAliveCountChange);
                    Assert.AreEqual(_writer.InstanceHandle, s.LastPublicationHandle);
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
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(1, count);

            // After one second and a half one liveliness should be lost
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(2, count);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnSubscriptionMatched()
        {
            // Attach to the event
            int count = 0;
            _listener.SubscriptionMatched += (r, s) =>
            {
                Assert.AreEqual(_reader, r);

                if (count == 0)
                {
                    // Liveliness alive
                    Assert.AreEqual(1, s.TotalCount);
                    Assert.AreEqual(1, s.TotalCountChange);
                    Assert.AreEqual(1, s.CurrentCount);
                    Assert.AreEqual(1, s.CurrentCountChange);
                    Assert.AreEqual(_writer.InstanceHandle, s.LastPublicationHandle);
                }
                else
                {
                    // Liveliness not alive
                    Assert.AreEqual(1, s.TotalCount);
                    Assert.AreEqual(0, s.TotalCountChange);
                    Assert.AreEqual(0, s.CurrentCount);
                    Assert.AreEqual(-1, s.CurrentCountChange);
                    Assert.AreEqual(_writer.InstanceHandle, s.LastPublicationHandle);
                }

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

            // Check subscription matched call
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(1, count);

            // Delete the writer
            result = _publisher.DeleteDataWriter(_writer);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Check subscription matched call
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(2, count);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnSampleLost()
        {
            // Attach to the event
            int count = 0;
            _listener.SampleLost += (r, s) =>
            {
                Assert.AreEqual(_reader, r);
                Assert.AreEqual(1, s.TotalCount);
                Assert.AreEqual(1, s.TotalCountChange);

                count++;
            };

            // Prepare QoS for the test
            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            drQos.DestinationOrder.Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos;
            drQos.History.Kind = HistoryQosPolicyKind.KeepLastHistoryQos;
            drQos.History.Depth = 1;
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
            InstanceHandle handle = _dataWriter.RegisterInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            Timestamp time = DateTime.Now.ToTimestamp();
            result = _dataWriter.Write(new TestStruct { Id = 1 }, handle, time);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);

            time = DateTime.Now.Subtract(TimeSpan.FromSeconds(10)).ToTimestamp();
            result = _dataWriter.Write(new TestStruct { Id = 1 }, handle, time);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, count);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

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
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one deadline should be lost
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(1, count);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
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
            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, count);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
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
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(0, count);

            // After one second and a half one liveliness should be lost
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(1, count);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
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

            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, count);

            // Remove the listener to avoid extra messages
            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestOnInconsistentTopic()
        {
            ManualResetEventSlim evt = new ManualResetEventSlim(false);

            // Attach to the event
            int count = 0;
            _listener.InconsistentTopic += (t, s) =>
            {
                Assert.AreEqual(_topic, t);
                Assert.AreEqual(1, s.TotalCount);
                Assert.AreEqual(1, s.TotalCountChange);
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
