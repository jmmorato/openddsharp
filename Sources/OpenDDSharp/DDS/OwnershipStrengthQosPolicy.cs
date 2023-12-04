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
/// This QoS policy should be used in combination with the <see cref="OwnershipQosPolicy" />.
/// It only applies to the situation case where ownership kind is set to
/// <see cref="OwnershipQosPolicyKind.ExclusiveOwnershipQos" />.
/// </summary>
public sealed class OwnershipStrengthQosPolicy : IEquatable<OwnershipStrengthQosPolicy>
{
    #region Properties
    /// <summary>
    /// Gets or sets the value of the ownership strength. The value member is used to determine which
    /// <see cref="DataWriter" /> is the owner of the data-object instance. The default value is zero.
    /// </summary>
    public int Value { get; set; }
    #endregion

    #region Constructors
    internal OwnershipStrengthQosPolicy()
    {
        Value = 0;
    }
    #endregion

    #region IEquatable<OwnershipStrengthQosPolicy> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the other parameter;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(OwnershipStrengthQosPolicy other)
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
        return (obj is OwnershipStrengthQosPolicy other) && Equals(other);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return -1937169414 + Value.GetHashCode();
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
    public static bool operator ==(OwnershipStrengthQosPolicy left, OwnershipStrengthQosPolicy right)
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
    public static bool operator !=(OwnershipStrengthQosPolicy left, OwnershipStrengthQosPolicy right)
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
internal struct OwnershipStrengthQosPolicyWrapper
{
    #region Fields
    public int Value;
    #endregion

    #region Operators
    /// <summary>
    /// Implicit conversion operator from <see cref="OwnershipStrengthQosPolicyWrapper" /> to
    /// <see cref="OwnershipStrengthQosPolicy" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="OwnershipStrengthQosPolicy" /> object.</returns>
    public static implicit operator OwnershipStrengthQosPolicy(OwnershipStrengthQosPolicyWrapper value)
    {
        return new OwnershipStrengthQosPolicy
        {
            Value = value.Value,
        };
    }

    /// <summary>
    /// Implicit conversion operator from <see cref="OwnershipStrengthQosPolicy" /> to
    /// <see cref="OwnershipStrengthQosPolicyWrapper" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="OwnershipStrengthQosPolicyWrapper" /> object.</returns>
    public static implicit operator OwnershipStrengthQosPolicyWrapper(OwnershipStrengthQosPolicy value)
    {
        return new OwnershipStrengthQosPolicyWrapper
        {
            Value = value.Value,
        };
    }
    #endregion
}