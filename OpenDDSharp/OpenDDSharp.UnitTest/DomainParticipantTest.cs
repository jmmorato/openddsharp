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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.Test;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class DomainParticipantTest
    {
        #region Fields
        static DomainParticipantFactory _dpf;
        DomainParticipant _participant;
        #endregion

        #region Initialization/Cleanup
        [ClassInitialize]
        public static void  ClassInitialize(TestContext context)
        {
            _dpf = ParticipantService.Instance.GetDomainParticipantFactory();
        }

        [TestInitialize]
        public void TestInitialize()
        {            
            _participant = _dpf.CreateParticipant(42);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_participant != null)
                _participant.DeleteContainedEntities();

            if (_dpf != null)
                _dpf.DeleteParticipant(_participant);            
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ParticipantService.Instance.Shutdown();
        }
        #endregion

        #region Test Methods
        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestDomainId()
        {
            Assert.IsNotNull(_participant);
            Assert.AreEqual(42, _participant.DomainId);
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

            ReturnCode ret = _participant.GetDefaultTopicQos(qos);
            
            Assert.AreEqual(ReturnCode.Ok, ret);
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

            ReturnCode ret = _participant.SetDefaultTopicQos(qos);
            Assert.AreEqual(ReturnCode.Ok, ret);

            ret = _participant.GetDefaultTopicQos(qos);
            Assert.AreEqual(ReturnCode.Ok, ret);
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
            if (result != ReturnCode.Ok)
            {
                throw new Exception("Could not register the type");
            }

            MyTopicListener listener = new MyTopicListener();
            Topic topic = _participant.CreateTopic("TopicName", typeName, qos, listener, StatusMask.DefaultStatusMask);
            Assert.IsNotNull(topic);
            Assert.IsNotNull(topic.GetListener());
            Assert.AreEqual("TopicName", topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);
            
            TopicQos getQos = new TopicQos();
            ReturnCode ret = topic.GetQos(getQos);
            Assert.AreEqual(ReturnCode.Ok, ret);           
            Assert.AreEqual(DurabilityQosPolicyKind.PersistentDurabilityQos, getQos.Durability.Kind);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestFindTopic()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            if (result != ReturnCode.Ok)
            {
                throw new Exception("Could not register the type");
            }
           
            Topic topic = _participant.CreateTopic("TopicName", typeName);
            Assert.IsNotNull(topic);

            Topic foundTopic = _participant.FindTopic("TopicName", new Duration
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
            if (result != ReturnCode.Ok)
            {
                throw new Exception("Could not register the type");
            }

            Topic topic = _participant.CreateTopic("TopicName", typeName);
            Assert.IsNotNull(topic);

            ITopicDescription foundTopic = _participant.LookupTopicDescription("TopicName");

            Assert.IsNotNull(foundTopic);
        }

        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestDeleteTopic()
        {
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            if (result != ReturnCode.Ok)
            {
                throw new Exception("Could not register the type");
            }

            Topic topic = _participant.CreateTopic("TopicName", typeName);
            Assert.IsNotNull(topic);

            ReturnCode ret = _participant.DeleteTopic(topic);
            Assert.AreEqual(ReturnCode.Ok, ret);
        }
        #endregion
    }

    class MyTopicListener : TopicListener
    {
        public override void OnInconsistentTopic(Topic topic, InconsistentTopicStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
