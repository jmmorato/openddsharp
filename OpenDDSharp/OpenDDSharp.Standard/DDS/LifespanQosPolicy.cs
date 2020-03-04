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

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// <para>The purpose of this QoS is to avoid delivering "stale" data to the application.</para>
    /// <para>Each data sample written by the <see cref="DataWriter" /> has an associated 'expiration time' beyond which the data should not be delivered
    /// to any application. Once the sample expires, the data will be removed from the <see cref="DataReader" /> caches as well as from the
    /// transient and persistent information caches.</para>
    /// </summary>
    /// <remarks>
    /// <para>The value of this policy may be changed at any time. Changes to this policy affect only data written after the change.</para>
    /// <para>The 'expiration time' of each sample is computed by adding the duration specified by the Lifespan QoS to the source timestamp.</para>
    /// <para>This QoS relies on the sender and receiving applications having their clocks sufficiently synchronized. If this is not the case
    /// and DDS can detect it, the <see cref="DataReader" /> is allowed to use the reception timestamp instead of the source timestamp in its
    /// computation of the 'expiration time'.</para>
    /// </remarks>
    public sealed class LifespanQosPolicy : IEquatable<LifespanQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the expiration time duration. The default value is infinite, which means samples never expire.
        /// </summary>
        public Duration Duration { get; set; }
        #endregion

        #region Constructors
        internal LifespanQosPolicy()
        {
            Duration = new Duration
            {
                Seconds = Duration.InfiniteSeconds,
                NanoSeconds = Duration.InfiniteNanoseconds,
            };
        }
        #endregion

        #region IEquatable<LifespanQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(LifespanQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return Duration == other.Duration;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is LifespanQosPolicy other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return -1943557835 + Duration.GetHashCode();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(LifespanQosPolicy left, LifespanQosPolicy right)
        {
            if (left == null && right == null)
            {
                return true;
            }

            if (left == null || right == null)
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
        public static bool operator !=(LifespanQosPolicy left, LifespanQosPolicy right)
        {
            if (left == null && right == null)
            {
                return false;
            }

            if (left == null || right == null)
            {
                return true;
            }

            return !left.Equals(right);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct LifespanQosPolicyWrapper
    {
        #region Fields
        public Duration Duration;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="LifespanQosPolicyWrapper" /> to <see cref="LifespanQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="LifespanQosPolicy" /> object.</returns>
        public static implicit operator LifespanQosPolicy(LifespanQosPolicyWrapper value)
        {
            return new LifespanQosPolicy
            {
                Duration = value.Duration,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="LifespanQosPolicy" /> to <see cref="LifespanQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="LifespanQosPolicyWrapper" /> object.</returns>
        public static implicit operator LifespanQosPolicyWrapper(LifespanQosPolicy value)
        {
            return new LifespanQosPolicyWrapper
            {
                Duration = value.Duration,
            };
        }
        #endregion
    }
}