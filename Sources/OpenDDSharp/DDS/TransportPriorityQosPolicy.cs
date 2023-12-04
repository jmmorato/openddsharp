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
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS;

/// <summary>
/// The purpose of this QoS is to allow the application to take advantage of transports capable of sending
/// messages with different priorities.
/// </summary>
/// <remarks>
/// This policy is considered a hint. The policy depends on the ability of the underlying transports to set a
/// priority on the messages they send. Any value within the range of a 32-bit signed integer may be chosen;
/// higher values indicate higher priority.
/// </remarks>
public sealed class TransportPriorityQosPolicy : IEquatable<TransportPriorityQosPolicy>
{
    #region Properties
    /// <summary>
    /// Gets or sets the transport priority value.
    /// </summary>
    public int Value { get; set; }
    #endregion

    #region Constructors
    internal TransportPriorityQosPolicy()
    {
        Value = 0;
    }
    #endregion

    #region IEquatable<TransportPriorityQosPolicy> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the other parameter;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(TransportPriorityQosPolicy other)
    {
        if (other == null)
        {
            return false;
        }

        return Value == other.Value;
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
        return (obj is TransportPriorityQosPolicy other) && Equals(other);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        var hashCode = -1644527362;
        hashCode = (hashCode * -1521134295) + Value.GetHashCode();
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
    public static bool operator ==(TransportPriorityQosPolicy left, TransportPriorityQosPolicy right)
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
    public static bool operator !=(TransportPriorityQosPolicy left, TransportPriorityQosPolicy right)
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
internal struct TransportPriorityQosPolicyWrapper
{
    #region Fields
    public int Value;
    #endregion

    #region Operators
    /// <summary>
    /// Implicit conversion operator from <see cref="TransportPriorityQosPolicyWrapper" /> to
    /// <see cref="TransportPriorityQosPolicy" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="TransportPriorityQosPolicy" /> object.</returns>
    public static implicit operator TransportPriorityQosPolicy(TransportPriorityQosPolicyWrapper value)
    {
        return new TransportPriorityQosPolicy
        {
            Value = value.Value,
        };
    }

    /// <summary>
    /// Implicit conversion operator from <see cref="TransportPriorityQosPolicy" /> to
    /// <see cref="TransportPriorityQosPolicyWrapper" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="TransportPriorityQosPolicyWrapper" /> object.</returns>
    public static implicit operator TransportPriorityQosPolicyWrapper(TransportPriorityQosPolicy value)
    {
        return new TransportPriorityQosPolicyWrapper
        {
            Value = value.Value,
        };
    }
    #endregion
}