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
/// This policy allows a <see cref="DataReader" /> to indicate that it does not necessarily want to see all values of
/// each instance published under the <see cref="Topic" />. Rather, it wants to see at most one change every
/// <see cref="MinimumSeparation" /> period.
/// </summary>
/// <remarks>
/// This QoS policy does not conserve bandwidth as instance value changes are still sent to the subscriber process.
/// It only affects which samples are made available via the <see cref="DataReader" />.
/// </remarks>
public sealed class TimeBasedFilterQosPolicy : IEquatable<TimeBasedFilterQosPolicy>
{
    #region Properties
    /// <summary>
    /// Gets or sets the interval that defines a minimum delay between instance value changes; this permits the
    /// <see cref="DataReader" /> to throttle changes without affecting the state of the associated
    /// <see cref="DataWriter" />. By default, MinimumSeparation is zero, which indicates that no data is filtered.
    /// </summary>
    public Duration MinimumSeparation { get; set; }
    #endregion

    #region Constructors
    internal TimeBasedFilterQosPolicy()
    {
        MinimumSeparation = new Duration
        {
            Seconds = Duration.ZeroSeconds,
            NanoSeconds = Duration.ZeroNanoseconds,
        };
    }
    #endregion

    #region IEquatable<TimeBasedFilterQosPolicy> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the other parameter;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(TimeBasedFilterQosPolicy other)
    {
        if (other == null)
        {
            return false;
        }

        return MinimumSeparation == other.MinimumSeparation;
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
        return (obj is TimeBasedFilterQosPolicy other) && Equals(other);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        var hashCode = -910965429;
        hashCode = (hashCode * -1521134295) + MinimumSeparation.GetHashCode();
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
    public static bool operator ==(TimeBasedFilterQosPolicy left, TimeBasedFilterQosPolicy right)
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
    public static bool operator !=(TimeBasedFilterQosPolicy left, TimeBasedFilterQosPolicy right)
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
internal struct TimeBasedFilterQosPolicyWrapper
{
    #region Fields
    public Duration MinimumSeparation;
    #endregion

    #region Operators
    /// <summary>
    /// Implicit conversion operator from <see cref="TimeBasedFilterQosPolicyWrapper" /> to
    /// <see cref="TimeBasedFilterQosPolicy" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="TimeBasedFilterQosPolicy" /> object.</returns>
    public static implicit operator TimeBasedFilterQosPolicy(TimeBasedFilterQosPolicyWrapper value)
    {
        return new TimeBasedFilterQosPolicy
        {
            MinimumSeparation = value.MinimumSeparation,
        };
    }

    /// <summary>
    /// Implicit conversion operator from <see cref="TimeBasedFilterQosPolicy" /> to
    /// <see cref="TimeBasedFilterQosPolicyWrapper" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="TimeBasedFilterQosPolicyWrapper" /> object.</returns>
    public static implicit operator TimeBasedFilterQosPolicyWrapper(TimeBasedFilterQosPolicy value)
    {
        return new TimeBasedFilterQosPolicyWrapper
        {
            MinimumSeparation = value.MinimumSeparation,
        };
    }
    #endregion
}