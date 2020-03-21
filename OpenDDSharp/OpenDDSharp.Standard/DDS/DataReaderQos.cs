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
    /// Holds the <see cref="DataReader" /> Quality of Service policies.
    /// </summary>
    public sealed class DataReaderQos : IEquatable<DataReaderQos>
    {
        #region Properties
        /// <summary>
        /// Gets the <see cref="DurabilityQosPolicy"/>.
        /// </summary>
        public DurabilityQosPolicy Durability { get; internal set; }

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
        /// Gets the <see cref="UserDataQosPolicy"/>.
        /// </summary>
        public UserDataQosPolicy UserData { get; internal set; }

        /// <summary>
        /// Gets the <see cref="OwnershipQosPolicy"/>.
        /// </summary>
        public OwnershipQosPolicy Ownership { get; internal set; }

        /// <summary>
        /// Gets the <see cref="TimeBasedFilterQosPolicy"/>.
        /// </summary>
        public TimeBasedFilterQosPolicy TimeBasedFilter { get; internal set; }

        /// <summary>
        /// Gets the <see cref="ReaderDataLifecycleQosPolicy"/>.
        /// </summary>
        public ReaderDataLifecycleQosPolicy ReaderDataLifecycle { get; internal set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataReaderQos"/> class.
        /// </summary>
        public DataReaderQos()
        {
            Durability = new DurabilityQosPolicy();
            Deadline = new DeadlineQosPolicy();
            LatencyBudget = new LatencyBudgetQosPolicy();
            Liveliness = new LivelinessQosPolicy();
            Reliability = new ReliabilityQosPolicy();
            DestinationOrder = new DestinationOrderQosPolicy();
            History = new HistoryQosPolicy();
            ResourceLimits = new ResourceLimitsQosPolicy();
            UserData = new UserDataQosPolicy();
            Ownership = new OwnershipQosPolicy();
            TimeBasedFilter = new TimeBasedFilterQosPolicy();
            ReaderDataLifecycle = new ReaderDataLifecycleQosPolicy();
        }
        #endregion

        #region Methods
        internal DataReaderQosWrapper ToNative()
        {
            var data = new DataReaderQosWrapper
            {
                Deadline = Deadline,
                DestinationOrder = DestinationOrder,
                Durability = Durability,
                LatencyBudget = LatencyBudget,
                Liveliness = Liveliness,
                Ownership = Ownership,
                Reliability = Reliability,
                History = History,
                ReaderDataLifecycle = ReaderDataLifecycle,
                ResourceLimits = ResourceLimits,
                TimeBasedFilter = TimeBasedFilter,
            };

            if (UserData != null)
            {
                data.UserData = UserData.ToNative();
            }

            return data;
        }

        internal void FromNative(DataReaderQosWrapper wrapper)
        {
            Deadline = wrapper.Deadline;
            DestinationOrder = wrapper.DestinationOrder;
            Durability = wrapper.Durability;
            LatencyBudget = wrapper.LatencyBudget;
            Liveliness = wrapper.Liveliness;
            Ownership = wrapper.Ownership;
            Reliability = wrapper.Reliability;
            History = wrapper.History;
            ReaderDataLifecycle = wrapper.ReaderDataLifecycle;
            ResourceLimits = wrapper.ResourceLimits;
            TimeBasedFilter = wrapper.TimeBasedFilter;

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

        #region IEquatable<DataReaderQos> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(DataReaderQos other)
        {
            if (other == null)
            {
                return false;
            }

            return Durability == other.Durability &&
                   Deadline == other.Deadline &&
                   LatencyBudget == other.LatencyBudget &&
                   Liveliness == other.Liveliness &&
                   Reliability == other.Reliability &&
                   DestinationOrder == other.DestinationOrder &&
                   History == other.History &&
                   ResourceLimits == other.ResourceLimits &&
                   UserData == other.UserData &&
                   Ownership == other.Ownership &&
                   TimeBasedFilter == other.TimeBasedFilter &&
                   DestinationOrder == other.DestinationOrder;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is DataReaderQos other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1476352029;
            hashCode = (hashCode * -1521134295) + EqualityComparer<DurabilityQosPolicy>.Default.GetHashCode(Durability);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DeadlineQosPolicy>.Default.GetHashCode(Deadline);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LatencyBudgetQosPolicy>.Default.GetHashCode(LatencyBudget);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LivelinessQosPolicy>.Default.GetHashCode(Liveliness);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ReliabilityQosPolicy>.Default.GetHashCode(Reliability);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DestinationOrderQosPolicy>.Default.GetHashCode(DestinationOrder);
            hashCode = (hashCode * -1521134295) + EqualityComparer<HistoryQosPolicy>.Default.GetHashCode(History);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ResourceLimitsQosPolicy>.Default.GetHashCode(ResourceLimits);
            hashCode = (hashCode * -1521134295) + EqualityComparer<UserDataQosPolicy>.Default.GetHashCode(UserData);
            hashCode = (hashCode * -1521134295) + EqualityComparer<OwnershipQosPolicy>.Default.GetHashCode(Ownership);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TimeBasedFilterQosPolicy>.Default.GetHashCode(TimeBasedFilter);
            hashCode = (hashCode * -1521134295) + EqualityComparer<DestinationOrderQosPolicy>.Default.GetHashCode(DestinationOrder);
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
        public static bool operator ==(DataReaderQos left, DataReaderQos right)
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
        public static bool operator !=(DataReaderQos left, DataReaderQos right)
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
    internal struct DataReaderQosWrapper
    {
        #region Fields
        public DurabilityQosPolicyWrapper Durability;
        public DeadlineQosPolicyWrapper Deadline;
        public LatencyBudgetQosPolicyWrapper LatencyBudget;
        public LivelinessQosPolicyWrapper Liveliness;
        public ReliabilityQosPolicyWrapper Reliability;
        public DestinationOrderQosPolicyWrapper DestinationOrder;
        public HistoryQosPolicyWrapper History;
        public ResourceLimitsQosPolicyWrapper ResourceLimits;
        public UserDataQosPolicyWrapper UserData;
        public OwnershipQosPolicyWrapper Ownership;
        public TimeBasedFilterQosPolicyWrapper TimeBasedFilter;
        public ReaderDataLifecycleQosPolicyWrapper ReaderDataLifecycle;
        #endregion
    }
}
