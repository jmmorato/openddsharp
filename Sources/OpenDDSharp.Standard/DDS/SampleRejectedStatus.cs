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
    /// The SampleRejected status indicates that a sample received by the data reader has been rejected.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SampleRejectedStatus : IEquatable<SampleRejectedStatus>
    {
        #region Fields
        private int _totalCount;
        private int _totalCountChange;
        private SampleRejectedStatusKind _lastReason;
        private InstanceHandle _lastInstanceHandle;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the cumulative count of samples that have been reported as rejected.
        /// </summary>
        public int TotalCount => _totalCount;

        /// <summary>
        /// Gets the incremental count of rejected samples since the last time this status was accessed.
        /// </summary>
        public int TotalCountChange => _totalCountChange;

        /// <summary>
        /// Gets the reason the most recently rejected sample was rejected.
        /// </summary>
        public SampleRejectedStatusKind LastReason => _lastReason;

        /// <summary>
        /// Gets the instance handle of the last rejected sample.
        /// </summary>
        public InstanceHandle LastInstanceHandle => _lastInstanceHandle;
        #endregion

        #region IEquatable<SampleRejectedStatus> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(SampleRejectedStatus other)
        {
            return TotalCount == other.TotalCount &&
                   TotalCountChange == other.TotalCountChange &&
                   LastReason == other.LastReason &&
                   LastInstanceHandle == other.LastInstanceHandle;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is SampleRejectedStatus other) && Equals(other);
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
            hashCode = (hashCode * -1521134295) + LastReason.GetHashCode();
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
        public static bool operator ==(SampleRejectedStatus left, SampleRejectedStatus right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(SampleRejectedStatus left, SampleRejectedStatus right)
        {
            return !(left == right);
        }
        #endregion
    }
}
