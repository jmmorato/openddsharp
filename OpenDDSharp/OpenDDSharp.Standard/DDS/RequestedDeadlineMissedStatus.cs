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
    /// The RequestedDeadlineMissed status indicates that the deadline requested via the <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RequestedDeadlineMissedStatus : IEquatable<RequestedDeadlineMissedStatus>
    {
        #region Properties
        /// <summary>
        /// Gets the cumulative count of missed requested deadlines that have been reported.
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Gets the incremental count of missed requested deadlines since the last time this status was accessed.
        /// </summary>
        public int TotalCountChange { get; }

        /// <summary>
        /// Gets the instance handle of the last missed deadline.
        /// </summary>
        public InstanceHandle LastInstanceHandle { get; }
        #endregion

        #region IEquatable<RequestedDeadlineMissedStatus> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(RequestedDeadlineMissedStatus other)
        {
            return TotalCount == other.TotalCount &&
                   TotalCountChange == other.TotalCountChange &&
                   LastInstanceHandle == other.LastInstanceHandle;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is RequestedDeadlineMissedStatus other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 543595584;
            hashCode = (hashCode * -1521134295) + TotalCount.GetHashCode();
            hashCode = (hashCode * -1521134295) + TotalCountChange.GetHashCode();
            hashCode = (hashCode * -1521134295) + LastInstanceHandle.GetHashCode();
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
        public static bool operator ==(RequestedDeadlineMissedStatus left, RequestedDeadlineMissedStatus right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(RequestedDeadlineMissedStatus left, RequestedDeadlineMissedStatus right)
        {
            return !(left == right);
        }
        #endregion
    }
}
