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
    /// The LivelinessChanged status indicates that there have been liveliness changes for one or more <see cref="DataWriter">DataWriters</see>
    /// that are publishing instances for this <see cref="DataReader"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LivelinessChangedStatus : IEquatable<LivelinessChangedStatus>
    {
        #region Fields
        private int _aliveCount;
        private int _notAliveCount;
        private int _aliveCountChange;
        private int _notAliveCountChange;
        private InstanceHandle _lastPublicationHandle;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the total number of <see cref="DataWriter">DataWriters</see> currently active on the topic this <see cref="DataReader"/> is reading.
        /// </summary>
        public int AliveCount => _aliveCount;

        /// <summary>
        /// Gets the total number of <see cref="DataWriter">DataWriters</see> writing to the <see cref="DataReader"/>'s topic that are no longer asserting their liveliness.
        /// </summary>
        public int NotAliveCount => _notAliveCount;

        /// <summary>
        /// Gets the change in the alive count since the last time the status was accessed.
        /// </summary>
        public int AliveCountChange => _aliveCountChange;

        /// <summary>
        /// Gets the change in the not alive count since the last time the status was accessed.
        /// </summary>
        public int NotAliveCountChange => _notAliveCountChange;

        /// <summary>
        /// Gets the handle of the last <see cref="DataWriter" /> whose liveliness has changed.
        /// </summary>
        public InstanceHandle LastPublicationHandle => _lastPublicationHandle;
        #endregion

        #region IEquatable<LivelinessChangedStatus> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(LivelinessChangedStatus other)
        {
            return AliveCount == other.AliveCount &&
                   NotAliveCount == other.NotAliveCount &&
                   AliveCountChange == other.NotAliveCountChange &&
                   NotAliveCountChange == other.NotAliveCountChange &&
                   LastPublicationHandle == other.LastPublicationHandle;
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

            return Equals((LivelinessChangedStatus)obj);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 751819449;
            hashCode = (hashCode * -1521134295) + AliveCount.GetHashCode();
            hashCode = (hashCode * -1521134295) + NotAliveCount.GetHashCode();
            hashCode = (hashCode * -1521134295) + AliveCountChange.GetHashCode();
            hashCode = (hashCode * -1521134295) + NotAliveCountChange.GetHashCode();
            hashCode = (hashCode * -1521134295) + LastPublicationHandle.GetHashCode();
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
        public static bool operator ==(LivelinessChangedStatus left, LivelinessChangedStatus right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(LivelinessChangedStatus left, LivelinessChangedStatus right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}
