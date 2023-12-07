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
using System.Linq;
using System.Runtime.InteropServices;
using OpenDDSharp.Helpers;

namespace OpenDDSharp.DDS;

/// <summary>
/// The OfferedIncompatibleQos status indicates that an offered QoS was incompatible with the requested
/// QoS of a <see cref="DataReader" />.
/// </summary>
public struct OfferedIncompatibleQosStatus : IEquatable<OfferedIncompatibleQosStatus>
{
    #region Fields
    private List<IntPtr> toRelease;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the cumulative count of times that data readers with incompatible QoS have been found.
    /// </summary>
    public int TotalCount { get; internal set; }

    /// <summary>
    /// Gets the incremental change in the total count since the last time this status was accessed.
    /// </summary>
    public int TotalCountChange { get; internal set; }

    /// <summary>
    /// Gets one of the QoS policies that was incompatible in the last incompatibility detected.
    /// </summary>
    public int LastPolicyId { get; internal set; }

    /// <summary>
    /// Gets the sequence of values that indicates the total number of incompatibilities that have been
    /// detected for each QoS policy.
    /// </summary>
    public ICollection<QosPolicyCount> Policies { get; internal set; }
    #endregion

    #region Methods
    internal OfferedIncompatibleQosStatusWrapper ToNative()
    {
        IntPtr ptr = IntPtr.Zero;

        if (toRelease == null)
        {
            toRelease = new List<IntPtr>();
        }

        if (Policies != null)
        {
            Policies.SequenceToPtr(ref ptr);
            toRelease.Add(ptr);
        }

        return new OfferedIncompatibleQosStatusWrapper
        {
            TotalCount = TotalCount,
            TotalCountChange = TotalCountChange,
            LastPolicyId = LastPolicyId,
            Policies = ptr,
        };
    }

    internal void FromNative(OfferedIncompatibleQosStatusWrapper wrapper)
    {
        IList<QosPolicyCount> list = new List<QosPolicyCount>();

        if (wrapper.Policies != IntPtr.Zero)
        {
            wrapper.Policies.PtrToSequence(ref list);
        }

        TotalCount = wrapper.TotalCount;
        TotalCountChange = wrapper.TotalCountChange;
        LastPolicyId = wrapper.LastPolicyId;
        Policies = list;
    }

    internal void Release()
    {
        if (toRelease == null)
        {
            return;
        }

        foreach (IntPtr ptr in toRelease)
        {
            Marshal.FreeHGlobal(ptr);
        }

        toRelease.Clear();
    }
    #endregion

    #region IEquatable<OfferedIncompatibleQosStatus> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
    public bool Equals(OfferedIncompatibleQosStatus other)
    {
        return TotalCount == other.TotalCount &&
               TotalCountChange == other.TotalCountChange &&
               LastPolicyId == other.LastPolicyId &&
               Policies.SequenceEqual(other.Policies);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        return Equals((OfferedIncompatibleQosStatus)obj);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        var hashCode = 751819449;

        hashCode = (hashCode * -1521134295) + TotalCount.GetHashCode();
        hashCode = (hashCode * -1521134295) + TotalCountChange.GetHashCode();
        hashCode = (hashCode * -1521134295) + LastPolicyId.GetHashCode();

        foreach (var p in Policies)
        {
            hashCode = (hashCode * -1521134295) + p.GetHashCode();
        }

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
    public static bool operator ==(OfferedIncompatibleQosStatus left, OfferedIncompatibleQosStatus right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Not equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
    public static bool operator !=(OfferedIncompatibleQosStatus left, OfferedIncompatibleQosStatus right)
    {
        return !left.Equals(right);
    }
    #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal struct OfferedIncompatibleQosStatusWrapper
{
    #region Fields
    public int TotalCount;
    public int TotalCountChange;
    public int LastPolicyId;
    public IntPtr Policies;
    #endregion
}