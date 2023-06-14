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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenDDSharp.Helpers;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// The purpose of this QoS is to allow the application to attach additional information to the created <see cref="Entity" /> objects such that when
    /// a remote application discovers their existence it can access that information and use it for its own purposes.
    /// </summary>
    public sealed class UserDataQosPolicy : IEquatable<UserDataQosPolicy>
    {
        #region Fields
        private readonly List<IntPtr> toRelease = new List<IntPtr>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the bytes assigned to the <see cref="UserDataQosPolicy" />.
        /// </summary>
        public IList<byte> Value { get; set; }
        #endregion

        #region Constructors
        internal UserDataQosPolicy()
        {
            Value = new List<byte>();
        }
        #endregion

        #region Methods
        internal UserDataQosPolicyWrapper ToNative()
        {
            IntPtr ptr = IntPtr.Zero;

            if (Value != null)
            {
                Value.SequenceToPtr(ref ptr);
                toRelease.Add(ptr);
            }

            return new UserDataQosPolicyWrapper
            {
                Value = ptr,
            };
        }

        internal void FromNative(UserDataQosPolicyWrapper wrapper)
        {
            IList<byte> list = new List<byte>();

            if (wrapper.Value != IntPtr.Zero)
            {
                wrapper.Value.PtrToSequence(ref list);
            }

            Value = list;
        }

        internal void Release()
        {
            if (toRelease == null)
            {
                return;
            }

            foreach (IntPtr ptr in toRelease)
            {
                Marshal.FreeHGlobal(ptr);
            }

            toRelease.Clear();
        }
        #endregion

        #region IEquatable<UserDataQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(UserDataQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return Value.SequenceEqual(other.Value);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is UserDataQosPolicy other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1476352029;
            if (Value != null)
            {
                foreach (var b in Value)
                {
                    hashCode = (hashCode * -1521134295) + b.GetHashCode();
                }
            }
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
        public static bool operator ==(UserDataQosPolicy left, UserDataQosPolicy right)
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
        public static bool operator !=(UserDataQosPolicy left, UserDataQosPolicy right)
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
    internal struct UserDataQosPolicyWrapper
    {
        #region Fields
        public IntPtr Value;
        #endregion
    }
}
