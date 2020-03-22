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
    /// Holds the <see cref="DataWriter" /> Quality of Service policies.
    /// </summary>
    public sealed class DataWriterQos : IEquatable<DataWriterQos>
    {
        #region Properties
        /// <summary>
        /// Gets the <see cref="DurabilityQosPolicy"/>.
        /// </summary>
        public DurabilityQosPolicy Durability { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DurabilityServiceQosPolicy"/>.
        /// </summary>
        public DurabilityServiceQosPolicy DurabilityService { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DeadlineQosPolicy"/>.
        /// </summary>
        public DeadlineQosPolicy Deadline { get; internal set; }

        /// <summary>
        /// Gets the <see cref="LatencyBudgetQosPolicy"/>.
        /// </summary>
        public LatencyBudgetQosPolicy LatencyBudget { get; internal set; }

        /// <summary>
        /// Gets the <see cref="LivelinessQosPolicy"/>.
        /// </summary>
        public LivelinessQosPolicy Liveliness { get; internal set; }

        /// <summary>
        /// Gets the <see cref="ReliabilityQosPolicy"/>.
        /// </summary>
        public ReliabilityQosPolicy Reliability { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DestinationOrderQosPolicy"/>.
        /// </summary>
        public DestinationOrderQosPolicy DestinationOrder { get; internal set; }

        /// <summary>
        /// Gets the <see cref="HistoryQosPolicy"/>.
        /// </summary>
        public HistoryQosPolicy History { get; internal set; }

        /// <summary>
        /// Gets the <see cref="ResourceLimitsQosPolicy"/>.
        /// </summary>
        public ResourceLimitsQosPolicy ResourceLimits { get; internal set; }

        /// <summary>
        /// Gets the <see cref="TransportPriorityQosPolicy"/>.
        /// </summary>
        public TransportPriorityQosPolicy TransportPriority { get; internal set; }

        /// <summary>
        /// Gets the <see cref="LifespanQosPolicy"/>.
        /// </summary>
        public LifespanQosPolicy Lifespan { get; internal set; }

        /// <summary>
        /// Gets the <see cref="OwnershipQosPolicy"/>.
        /// </summary>
        public OwnershipQosPolicy Ownership { get; internal set; }

        /// <summary>
        /// Gets the <see cref="UserDataQosPolicy"/>.
        /// </summary>
        public UserDataQosPolicy UserData { get; internal set; }

        /// <summary>
        /// Gets the <see cref="OwnershipStrengthQosPolicy"/>.
        /// </summary>
        public OwnershipStrengthQosPolicy OwnershipStrength { get; internal set; }

        /// <summary>
        /// Gets the <see cref="WriterDataLifecycleQosPolicy"/>.
        /// </summary>
        public WriterDataLifecycleQosPolicy WriterDataLifecycle { get; internal set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataWriterQos"/> class.
        /// </summary>
        public DataWriterQos()
        {
            Durability = new DurabilityQosPolicy();
            DurabilityService = new DurabilityServiceQosPolicy();
            Deadline = new DeadlineQosPolicy();
            LatencyBudget = new LatencyBudgetQosPolicy();
            Liveliness = new LivelinessQosPolicy();
            Reliability = new ReliabilityQosPolicy();
            DestinationOrder = new DestinationOrderQosPolicy();
            History = new HistoryQosPolicy();
            ResourceLimits = new ResourceLimitsQosPolicy();
            TransportPriority = new TransportPriorityQosPolicy();
            Lifespan = new LifespanQosPolicy();
            Ownership = new OwnershipQosPolicy();
            UserData = new UserDataQosPolicy();
            OwnershipStrength = new OwnershipStrengthQosPolicy();
            WriterDataLifecycle = new WriterDataLifecycleQosPolicy();
        }
        #endregion

        #region Methods
        internal DataWriterQosWrapper ToNative()
        {
            var data = new DataWriterQosWrapper
            {
                Durability = Durability,
                DurabilityService = DurabilityService,
                Deadline = Deadline,
                LatencyBudget = LatencyBudget,
                Liveliness = Liveliness,
                Reliability = Reliability,
                DestinationOrder = DestinationOrder,
                History = History,
                ResourceLimits = ResourceLimits,
                TransportPriority = TransportPriority,
                Lifespan = Lifespan,
                Ownership = Ownership,
                OwnershipStrength = OwnershipStrength,
                WriterDataLifecycle = WriterDataLifecycle,
            };

            if (UserData != null)
            {
                data.UserData = UserData.ToNative();
            }

            return data;
        }

        internal void FromNative(DataWriterQosWrapper wrapper)
        {
            Durability = wrapper.Durability;
            DurabilityService = wrapper.DurabilityService;
            Deadline = wrapper.Deadline;
            LatencyBudget = wrapper.LatencyBudget;
            Liveliness = wrapper.Liveliness;
            Reliability = wrapper.Reliability;
            DestinationOrder = wrapper.DestinationOrder;
            History = wrapper.History;
            ResourceLimits = wrapper.ResourceLimits;
            TransportPriority = wrapper.TransportPriority;
            Lifespan = wrapper.Lifespan;
            Ownership = wrapper.Ownership;
            OwnershipStrength = wrapper.OwnershipStrength;
            WriterDataLifecycle = wrapper.WriterDataLifecycle;

            if (UserData == null)
            {
                UserData = new UserDataQosPolicy();
            }
            UserData.FromNative(wrapper.UserData);
        }

        internal void Release()
        {
            UserData?.Release();
        }
        #endregion

        #region IEquatable<DataWriterQos> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(DataWriterQos other)
        {
            if (other == null)
            {
                return false;
            }

            return Durability == other.Durability &&
                   DurabilityService == other.DurabilityService &&
                   Deadline == other.Deadline &&
                   LatencyBudget == other.LatencyBudget &&
                   Liveliness == other.Liveliness &&
                   Reliability == other.Reliability &&
                   DestinationOrder == other.DestinationOrder &&
                   History == other.History &&
                   ResourceLimits == other.ResourceLimits &&
                   TransportPriority == other.TransportPriority &&
                   Lifespan == other.Lifespan &&
                   Ownership == other.Ownership &&
                   UserData == other.UserData &&
                   OwnershipStrength == other.OwnershipStrength &&
                   WriterDataLifecycle == other.WriterDataLifecycle;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is DataWriterQos other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1476352029;
            hashCode = (hashCode * -1521134295) + EqualityComparer<DurabilityQosPolicy>.Default.GetHashCode(Durability);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DurabilityServiceQosPolicy>.Default.GetHashCode(DurabilityService);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DeadlineQosPolicy>.Default.GetHashCode(Deadline);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LatencyBudgetQosPolicy>.Default.GetHashCode(LatencyBudget);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LivelinessQosPolicy>.Default.GetHashCode(Liveliness);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ReliabilityQosPolicy>.Default.GetHashCode(Reliability);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DestinationOrderQosPolicy>.Default.GetHashCode(DestinationOrder);
            hashCode = (hashCode * -1521134295) + EqualityComparer<HistoryQosPolicy>.Default.GetHashCode(History);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ResourceLimitsQosPolicy>.Default.GetHashCode(ResourceLimits);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TransportPriorityQosPolicy>.Default.GetHashCode(TransportPriority);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LifespanQosPolicy>.Default.GetHashCode(Lifespan);
            hashCode = (hashCode * -1521134295) + EqualityComparer<OwnershipQosPolicy>.Default.GetHashCode(Ownership);
            hashCode = (hashCode * -1521134295) + EqualityComparer<UserDataQosPolicy>.Default.GetHashCode(UserData);
            hashCode = (hashCode * -1521134295) + EqualityComparer<OwnershipStrengthQosPolicy>.Default.GetHashCode(OwnershipStrength);
            hashCode = (hashCode * -1521134295) + EqualityComparer<WriterDataLifecycleQosPolicy>.Default.GetHashCode(WriterDataLifecycle);
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
        public static bool operator ==(DataWriterQos left, DataWriterQos right)
        {
            if (left is null && right is null)
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(DataWriterQos left, DataWriterQos right)
        {
            if (left is null && right is null)
            {
                return false;
            }

            if (left is null || right is null)
            {
                return true;
            }

            return !left.Equals(right);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DataWriterQosWrapper
    {
        #region Fields
        [MarshalAs(UnmanagedType.Struct)]
        public DurabilityQosPolicyWrapper Durability;
        [MarshalAs(UnmanagedType.Struct)]
        public DurabilityServiceQosPolicyWrapper DurabilityService;
        [MarshalAs(UnmanagedType.Struct)]
        public DeadlineQosPolicyWrapper Deadline;
        [MarshalAs(UnmanagedType.Struct)]
        public LatencyBudgetQosPolicyWrapper LatencyBudget;
        [MarshalAs(UnmanagedType.Struct)]
        public LivelinessQosPolicyWrapper Liveliness;
        [MarshalAs(UnmanagedType.Struct)]
        public ReliabilityQosPolicyWrapper Reliability;
        [MarshalAs(UnmanagedType.Struct)]
        public DestinationOrderQosPolicyWrapper DestinationOrder;
        [MarshalAs(UnmanagedType.Struct)]
        public HistoryQosPolicyWrapper History;
        [MarshalAs(UnmanagedType.Struct)]
        public ResourceLimitsQosPolicyWrapper ResourceLimits;
        [MarshalAs(UnmanagedType.Struct)]
        public TransportPriorityQosPolicyWrapper TransportPriority;
        [MarshalAs(UnmanagedType.Struct)]
        public LifespanQosPolicyWrapper Lifespan;
        [MarshalAs(UnmanagedType.Struct)]
        public OwnershipQosPolicyWrapper Ownership;
        [MarshalAs(UnmanagedType.Struct)]
        public UserDataQosPolicyWrapper UserData;
        [MarshalAs(UnmanagedType.Struct)]
        public OwnershipStrengthQosPolicyWrapper OwnershipStrength;
        [MarshalAs(UnmanagedType.Struct)]
        public TimeBasedFilterQosPolicyWrapper TimeBasedFilter;
        [MarshalAs(UnmanagedType.Struct)]
        public WriterDataLifecycleQosPolicyWrapper WriterDataLifecycle;
        #endregion
    }
}
