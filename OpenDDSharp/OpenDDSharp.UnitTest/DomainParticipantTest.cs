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
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.Test;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class DomainParticipantTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;
        private const int OTHER_DOMAIN_ID = 23;
        #endregion

        #region Fields
        static DomainParticipantFactory _dpf;
        DomainParticipant _participant;
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
        [TestCategory("DomainParticipant")]
        public void TestDomainId()
        {
            Assert.IsNotNull(_participant);
            Assert.AreEqual(DOMAIN_ID, _participant.DomainId);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestNewParticipantQos()
        {
            DomainParticipantQos qos = new DomainParticipantQos();

            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.UserData);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(0, qos.UserData.Value.Count());
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetQos()
        {
            DomainParticipantQos qos = new DomainParticipantQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.UserData.Value = new List<byte> { 0x42 };

            ReturnCode result = _participant.GetQos(qos);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.UserData);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(0, qos.UserData.Value.Count());
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestSetQos()
        {
            // Creates a non-default QoS, set it an check it
            DomainParticipantQos qos = new DomainParticipantQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.UserData.Value = new List<byte> { 0x42 };

            ReturnCode result = _participant.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DomainParticipantQos();
            result = _participant.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.UserData);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(1, qos.UserData.Value.Count());
            Assert.AreEqual(0x42, qos.UserData.Value.First());

            // Put back the default QoS and check it
            qos = new DomainParticipantQos();
            result = _participant.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.UserData);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(0, qos.UserData.Value.Count());
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetListener()
        {
            DomainParticipantListener listener = _participant.GetListener();
            Assert.IsNull(listener);

            MyParticipantListener otherListener = new MyParticipantListener();
            DomainParticipant other = _dpf.CreateParticipant(OTHER_DOMAIN_ID, otherListener);
            Assert.IsNotNull(other);

            listener = other.GetListener();
            Assert.IsNotNull(listener);

            ReturnCode result = _dpf.DeleteParticipant(other);
            Assert.AreEqual(ReturnCode.Ok, result);            
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestSetListener()
        {
            DomainParticipantListener listener = _participant.GetListener();
            Assert.IsNull(listener);

            MyParticipantListener myListener = new MyParticipantListener();
            ReturnCode result = _participant.SetListener(myListener);
            Assert.AreEqual(ReturnCode.Ok, result);

            listener = _participant.GetListener();
            Assert.AreEqual(myListener, listener);

            result = _participant.SetListener(null);
            Assert.AreEqual(ReturnCode.Ok, result);

            listener = _participant.GetListener();
            Assert.IsNull(listener);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestDeleteContainedEntities()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteContainedEntities), typeName);
            Assert.IsNotNull(topic);
            Assert.IsTrue(_participant.ContainsEntity(topic.InstanceHandle));

            Publisher pub = _participant.CreatePublisher();
            Assert.IsNotNull(pub);
            Assert.IsTrue(_participant.ContainsEntity(pub.InstanceHandle));

            Subscriber sub = _participant.CreateSubscriber();            
            Assert.IsNotNull(sub);            
            Assert.IsTrue(_participant.ContainsEntity(sub.InstanceHandle));

            DataWriter dataWriter = pub.CreateDataWriter(topic);
            Assert.IsNotNull(dataWriter);
            Assert.IsTrue(_participant.ContainsEntity(dataWriter.InstanceHandle));

            DataReader dataReader = sub.CreateDataReader(topic);
            Assert.IsNotNull(dataReader);
            Assert.IsTrue(_participant.ContainsEntity(dataReader.InstanceHandle));

            result = _participant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestNewTopicQos()
        {
            TopicQos qos = new TopicQos();

            Assert.IsNotNull(qos.Deadline);
            Assert.IsNotNull(qos.DestinationOrder);
            Assert.IsNotNull(qos.Durability);
            Assert.IsNotNull(qos.DurabilityService);
            Assert.IsNotNull(qos.History);
            Assert.IsNotNull(qos.LatencyBudget);
            Assert.IsNotNull(qos.Lifespan);
            Assert.IsNotNull(qos.Liveliness);
            Assert.IsNotNull(qos.Ownership);
            Assert.IsNotNull(qos.Reliability);
            Assert.IsNotNull(qos.ResourceLimits);
            Assert.IsNotNull(qos.TopicData);
            Assert.IsNotNull(qos.TransportPriority);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.ByReceptionTimestampDestinationOrderQos, qos.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.VolatileDurabilityQos, qos.Durability.Kind);
            Assert.AreEqual(HistoryQosPolicyKind.KeepLastHistoryQos, qos.DurabilityService.HistoryKind);
            Assert.AreEqual(1, qos.DurabilityService.HistoryDepth);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.DurabilityService.MaxInstances);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.DurabilityService.MaxSamples);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.DurabilityService.MaxSamplesPerInstance);
            Assert.AreEqual(HistoryQosPolicyKind.KeepLastHistoryQos, qos.History.Kind);
            Assert.AreEqual(1, qos.History.Depth);
            Assert.AreEqual(Duration.ZeroSeconds, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Lifespan.Duration.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.AutomaticLivelinessQos, qos.Liveliness.Kind);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.SharedOwnershipQos, qos.Ownership.Kind);
            Assert.AreEqual(ReliabilityQosPolicyKind.BestEffortReliabilityQos, qos.Reliability.Kind);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(0, qos.TopicData.Value.Count());
            Assert.AreEqual(0, qos.TransportPriority.Value);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetDefaultTopicQos()
        {
            TopicQos qos = new TopicQos();
            qos.Deadline.Period = new Duration
            {
                Seconds = 5,
                NanoSeconds = 0
            };
            qos.DestinationOrder.Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos;
            qos.Durability.Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos;
            qos.DurabilityService.HistoryDepth = 5;
            qos.DurabilityService.HistoryKind = HistoryQosPolicyKind.KeepAllHistoryQos;
            qos.DurabilityService.MaxInstances = 5;
            qos.DurabilityService.MaxSamples = 5;
            qos.DurabilityService.MaxSamplesPerInstance = 5;
            qos.History.Depth = 5;
            qos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            qos.LatencyBudget.Duration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.Lifespan.Duration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.Liveliness.Kind = LivelinessQosPolicyKind.ManualByParticipantLivelinessQos;
            qos.Liveliness.LeaseDuration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.Ownership.Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos;
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            qos.Reliability.MaxBlockingTime = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.ResourceLimits.MaxInstances = 5;
            qos.ResourceLimits.MaxSamples = 5;
            qos.ResourceLimits.MaxSamplesPerInstance = 5;
            qos.TopicData.Value = new List<byte> { 0x5 };
            qos.TransportPriority.Value = 5;

            ReturnCode result = _participant.GetDefaultTopicQos(qos);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos);
            Assert.IsNotNull(qos.Deadline);
            Assert.IsNotNull(qos.DestinationOrder);
            Assert.IsNotNull(qos.Durability);
            Assert.IsNotNull(qos.DurabilityService);
            Assert.IsNotNull(qos.History);
            Assert.IsNotNull(qos.LatencyBudget);
            Assert.IsNotNull(qos.Lifespan);
            Assert.IsNotNull(qos.Liveliness);
            Assert.IsNotNull(qos.Ownership);
            Assert.IsNotNull(qos.Reliability);
            Assert.IsNotNull(qos.ResourceLimits);
            Assert.IsNotNull(qos.TopicData);
            Assert.IsNotNull(qos.TransportPriority);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.ByReceptionTimestampDestinationOrderQos, qos.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.VolatileDurabilityQos, qos.Durability.Kind);
            Assert.AreEqual(HistoryQosPolicyKind.KeepLastHistoryQos, qos.DurabilityService.HistoryKind);
            Assert.AreEqual(1, qos.DurabilityService.HistoryDepth);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.DurabilityService.MaxInstances);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.DurabilityService.MaxSamples);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.DurabilityService.MaxSamplesPerInstance);
            Assert.AreEqual(HistoryQosPolicyKind.KeepLastHistoryQos, qos.History.Kind);
            Assert.AreEqual(1, qos.History.Depth);
            Assert.AreEqual(Duration.ZeroSeconds, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Lifespan.Duration.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.AutomaticLivelinessQos, qos.Liveliness.Kind);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.SharedOwnershipQos, qos.Ownership.Kind);
            Assert.AreEqual(ReliabilityQosPolicyKind.BestEffortReliabilityQos, qos.Reliability.Kind);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(0, qos.TopicData.Value.Count());
            Assert.AreEqual(0, qos.TransportPriority.Value);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestSetDefaultTopicQos()
        {
            TopicQos qos = new TopicQos();

            qos.Deadline.Period = new Duration
            {
                Seconds = 5,
                NanoSeconds = 0
            };
            qos.DestinationOrder.Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos;
            qos.Durability.Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos;
            qos.DurabilityService.HistoryDepth = 5;
            qos.DurabilityService.HistoryKind = HistoryQosPolicyKind.KeepAllHistoryQos;
            qos.DurabilityService.MaxInstances = 5;
            qos.DurabilityService.MaxSamples = 5;
            qos.DurabilityService.MaxSamplesPerInstance = 5;
            qos.History.Depth = 5;
            qos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            qos.LatencyBudget.Duration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.Lifespan.Duration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.Liveliness.Kind = LivelinessQosPolicyKind.ManualByParticipantLivelinessQos;
            qos.Liveliness.LeaseDuration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.Ownership.Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos;
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            qos.Reliability.MaxBlockingTime = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.ResourceLimits.MaxInstances = 5;
            qos.ResourceLimits.MaxSamples = 5;
            qos.ResourceLimits.MaxSamplesPerInstance = 5;
            qos.TopicData.Value = new List<byte> { 0x5 };
            qos.TransportPriority.Value = 5;

            ReturnCode result = _participant.SetDefaultTopicQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new TopicQos();
            result = _participant.GetDefaultTopicQos(qos);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos);
            Assert.IsNotNull(qos.Deadline);
            Assert.IsNotNull(qos.DestinationOrder);
            Assert.IsNotNull(qos.Durability);
            Assert.IsNotNull(qos.DurabilityService);
            Assert.IsNotNull(qos.History);
            Assert.IsNotNull(qos.LatencyBudget);
            Assert.IsNotNull(qos.Lifespan);
            Assert.IsNotNull(qos.Liveliness);
            Assert.IsNotNull(qos.Ownership);
            Assert.IsNotNull(qos.Reliability);
            Assert.IsNotNull(qos.ResourceLimits);
            Assert.IsNotNull(qos.TopicData);
            Assert.IsNotNull(qos.TransportPriority);
            Assert.AreEqual(5, qos.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, qos.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos, qos.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.TransientLocalDurabilityQos, qos.Durability.Kind);
            Assert.AreEqual(HistoryQosPolicyKind.KeepAllHistoryQos, qos.DurabilityService.HistoryKind);
            Assert.AreEqual(5, qos.DurabilityService.HistoryDepth);
            Assert.AreEqual(5, qos.DurabilityService.MaxInstances);
            Assert.AreEqual(5, qos.DurabilityService.MaxSamples);
            Assert.AreEqual(5, qos.DurabilityService.MaxSamplesPerInstance);
            Assert.AreEqual(HistoryQosPolicyKind.KeepAllHistoryQos, qos.History.Kind);
            Assert.AreEqual(5, qos.History.Depth);
            Assert.AreEqual(5, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual((uint)5, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(5, qos.Lifespan.Duration.Seconds);
            Assert.AreEqual((uint)5, qos.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, qos.Liveliness.Kind);
            Assert.AreEqual(5, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual((uint)5, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, qos.Ownership.Kind);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, qos.Reliability.Kind);
            Assert.AreEqual(5, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual((uint)5, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(5, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(1, qos.TopicData.Value.Count());
            Assert.AreEqual(0x5, qos.TopicData.Value.First());
            Assert.AreEqual(5, qos.TransportPriority.Value);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestCreateTopic()
        {
            TopicQos qos = new TopicQos();
            qos.Durability.Kind = DurabilityQosPolicyKind.PersistentDurabilityQos;

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            MyTopicListener listener = new MyTopicListener();
            Topic topic = _participant.CreateTopic(nameof(TestCreateTopic), typeName, qos, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(topic);
            Assert.IsNotNull(topic.GetListener());
            Assert.AreEqual(nameof(TestCreateTopic), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            TopicQos getQos = new TopicQos();
            result = topic.GetQos(getQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(DurabilityQosPolicyKind.PersistentDurabilityQos, getQos.Durability.Kind);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestFindTopic()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestFindTopic), typeName);
            Assert.IsNotNull(topic);

            Topic foundTopic = _participant.FindTopic(nameof(TestFindTopic), new Duration
            {
                Seconds = Duration.InfiniteSeconds,
                NanoSeconds = Duration.InfiniteNanoseconds
            });

            Assert.IsNotNull(foundTopic);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestLookupTopicDescription()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestLookupTopicDescription), typeName);
            Assert.IsNotNull(topic);

            ITopicDescription foundTopic = _participant.LookupTopicDescription(nameof(TestLookupTopicDescription));

            Assert.IsNotNull(foundTopic);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestDeleteTopic()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteTopic), typeName);
            Assert.IsNotNull(topic);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(reader);            

            result = _participant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);            

            result = publisher.DeleteDataWriter(writer);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = _participant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, result);            
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestNewPublisherQos()
        {
            PublisherQos qos = new PublisherQos();

            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(0, qos.GroupData.Value.Count());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(0, qos.Partition.Name.Count());
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetDefaultPublisherQos()
        {
            PublisherQos qos = new PublisherQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;

            ReturnCode result = _participant.GetDefaultPublisherQos(qos);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(0, qos.GroupData.Value.Count());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(0, qos.Partition.Name.Count());
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestSetDefaultPulisherQos()
        {
            PublisherQos qos = new PublisherQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;

            ReturnCode result = _participant.SetDefaultPublisherQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new PublisherQos();
            result = _participant.GetDefaultPublisherQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count());
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count());
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestCreatePublisher()
        {
            PublisherQos qos = new PublisherQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;

            MyPublisherListener listener = new MyPublisherListener();
            Publisher publisher = _participant.CreatePublisher(qos, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(publisher);
            Assert.IsNotNull(publisher.GetListener());

            PublisherQos getPublisherQos = new PublisherQos();
            ReturnCode result = publisher.GetQos(getPublisherQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count());
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count());
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);           
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestDeletePublisher()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeletePublisher), typeName);
            Assert.IsNotNull(topic);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriter writer = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(writer);

            // Cannot be deleted if have active datawriters
            result = _participant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);            

            result = publisher.DeleteDataWriter(writer);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = _participant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, result);            
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestNewSubscriberQos()
        {
            SubscriberQos qos = new SubscriberQos();

            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(0, qos.GroupData.Value.Count());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(0, qos.Partition.Name.Count());
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetDefaultSubscriberQos()
        {
            SubscriberQos qos = new SubscriberQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;

            ReturnCode result = _participant.GetDefaultSubscriberQos(qos);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(0, qos.GroupData.Value.Count());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(0, qos.Partition.Name.Count());
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestSetDefaultSubscriberQos()
        {
            SubscriberQos qos = new SubscriberQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;

            ReturnCode result = _participant.SetDefaultSubscriberQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new SubscriberQos();
            result = _participant.GetDefaultSubscriberQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count());
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count());
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestCreateSubscriber()
        {
            SubscriberQos qos = new SubscriberQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;

            MySubscriberListener listener = new MySubscriberListener();
            Subscriber subscriber = _participant.CreateSubscriber(qos, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(subscriber);
            Assert.IsNotNull(subscriber.GetListener());

            SubscriberQos getSubscriberQos = new SubscriberQos();
            ReturnCode result = subscriber.GetQos(getSubscriberQos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count());
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count());
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestDeleteSubscriber()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteSubscriber), typeName);
            Assert.IsNotNull(topic);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(reader);

            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);            

            result = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, result);            
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetBuiltinSubscriber()
        {
            Subscriber subscriber = _participant.GetBuiltinSubscriber();
            Assert.IsNotNull(subscriber);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestContainsEntity()
        {
            Subscriber builtin = _participant.GetBuiltinSubscriber();
            Assert.IsNotNull(builtin);            

            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteContainedEntities), typeName);
            Assert.IsNotNull(topic);            

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);            

            Subscriber subscriber = _participant.CreateSubscriber();            
            Assert.IsNotNull(subscriber);                        

            DataWriter dataWriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(dataWriter);

            DataReader dataReader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(dataReader);

            DomainParticipant otherParticipant = _dpf.CreateParticipant(OTHER_DOMAIN_ID);
            Assert.IsNotNull(otherParticipant);
            Assert.AreNotEqual(otherParticipant.InstanceHandle, _participant.InstanceHandle);

            result = support.RegisterType(otherParticipant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic otherTopic = otherParticipant.CreateTopic("Other" + nameof(TestDeleteContainedEntities), typeName);
            Assert.IsNotNull(otherTopic);

            Subscriber otherBuiltin = otherParticipant.GetBuiltinSubscriber();
            Assert.IsNotNull(otherBuiltin);            

            Subscriber otherSubscriber = otherParticipant.CreateSubscriber();
            Assert.IsNotNull(otherSubscriber);

            Publisher otherPublisher = otherParticipant.CreatePublisher();
            Assert.IsNotNull(otherPublisher);

            DataReader otherDataReader = otherSubscriber.CreateDataReader(otherTopic);
            Assert.IsNotNull(otherDataReader);

            DataWriter otherDataWriter = otherPublisher.CreateDataWriter(otherTopic);
            Assert.IsNotNull(otherDataWriter);

            Assert.IsTrue(_participant.ContainsEntity(builtin.InstanceHandle));
            Assert.IsTrue(_participant.ContainsEntity(topic.InstanceHandle));
            Assert.IsTrue(_participant.ContainsEntity(publisher.InstanceHandle));
            Assert.IsTrue(_participant.ContainsEntity(subscriber.InstanceHandle));
            Assert.IsTrue(_participant.ContainsEntity(dataWriter.InstanceHandle));
            Assert.IsTrue(_participant.ContainsEntity(dataReader.InstanceHandle));

            Assert.IsTrue(otherParticipant.ContainsEntity(otherBuiltin.InstanceHandle));
            Assert.IsTrue(otherParticipant.ContainsEntity(otherTopic.InstanceHandle));
            Assert.IsTrue(otherParticipant.ContainsEntity(otherSubscriber.InstanceHandle));
            Assert.IsTrue(otherParticipant.ContainsEntity(otherPublisher.InstanceHandle));            
            Assert.IsTrue(otherParticipant.ContainsEntity(otherDataWriter.InstanceHandle));
            Assert.IsTrue(otherParticipant.ContainsEntity(otherDataReader.InstanceHandle));

            #region OpenDDS ISSUE
            // ISSUE: Instance handle overlap between participants.
            // Possible solution: Move the InstanceHandleGenerator to the Service_Participant
            //Assert.IsFalse(_participant.ContainsEntity(otherBuiltin.InstanceHandle));
            //Assert.IsFalse(_participant.ContainsEntity(otherTopic.InstanceHandle));
            //Assert.IsFalse(_participant.ContainsEntity(otherSubscriber.InstanceHandle));
            //Assert.IsFalse(_participant.ContainsEntity(otherPublisher.InstanceHandle));            
            //Assert.IsFalse(_participant.ContainsEntity(otherDataWriter.InstanceHandle));
            //Assert.IsFalse(_participant.ContainsEntity(otherDataReader.InstanceHandle));

            //Assert.IsFalse(otherParticipant.ContainsEntity(builtin.InstanceHandle));
            //Assert.IsFalse(otherParticipant.ContainsEntity(topic.InstanceHandle));
            //Assert.IsFalse(otherParticipant.ContainsEntity(publisher.InstanceHandle));
            //Assert.IsFalse(otherParticipant.ContainsEntity(subscriber.InstanceHandle));
            //Assert.IsFalse(otherParticipant.ContainsEntity(dataWriter.InstanceHandle));
            //Assert.IsFalse(otherParticipant.ContainsEntity(dataReader.InstanceHandle));
            #endregion

            result = otherParticipant.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);
            result = _dpf.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);            
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestIgnoreParticipant()
        {
            DomainParticipant other = _dpf.CreateParticipant(OTHER_DOMAIN_ID);
            Assert.IsNotNull(other);

            ReturnCode result = _participant.IgnoreParticipant(other.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);

            // TODO: Test that actually is ignored
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestIgnoreTopic()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestIgnoreTopic), support.GetTypeName());
            Assert.IsNotNull(topic);

            result = _participant.IgnoreTopic(topic.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);

            // TODO: Test that actually is ignored
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestIgnorePublication()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestIgnorePublication), support.GetTypeName());
            Assert.IsNotNull(topic);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriter dataWriter = publisher.CreateDataWriter(topic);
            Assert.IsNotNull(dataWriter);

            result = _participant.IgnorePublication(dataWriter.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);

            // TODO: Test that actually is ignored
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestIgnoreSubscription()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestIgnoreSubscription), support.GetTypeName());
            Assert.IsNotNull(topic);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader dataReader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(dataReader);

            result = _participant.IgnoreSubscription(dataReader.InstanceHandle);
            Assert.AreEqual(ReturnCode.Ok, result);

            // TODO: Test that actually is ignored
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestAssertLiveliness()
        {
            ReturnCode result = _participant.AssertLiveliness();
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetCurrentTimestamp()
        {
            Timestamp ts = new Timestamp()
            {
                Seconds = 0,
                NanoSeconds = 0
            };

            DateTime now = DateTime.Now;
            ReturnCode result = _participant.GetCurrentTimestamp(ref ts);

            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsTrue(ts.Seconds > 0);

            TimeSpan dt = TimeSpan.FromSeconds(ts.Seconds);
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(ts.Seconds);
            dtDateTime.AddMilliseconds(ts.NanoSeconds / 100000);

            Assert.AreEqual(dtDateTime.Year, now.Year);
            Assert.AreEqual(dtDateTime.Month, now.Month);
            Assert.AreEqual(dtDateTime.Day, now.Day);
            Assert.AreEqual(dtDateTime.DayOfWeek, now.DayOfWeek);
            Assert.AreEqual(dtDateTime.DayOfYear, now.DayOfYear);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestCreateContentFilteredTopic()
        {           
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestCreateContentFilteredTopic), typeName);
            Assert.IsNotNull(topic);

            int totalInstances = 10;
            int filterCount = 5;
            ContentFilteredTopic filteredTopic = _participant.CreateContentFilteredTopic("FilteredTopic", topic, "(Id <= %0)", filterCount.ToString());
            Assert.IsNotNull(filteredTopic);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader reader = subscriber.CreateDataReader(topic, drQos);
            Assert.IsNotNull(reader);

            DataReader filteredReader = subscriber.CreateDataReader(filteredTopic, drQos);
            Assert.IsNotNull(filteredReader);

            TestStructDataReader dataReader = new TestStructDataReader(reader);
            TestStructDataReader filteredDataReader = new TestStructDataReader(filteredReader);

            WaitSet waitSet = new WaitSet();
            GuardCondition cancelConditionReader = new GuardCondition();
            StatusCondition statusConditionReader = reader.StatusCondition;
            waitSet.AttachCondition(cancelConditionReader);
            waitSet.AttachCondition(statusConditionReader);
            statusConditionReader.EnabledStatuses = StatusKind.DataAvailableStatus;

            int countReader = 0;
            Thread threadReader = new Thread(() =>
            {
                while (true)
                {
                    ICollection<Condition> conditions = new List<Condition>();
                    Duration duration = new Duration
                    {
                        Seconds = Duration.InfiniteSeconds
                    };
                    waitSet.Wait(conditions, duration);

                    foreach (Condition cond in conditions)
                    {
                        if (cond == statusConditionReader && cond.TriggerValue)
                        {
                            StatusCondition sCond = (StatusCondition)cond;
                            StatusMask mask = sCond.EnabledStatuses;
                            if ((mask & StatusKind.DataAvailableStatus) != 0)
                            {
                                List<TestStruct> receivedData = new List<TestStruct>();
                                List<SampleInfo> sampleInfos = new List<SampleInfo>();
                                dataReader.Take(receivedData, sampleInfos);

                                foreach (var sampleInfo in sampleInfos)
                                {
                                    if (sampleInfo.ValidData && sampleInfo.InstanceState == InstanceStateKind.AliveInstanceState)
                                    {                                        
                                        countReader++;
                                    }
                                }
                            }
                        }

                        if (cond == cancelConditionReader && cond.TriggerValue)
                        {
                            // We reset the cancellation condition because it is a good practice, but in this implementation it probably doesn't change anything.
                            cancelConditionReader.TriggerValue = false;

                            // The thread activity has been canceled.
                            return;
                        }
                    }
                }
            });

            WaitSet waitSetFiltered = new WaitSet();
            GuardCondition cancelConditionFilteredReader = new GuardCondition();
            StatusCondition statusConditionFilteredReader = filteredReader.StatusCondition;
            waitSetFiltered.AttachCondition(cancelConditionFilteredReader);
            waitSetFiltered.AttachCondition(statusConditionFilteredReader);
            statusConditionFilteredReader.EnabledStatuses = StatusKind.DataAvailableStatus;

            int countFilteredReader = 0;
            Thread threadFilteredReader = new Thread(() =>
            {
                while (true)
                {
                    ICollection<Condition> conditions = new List<Condition>();
                    Duration duration = new Duration
                    {
                        Seconds = Duration.InfiniteSeconds
                    };
                    waitSetFiltered.Wait(conditions, duration);

                    foreach (Condition cond in conditions)
                    {
                        if (cond == statusConditionFilteredReader && cond.TriggerValue)
                        {
                            StatusCondition sCond = (StatusCondition)cond;
                            StatusMask mask = sCond.EnabledStatuses;
                            if ((mask & StatusKind.DataAvailableStatus) != 0)
                            {
                                List<TestStruct> receivedData = new List<TestStruct>();
                                List<SampleInfo> sampleInfos = new List<SampleInfo>();
                                filteredDataReader.Take(receivedData, sampleInfos);

                                foreach (var sampleInfo in sampleInfos)
                                {
                                    if (sampleInfo.ValidData && sampleInfo.InstanceState == InstanceStateKind.AliveInstanceState)
                                    {
                                        countFilteredReader++;
                                    }
                                }
                            }
                        }

                        if (cond == cancelConditionFilteredReader && cond.TriggerValue)
                        {
                            // We reset the cancellation condition because it is a good practice, but in this implementation it probably doesn't change anything.
                            cancelConditionFilteredReader.TriggerValue = false;

                            // The thread activity has been canceled.
                            return;
                        }
                    }
                }
            });

            threadReader.IsBackground = true;            
            threadReader.Priority = ThreadPriority.Highest;
            threadReader.Start();

            threadFilteredReader.IsBackground = true;           
            threadFilteredReader.Priority = ThreadPriority.Highest;
            threadFilteredReader.Start();

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter writer = publisher.CreateDataWriter(topic, dwQos);
            Assert.IsNotNull(writer);

            TestStructDataWriter dataWriter = new TestStructDataWriter(writer);

            // Wait for subscriptions
            PublicationMatchedStatus status = new PublicationMatchedStatus();
            do
            {
                result = dataWriter.GetPublicationMatchedStatus(ref status);
                Thread.Sleep(100);
            }
            while (status.CurrentCount < 2);
            
            for (int i = 1; i <= totalInstances; i++)
            {
                result = dataWriter.Write(new TestStruct
                {
                    Id = i,
                });
                Assert.AreEqual(ReturnCode.Ok, result);
                
                result = dataWriter.WaitForAcknowledgments(new Duration
                {
                    Seconds = 5
                });
                Assert.AreEqual(ReturnCode.Ok, result);
            }
            
            cancelConditionReader.TriggerValue = true;
            cancelConditionFilteredReader.TriggerValue = true;
            threadFilteredReader.Join();
            threadReader.Join();

            result = waitSet.DetachCondition(cancelConditionReader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = waitSet.DetachCondition(statusConditionReader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = waitSetFiltered.DetachCondition(cancelConditionFilteredReader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = waitSetFiltered.DetachCondition(statusConditionFilteredReader);
            Assert.AreEqual(ReturnCode.Ok, result);

            result = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = subscriber.DeleteDataReader(filteredReader);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = publisher.DeleteDataWriter(writer);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = _participant.DeletePublisher(publisher);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = _participant.DeleteContentFilteredTopic(filteredTopic);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = _participant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, result);            
            
            Assert.AreEqual(totalInstances, countReader);
            Assert.AreEqual(filterCount, countFilteredReader);                     
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestDeleteContentFilteredTopic()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteContentFilteredTopic), support.GetTypeName());
            Assert.IsNotNull(topic);

            int filterCount = 5;
            ContentFilteredTopic filteredTopic = _participant.CreateContentFilteredTopic("FilteredTopic", topic, "(Id < %0)", filterCount.ToString());
            Assert.IsNotNull(filteredTopic);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(filteredTopic);
            Assert.IsNotNull(reader);

            result = _participant.DeleteContentFilteredTopic(filteredTopic);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);            

            result = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = _participant.DeleteContentFilteredTopic(filteredTopic);
            Assert.AreEqual(ReturnCode.Ok, result);            
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestCreateMultiTopic()
        {
            AthleteTypeSupport athleteSupport = new AthleteTypeSupport();
            string athleteTypeName = athleteSupport.GetTypeName();
            ReturnCode result = athleteSupport.RegisterType(_participant, athleteTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic athleteTopic = _participant.CreateTopic("AthleteTopic", athleteTypeName);
            Assert.IsNotNull(athleteTopic);

            ResultTypeSupport resultSupport = new ResultTypeSupport();
            string resultTypeName = resultSupport.GetTypeName();
            result = resultSupport.RegisterType(_participant, resultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic resultTopic = _participant.CreateTopic("ResultTopic", resultTypeName);
            Assert.IsNotNull(resultTopic);

            AthleteResultTypeSupport athleteResultSupport = new AthleteResultTypeSupport();
            string athleteResultTypeName = athleteResultSupport.GetTypeName();
            result = athleteResultSupport.RegisterType(_participant, athleteResultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            MultiTopic multiTopic = _participant.CreateMultiTopic("AthleteResultTopic", athleteResultTypeName, "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic");
            Assert.IsNotNull(multiTopic);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReaderQos drQos = new DataReaderQos();
            drQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataReader dr = subscriber.CreateDataReader(multiTopic, drQos);
            Assert.IsNotNull(dr);
            AthleteResultDataReader dataReader = new AthleteResultDataReader(dr);

            Publisher publisher = _participant.CreatePublisher();
            Assert.IsNotNull(publisher);

            DataWriterQos dwQos = new DataWriterQos();
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            DataWriter dwAthlete = publisher.CreateDataWriter(athleteTopic, dwQos);
            Assert.IsNotNull(dwAthlete);
            AthleteDataWriter athleteDataWriter = new AthleteDataWriter(dwAthlete);

            DataWriter dwResult = publisher.CreateDataWriter(resultTopic, dwQos);
            Assert.IsNotNull(dwResult);
            ResultDataWriter resultDataWriter = new ResultDataWriter(dwResult);

            // Wait for subscriptions
            PublicationMatchedStatus statusAthlete = new PublicationMatchedStatus();
            PublicationMatchedStatus statusResult = new PublicationMatchedStatus();
            do
            {
                result = athleteDataWriter.GetPublicationMatchedStatus(ref statusAthlete);
                result = resultDataWriter.GetPublicationMatchedStatus(ref statusResult);
                Thread.Sleep(100);
            }
            while (statusAthlete.CurrentCount < 1 && statusResult.CurrentCount < 1);

            for (int i = 1; i <= 5; i++)
            {
                athleteDataWriter.Write(new Athlete
                {
                    AthleteId = i,
                    FirstName = "FirstName" + i.ToString(),
                    SecondName = "SecondName" + i.ToString(),
                    Country = "Country" + i.ToString(),
                });

                result = athleteDataWriter.WaitForAcknowledgments(new Duration
                {
                    Seconds = 5
                });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            for (int i = 1; i <= 3; i++)
            {
                resultDataWriter.Write(new Result
                {
                    AthleteId = i,
                    Rank = i,
                    Score = 10f - i
                });

                result = resultDataWriter.WaitForAcknowledgments(new Duration
                {
                    Seconds = 5
                });
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            Thread.Sleep(500);
            List<AthleteResult> receivedData = new List<AthleteResult>();
            List<SampleInfo> sampleInfos = new List<SampleInfo>();
            result = dataReader.Read(receivedData, sampleInfos);
            Assert.AreEqual(ReturnCode.Ok, result);

            Assert.AreEqual(3, receivedData.Count);            
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(i + 1, receivedData[i].AthleteId);
                Assert.AreEqual(i + 1, receivedData[i].Rank);
                Assert.AreEqual(10f - (i + 1), receivedData[i].Score);
                Assert.AreEqual("FirstName" + (i + 1).ToString(), receivedData[i].FirstName);
                Assert.AreEqual("SecondName" + (i + 1).ToString(), receivedData[i].SecondName);
                Assert.AreEqual("Country" + (i + 1).ToString(), receivedData[i].Country);
            }            
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestDeleteMultiTopic()
        {
            AthleteTypeSupport athleteSupport = new AthleteTypeSupport();
            string athleteTypeName = athleteSupport.GetTypeName();
            ReturnCode result = athleteSupport.RegisterType(_participant, athleteTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic athleteTopic = _participant.CreateTopic("AthleteTopic", athleteTypeName);
            Assert.IsNotNull(athleteTopic);

            ResultTypeSupport resultSupport = new ResultTypeSupport();
            string resultTypeName = resultSupport.GetTypeName();
            result = resultSupport.RegisterType(_participant, resultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic resultTopic = _participant.CreateTopic("ResultTopic", resultTypeName);
            Assert.IsNotNull(resultTopic);

            AthleteResultTypeSupport athleteResultSupport = new AthleteResultTypeSupport();
            string athleteResultTypeName = athleteResultSupport.GetTypeName();
            result = athleteResultSupport.RegisterType(_participant, athleteResultTypeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            MultiTopic multiTopic = _participant.CreateMultiTopic(nameof(TestDeleteMultiTopic), athleteResultTypeName, "SELECT * FROM AthleteTopic NATURAL JOIN ResultTopic");
            Assert.IsNotNull(multiTopic);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            DataReader reader = subscriber.CreateDataReader(multiTopic);
            Assert.IsNotNull(reader);

            result = _participant.DeleteMultiTopic(multiTopic);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);            

            result = subscriber.DeleteDataReader(reader);
            Assert.AreEqual(ReturnCode.Ok, result);            

            result = _participant.DeleteMultiTopic(multiTopic);
            Assert.AreEqual(ReturnCode.Ok, result);            
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetDiscoveredParticipants()
        {
            List<InstanceHandle> handles = new List<InstanceHandle>();
            ReturnCode result = _participant.GetDiscoveredParticipants(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(0, handles.Count);

            DomainParticipant otherParticipant = _dpf.CreateParticipant(DOMAIN_ID);
            Assert.IsNotNull(otherParticipant);

            Thread.Sleep(500);

            result = _participant.GetDiscoveredParticipants(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, handles.Count);

            handles = new List<InstanceHandle>();
            result = otherParticipant.GetDiscoveredParticipants(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, handles.Count);

            result = _dpf.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetDiscoveredParticipantData()
        {
            DomainParticipantQos qos = new DomainParticipantQos();
            qos.UserData.Value = new List<byte> { 0x42 };
            DomainParticipant otherParticipant = _dpf.CreateParticipant(DOMAIN_ID, qos);
            Assert.IsNotNull(otherParticipant);

            Thread.Sleep(500);

            List<InstanceHandle> handles = new List<InstanceHandle>();
            ReturnCode result = _participant.GetDiscoveredParticipants(handles);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, handles.Count);

            ParticipantBuiltinTopicData data = new ParticipantBuiltinTopicData();
            result = _participant.GetDiscoveredParticipantData(ref data, handles.First());
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.AreEqual(1, data.UserData.Value.Count());
            Assert.AreEqual(0x42, data.UserData.Value.First());

            result = _dpf.DeleteParticipant(otherParticipant);
            Assert.AreEqual(ReturnCode.Ok, result);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetDiscoveredTopics()
        {
            #region OpenDDS ISSUE
            // The handles are not updated till calling get_instance_handle.
            // Perhaps, creating a datareader or datawriter also activate it.
            // Workaround: After create_new_topic call
            //if (new_topic) {
            //  TopicImpl* the_topic_servant = dynamic_cast<TopicImpl*>(new_topic);
            //  this->id_to_handle(the_topic_servant->get_id());
            //}

            //List<InstanceHandle> handles = new List<InstanceHandle>();
            //ReturnCode result = _participant.GetDiscoveredTopics(handles);
            //Assert.AreEqual(ReturnCode.Ok, result);
            //Assert.AreEqual(4, handles.Count);

            //TestStructTypeSupport support = new TestStructTypeSupport();
            //string typeName = support.GetTypeName();
            //result = support.RegisterType(_participant, typeName);
            //Assert.AreEqual(ReturnCode.Ok, result);

            //Topic topic = _participant.CreateTopic(nameof(TestGetDiscoveredTopics), typeName);
            //Assert.IsNotNull(topic);

            //Thread.Sleep(100);

            //result = _participant.GetDiscoveredTopics(handles);
            //Assert.AreEqual(ReturnCode.Ok, result);
            //Assert.AreEqual(5, handles.Count);

            //DomainParticipant otherParticipant = _dpf.CreateParticipant(DOMAIN_ID);
            //Assert.IsNotNull(otherParticipant);

            //result = support.RegisterType(otherParticipant, typeName);
            //Assert.AreEqual(ReturnCode.Ok, result);
            #endregion

            #region OpenDDS ISSUE
            // OpenDDS only returns local topics. The specification mention all the topics in the domain.

            // Topic otherTopic = otherParticipant.CreateTopic("Other" + nameof(TestGetDiscoveredTopics), typeName);
            // Assert.IsNotNull(otherTopic);

            //Thread.Sleep(500);

            //handles = new List<InstanceHandle>();
            //result = _participant.GetDiscoveredTopics(handles);
            //Assert.AreEqual(ReturnCode.Ok, result);
            //Assert.AreEqual(6, handles.Count);
            #endregion
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestGetDiscoveredTopicData()
        {
            #region OpenDDS ISSUE
            // See TestGetDiscoveredTopics issues.
            // This unit test will be implemented after fix them.
            #endregion
        }
        #endregion
    }

    #region Auxiliar Classes
    class MyParticipantListener : DomainParticipantListener
    {
        public override void OnBudgetExceeded(DataReader reader, BudgetExceededStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnConnectionDeleted(DataWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void OnConnectionDeleted(DataReader reader)
        {
            throw new NotImplementedException();
        }

        public override void OnDataAvailable(DataReader reader)
        {
            throw new NotImplementedException();
        }

        public override void OnDataOnReaders(Subscriber subscriber)
        {
            throw new NotImplementedException();
        }

        public override void OnInconsistentTopic(Topic topic, InconsistentTopicStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnLivelinessChanged(DataReader reader, LivelinessChangedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnLivelinessLost(DataWriter writer, LivelinessLostStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnOfferedDeadlineMissed(DataWriter writer, OfferedDeadlineMissedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnOfferedIncompatibleQos(DataWriter writer, OfferedIncompatibleQosStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnPublicationDisconnected(DataWriter writer, PublicationDisconnectedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnPublicationLost(DataWriter writer, PublicationLostStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnPublicationMatched(DataWriter writer, PublicationMatchedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnPublicationReconnected(DataWriter writer, PublicationReconnectedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnRequestedDeadlineMissed(DataReader reader, RequestedDeadlineMissedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnRequestedIncompatibleQos(DataReader reader, RequestedIncompatibleQosStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSampleLost(DataReader reader, SampleLostStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSampleRejected(DataReader reader, SampleRejectedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSubscriptionDisconnected(DataReader reader, SubscriptionDisconnectedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSubscriptionLost(DataReader reader, SubscriptionLostStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSubscriptionMatched(DataReader reader, SubscriptionMatchedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSubscriptionReconnected(DataReader reader, SubscriptionReconnectedStatus status)
        {
            throw new NotImplementedException();
        }
    }

    class MyTopicListener : TopicListener
    {
        public override void OnInconsistentTopic(Topic topic, InconsistentTopicStatus status)
        {
            throw new NotImplementedException();
        }
    }

    class MyPublisherListener : PublisherListener
    {
        public override void OnConnectionDeleted(DataWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void OnLivelinessLost(DataWriter writer, LivelinessLostStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnOfferedDeadlineMissed(DataWriter writer, OfferedDeadlineMissedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnOfferedIncompatibleQos(DataWriter writer, OfferedIncompatibleQosStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnPublicationDisconnected(DataWriter writer, PublicationDisconnectedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnPublicationLost(DataWriter writer, PublicationLostStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnPublicationMatched(DataWriter writer, PublicationMatchedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnPublicationReconnected(DataWriter writer, PublicationReconnectedStatus status)
        {
            throw new NotImplementedException();
        }
    }

    class MySubscriberListener : SubscriberListener
    {
        public override void OnBudgetExceeded(DataReader reader, BudgetExceededStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnConnectionDeleted(DataReader reader)
        {
            throw new NotImplementedException();
        }

        public override void OnDataAvailable(DataReader reader)
        {
            throw new NotImplementedException();
        }

        public override void OnDataOnReaders(Subscriber subscriber)
        {
            throw new NotImplementedException();
        }

        public override void OnLivelinessChanged(DataReader reader, LivelinessChangedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnRequestedDeadlineMissed(DataReader reader, RequestedDeadlineMissedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnRequestedIncompatibleQos(DataReader reader, RequestedIncompatibleQosStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSampleLost(DataReader reader, SampleLostStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSampleRejected(DataReader reader, SampleRejectedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSubscriptionDisconnected(DataReader reader, SubscriptionDisconnectedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSubscriptionLost(DataReader reader, SubscriptionLostStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSubscriptionMatched(DataReader reader, SubscriptionMatchedStatus status)
        {
            throw new NotImplementedException();
        }

        public override void OnSubscriptionReconnected(DataReader reader, SubscriptionReconnectedStatus status)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
