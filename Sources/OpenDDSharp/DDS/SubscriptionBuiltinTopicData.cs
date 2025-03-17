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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// Class that contains information about available <see cref="DataReader" />s within the system.
    /// </summary>
    /// <remarks>
    /// The DCPSSubscription topic communicates the existence of <see cref="DataReader" />s by means of the SubscriptionBuiltinTopicData datatype.
    /// Each SubscriptionBuiltinTopicData sample in a Domain represents a datareader in that Domain: a new SubscriptionBuiltinTopicData instance is created when a newly-added
    /// <see cref="DataReader" /> is enabled, and it is disposed when that <see cref="DataReader" /> is deleted. An updated SubscriptionBuiltinTopicData sample is written each time
    /// the <see cref="DataReader" /> (or the <see cref="Subscriber" /> to which it belongs) modifies a QoS policy that applies to the entities connected to it.
    /// </remarks>
    public struct SubscriptionBuiltinTopicData : IEquatable<SubscriptionBuiltinTopicData>
    {
        #region Fields
        private List<IntPtr> toRelease;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the global unique identifier of the <see cref="DataReader" />.
        /// </summary>
        public BuiltinTopicKey Key { get; internal set; }

        /// <summary>
        /// Gets the global unique identifier of the <see cref="DomainParticipant" /> to which the <see cref="DataReader" /> belongs.
        /// </summary>
        public BuiltinTopicKey ParticipantKey { get; internal set; }

        /// <summary>
        /// Gets the name of the <see cref="Topic" /> used by the <see cref="DataReader" />.
        /// </summary>
        public string TopicName { get; internal set; }

        /// <summary>
        /// Gets the type name of the <see cref="Topic" /> used by the <see cref="DataReader" />.
        /// </summary>
        public string TypeName { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DurabilityQosPolicy" /> attached to the <see cref="DataReader" />.
        /// </summary>
        public DurabilityQosPolicy Durability { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DeadlineQosPolicy" /> attached to the <see cref="DataReader" />.
        /// </summary>
        public DeadlineQosPolicy Deadline { get; internal set; }

        /// <summary>
        /// Gets the <see cref="LatencyBudgetQosPolicy" /> attached to the <see cref="DataReader" />.
        /// </summary>
        public LatencyBudgetQosPolicy LatencyBudget { get; internal set; }

        /// <summary>
        /// Gets the <see cref="LivelinessQosPolicy" /> attached to the <see cref="DataReader" />.
        /// </summary>
        public LivelinessQosPolicy Liveliness { get; internal set; }

        /// <summary>
        /// Gets the <see cref="ReliabilityQosPolicy" /> attached to the <see cref="DataReader" />.
        /// </summary>
        public ReliabilityQosPolicy Reliability { get; internal set; }

        /// <summary>
        /// Gets the <see cref="OwnershipQosPolicy" /> attached to the <see cref="DataReader" />.
        /// </summary>
        public OwnershipQosPolicy Ownership { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DestinationOrderQosPolicy" /> attached to the <see cref="DataReader" />.
        /// </summary>
        public DestinationOrderQosPolicy DestinationOrder { get; internal set; }

        /// <summary>
        /// Gets the <see cref="UserDataQosPolicy" /> attached to the <see cref="DataReader" />.
        /// </summary>
        public UserDataQosPolicy UserData { get; internal set; }

        /// <summary>
        /// Gets the <see cref="TimeBasedFilterQosPolicy" /> attached to the <see cref="DataReader" />.
        /// </summary>
        public TimeBasedFilterQosPolicy TimeBasedFilter { get; internal set; }

        /// <summary>
        /// Gets the <see cref="PresentationQosPolicy" /> attached to the <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs.
        /// </summary>
        public PresentationQosPolicy Presentation { get; internal set; }

        /// <summary>
        /// Gets the <see cref="PartitionQosPolicy" /> attached to the  <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs.
        /// </summary>
        public PartitionQosPolicy Partition { get; internal set; }

        /// <summary>
        /// Gets the <see cref="TopicDataQosPolicy" /> attached to the  <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs.
        /// </summary>
        public TopicDataQosPolicy TopicData { get; internal set; }

        /// <summary>
        /// Gets the <see cref="GroupDataQosPolicy" /> attached to the  <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs.
        /// </summary>
        public GroupDataQosPolicy GroupData { get; internal set; }
        #endregion

        #region Methods
        internal SubscriptionBuiltinTopicDataWrapper ToNative()
        {
            if (toRelease == null)
            {
                toRelease = new List<IntPtr>();
            }

            var data = new SubscriptionBuiltinTopicDataWrapper
            {
                Deadline = Deadline,
                DestinationOrder = DestinationOrder,
                Durability = Durability,
                Key = Key,
                LatencyBudget = LatencyBudget,
                Liveliness = Liveliness,
                Ownership = Ownership,
                ParticipantKey = ParticipantKey,
                Presentation = Presentation,
                Reliability = Reliability,
                TimeBasedFilter = TimeBasedFilter,
            };

            if (Partition != null)
            {
                data.Partition = Partition.ToNative();
            }

            if (GroupData != null)
            {
                data.GroupData = GroupData.ToNative();
            }

            if (TopicData != null)
            {
                data.TopicData = TopicData.ToNative();
            }

            if (UserData != null)
            {
                data.UserData = UserData.ToNative();
            }

            if (TopicName != null)
            {
                data.TopicName = Marshal.StringToHGlobalAnsi(TopicName);
                toRelease.Add(data.TopicName);
            }

            if (TypeName != null)
            {
                data.TypeName = Marshal.StringToHGlobalAnsi(TypeName);
                toRelease.Add(data.TypeName);
            }

            return data;
        }

        internal void FromNative(SubscriptionBuiltinTopicDataWrapper wrapper)
        {
            Deadline = wrapper.Deadline;
            DestinationOrder = wrapper.DestinationOrder;
            Durability = wrapper.Durability;
            Key = wrapper.Key;
            LatencyBudget = wrapper.LatencyBudget;
            Liveliness = wrapper.Liveliness;
            Ownership = wrapper.Ownership;
            ParticipantKey = wrapper.ParticipantKey;
            Presentation = wrapper.Presentation;
            Reliability = wrapper.Reliability;
            TimeBasedFilter = wrapper.TimeBasedFilter;

            if (Partition == null)
            {
                Partition = new PartitionQosPolicy();
            }
            Partition.FromNative(wrapper.Partition);

            if (GroupData == null)
            {
                GroupData = new GroupDataQosPolicy();
            }
            GroupData.FromNative(wrapper.GroupData);

            if (TopicData == null)
            {
                TopicData = new TopicDataQosPolicy();
            }
            TopicData.FromNative(wrapper.TopicData);

            if (UserData == null)
            {
                UserData = new UserDataQosPolicy();
            }
            UserData.FromNative(wrapper.UserData);

            if (wrapper.TopicName != IntPtr.Zero)
            {
                TopicName = Marshal.PtrToStringAnsi(wrapper.TopicName);
            }
            else
            {
                TopicName = null;
            }

            if (wrapper.TypeName != IntPtr.Zero)
            {
                TypeName = Marshal.PtrToStringAnsi(wrapper.TypeName);
            }
            else
            {
                TypeName = null;
            }
        }

        internal void Release()
        {
            Partition?.Release();
            GroupData?.Release();
            TopicData?.Release();
            UserData?.Release();

            foreach (IntPtr ptr in toRelease)
            {
                Marshal.FreeHGlobal(ptr);
            }

            toRelease.Clear();
        }
        #endregion

        #region IEquatable<SubscriptionBuiltinTopicData> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(SubscriptionBuiltinTopicData other)
        {
            return Key == other.Key &&
                   ParticipantKey == other.ParticipantKey &&
                   TopicName == other.TopicName &&
                   TypeName == other.TypeName &&
                   Durability == other.Durability &&
                   Deadline == other.Deadline &&
                   LatencyBudget == other.LatencyBudget &&
                   Liveliness == other.Liveliness &&
                   Reliability == other.Reliability &&
                   UserData == other.UserData &&
                   Ownership == other.Ownership &&
                   DestinationOrder == other.DestinationOrder &&
                   Presentation == other.Presentation &&
                   Partition == other.Partition &&
                   TopicData == other.TopicData &&
                   GroupData == other.GroupData &&
                   TimeBasedFilter == other.TimeBasedFilter;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is SubscriptionBuiltinTopicData other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1476352029;
            hashCode = (hashCode * -1521134295) + Key.GetHashCode();
            hashCode = (hashCode * -1521134295) + ParticipantKey.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(TopicName);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(TypeName);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DurabilityQosPolicy>.Default.GetHashCode(Durability);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DeadlineQosPolicy>.Default.GetHashCode(Deadline);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LatencyBudgetQosPolicy>.Default.GetHashCode(LatencyBudget);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LivelinessQosPolicy>.Default.GetHashCode(Liveliness);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ReliabilityQosPolicy>.Default.GetHashCode(Reliability);
            hashCode = (hashCode * -1521134295) + EqualityComparer<OwnershipQosPolicy>.Default.GetHashCode(Ownership);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DestinationOrderQosPolicy>.Default.GetHashCode(DestinationOrder);
            hashCode = (hashCode * -1521134295) + EqualityComparer<UserDataQosPolicy>.Default.GetHashCode(UserData);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TimeBasedFilterQosPolicy>.Default.GetHashCode(TimeBasedFilter);
            hashCode = (hashCode * -1521134295) + EqualityComparer<PresentationQosPolicy>.Default.GetHashCode(Presentation);
            hashCode = (hashCode * -1521134295) + EqualityComparer<PartitionQosPolicy>.Default.GetHashCode(Partition);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TopicDataQosPolicy>.Default.GetHashCode(TopicData);
            hashCode = (hashCode * -1521134295) + EqualityComparer<GroupDataQosPolicy>.Default.GetHashCode(GroupData);
            return hashCode;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(SubscriptionBuiltinTopicData left, SubscriptionBuiltinTopicData right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(SubscriptionBuiltinTopicData left, SubscriptionBuiltinTopicData right)
        {
            return !left.Equals(right);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SubscriptionBuiltinTopicDataWrapper
    {
        #region Fields
        public BuiltinTopicKey Key;
        public BuiltinTopicKey ParticipantKey;
        public IntPtr TopicName;
        public IntPtr TypeName;
        public DurabilityQosPolicyWrapper Durability;
        public DeadlineQosPolicyWrapper Deadline;
        public LatencyBudgetQosPolicyWrapper LatencyBudget;
        public LivelinessQosPolicyWrapper Liveliness;
        public ReliabilityQosPolicyWrapper Reliability;
        public OwnershipQosPolicyWrapper Ownership;
        public DestinationOrderQosPolicyWrapper DestinationOrder;
        public UserDataQosPolicyWrapper UserData;
        public TimeBasedFilterQosPolicyWrapper TimeBasedFilter;
        public PresentationQosPolicyWrapper Presentation;
        public PartitionQosPolicyWrapper Partition;
        public TopicDataQosPolicyWrapper TopicData;
        public GroupDataQosPolicyWrapper GroupData;
        #endregion
    }
}