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
using System.Collections.Generic;
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class DataWriterTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
        private DomainParticipant _participant;
        private Topic _topic;
        private Publisher _publisher;
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

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            _topic = _participant.CreateTopic(TestContext.TestName, typeName);
            Assert.IsNotNull(_topic);
            Assert.IsNull(_topic.GetListener());
            Assert.AreEqual(TestContext.TestName, _topic.Name);
            Assert.AreEqual(typeName, _topic.TypeName);

            _publisher = _participant.CreatePublisher();
            Assert.IsNotNull(_publisher);
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
        [TestCategory("DataWriter")]
        public void TestProperties()
        {            
            // Create a DataWriter and check the Topic and Participant properties
            DataWriter writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            Assert.AreEqual(_topic, writer.Topic);
            Assert.AreEqual(_publisher, writer.Publisher);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a datawriter with it
            DataWriterQos qos = TestHelper.CreateNonDefaultDataWriterQos();

            DataWriter dataWriter = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(dataWriter);

            // Call GetQos and check the values received
            qos = new DataWriterQos();
            ReturnCode result = dataWriter.GetQos(qos);
            TestHelper.TestNonDefaultDataWriterQos(qos);

            // Test GetQos with null parameter
            result = dataWriter.GetQos(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestSetQos()
        {
            // Create a new DataWriter using the default QoS
            DataWriter dataWriter = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(dataWriter);

            // Get the qos to ensure that is using the default properties
            DataWriterQos qos = new DataWriterQos();
            ReturnCode result = dataWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestHelper.TestDefaultDataWriterQos(qos);

            // Try to change an immutable property
            qos.Ownership.Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos;
            result = dataWriter.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them
            qos = new DataWriterQos();
            qos.OwnershipStrength.Value = 100;

            result = dataWriter.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataWriterQos();
            result = dataWriter.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(100, qos.OwnershipStrength.Value);

            // Try to set immutable QoS properties before enable the datawriter
            PublisherQos pubQos = new PublisherQos();
            pubQos.EntityFactory.AutoenableCreatedEntities = false;
            result = _publisher.SetQos(pubQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            DataWriter otherDataWriter = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(otherDataWriter);

            qos = new DataWriterQos();
            qos.Ownership.Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos;
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
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetListener()
        {
            // Create a new DataWriter with a listener
            MyDataWriterListener listener = new MyDataWriterListener();
            DataWriter dataWriter = _publisher.CreateDataWriter(_topic, listener);
            Assert.IsNotNull(dataWriter);

            // Call to GetListener and check the listener received
            MyDataWriterListener received = (MyDataWriterListener)dataWriter.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestSetListener()
        {
            // Create a new DataWriter without listener
            DataWriter dataWriter = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(dataWriter);

            MyDataWriterListener listener = (MyDataWriterListener)dataWriter.GetListener();
            Assert.IsNull(listener);

            // Create a listener, set it and check that is correctly setted
            listener = new MyDataWriterListener();
            ReturnCode result = dataWriter.SetListener(listener, StatusMask.AllStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            MyDataWriterListener received = (MyDataWriterListener)dataWriter.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            // Remove the listener calling SetListener with null and check it
            result = dataWriter.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            received = (MyDataWriterListener)dataWriter.GetListener();
            Assert.IsNull(received);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestWaitForAcknowledgments()
        {
            // Initialize entities
            DataWriter writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            // Write some instances and wait for acknowledgments
            for (int i = 0; i < 10; i++)
            {
                ReturnCode result = dataWriter.Write(new TestStruct
                {
                    Id = i
                });
                Assert.AreEqual(ReturnCode.Ok, result);

                result = dataWriter.WaitForAcknowledgments(new Duration { Seconds = 5 });
                Assert.AreEqual(ReturnCode.Ok, result);
            }
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetLivelinessLostStatus()
        {
            // Initialize entities
            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Liveliness.Kind = LivelinessQosPolicyKind.ManualByTopicLivelinessQos;
            dwQos.Liveliness.LeaseDuration = new Duration
            {
                Seconds = 1
            };
            DataWriter writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // After half second liveliness should not be lost yet
            System.Threading.Thread.Sleep(500);
            
            LivelinessLostStatus status = new LivelinessLostStatus();
            ReturnCode result = writer.GetLivelinessLostStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);

            // After one second and a half one liveliness should be lost
            System.Threading.Thread.Sleep(1000);

            result = writer.GetLivelinessLostStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetOfferedDeadlineMissedStatus()
        {
            // Initialize entities
            DataWriterQos qos = new DataWriterQos();
            qos.Deadline.Period = new Duration
            {
                Seconds = 1
            };
            DataWriter writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(reader);

            // Wait for discovery and write an instance
            bool found = writer.WaitForSubscriptions(1, 1000);
            Assert.IsTrue(found);

            dataWriter.Write(new TestStruct
            {
                Id = 1
            });

            // After half second deadline should not be lost yet
            System.Threading.Thread.Sleep(500);

            OfferedDeadlineMissedStatus status = new OfferedDeadlineMissedStatus();
            ReturnCode result = writer.GetOfferedDeadlineMissedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);

            // After one second and a half one deadline should be lost
            System.Threading.Thread.Sleep(1000);

            result = writer.GetOfferedDeadlineMissedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreNotEqual(InstanceHandle.HandleNil, status.LastInstanceHandle);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetOfferedIncompatibleQosStatus()
        {
            // Initialize entities
            DataWriterQos qos = new DataWriterQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            DataWriter writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);

            // If not matched readers should retur the default status
            OfferedIncompatibleQosStatus status = new OfferedIncompatibleQosStatus();
            ReturnCode result = writer.GetOfferedIncompatibleQosStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.IsNotNull(status.Policies);
            Assert.AreEqual(0, status.Policies.Count());
            Assert.AreEqual(0, status.LastPolicyId);

            // Create a not compatible reader
            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            // Wait for discovery and check the status
            System.Threading.Thread.Sleep(100);

            status = new OfferedIncompatibleQosStatus();
            result = writer.GetOfferedIncompatibleQosStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreEqual(11, status.LastPolicyId);
            Assert.IsNotNull(status.Policies);
            Assert.AreEqual(1, status.Policies.Count());
            Assert.AreEqual(1, status.Policies.First().Count);
            Assert.AreEqual(11, status.Policies.First().PolicyId);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetPublicationMatchedStatus()
        {
            // Initialize entities
            DataWriterQos qos = new DataWriterQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            DataWriter writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);

            // If not datareaders are created should return the default status
            PublicationMatchedStatus status = new PublicationMatchedStatus();
            ReturnCode result = writer.GetPublicationMatchedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.CurrentCount);
            Assert.AreEqual(0, status.CurrentCountChange);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastSubscriptionHandle);

            // Create a not compatible reader
            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            // Wait for discovery and check the status
            System.Threading.Thread.Sleep(100);

            result = writer.GetPublicationMatchedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.CurrentCount);
            Assert.AreEqual(0, status.CurrentCountChange);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
            Assert.AreEqual(InstanceHandle.HandleNil, status.LastSubscriptionHandle);

            // Create a compatible reader
            DataReader otherReader = subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(otherReader);

            // Wait for discovery and check the status
            System.Threading.Thread.Sleep(100);

            result = writer.GetPublicationMatchedStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, status.CurrentCount);
            Assert.AreEqual(1, status.CurrentCountChange);
            Assert.AreEqual(1, status.TotalCount);
            Assert.AreEqual(1, status.TotalCountChange);
            Assert.AreEqual(otherReader.InstanceHandle, status.LastSubscriptionHandle);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestAssertLiveliness()
        {
            // Initialize entities
            DataWriterQos qos = new DataWriterQos();
            qos.Liveliness.Kind = LivelinessQosPolicyKind.ManualByTopicLivelinessQos;
            qos.Liveliness.LeaseDuration = new Duration
            {
                Seconds = 1
            };
            DataWriter writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);

            // Manually assert liveliness
            for (int i = 0; i < 5; i++)
            {
                ReturnCode assertResult = writer.AssertLiveliness();
                Assert.AreEqual(ReturnCode.Ok, assertResult);
                System.Threading.Thread.Sleep(500);
            }

            // Check that no liveliness has been lost
            LivelinessLostStatus status = new LivelinessLostStatus();
            ReturnCode result = writer.GetLivelinessLostStatus(ref status);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, status.TotalCount);
            Assert.AreEqual(0, status.TotalCountChange);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetMatchedSubscriptions()
        {
            // Initialize entities  
            DataWriterQos qos = new DataWriterQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            DataWriter writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);

            // Test matched subscriptions without any match
            List<InstanceHandle> list = new List<InstanceHandle>
            {
                InstanceHandle.HandleNil
            };
            ReturnCode result = writer.GetMatchedSubscriptions(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, list.Count);

            // Create a not compatible reader
            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = subscriber.CreateDataReader(_topic, drQos);
            Assert.IsNotNull(reader);

            // Wait for discovery and check the matched subscriptions
            System.Threading.Thread.Sleep(100);

            result = writer.GetMatchedSubscriptions(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, list.Count);

            // Create a compatible reader
            DataReader otherReader = subscriber.CreateDataReader(_topic);
            Assert.IsNotNull(otherReader);

            // Wait for discovery and check the matched subscriptions
            System.Threading.Thread.Sleep(100);

            result = writer.GetMatchedSubscriptions(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(otherReader.InstanceHandle, list.First());

            // Test with null parameter
            result = writer.GetMatchedSubscriptions(null);
            Assert.AreEqual(ReturnCode.BadParameter, result);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetMatchedSubscriptionData()
        {
            // Initialize entities
            DataWriterQos dwQos = TestHelper.CreateNonDefaultDataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = _publisher.CreateDataWriter(_topic, dwQos);
            Assert.IsNotNull(writer);

            // DCPSInfoRepo-based discovery generates Built-In Topic data once (inside the
            // info repo process) and therefore all known entities in the domain are
            // reflected in the Built-In Topics.  RTPS discovery, on the other hand, follows
            // the DDS specification and omits "local" entities from the Built-In Topics.
            // The definition of "local" means those entities belonging to the same Domain
            // Participant as the given Built-In Topic Subscriber.
            // https://github.com/objectcomputing/OpenDDS/blob/master/docs/design/RTPS

            // OPENDDS ISSUE: GetMatchedSubscriptions returns local entities but GetMatchedSubscriptionData doesn't
            // because is looking in the Built-in topic. If not found in the built-in, shouldn't try to look locally?
            // WORKAROUND: Create another particpant for the DataReader.
            DomainParticipant otherParticipant = _dpf.CreateParticipant(DOMAIN_ID);
            Assert.IsNotNull(otherParticipant);

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic otherTopic = otherParticipant.CreateTopic(nameof(TestGetMatchedSubscriptionData), typeName);
            Assert.IsNotNull(otherTopic);

            Subscriber subscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = TestHelper.CreateNonDefaultDataReaderQos();            
            DataReader reader = subscriber.CreateDataReader(otherTopic, drQos);
            Assert.IsNotNull(reader);

            // Wait for subscriptions
            bool found = writer.WaitForSubscriptions(1, 5000);
            Assert.IsTrue(found);

            // Get the matched subscriptions
            List<InstanceHandle> list = new List<InstanceHandle>();
            result = writer.GetMatchedSubscriptions(list);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, list.Count);

            // Get the matched subscription data
            SubscriptionBuiltinTopicData data = new SubscriptionBuiltinTopicData();
            result = writer.GetMatchedSubscriptionData(list.First(), ref data);
            TestHelper.TestNonDefaultSubscriptionData(data);
            //Assert.AreEqual(ReturnCode.Ok, result);
            //Assert.IsNotNull(data.UserData);
            //Assert.IsNotNull(data.UserData.Value);
            //Assert.AreEqual(1, data.UserData.Value.Count());
            //Assert.AreEqual(0x42, data.UserData.Value.First());
            //Assert.IsNotNull(data.Key);
            //Assert.IsNotNull(data.Key.Value);

            // Destroy the other participant
            result = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _dpf.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestRegisterInstance()
        {
            // Initialize entities
            DataWriter writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Register an instance
            InstanceHandle handle = dataWriter.RegisterInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            // Register an instance with timestamp
            InstanceHandle otherHandle = dataWriter.RegisterInstance(new TestStruct { Id = 2 }, DateTime.Now.ToTimestamp());
            Assert.AreNotEqual(InstanceHandle.HandleNil, otherHandle);
            Assert.AreNotEqual(handle, otherHandle);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestUnregisterInstance()
        {
            // Initialize entities
            DataWriter writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Unregister not registered instance
            ReturnCode result = dataWriter.UnregisterInstance(new TestStruct { Id = 1 });
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Register an instance
            TestStruct instance1 = new TestStruct { Id = 1 };
            InstanceHandle handle1 = dataWriter.RegisterInstance(instance1);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle1);

            // Unregister the previous registered instance with the simplest overload
            result = dataWriter.UnregisterInstance(new TestStruct { Id = 1 });
            Assert.AreEqual(ReturnCode.Ok, result);

            // Register a new instance
            TestStruct instance2 = new TestStruct { Id = 2 };
            InstanceHandle handle2 = dataWriter.RegisterInstance(instance2);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle2);

            // Unregister the previous registered instance with the handle
            result = dataWriter.UnregisterInstance(new TestStruct { Id = 2 }, handle2);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Register a new instance
            TestStruct instance3 = new TestStruct { Id = 3 };
            InstanceHandle handle3 = dataWriter.RegisterInstance(instance3);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle3);

            // Unregister the previous registered instance with the handle and the timestamp
            result = dataWriter.UnregisterInstance(new TestStruct { Id = 3 }, handle3, DateTime.Now.ToTimestamp());
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestWrite()
        {
            // Initialize entities
            Duration duration = new Duration { Seconds = 5 };

            DataWriter writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);
            
            DataReaderQos qos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            MyDataReaderListener listener = new MyDataReaderListener();
            DataReader dataReader = subscriber.CreateDataReader(_topic, qos, listener);
            int count = 0;
            Timestamp timestamp = new Timestamp();
            listener.DataAvailable += (reader) =>
            {
                count++;
                if (count == 4)
                {
                    TestStructDataReader dr = new TestStructDataReader(reader);

                    List<TestStruct> samples = new List<TestStruct>();
                    List<SampleInfo> infos = new List<SampleInfo>();

                    InstanceHandle h = dr.LookupInstance(new TestStruct { Id = count });
                    Assert.AreNotEqual(InstanceHandle.HandleNil, h);

                    ReturnCode ret = dr.ReadInstance(samples, infos, h);
                    Assert.AreEqual(ReturnCode.Ok, ret);
                    Assert.IsNotNull(infos.Count);
                    Assert.AreEqual(1, infos.Count);
                    timestamp = infos.First().SourceTimestamp;                    
                }
            };

            // Wait for discovery
            writer.WaitForSubscriptions(1, 1000);

            // Write an instance with the simplest overload
            ReturnCode result = dataWriter.Write(new TestStruct { Id = 1 });
            Assert.AreEqual(ReturnCode.Ok, result);
            
            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(10);
            Assert.AreEqual(1, count);

            // Write an instance with the handle parameter as HandleNil
            result = dataWriter.Write(new TestStruct { Id = 2 }, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(10);
            Assert.AreEqual(2, count);

            // Write an instance with the handle parameter with a previously registered instance
            TestStruct instance = new TestStruct { Id = 3 };
            InstanceHandle handle = dataWriter.RegisterInstance(instance);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            result = dataWriter.Write(instance, handle);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(10);
            Assert.AreEqual(3, count);

            // Write an instance with the handle parameter and the timestamp
            Timestamp now = DateTime.Now.ToTimestamp();
            TestStruct instance1 = new TestStruct { Id = 4 };
            InstanceHandle handle1 = dataWriter.RegisterInstance(instance1);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle1);

            result = dataWriter.Write(instance1, handle1, now);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(10);
            Assert.AreEqual(4, count);
            Assert.AreEqual(now.Seconds, timestamp.Seconds);
            Assert.AreEqual(now.NanoSeconds, timestamp.NanoSeconds);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestDispose()
        {
            // Initialize entities
            Duration duration = new Duration { Seconds = 5 };

            DataWriterQos qos = new DataWriterQos();
            qos.WriterDataLifecycle.AutodisposeUnregisteredInstances = false;            
            DataWriter writer = _publisher.CreateDataWriter(_topic, qos);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = new DataReaderQos();
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            MyDataReaderListener listener = new MyDataReaderListener();
            DataReader dataReader = subscriber.CreateDataReader(_topic, drQos, listener);
            int count = 0;
            Timestamp timestamp = new Timestamp();
            listener.DataAvailable += (reader) =>
            {
                List<TestStruct> samples = new List<TestStruct>();
                List<SampleInfo> infos = new List<SampleInfo>();
                TestStructDataReader dr = new TestStructDataReader(reader);
                ReturnCode ret = dr.Take(samples, infos);
                if (ret == ReturnCode.Ok)
                {
                    foreach (var info in infos)
                    {
                        if (info.InstanceState == InstanceStateKind.NotAliveDisposedInstanceState)
                        {
                            count++;
                            if (count == 3)
                            {
                                timestamp = infos.First().SourceTimestamp;
                            }
                        }
                    }
                }
            };

            // Wait for discovery
            writer.WaitForSubscriptions(1, 1000);

            // Dispose an instance that does not exist
            ReturnCode result = dataWriter.Dispose(new TestStruct { Id = 1 }, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.Error, result);

            // Call dispose with the simplest overload
            TestStruct instance1 = new TestStruct { Id = 1 };
            result = dataWriter.Write(instance1);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(0, count);

            result = dataWriter.Dispose(instance1);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, count);

            // Call dispose with the handle parameter
            TestStruct instance2 = new TestStruct { Id = 2 };
            InstanceHandle handle2 = dataWriter.RegisterInstance(instance2);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle2);

            result = dataWriter.Write(instance2, handle2);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(1, count);

            result = dataWriter.Dispose(instance2, handle2);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(2, count);

            // Call dispose with the handle parameter and specific timestamp 
            Timestamp now = DateTime.Now.ToTimestamp();
            TestStruct instance3 = new TestStruct { Id = 3 };
            InstanceHandle handle3 = dataWriter.RegisterInstance(instance3);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle3);

            result = dataWriter.Write(instance3, handle3);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(2, count);

            result = dataWriter.Dispose(instance3, handle3, now);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = dataWriter.WaitForAcknowledgments(duration);
            Assert.AreEqual(ReturnCode.Ok, result);

            System.Threading.Thread.Sleep(100);
            Assert.AreEqual(3, count);
            Assert.AreEqual(now.Seconds, timestamp.Seconds);
            Assert.AreEqual(now.NanoSeconds, timestamp.NanoSeconds);
        }        

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestGetKeyValue()
        {
            // Initialize entities
            DataWriter writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Call GetKeyValue with HandleNil
            TestStruct data = new TestStruct();
            ReturnCode result = dataWriter.GetKeyValue(data, InstanceHandle.HandleNil);
            Assert.AreEqual(ReturnCode.BadParameter, result);

            // Register an instance
            TestStruct instance = new TestStruct { Id = 1 };
            InstanceHandle handle = dataWriter.RegisterInstance(instance);
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);

            // Call GetKeyValue
            data = new TestStruct();
            result = dataWriter.GetKeyValue(data, handle);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, data.Id);
        }

        [TestMethod]
        [TestCategory("DataWriter")]
        public void TestLookupInstance()
        {
            // Initialize entities
            DataWriter writer = _publisher.CreateDataWriter(_topic);
            Assert.IsNotNull(writer);
            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Lookup for a non-existing instance
            InstanceHandle handle = dataWriter.LookupInstance(new TestStruct { Id = 1 });
            Assert.AreEqual(InstanceHandle.HandleNil, handle);

            // Register an instance
            InstanceHandle handle1 = dataWriter.RegisterInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle1);

            // Lookup for an existing instance
            handle = dataWriter.LookupInstance(new TestStruct { Id = 1 });
            Assert.AreNotEqual(InstanceHandle.HandleNil, handle);
            Assert.AreEqual(handle1, handle);
        }
        #endregion
    }
}
