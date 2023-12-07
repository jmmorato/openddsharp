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

namespace OpenDDSharp.DDS;

/// <summary>
/// Holds the <see cref="Topic" /> Quality of Service policies.
/// </summary>
public sealed class TopicQos : IEquatable<TopicQos>
{
    #region Properties
    /// <summary>
    /// Gets the <see cref="TopicDataQosPolicy" />.
    /// </summary>
    public TopicDataQosPolicy TopicData { get; internal set; }

    /// <summary>
    /// Gets the <see cref="DurabilityQosPolicy" />.
    /// </summary>
    public DurabilityQosPolicy Durability { get; internal set; }

    /// <summary>
    /// Gets the <see cref="DurabilityServiceQosPolicy" />.
    /// </summary>
    public DurabilityServiceQosPolicy DurabilityService { get; internal set; }

    /// <summary>
    /// Gets the <see cref="DeadlineQosPolicy" />.
    /// </summary>
    public DeadlineQosPolicy Deadline { get; internal set; }

    /// <summary>
    /// Gets the <see cref="LatencyBudgetQosPolicy" />.
    /// </summary>
    public LatencyBudgetQosPolicy LatencyBudget { get; internal set; }

    /// <summary>
    /// Gets the <see cref="LivelinessQosPolicy" />.
    /// </summary>
    public LivelinessQosPolicy Liveliness { get; internal set; }

    /// <summary>
    /// Gets the <see cref="ReliabilityQosPolicy" />.
    /// </summary>
    public ReliabilityQosPolicy Reliability { get; internal set; }

    /// <summary>
    /// Gets the <see cref="DestinationOrderQosPolicy" />.
    /// </summary>
    public DestinationOrderQosPolicy DestinationOrder { get; internal set; }

    /// <summary>
    /// Gets the <see cref="HistoryQosPolicy" />.
    /// </summary>
    public HistoryQosPolicy History { get; internal set; }

    /// <summary>
    /// Gets the <see cref="ResourceLimitsQosPolicy" />.
    /// </summary>
    public ResourceLimitsQosPolicy ResourceLimits { get; internal set; }

    /// <summary>
    /// Gets the <see cref="TransportPriorityQosPolicy" />.
    /// </summary>
    public TransportPriorityQosPolicy TransportPriority { get; internal set; }

    /// <summary>
    /// Gets the <see cref="LifespanQosPolicy" />.
    /// </summary>
    public LifespanQosPolicy Lifespan { get; internal set; }

    /// <summary>
    /// Gets the <see cref="OwnershipQosPolicy" />.
    /// </summary>
    public OwnershipQosPolicy Ownership { get; internal set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="TopicQos"/> class.
    /// </summary>
    public TopicQos()
    {
        TopicData = new TopicDataQosPolicy();
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
    }
    #endregion

    #region Methods
    internal TopicQosWrapper ToNative()
    {
        var data = new TopicQosWrapper
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
        };

        if (TopicData != null)
        {
            data.TopicData = TopicData.ToNative();
        }

        return data;
    }

    internal void FromNative(TopicQosWrapper wrapper)
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

        if (TopicData == null)
        {
            TopicData = new TopicDataQosPolicy();
        }
        TopicData.FromNative(wrapper.TopicData);
    }

    internal void Release()
    {
        TopicData?.Release();
    }
    #endregion

    #region IEquatable<TopicQos> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the other parameter;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(TopicQos other)
    {
        if (other == null)
        {
            return false;
        }

        return TopicData == other.TopicData &&
               Durability == other.Durability &&
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
               Ownership == other.Ownership;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object is equal to the current object;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public override bool Equals(object obj)
    {
        return (obj is TopicQos other) && Equals(other);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        var hashCode = 1476352029;
        hashCode = (hashCode * -1521134295) + EqualityComparer<TopicDataQosPolicy>.Default.GetHashCode(TopicData);
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
        return hashCode;
    }
    #endregion

    #region Operators
    /// <summary>
    /// Equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns>
    /// <see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(TopicQos left, TopicQos right)
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
    /// <returns>
    /// <see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.
    /// </returns>
    public static bool operator !=(TopicQos left, TopicQos right)
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
internal struct TopicQosWrapper
{
    #region Fields
    [MarshalAs(UnmanagedType.Struct)]
    public TopicDataQosPolicyWrapper TopicData;
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
    #endregion
}