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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// Class that contains information about available <see cref="DomainParticipant" />s within the system.
    /// </summary>
    /// <remarks>
    /// The DCPSParticipant topic communicates the existence of <see cref="DomainParticipant" />s by means of the ParticipantBuiltinTopicData datatype.
    /// Each ParticipantBuiltinTopicData sample in a Domain represents a <see cref="DomainParticipant" /> that participates in that Domain: a new ParticipantBuiltinTopicData instance
    /// is created when a newly-added <see cref="DomainParticipant" /> is enabled, and it is disposed when that <see cref="DomainParticipant" /> is deleted.
    /// An updated ParticipantBuiltinTopicData sample is written each time the <see cref="DomainParticipant" /> modifies its <see cref="UserDataQosPolicy" />.
    /// </remarks>
    public struct ParticipantBuiltinTopicData : IEquatable<ParticipantBuiltinTopicData>
    {
        #region Fields
        private List<IntPtr> toRelease;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the global unique identifier of the <see cref="DomainParticipant" />.
        /// </summary>
        public BuiltinTopicKey Key { get; internal set; }

        /// <summary>
        /// Gets the <see cref="UserDataQosPolicy" /> attached to the <see cref="DomainParticipant" />.
        /// </summary>
        public UserDataQosPolicy UserData { get; internal set; }
        #endregion

        #region Methods
        internal ParticipantBuiltinTopicDataWrapper ToNative()
        {
            if (toRelease == null)
            {
                toRelease = new List<IntPtr>();
            }

            var data = new ParticipantBuiltinTopicDataWrapper
            {
                Key = Key,
            };

            if (UserData != null)
            {
                data.UserData = UserData.ToNative();
            }

            return data;
        }

        internal void FromNative(ParticipantBuiltinTopicDataWrapper wrapper)
        {
            Key = wrapper.Key;

            if (UserData == null)
            {
                UserData = new UserDataQosPolicy();
            }
            UserData.FromNative(wrapper.UserData);
        }

        internal void Release()
        {
            UserData?.Release();

            foreach (IntPtr ptr in toRelease)
            {
                Marshal.FreeHGlobal(ptr);
            }

            toRelease.Clear();
        }
        #endregion

        #region IEquatable<ParticipantBuiltinTopicData> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(ParticipantBuiltinTopicData other)
        {
            return Key == other.Key &&
                   UserData == other.UserData;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is ParticipantBuiltinTopicData other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1476352029;
            hashCode = (hashCode * -1521134295) + Key.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<UserDataQosPolicy>.Default.GetHashCode(UserData);
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
        public static bool operator ==(ParticipantBuiltinTopicData left, ParticipantBuiltinTopicData right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(ParticipantBuiltinTopicData left, ParticipantBuiltinTopicData right)
        {
            return !left.Equals(right);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ParticipantBuiltinTopicDataWrapper
    {
        #region Fields
        public BuiltinTopicKey Key;
        public UserDataQosPolicyWrapper UserData;
        #endregion
    }
}