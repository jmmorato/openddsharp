/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

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
using OpenDDSharp.OpenDDS.DCPS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest.Helpers
{
    internal static class TestHelper
    {
        #region DomainParticipant QoS
        public static DomainParticipantQos CreateNonDefaultDomainParticipantQos()
        {
            DomainParticipantQos qos = new DomainParticipantQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.UserData.Value = new List<byte> { 0x42 };

            return qos;
        }

        public static void TestDefaultDomainParticipantQos(DomainParticipantQos qos)
        {
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.UserData);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(0, qos.UserData.Value.Count());
        }

        public static void TestNonDefaultDomainParticipantQos(DomainParticipantQos qos)
        {
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.UserData);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(1, qos.UserData.Value.Count());
            Assert.AreEqual(0x42, qos.UserData.Value.First());
        }
        #endregion

        #region Topic QoS
        public static TopicQos CreateNonDefaultTopicQos()
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
            qos.DurabilityService.ServiceCleanupDelay = new Duration { Seconds = 5, NanoSeconds = 5 };
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

            return qos;

        }

        public static void TestDefaultTopicQos(TopicQos qos)
        {
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

        public static void TestNonDefaultTopicQos(TopicQos qos)
        {
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
            Assert.IsNotNull(qos.DurabilityService.ServiceCleanupDelay);
            Assert.AreEqual(5, qos.DurabilityService.ServiceCleanupDelay.Seconds);
            Assert.AreEqual((uint)5, qos.DurabilityService.ServiceCleanupDelay.NanoSeconds);
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
        #endregion

        #region Publisher QoS
        public static PublisherQos CreateNonDefaultPublisherQos()
        {
            PublisherQos qos = new PublisherQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;

            return qos;
        }

        public static void TestDefaultPublisherQos(PublisherQos qos)
        {
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

        public static void TestNonDefaultPublisherQos(PublisherQos qos)
        {
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
        #endregion

        #region Subscriber QoS
        public static SubscriberQos CreateNonDefaultSubscriberQos()
        {
            SubscriberQos qos = new SubscriberQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;

            return qos;
        }

        public static void TestDefaultSubscriberQos(SubscriberQos qos)
        {
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

        public static void TestNonDefaultSubscriberQos(SubscriberQos qos)
        {
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
        #endregion

        #region DataWriter QoS
        public static DataWriterQos CreateNonDefaultDataWriterQos()
        {
            DataWriterQos qos = new DataWriterQos();
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
            qos.OwnershipStrength.Value = 5;
            qos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            qos.Reliability.MaxBlockingTime = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.ResourceLimits.MaxInstances = 5;
            qos.ResourceLimits.MaxSamples = 5;
            qos.ResourceLimits.MaxSamplesPerInstance = 5;
            qos.UserData.Value = new List<byte> { 0x5 };
            qos.TransportPriority.Value = 5;
            qos.WriterDataLifecycle.AutodisposeUnregisteredInstances = true;

            return qos;
        }

        public static void TestDefaultDataWriterQos(DataWriterQos qos)
        {
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
            Assert.IsNotNull(qos.OwnershipStrength);
            Assert.IsNotNull(qos.Reliability);
            Assert.IsNotNull(qos.ResourceLimits);
            Assert.IsNotNull(qos.TransportPriority);
            Assert.IsNotNull(qos.UserData);
            Assert.IsNotNull(qos.WriterDataLifecycle);
            Assert.IsNotNull(qos.Deadline.Period);
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
            Assert.IsNotNull(qos.LatencyBudget.Duration);
            Assert.AreEqual(Duration.ZeroSeconds, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.IsNotNull(qos.Lifespan.Duration);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Lifespan.Duration.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.AutomaticLivelinessQos, qos.Liveliness.Kind);
            Assert.IsNotNull(qos.Liveliness.LeaseDuration);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.SharedOwnershipQos, qos.Ownership.Kind);
            Assert.AreEqual(0, qos.OwnershipStrength.Value);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, qos.Reliability.Kind);
            Assert.IsNotNull(qos.Reliability.MaxBlockingTime);
            Assert.AreEqual(0, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual((uint)100000000, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(0, qos.TransportPriority.Value);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(0, qos.UserData.Value.Count());
            Assert.IsTrue(qos.WriterDataLifecycle.AutodisposeUnregisteredInstances);
        }

        public static void TestNonDefaultDataWriterQos(DataWriterQos qos)
        {
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
            Assert.IsNotNull(qos.OwnershipStrength);
            Assert.IsNotNull(qos.Reliability);
            Assert.IsNotNull(qos.ResourceLimits);
            Assert.IsNotNull(qos.TransportPriority);
            Assert.IsNotNull(qos.UserData);
            Assert.IsNotNull(qos.WriterDataLifecycle);

            Assert.IsNotNull(qos.Deadline.Period);
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
            Assert.IsNotNull(qos.LatencyBudget.Duration);
            Assert.AreEqual(5, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual((uint)5, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.IsNotNull(qos.Lifespan.Duration);
            Assert.AreEqual(5, qos.Lifespan.Duration.Seconds);
            Assert.AreEqual((uint)5, qos.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, qos.Liveliness.Kind);
            Assert.IsNotNull(qos.Liveliness.LeaseDuration);
            Assert.AreEqual(5, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual((uint)5, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, qos.Ownership.Kind);
            Assert.AreEqual(5, qos.OwnershipStrength.Value);
            Assert.AreEqual(ReliabilityQosPolicyKind.BestEffortReliabilityQos, qos.Reliability.Kind);
            Assert.IsNotNull(qos.Reliability.MaxBlockingTime);
            Assert.AreEqual(5, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual((uint)5, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(5, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(1, qos.UserData.Value.Count());
            Assert.AreEqual(0x5, qos.UserData.Value.First());
            Assert.AreEqual(5, qos.TransportPriority.Value);
            Assert.IsTrue(qos.WriterDataLifecycle.AutodisposeUnregisteredInstances);
        }        
        #endregion

        #region DataReader QoS
        public static DataReaderQos CreateNonDefaultDataReaderQos()
        {
            DataReaderQos qos = new DataReaderQos();

            qos.Deadline.Period = new Duration
            {
                Seconds = 5,
                NanoSeconds = 0
            };
            qos.DestinationOrder.Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos;
            qos.Durability.Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos;
            qos.History.Depth = 5;
            qos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            qos.LatencyBudget.Duration = new Duration
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
            qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            qos.Reliability.MaxBlockingTime = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.ResourceLimits.MaxInstances = 5;
            qos.ResourceLimits.MaxSamples = 5;
            qos.ResourceLimits.MaxSamplesPerInstance = 5;
            qos.TimeBasedFilter.MinimumSeparation = new Duration
            {
                Seconds = 3,
                NanoSeconds = 3
            };
            qos.UserData.Value = new List<byte> { 0x5 };

            return qos;
        }

        public static void TestDefaultDataReaderQos(DataReaderQos qos)
        {
            Assert.IsNotNull(qos);
            Assert.IsNotNull(qos.Deadline);
            Assert.IsNotNull(qos.DestinationOrder);
            Assert.IsNotNull(qos.Durability);
            Assert.IsNotNull(qos.History);
            Assert.IsNotNull(qos.LatencyBudget);
            Assert.IsNotNull(qos.Liveliness);
            Assert.IsNotNull(qos.Ownership);
            Assert.IsNotNull(qos.ReaderDataLifecycle);
            Assert.IsNotNull(qos.Reliability);
            Assert.IsNotNull(qos.ResourceLimits);
            Assert.IsNotNull(qos.TimeBasedFilter);
            Assert.IsNotNull(qos.UserData);
            Assert.IsNotNull(qos.Deadline.Period);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.ByReceptionTimestampDestinationOrderQos, qos.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.VolatileDurabilityQos, qos.Durability.Kind);
            Assert.AreEqual(HistoryQosPolicyKind.KeepLastHistoryQos, qos.History.Kind);
            Assert.AreEqual(1, qos.History.Depth);
            Assert.IsNotNull(qos.LatencyBudget.Duration);
            Assert.AreEqual(Duration.ZeroSeconds, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.AutomaticLivelinessQos, qos.Liveliness.Kind);
            Assert.IsNotNull(qos.Liveliness.LeaseDuration);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.SharedOwnershipQos, qos.Ownership.Kind);
            Assert.IsNotNull(qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay.NanoSeconds);
            Assert.IsNotNull(qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay.NanoSeconds);
            Assert.AreEqual(ReliabilityQosPolicyKind.BestEffortReliabilityQos, qos.Reliability.Kind);
            Assert.IsNotNull(qos.Reliability.MaxBlockingTime);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.IsNotNull(qos.TimeBasedFilter.MinimumSeparation);
            Assert.AreEqual(0, qos.TimeBasedFilter.MinimumSeparation.Seconds);
            Assert.AreEqual((uint)0, qos.TimeBasedFilter.MinimumSeparation.NanoSeconds);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(0, qos.UserData.Value.Count());
        }

        public static void TestNonDefaultDataReaderQos(DataReaderQos qos)
        {
            Assert.IsNotNull(qos);
            Assert.IsNotNull(qos.Deadline);
            Assert.IsNotNull(qos.DestinationOrder);
            Assert.IsNotNull(qos.Durability);
            Assert.IsNotNull(qos.History);
            Assert.IsNotNull(qos.LatencyBudget);
            Assert.IsNotNull(qos.Liveliness);
            Assert.IsNotNull(qos.Ownership);
            Assert.IsNotNull(qos.ReaderDataLifecycle);
            Assert.IsNotNull(qos.Reliability);
            Assert.IsNotNull(qos.ResourceLimits);
            Assert.IsNotNull(qos.TimeBasedFilter);
            Assert.IsNotNull(qos.UserData);

            Assert.IsNotNull(qos.Deadline.Period);
            Assert.AreEqual(5, qos.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, qos.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos, qos.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.TransientLocalDurabilityQos, qos.Durability.Kind);
            Assert.AreEqual(HistoryQosPolicyKind.KeepAllHistoryQos, qos.History.Kind);
            Assert.AreEqual(5, qos.History.Depth);
            Assert.IsNotNull(qos.LatencyBudget.Duration);
            Assert.AreEqual(5, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual((uint)5, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, qos.Liveliness.Kind);
            Assert.IsNotNull(qos.Liveliness.LeaseDuration);
            Assert.AreEqual(5, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual((uint)5, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, qos.Ownership.Kind);
            Assert.IsNotNull(qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay);
            Assert.AreEqual(5, qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay.Seconds);
            Assert.AreEqual((uint)5, qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay.NanoSeconds);
            Assert.IsNotNull(qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay);
            Assert.AreEqual(5, qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay.Seconds);
            Assert.AreEqual((uint)5, qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay.NanoSeconds);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, qos.Reliability.Kind);
            Assert.IsNotNull(qos.Reliability.MaxBlockingTime);
            Assert.AreEqual(5, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual((uint)5, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(5, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(1, qos.UserData.Value.Count());
            Assert.IsNotNull(qos.TimeBasedFilter.MinimumSeparation);
            Assert.AreEqual(3, qos.TimeBasedFilter.MinimumSeparation.Seconds);
            Assert.AreEqual((uint)3, qos.TimeBasedFilter.MinimumSeparation.NanoSeconds);
            Assert.AreEqual(0x5, qos.UserData.Value.First());
        }
        #endregion

        #region Builtin Data
        public static void TestNonDefaultSubscriptionData(SubscriptionBuiltinTopicData data)
        {
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.Deadline);
            Assert.IsNotNull(data.DestinationOrder);
            Assert.IsNotNull(data.Durability);
            Assert.IsNotNull(data.LatencyBudget);
            Assert.IsNotNull(data.Liveliness);
            Assert.IsNotNull(data.Ownership);
            Assert.IsNotNull(data.Reliability);
            Assert.IsNotNull(data.TimeBasedFilter);
            Assert.IsNotNull(data.UserData);
            Assert.IsNotNull(data.Key);
            Assert.IsNotNull(data.ParticipantKey);
            Assert.IsNotNull(data.GroupData);
            Assert.IsNotNull(data.Partition);
            Assert.IsNotNull(data.Presentation);
            Assert.IsNotNull(data.TopicData);
            Assert.IsFalse(string.IsNullOrWhiteSpace(data.TopicName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(data.TypeName));

            Assert.IsNotNull(data.Deadline.Period);
            Assert.AreEqual(5, data.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, data.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos, data.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.TransientLocalDurabilityQos, data.Durability.Kind);
            Assert.IsNotNull(data.LatencyBudget.Duration);
            Assert.AreEqual(5, data.LatencyBudget.Duration.Seconds);
            Assert.AreEqual((uint)5, data.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, data.Liveliness.Kind);
            Assert.IsNotNull(data.Liveliness.LeaseDuration);
            Assert.AreEqual(5, data.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual((uint)5, data.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, data.Ownership.Kind);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, data.Reliability.Kind);
            Assert.IsNotNull(data.Reliability.MaxBlockingTime);
            Assert.AreEqual(5, data.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual((uint)5, data.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(1, data.UserData.Value.Count());
            Assert.IsNotNull(data.TimeBasedFilter.MinimumSeparation);
            Assert.AreEqual(3, data.TimeBasedFilter.MinimumSeparation.Seconds);
            Assert.AreEqual((uint)3, data.TimeBasedFilter.MinimumSeparation.NanoSeconds);
            Assert.AreEqual(0x5, data.UserData.Value.First());
        }

        public static void TestNonDefaultPublicationData(PublicationBuiltinTopicData data)
        {
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.Deadline);
            Assert.IsNotNull(data.DestinationOrder);
            Assert.IsNotNull(data.Durability);
            Assert.IsNotNull(data.DurabilityService);
            Assert.IsNotNull(data.LatencyBudget);
            Assert.IsNotNull(data.Lifespan);
            Assert.IsNotNull(data.Liveliness);
            Assert.IsNotNull(data.Ownership);
            Assert.IsNotNull(data.OwnershipStrength);
            Assert.IsNotNull(data.Reliability);
            Assert.IsNotNull(data.UserData);
            Assert.IsNotNull(data.Key);
            Assert.IsNotNull(data.ParticipantKey);
            Assert.IsNotNull(data.GroupData);
            Assert.IsNotNull(data.Partition);
            Assert.IsNotNull(data.Presentation);
            Assert.IsNotNull(data.TopicData);
            Assert.IsFalse(string.IsNullOrWhiteSpace(data.TopicName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(data.TypeName));

            Assert.IsNotNull(data.Deadline.Period);
            Assert.AreEqual(5, data.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, data.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos, data.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.TransientLocalDurabilityQos, data.Durability.Kind);
            Assert.AreEqual(HistoryQosPolicyKind.KeepAllHistoryQos, data.DurabilityService.HistoryKind);
            Assert.AreEqual(5, data.DurabilityService.HistoryDepth);
            Assert.AreEqual(5, data.DurabilityService.MaxInstances);
            Assert.AreEqual(5, data.DurabilityService.MaxSamples);
            Assert.AreEqual(5, data.DurabilityService.MaxSamplesPerInstance);
            Assert.IsNotNull(data.LatencyBudget.Duration);
            Assert.AreEqual(5, data.LatencyBudget.Duration.Seconds);
            Assert.AreEqual((uint)5, data.LatencyBudget.Duration.NanoSeconds);
            Assert.IsNotNull(data.Lifespan.Duration);
            Assert.AreEqual(5, data.Lifespan.Duration.Seconds);
            Assert.AreEqual((uint)5, data.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, data.Liveliness.Kind);
            Assert.IsNotNull(data.Liveliness.LeaseDuration);
            Assert.AreEqual(5, data.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual((uint)5, data.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.SharedOwnershipQos, data.Ownership.Kind);
            Assert.AreEqual(5, data.OwnershipStrength.Value);
            Assert.AreEqual(ReliabilityQosPolicyKind.BestEffortReliabilityQos, data.Reliability.Kind);
            Assert.IsNotNull(data.Reliability.MaxBlockingTime);
            Assert.AreEqual(5, data.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual((uint)5, data.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(1, data.UserData.Value.Count());
            Assert.AreEqual(0x5, data.UserData.Value.First());
        }

        public static void TestNonDefaultTopicData(TopicBuiltinTopicData data)
        {
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.Deadline);
            Assert.IsNotNull(data.DestinationOrder);
            Assert.IsNotNull(data.Durability);
            Assert.IsNotNull(data.DurabilityService);
            Assert.IsNotNull(data.History);
            Assert.IsNotNull(data.LatencyBudget);
            Assert.IsNotNull(data.Lifespan);
            Assert.IsNotNull(data.Liveliness);
            Assert.IsNotNull(data.Ownership);
            Assert.IsNotNull(data.Reliability);
            Assert.IsNotNull(data.ResourceLimits);
            Assert.IsNotNull(data.TopicData);
            Assert.IsNotNull(data.TransportPriority);
            Assert.AreEqual(5, data.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, data.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos, data.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.TransientLocalDurabilityQos, data.Durability.Kind);
            Assert.AreEqual(HistoryQosPolicyKind.KeepAllHistoryQos, data.DurabilityService.HistoryKind);
            Assert.AreEqual(5, data.DurabilityService.HistoryDepth);
            Assert.AreEqual(5, data.DurabilityService.MaxInstances);
            Assert.AreEqual(5, data.DurabilityService.MaxSamples);
            Assert.AreEqual(5, data.DurabilityService.MaxSamplesPerInstance);
            Assert.IsNotNull(data.DurabilityService.ServiceCleanupDelay);
            Assert.AreEqual(5, data.DurabilityService.ServiceCleanupDelay.Seconds);
            Assert.AreEqual((uint)5, data.DurabilityService.ServiceCleanupDelay.NanoSeconds);
            Assert.AreEqual(HistoryQosPolicyKind.KeepAllHistoryQos, data.History.Kind);
            Assert.AreEqual(5, data.History.Depth);
            Assert.AreEqual(5, data.LatencyBudget.Duration.Seconds);
            Assert.AreEqual((uint)5, data.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(5, data.Lifespan.Duration.Seconds);
            Assert.AreEqual((uint)5, data.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, data.Liveliness.Kind);
            Assert.AreEqual(5, data.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual((uint)5, data.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, data.Ownership.Kind);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, data.Reliability.Kind);
            Assert.AreEqual(5, data.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual((uint)5, data.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(5, data.ResourceLimits.MaxInstances);
            Assert.AreEqual(5, data.ResourceLimits.MaxSamples);
            Assert.AreEqual(5, data.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(1, data.TopicData.Value.Count());
            Assert.AreEqual(0x5, data.TopicData.Value.First());
            Assert.AreEqual(5, data.TransportPriority.Value);
        }
        #endregion

        #region Extensions
        public static bool WaitForSubscriptions(this DataWriter writer, int subscriptionsCount, int milliseconds)
        {
            PublicationMatchedStatus status = new PublicationMatchedStatus();
            writer.GetPublicationMatchedStatus(ref status);
            int count = milliseconds / 100;
            while (status.CurrentCount != subscriptionsCount && count > 0)
            {
                System.Threading.Thread.Sleep(100);
                writer.GetPublicationMatchedStatus(ref status);
                count--;
            }

            if (count == 0 && status.CurrentCount != subscriptionsCount)
            {
                return false;
            }

            return true;
        }

        public static bool WaitForPublications(this DataReader reader, int publicationsCount, int milliseconds)
        {
            List<InstanceHandle> handles = new List<InstanceHandle>();
            reader.GetMatchedPublications(handles);
            int count = milliseconds / 100;
            while (handles.Count != publicationsCount && count > 0)
            {
                System.Threading.Thread.Sleep(100);
                reader.GetMatchedPublications(handles);
                count--;
            }

            if (count == 0 && handles.Count != publicationsCount)
            {
                return false;
            }

            return true;
        }

        public static bool WaitForParticipants(this DomainParticipant participant, int participantCount, int milliseconds)
        {
            List<InstanceHandle> handles = new List<InstanceHandle>();
            participant.GetDiscoveredParticipants(handles);
            int count = milliseconds / 100;
            while (handles.Count != participantCount && count > 0)
            {
                System.Threading.Thread.Sleep(100);
                participant.GetDiscoveredParticipants(handles);
                count--;
            }

            if (count == 0 && handles.Count != participantCount)
            {
                return false;
            }

            return true;
        }

        public static void BindRtpsUdpTransportConfig(this Entity entity)
        {
            string guid = Guid.NewGuid().ToString("N");
            string configName = "openddsharp_rtps_interop_" + guid;
            string instName = "internal_openddsharp_rtps_transport_" + guid;

            TransportConfig config = TransportRegistry.Instance.CreateConfig(configName);
            TransportInst inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
            RtpsUdpInst rui = new RtpsUdpInst(inst);
            config.Insert(inst);

            TransportRegistry.Instance.BindConfig(configName, entity);
        }

        public static void BindTcpTransportConfig(this Entity entity)
        {
            string guid = Guid.NewGuid().ToString("N");
            string configName = "openddsharp_tcp_" + guid;
            string instName = "internal_openddsharp_tcp_transport_" + guid;

            TransportConfig config = TransportRegistry.Instance.CreateConfig(configName);
            TransportInst inst = TransportRegistry.Instance.CreateInst(instName, "tcp");
            TcpInst tcpi = new TcpInst(inst)
            {
                LocalAddress = "127.0.0.1:0",
                DatalinkReleaseDelay = 1000,

            };
            config.Insert(inst);

            TransportRegistry.Instance.BindConfig(config, entity);
        }

        public static DateTime ToDateTime(this Timestamp timestamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(timestamp.Seconds).AddMilliseconds(timestamp.NanoSeconds / 1000000);
        }

        public static Timestamp ToTimestamp(this DateTime dateTime)
        {
            DateTime epoc = new DateTime(1970, 1, 1);
            TimeSpan span = dateTime - epoc;

            return new Timestamp
            {
                Seconds = span.Seconds,
                NanoSeconds = (uint)span.Milliseconds / 1000000
            };
        }
        #endregion
    }
}
