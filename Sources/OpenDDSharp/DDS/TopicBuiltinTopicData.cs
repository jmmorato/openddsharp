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
    /// Class that contains information about available <see cref="Topic" />s within the system.
    /// </summary>
    /// <remarks>
    /// The DCPSTopic topic communicates the existence of topics by means of the TopicBuiltinTopicData datatype. Each TopicBuiltinTopicData sample in a Domain represents a <see cref="Topic" /> 
    /// in that Domain: a new TopicBuiltinTopicData instance is created when a newly-added <see cref="Topic" />  is enabled. However, the instance is not disposed when a <see cref="Topic" />  is 
    /// deleted by its participant because a topic lifecycle is tied to the lifecycle of a Domain, not to the lifecycle of an individual participant.
    /// An updated TopicBuiltinTopicData sample is written each time a <see cref="Topic" />  modifies one or more of its QoS policy values.
    /// </remarks>
    public struct TopicBuiltinTopicData : IEquatable<TopicBuiltinTopicData>
    {
        #region Fields
        private List<IntPtr> toRelease;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the global unique identifier of the <see cref="Topic" />.
        /// </summary>
        public BuiltinTopicKey Key { get; internal set; }

        /// <summary>
        /// Gets the name of the <see cref="Topic" />.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the type name of the <see cref="Topic" />.
        /// </summary>
        public string TypeName { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DurabilityQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public DurabilityQosPolicy Durability { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DurabilityServiceQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public DurabilityServiceQosPolicy DurabilityService { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DeadlineQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public DeadlineQosPolicy Deadline { get; internal set; }

        /// <summary>
        /// Gets the <see cref="LatencyBudgetQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public LatencyBudgetQosPolicy LatencyBudget { get; internal set; }

        /// <summary>
        /// Gets the <see cref="LivelinessQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public LivelinessQosPolicy Liveliness { get; internal set; }

        /// <summary>
        /// Gets the <see cref="ReliabilityQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public ReliabilityQosPolicy Reliability { get; internal set; }

        /// <summary>
        /// Gets the <see cref="TransportPriorityQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public TransportPriorityQosPolicy TransportPriority { get; internal set; }

        /// <summary>
        /// Gets the <see cref="LifespanQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public LifespanQosPolicy Lifespan { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DestinationOrderQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public DestinationOrderQosPolicy DestinationOrder { get; internal set; }

        /// <summary>
        /// Gets the <see cref="HistoryQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public HistoryQosPolicy History { get; internal set; }

        /// <summary>
        /// Gets the <see cref="ResourceLimitsQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public ResourceLimitsQosPolicy ResourceLimits { get; internal set; }

        /// <summary>
        /// Gets the <see cref="OwnershipQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public OwnershipQosPolicy Ownership { get; internal set; }

        /// <summary>
        /// Gets the <see cref="TopicDataQosPolicy" /> attached to the <see cref="Topic" />.
        /// </summary>
        public TopicDataQosPolicy TopicData { get; internal set; }
        #endregion

        #region Methods
        internal TopicBuiltinTopicDataWrapper ToNative()
        {
            if (toRelease == null)
            {
                toRelease = new List<IntPtr>();
            }

            var data = new TopicBuiltinTopicDataWrapper
            {
                Deadline = Deadline,
                DestinationOrder = DestinationOrder,
                Durability = Durability,
                DurabilityService = DurabilityService,
                Key = Key,
                LatencyBudget = LatencyBudget,
                Lifespan = Lifespan,
                Liveliness = Liveliness,
                Ownership = Ownership,
                Reliability = Reliability,
                History = History,
                ResourceLimits = ResourceLimits,
                TransportPriority = TransportPriority,
            };

            if (Name != null)
            {
                data.Name = Marshal.StringToHGlobalAnsi(Name);
                toRelease.Add(data.Name);
            }

            if (TypeName != null)
            {
                data.TypeName = Marshal.StringToHGlobalAnsi(TypeName);
                toRelease.Add(data.TypeName);
            }

            if (TopicData != null)
            {
                data.TopicData = TopicData.ToNative();
            }

            return data;
        }

        internal void FromNative(TopicBuiltinTopicDataWrapper wrapper)
        {
            Deadline = wrapper.Deadline;
            DestinationOrder = wrapper.DestinationOrder;
            Durability = wrapper.Durability;
            DurabilityService = wrapper.DurabilityService;
            Key = wrapper.Key;
            LatencyBudget = wrapper.LatencyBudget;
            Lifespan = wrapper.Lifespan;
            Liveliness = wrapper.Liveliness;
            Ownership = wrapper.Ownership;
            Reliability = wrapper.Reliability;
            History = wrapper.History;
            ResourceLimits = wrapper.ResourceLimits;
            TransportPriority = wrapper.TransportPriority;

            if (wrapper.Name != IntPtr.Zero)
            {
                Name = Marshal.PtrToStringAnsi(wrapper.Name);
            }
            else
            {
                Name = null;
            }

            if (wrapper.TypeName != IntPtr.Zero)
            {
                TypeName = Marshal.PtrToStringAnsi(wrapper.TypeName);
            }
            else
            {
                TypeName = null;
            }

            if (TopicData == null)
            {
                TopicData = new TopicDataQosPolicy();
            }
            TopicData.FromNative(wrapper.TopicData);
        }

        internal void Release()
        {
            TopicData?.Release();

            foreach (IntPtr ptr in toRelease)
            {
                Marshal.FreeHGlobal(ptr);
            }

            toRelease.Clear();
        }
        #endregion

        #region IEquatable<TopicBuiltinTopicData> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(TopicBuiltinTopicData other)
        {
            return Key == other.Key &&
                   Name == other.Name &&
                   TypeName == other.TypeName &&
                   Durability == other.Durability &&
                   DurabilityService == other.DurabilityService &&
                   Deadline == other.Deadline &&
                   LatencyBudget == other.LatencyBudget &&
                   Liveliness == other.Liveliness &&
                   Reliability == other.Reliability &&
                   TransportPriority == other.TransportPriority &&
                   Lifespan == other.Lifespan &&
                   DestinationOrder == other.DestinationOrder &&
                   History == other.History &&
                   ResourceLimits == other.ResourceLimits &&
                   Ownership == other.Ownership &&
                   TopicData == other.TopicData;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is TopicBuiltinTopicData other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1476352029;
            hashCode = (hashCode * -1521134295) + Key.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(TypeName);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DurabilityQosPolicy>.Default.GetHashCode(Durability);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DurabilityServiceQosPolicy>.Default.GetHashCode(DurabilityService);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DeadlineQosPolicy>.Default.GetHashCode(Deadline);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LatencyBudgetQosPolicy>.Default.GetHashCode(LatencyBudget);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LivelinessQosPolicy>.Default.GetHashCode(Liveliness);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ReliabilityQosPolicy>.Default.GetHashCode(Reliability);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TransportPriorityQosPolicy>.Default.GetHashCode(TransportPriority);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LifespanQosPolicy>.Default.GetHashCode(Lifespan);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DestinationOrderQosPolicy>.Default.GetHashCode(DestinationOrder);
            hashCode = (hashCode * -1521134295) + EqualityComparer<HistoryQosPolicy>.Default.GetHashCode(History);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ResourceLimitsQosPolicy>.Default.GetHashCode(ResourceLimits);
            hashCode = (hashCode * -1521134295) + EqualityComparer<OwnershipQosPolicy>.Default.GetHashCode(Ownership);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TopicDataQosPolicy>.Default.GetHashCode(TopicData);

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
        public static bool operator ==(TopicBuiltinTopicData left, TopicBuiltinTopicData right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(TopicBuiltinTopicData left, TopicBuiltinTopicData right)
        {
            return !left.Equals(right);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
    internal struct TopicBuiltinTopicDataWrapper
    {
        #region Fields
        public BuiltinTopicKey Key;
        public IntPtr Name;
        public IntPtr TypeName;
        public DurabilityQosPolicyWrapper Durability;
        public DurabilityServiceQosPolicyWrapper DurabilityService;
        public DeadlineQosPolicyWrapper Deadline;
        public LatencyBudgetQosPolicyWrapper LatencyBudget;
        public LivelinessQosPolicyWrapper Liveliness;
        public ReliabilityQosPolicyWrapper Reliability;
        public TransportPriorityQosPolicyWrapper TransportPriority;
        public LifespanQosPolicyWrapper Lifespan;
        public DestinationOrderQosPolicyWrapper DestinationOrder;
        public HistoryQosPolicyWrapper History;
        public ResourceLimitsQosPolicyWrapper ResourceLimits;
        public OwnershipQosPolicyWrapper Ownership;
        public TopicDataQosPolicyWrapper TopicData;
        #endregion
    }
}