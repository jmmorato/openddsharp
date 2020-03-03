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
    /// Represent a bit-mask of <see cref="StatusKind" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct StatusMask : IEquatable<StatusMask>
    {
        #region Constants
        /// <summary>
        /// A mask containing the default <see cref="StatusKind" />.
        /// </summary>
        public static readonly StatusMask DefaultStatusMask = 4294967295U;

        /// <summary>
        /// A mask containing all <see cref="StatusKind" />.
        /// </summary>
        public static readonly StatusMask AllStatusMask = 4294967295U;

        /// <summary>
        /// A mask containing none <see cref="StatusKind" />.
        /// </summary>
        public static readonly StatusMask NoStatusMask = 0U;
        #endregion

        #region Fields
        private readonly uint _value;
        #endregion

        #region Constructors
        internal StatusMask(uint value)
        {
            _value = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="StatusMask"/> from an <see cref="uint"/> value.
        /// </summary>
        /// <param name="value">The <see cref="uint"/> value.</param>
        /// <returns>A newly created <see cref="StatusMask"/> object.</returns>
        public static StatusMask FromUInt32(uint value)
        {
            return new StatusMask(value);
        }

        /// <summary>
        /// Gets the <see cref="uint"/> value of the <see cref="StatusMask"/>.
        /// </summary>
        /// <returns>The <see cref="uint"/> value.</returns>
        public uint ToUInt32()
        {
            return _value;
        }
        #endregion

        #region IEquatable<StatusMask> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(StatusMask other)
        {
            return _value == other._value;
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

            if (GetType() != obj.GetType() && !(obj is uint))
            {
                return false;
            }

            StatusMask aux;
            if (obj is uint)
            {
                aux = (uint)obj;
            }
            else
            {
                aux = (StatusMask)obj;
            }

            return _value == aux._value;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="StatusMask" /> to <see cref="uint" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="uint" /> value.</returns>
        public static implicit operator uint(StatusMask value)
        {
            return value.ToUInt32();
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="uint" /> to <see cref="StatusMask" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="InstanceHandle" /> value.</returns>
        public static implicit operator StatusMask(uint value)
        {
            return FromUInt32(value);
        }

        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(StatusMask left, StatusMask right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(StatusMask left, StatusMask right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}
