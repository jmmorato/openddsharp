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
    /// This policy provides a means for the application to indicate to the middleware the "urgency" of the data-communication. By
    /// having a non-zero duration DDS can optimize its internal operation. This policy is considered a hint. There is no specified mechanism as
    /// to how the service should take advantage of this hint.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public sealed class LatencyBudgetQosPolicy : IEquatable<LatencyBudgetQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the maximum duration for the latency budget.
        /// </summary>
        /// <remarks>
        /// The value offered is considered compatible with the value requested if and only if the inequality
        /// "offered duration &lt;= requested duration" evaluates to 'true'.
        /// </remarks>
        public Duration Duration { get; set; }
        #endregion

        #region Constructors
        internal LatencyBudgetQosPolicy()
        {
            Duration = new Duration
            {
                Seconds = Duration.ZeroSeconds,
                NanoSeconds = Duration.ZeroNanoseconds,
            };
        }
        #endregion

        #region IEquatable<LatencyBudgetQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(LatencyBudgetQosPolicy other)
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
            return (obj is LatencyBudgetQosPolicy other) && Equals(other);
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
        public static bool operator ==(LatencyBudgetQosPolicy left, LatencyBudgetQosPolicy right)
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
        public static bool operator !=(LatencyBudgetQosPolicy left, LatencyBudgetQosPolicy right)
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
    internal struct LatencyBudgetQosPolicyWrapper
    {
        #region Fields
        public Duration Duration;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="LatencyBudgetQosPolicyWrapper" /> to <see cref="LatencyBudgetQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="LatencyBudgetQosPolicy" /> object.</returns>
        public static implicit operator LatencyBudgetQosPolicy(LatencyBudgetQosPolicyWrapper value)
        {
            return new LatencyBudgetQosPolicy
            {
                Duration = value.Duration,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="LatencyBudgetQosPolicy" /> to <see cref="LatencyBudgetQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="LatencyBudgetQosPolicyWrapper" /> object.</returns>
        public static implicit operator LatencyBudgetQosPolicyWrapper(LatencyBudgetQosPolicy value)
        {
            return new LatencyBudgetQosPolicyWrapper
            {
                Duration = value.Duration,
            };
        }
        #endregion
    }
}