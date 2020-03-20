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
    /// This policy indicates the level of reliability requested by a <see cref="DataReader" /> or offered by a <see cref="DataWriter" />.
    /// </summary>
    /// <remarks>
    /// This policy is considered during the creation of associations between data writers and data readers. The value of both sides of the association must be compatible in order for an
    /// association to be created. The value offered is considered compatible with the value requested if and only if the inequality "offered kind &gt;= requested kind" evaluates to '<see langword="true" />'.
    /// For the purposes of this inequality, the values of Reliability kind are considered ordered such that BestEffort &lt; Reliable.
    /// </remarks>
    public sealed class ReliabilityQosPolicy : IEquatable<ReliabilityQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the reliability kind applied to the <see cref="Entity" />.
        /// </summary>
        public ReliabilityQosPolicyKind Kind { get; set; }

        /// <summary>
        /// Gets or sets the maximum blocking time when the history QoS policy is set to
        /// "keep all" and the writer is unable to proceed because of resource limits.
        /// </summary>
        public Duration MaxBlockingTime { get; set; }
        #endregion

        #region Constructors
        internal ReliabilityQosPolicy()
        {
            Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            MaxBlockingTime = new Duration
            {
                Seconds = Duration.InfiniteSeconds,
                NanoSeconds = Duration.InfiniteNanoseconds,
            };
        }
        #endregion

        #region IEquatable<ReliabilityQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(ReliabilityQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return Kind == other.Kind &&
                   MaxBlockingTime == other.MaxBlockingTime;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is ReliabilityQosPolicy other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = -1644527362;
            hashCode = (hashCode * -1521134295) + Kind.GetHashCode();
            hashCode = (hashCode * -1521134295) + MaxBlockingTime.GetHashCode();
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
        public static bool operator ==(ReliabilityQosPolicy left, ReliabilityQosPolicy right)
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
        public static bool operator !=(ReliabilityQosPolicy left, ReliabilityQosPolicy right)
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
    internal struct ReliabilityQosPolicyWrapper
    {
        #region Fields
        public ReliabilityQosPolicyKind Kind;
        public Duration MaxBlockingTime;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="ReliabilityQosPolicyWrapper" /> to <see cref="ReliabilityQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="ReliabilityQosPolicy" /> object.</returns>
        public static implicit operator ReliabilityQosPolicy(ReliabilityQosPolicyWrapper value)
        {
            return new ReliabilityQosPolicy
            {
                Kind = value.Kind,
                MaxBlockingTime = value.MaxBlockingTime,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="ReliabilityQosPolicy" /> to <see cref="ReliabilityQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="ReliabilityQosPolicyWrapper" /> object.</returns>
        public static implicit operator ReliabilityQosPolicyWrapper(ReliabilityQosPolicy value)
        {
            return new ReliabilityQosPolicyWrapper
            {
                Kind = value.Kind,
                MaxBlockingTime = value.MaxBlockingTime,
            };
        }
        #endregion
    }
}