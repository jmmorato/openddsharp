/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
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
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.UnitTest.Helpers
{
    internal static class TestHelper
    {
        #region DomainParticipant QoS
        public static DomainParticipantQos CreateNonDefaultDomainParticipantQos()
        {
            var qos = new DomainParticipantQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
                UserData =
                {
                    Value = new List<byte> { 0x42 },
                },
            };

            return qos;
        }

        public static void TestDefaultDomainParticipantQos(DomainParticipantQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.UserData);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(0, qos.UserData.Value.Count);
        }

        public static void TestNonDefaultDomainParticipantQos(DomainParticipantQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.UserData);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(1, qos.UserData.Value.Count);
            Assert.AreEqual(0x42, qos.UserData.Value[0]);
        }
        #endregion

        #region Topic QoS
        public static TopicQos CreateNonDefaultTopicQos()
        {
            var qos = new TopicQos
            {
                Deadline =
                {
                    Period = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 0,
                    },
                },
                DestinationOrder =
                {
                    Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos,
                },
                Durability =
                {
                    Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos,
                },
                DurabilityService =
                {
                    HistoryDepth = 5,
                    HistoryKind = HistoryQosPolicyKind.KeepAllHistoryQos,
                    MaxInstances = 5,
                    MaxSamples = 5,
                    MaxSamplesPerInstance = 5,
                    ServiceCleanupDelay = new Duration { Seconds = 5, NanoSeconds = 5 },
                },
                History =
                {
                    Depth = 5,
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
                LatencyBudget =
                {
                    Duration = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                Lifespan =
                {
                    Duration = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                Liveliness =
                {
                    Kind = LivelinessQosPolicyKind.ManualByParticipantLivelinessQos,
                    LeaseDuration = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                Ownership =
                {
                    Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos,
                },
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                    MaxBlockingTime = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                ResourceLimits =
                {
                    MaxInstances = 5,
                    MaxSamples = 5,
                    MaxSamplesPerInstance = 5,
                },
                TopicData =
                {
                    Value = new List<byte> { 0x5 },
                },
                TransportPriority =
                {
                    Value = 5,
                },
            };

            return qos;
        }

        public static void TestDefaultTopicQos(TopicQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

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
            Assert.AreEqual(0, qos.TopicData.Value.Count);
            Assert.AreEqual(0, qos.TransportPriority.Value);
        }

        public static void TestNonDefaultTopicQos(TopicQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

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
            Assert.AreEqual(5U, qos.DurabilityService.ServiceCleanupDelay.NanoSeconds);
            Assert.AreEqual(HistoryQosPolicyKind.KeepAllHistoryQos, qos.History.Kind);
            Assert.AreEqual(5, qos.History.Depth);
            Assert.AreEqual(5, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual(5U, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(5, qos.Lifespan.Duration.Seconds);
            Assert.AreEqual(5U, qos.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, qos.Liveliness.Kind);
            Assert.AreEqual(5, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(5U, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, qos.Ownership.Kind);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, qos.Reliability.Kind);
            Assert.AreEqual(5, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual(5U, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(5, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(1, qos.TopicData.Value.Count);
            Assert.AreEqual(0x5, qos.TopicData.Value[0]);
            Assert.AreEqual(5, qos.TransportPriority.Value);
        }
        #endregion

        #region Publisher QoS
        public static PublisherQos CreateNonDefaultPublisherQos()
        {
            var qos = new PublisherQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
                GroupData =
                {
                    Value = new List<byte> { 0x42 },
                },
                Partition =
                {
                    Name = new List<string> { "TestPartition" },
                },
                Presentation =
                {
                    AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos,
                    CoherentAccess = true,
                    OrderedAccess = true,
                },
            };

            return qos;
        }

        public static void TestDefaultPublisherQos(PublisherQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(0, qos.GroupData.Value.Count);
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(0, qos.Partition.Name.Count);
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);
        }

        public static void TestNonDefaultPublisherQos(PublisherQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count);
            Assert.AreEqual(0x42, qos.GroupData.Value[0]);
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count);
            Assert.AreEqual("TestPartition", qos.Partition.Name[0]);
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);
        }
        #endregion

        #region Subscriber QoS
        public static SubscriberQos CreateNonDefaultSubscriberQos()
        {
            var qos = new SubscriberQos
            {
                EntityFactory =
                {
                    AutoenableCreatedEntities = false,
                },
                GroupData =
                {
                    Value = new List<byte> { 0x42 },
                },
                Partition =
                {
                    Name = new List<string> { "TestPartition" },
                },
                Presentation =
                {
                    AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos,
                    CoherentAccess = true,
                    OrderedAccess = true,
                },
            };

            return qos;
        }

        public static void TestDefaultSubscriberQos(SubscriberQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(0, qos.GroupData.Value.Count);
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(0, qos.Partition.Name.Count);
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);
        }

        public static void TestNonDefaultSubscriberQos(SubscriberQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count);
            Assert.AreEqual(0x42, qos.GroupData.Value[0]);
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count);
            Assert.AreEqual("TestPartition", qos.Partition.Name[0]);
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);
        }
        #endregion

        #region DataWriter QoS
        public static DataWriterQos CreateNonDefaultDataWriterQos()
        {
            var qos = new DataWriterQos
            {
                Deadline =
                {
                    Period = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 0,
                    },
                },
                DestinationOrder =
                {
                    Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos,
                },
                Durability =
                {
                    Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos,
                },
                DurabilityService =
                {
                    HistoryDepth = 5,
                    HistoryKind = HistoryQosPolicyKind.KeepAllHistoryQos,
                    MaxInstances = 5,
                    MaxSamples = 5,
                    MaxSamplesPerInstance = 5,
                },
                History =
                {
                    Depth = 5,
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
                LatencyBudget =
                {
                    Duration = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                Lifespan =
                {
                    Duration = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                Liveliness =
                {
                    Kind = LivelinessQosPolicyKind.ManualByParticipantLivelinessQos,
                    LeaseDuration = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                Ownership =
                {
                    Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos,
                },
                OwnershipStrength =
                {
                    Value = 5,
                },
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos,
                    MaxBlockingTime = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                ResourceLimits =
                {
                    MaxInstances = 5,
                    MaxSamples = 5,
                    MaxSamplesPerInstance = 5,
                },
                UserData =
                {
                    Value = new List<byte> { 0x5 },
                },
                TransportPriority =
                {
                    Value = 5,
                },
                WriterDataLifecycle =
                {
                    AutodisposeUnregisteredInstances = true,
                },
                Representation =
                {
                    Value = new List<short>
                    {
                        DataRepresentationQosPolicy.XCDR_DATA_REPRESENTATION,
                        DataRepresentationQosPolicy.XCDR2_DATA_REPRESENTATION,
                        DataRepresentationQosPolicy.XML_DATA_REPRESENTATION,
                    },
                },
            };

            return qos;
        }

        public static void TestDefaultDataWriterQos(DataWriterQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

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
            Assert.IsNotNull(qos.Representation.Value);
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
            Assert.AreEqual(100000000U, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(0, qos.TransportPriority.Value);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(0, qos.UserData.Value.Count);
            Assert.IsTrue(qos.WriterDataLifecycle.AutodisposeUnregisteredInstances);
            Assert.AreEqual(0, qos.Representation.Value.Count);
        }

        public static void TestNonDefaultDataWriterQos(DataWriterQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

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
            Assert.IsNotNull(qos.Representation.Value);

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
            Assert.AreEqual(5U, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.IsNotNull(qos.Lifespan.Duration);
            Assert.AreEqual(5, qos.Lifespan.Duration.Seconds);
            Assert.AreEqual(5U, qos.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, qos.Liveliness.Kind);
            Assert.IsNotNull(qos.Liveliness.LeaseDuration);
            Assert.AreEqual(5, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(5U, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, qos.Ownership.Kind);
            Assert.AreEqual(5, qos.OwnershipStrength.Value);
            Assert.AreEqual(ReliabilityQosPolicyKind.BestEffortReliabilityQos, qos.Reliability.Kind);
            Assert.IsNotNull(qos.Reliability.MaxBlockingTime);
            Assert.AreEqual(5, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual(5U, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(5, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(1, qos.UserData.Value.Count);
            Assert.AreEqual(0x5, qos.UserData.Value[0]);
            Assert.AreEqual(5, qos.TransportPriority.Value);
            Assert.IsTrue(qos.WriterDataLifecycle.AutodisposeUnregisteredInstances);
            Assert.AreEqual(3, qos.Representation.Value.Count);
        }
        #endregion

        #region DataReader QoS
        public static DataReaderQos CreateNonDefaultDataReaderQos()
        {
            var qos = new DataReaderQos
            {
                Deadline =
                {
                    Period = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 0,
                    },
                },
                DestinationOrder =
                {
                    Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos,
                },
                Durability =
                {
                    Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos,
                },
                History =
                {
                    Depth = 5,
                    Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                },
                LatencyBudget =
                {
                    Duration = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                Liveliness =
                {
                    Kind = LivelinessQosPolicyKind.ManualByParticipantLivelinessQos,
                    LeaseDuration = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                Ownership =
                {
                    Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos,
                },
                ReaderDataLifecycle =
                {
                    AutopurgeDisposedSamplesDelay = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                    AutopurgeNowriterSamplesDelay = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                Reliability =
                {
                    Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                    MaxBlockingTime = new Duration
                    {
                        Seconds = 5,
                        NanoSeconds = 5,
                    },
                },
                ResourceLimits =
                {
                    MaxInstances = 5,
                    MaxSamples = 5,
                    MaxSamplesPerInstance = 5,
                },
                TimeBasedFilter =
                {
                    MinimumSeparation = new Duration
                    {
                        Seconds = 3,
                        NanoSeconds = 3,
                    },
                },
                UserData =
                {
                    Value = new List<byte> { 0x5 },
                },
                Representation =
                {
                    Value = new List<short>
                    {
                        DataRepresentationQosPolicy.XCDR_DATA_REPRESENTATION,
                        DataRepresentationQosPolicy.XCDR2_DATA_REPRESENTATION,
                        DataRepresentationQosPolicy.XML_DATA_REPRESENTATION,
                    },
                },
            };

            return qos;
        }

        public static void TestDefaultDataReaderQos(DataReaderQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

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
            Assert.IsNotNull(qos.Representation.Value);
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
            Assert.AreEqual(0U, qos.TimeBasedFilter.MinimumSeparation.NanoSeconds);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(0, qos.UserData.Value.Count);
            Assert.AreEqual(0, qos.Representation.Value.Count);
        }

        public static void TestNonDefaultDataReaderQos(DataReaderQos qos)
        {
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

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
            Assert.IsNotNull(qos.Representation.Value);

            Assert.IsNotNull(qos.Deadline.Period);
            Assert.AreEqual(5, qos.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, qos.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos, qos.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.TransientLocalDurabilityQos, qos.Durability.Kind);
            Assert.AreEqual(HistoryQosPolicyKind.KeepAllHistoryQos, qos.History.Kind);
            Assert.AreEqual(5, qos.History.Depth);
            Assert.IsNotNull(qos.LatencyBudget.Duration);
            Assert.AreEqual(5, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual(5U, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, qos.Liveliness.Kind);
            Assert.IsNotNull(qos.Liveliness.LeaseDuration);
            Assert.AreEqual(5, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(5U, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, qos.Ownership.Kind);
            Assert.IsNotNull(qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay);
            Assert.AreEqual(5, qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay.Seconds);
            Assert.AreEqual(5U, qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay.NanoSeconds);
            Assert.IsNotNull(qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay);
            Assert.AreEqual(5, qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay.Seconds);
            Assert.AreEqual(5U, qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay.NanoSeconds);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, qos.Reliability.Kind);
            Assert.IsNotNull(qos.Reliability.MaxBlockingTime);
            Assert.AreEqual(5, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual(5U, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(5, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(1, qos.UserData.Value.Count);
            Assert.IsNotNull(qos.TimeBasedFilter.MinimumSeparation);
            Assert.AreEqual(3, qos.TimeBasedFilter.MinimumSeparation.Seconds);
            Assert.AreEqual(3U, qos.TimeBasedFilter.MinimumSeparation.NanoSeconds);
            Assert.AreEqual(0x5, qos.UserData.Value[0]);
            Assert.AreEqual(3, qos.Representation.Value.Count);
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
            Assert.AreEqual(5U, data.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, data.Liveliness.Kind);
            Assert.IsNotNull(data.Liveliness.LeaseDuration);
            Assert.AreEqual(5, data.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(5U, data.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, data.Ownership.Kind);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, data.Reliability.Kind);
            Assert.IsNotNull(data.Reliability.MaxBlockingTime);
            Assert.AreEqual(5, data.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual(5U, data.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(1, data.UserData.Value.Count);
            Assert.IsNotNull(data.TimeBasedFilter.MinimumSeparation);
            Assert.AreEqual(3, data.TimeBasedFilter.MinimumSeparation.Seconds);
            Assert.AreEqual(3U, data.TimeBasedFilter.MinimumSeparation.NanoSeconds);
            Assert.AreEqual(0x5, data.UserData.Value[0]);
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
            Assert.AreEqual(5U, data.LatencyBudget.Duration.NanoSeconds);
            Assert.IsNotNull(data.Lifespan.Duration);
            Assert.AreEqual(5, data.Lifespan.Duration.Seconds);
            Assert.AreEqual(5U, data.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, data.Liveliness.Kind);
            Assert.IsNotNull(data.Liveliness.LeaseDuration);
            Assert.AreEqual(5, data.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(5U, data.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.SharedOwnershipQos, data.Ownership.Kind);
            Assert.AreEqual(5, data.OwnershipStrength.Value);
            Assert.AreEqual(ReliabilityQosPolicyKind.BestEffortReliabilityQos, data.Reliability.Kind);
            Assert.IsNotNull(data.Reliability.MaxBlockingTime);
            Assert.AreEqual(5, data.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual(5U, data.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(1, data.UserData.Value.Count);
            Assert.AreEqual(0x5, data.UserData.Value[0]);
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
            Assert.AreEqual(5U, data.DurabilityService.ServiceCleanupDelay.NanoSeconds);
            Assert.AreEqual(HistoryQosPolicyKind.KeepAllHistoryQos, data.History.Kind);
            Assert.AreEqual(5, data.History.Depth);
            Assert.AreEqual(5, data.LatencyBudget.Duration.Seconds);
            Assert.AreEqual(5U, data.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(5, data.Lifespan.Duration.Seconds);
            Assert.AreEqual(5U, data.Lifespan.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, data.Liveliness.Kind);
            Assert.AreEqual(5, data.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(5U, data.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, data.Ownership.Kind);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, data.Reliability.Kind);
            Assert.AreEqual(5, data.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual(5U, data.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(5, data.ResourceLimits.MaxInstances);
            Assert.AreEqual(5, data.ResourceLimits.MaxSamples);
            Assert.AreEqual(5, data.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(1, data.TopicData.Value.Count);
            Assert.AreEqual(0x5, data.TopicData.Value[0]);
            Assert.AreEqual(5, data.TransportPriority.Value);
        }
        #endregion

        #region Extensions
        public static bool WaitForSubscriptions(this DataWriter writer, int subscriptionsCount, int milliseconds)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            PublicationMatchedStatus status = default;
            writer.GetPublicationMatchedStatus(ref status);
            int count = milliseconds / 100;
            while (status.CurrentCount != subscriptionsCount && count > 0)
            {
                Thread.Sleep(100);
                writer.GetPublicationMatchedStatus(ref status);
                count--;
            }

            return count != 0 || status.CurrentCount == subscriptionsCount;
        }

        public static bool WaitForPublications(this DataReader reader, int publicationsCount, int milliseconds)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var handles = new List<InstanceHandle>();
            reader.GetMatchedPublications(handles);
            var count = milliseconds / 100;
            while (handles.Count != publicationsCount && count > 0)
            {
                Thread.Sleep(100);
                reader.GetMatchedPublications(handles);
                count--;
            }

            return count != 0 || handles.Count == publicationsCount;
        }

        public static bool WaitForParticipants(this DomainParticipant participant, int participantCount, int milliseconds)
        {
            var handles = new List<InstanceHandle>();
            participant.GetDiscoveredParticipants(handles);
            var count = milliseconds / 100;
            while (handles.Count != participantCount && count > 0)
            {
                Thread.Sleep(100);
                participant.GetDiscoveredParticipants(handles);
                count--;
            }

            return count != 0 || handles.Count == participantCount;
        }

        public static void BindRtpsUdpTransportConfig(this Entity entity)
        {
            var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var configName = "openddsharp_rtps_interop_" + guid;
            var instName = "internal_openddsharp_rtps_transport_" + guid;

            var config = TransportRegistry.Instance.CreateConfig(configName);
            var inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
            var rui = new RtpsUdpInst(inst)
            {
                UseMulticast = false,
            };
            config.Insert(rui);

            TransportRegistry.Instance.BindConfig(configName, entity);
        }

        public static void BindUdpTransportConfig(this Entity entity)
        {
            var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var configName = "openddsharp_udp_interop_" + guid;
            var instName = "internal_openddsharp_udp_transport_" + guid;

            var config = TransportRegistry.Instance.CreateConfig(configName);
            var inst = TransportRegistry.Instance.CreateInst(instName, "udp");
            var ui = new UdpInst(inst);
            config.Insert(ui);

            TransportRegistry.Instance.BindConfig(configName, entity);
        }

        public static void BindTcpTransportConfig(this Entity entity)
        {
            var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var configName = "openddsharp_tcp_" + guid;
            var instName = "internal_openddsharp_tcp_transport_" + guid;

            var config = TransportRegistry.Instance.CreateConfig(configName);
            var inst = TransportRegistry.Instance.CreateInst(instName, "tcp");
            var tcpInst = new TcpInst(inst)
            {
                LocalAddress = "localhost:0",
            };
            config.Insert(tcpInst);

            TransportRegistry.Instance.BindConfig(configName, entity);
        }

        public static void BindShmemTransportConfig(this Entity entity)
        {
            var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var configName = "openddsharp_shmem_" + guid;
            var instName = "internal_openddsharp_shmem_transport_" + guid;

            var config = TransportRegistry.Instance.CreateConfig(configName);
            var inst = TransportRegistry.Instance.CreateInst(instName, "shmem");
            var shmemInst = new ShmemInst(inst);
            config.Insert(shmemInst);

            TransportRegistry.Instance.BindConfig(configName, entity);
        }

        public static Timestamp ToTimestamp(this DateTime dateTime)
        {
            var epoc = new DateTime(1970, 1, 1);
            var span = dateTime - epoc;

            return new Timestamp
            {
                Seconds = span.Seconds,
                NanoSeconds = (uint)span.Milliseconds / 1000000,
            };
        }

        public static void CreateWaitSetThread(ManualResetEventSlim evt, StatusCondition condition)
        {
            var waitSet = new WaitSet();
            waitSet.AttachCondition(condition);
            var thread = new Thread(() =>
            {
                var isSet = false;
                while (!isSet)
                {
                    ICollection<Condition> conditions = new List<Condition>();
                    waitSet.Wait(conditions);

                    if (!conditions.Any(cond => cond == condition && cond.TriggerValue))
                    {
                        continue;
                    }

                    evt.Set();

                    isSet = true;
                }
            })
            {
                IsBackground = true,
            };
            thread.Start();
        }
        #endregion
    }
}
