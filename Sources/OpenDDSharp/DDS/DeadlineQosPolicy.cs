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
/// This policy is useful for cases where a <see cref="Topic" /> is expected to have each instance updated periodically. On the publishing side this
/// setting establishes a contract that the application must meet. On the subscribing side the setting establishes a minimum
/// requirement for the remote publishers that are expected to supply the data values.
/// </summary>
/// <remarks>
/// <para>When the DDS 'matches' a <see cref="DataWriter" /> and a <see cref="DataReader" /> it checks whether the settings are compatible (i.e., offered
/// deadline period &lt;= requested deadline period) if they are not, the two entities are informed (via the listener or condition
/// mechanism) of the incompatibility of the QoS settings and communication will not occur.</para>
/// <para>Assuming that the reader and writer ends have compatible settings, the fulfillment of this contract is monitored by DDS
/// and the application is informed of any violations by means of the proper listener or condition.</para>
/// <para>The value offered is considered compatible with the value requested if and only if the inequality "offered deadline period &lt;=
/// requested deadline period" evaluates to 'true'.</para>
/// <para>The setting of the Deadline policy must be set consistently with that of the TimeBasedFilter. For these two policies
/// to be consistent the settings must be such that "deadline period &gt;= minimum separation".</para>
/// </remarks>
public sealed class DeadlineQosPolicy : IEquatable<DeadlineQosPolicy>
{
    #region Properties
    /// <summary>
    /// Gets or sets the duration of the deadline period. The default value of the period member is infinite, which requires no behavior.
    /// </summary>
    /// <remarks>
    /// When this policy is set to a finite value, then the <see cref="DataWriter" /> monitors the changes to data made by the
    /// application and indicates failure to honor the policy by setting the corresponding status
    /// condition and triggering the OnOfferedDeadlineMissed() listener callback. A <see cref="DataReader" />
    /// that detects that the data has not changed before the period has expired sets the
    /// corresponding status condition and triggers the OnRequestedDeadlineMissed() listener callback.
    /// </remarks>
    public Duration Period { get; set; }
    #endregion

    #region Constructors
    internal DeadlineQosPolicy()
    {
        Period = new Duration
        {
            Seconds = Duration.InfiniteSeconds,
            NanoSeconds = Duration.InfiniteNanoSeconds,
        };
    }
    #endregion

    #region IEquatable<DeadlineQosPolicy> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
    public bool Equals(DeadlineQosPolicy other)
    {
        if (other == null)
        {
            return false;
        }

        return Period == other.Period;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        return (obj is DeadlineQosPolicy other) && Equals(other);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return -659743292 + Period.GetHashCode();
    }
    #endregion

    #region Operators
    /// <summary>
    /// Equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(DeadlineQosPolicy left, DeadlineQosPolicy right)
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
    public static bool operator !=(DeadlineQosPolicy left, DeadlineQosPolicy right)
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
internal struct DeadlineQosPolicyWrapper
{
    #region Fields
    public Duration Period;
    #endregion

    #region Operators
    /// <summary>
    /// Implicit conversion operator from <see cref="DeadlineQosPolicyWrapper" /> to <see cref="DeadlineQosPolicy" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="DeadlineQosPolicy" /> object.</returns>
    public static implicit operator DeadlineQosPolicy(DeadlineQosPolicyWrapper value)
    {
        return new DeadlineQosPolicy
        {
            Period = value.Period,
        };
    }

    /// <summary>
    /// Implicit conversion operator from <see cref="DeadlineQosPolicy" /> to <see cref="DeadlineQosPolicyWrapper" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="DeadlineQosPolicyWrapper" /> object.</returns>
    public static implicit operator DeadlineQosPolicyWrapper(DeadlineQosPolicy value)
    {
        return new DeadlineQosPolicyWrapper
        {
            Period = value.Period,
        };
    }
    #endregion
}