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
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.Standard.UnitTest.Helpers
{
    public static class TestHelper
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
            Assert.AreEqual(1, qos.UserData.Value.Count);
            Assert.IsNotNull(qos.TimeBasedFilter.MinimumSeparation);
            Assert.AreEqual(3, qos.TimeBasedFilter.MinimumSeparation.Seconds);
            Assert.AreEqual((uint)3, qos.TimeBasedFilter.MinimumSeparation.NanoSeconds);
            Assert.AreEqual(0x5, qos.UserData.Value.First());
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
                Thread.Sleep(100);
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
                Thread.Sleep(100);
                reader.GetMatchedPublications(handles);
                count--;
            }

            if (count == 0 && handles.Count != publicationsCount)
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
        #endregion
    }
}
