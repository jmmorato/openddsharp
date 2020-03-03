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
    public struct LatencyBudgetQosPolicy : IEquatable<LatencyBudgetQosPolicy>
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

        #region IEquatable<LatencyBudgetQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(LatencyBudgetQosPolicy other)
        {
            return Duration == other.Duration;
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

            return Equals((LatencyBudgetQosPolicy)obj);
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
            return !left.Equals(right);
        }
        #endregion
    }
}