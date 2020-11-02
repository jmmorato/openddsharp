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
using System.Globalization;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// Type definition for an instance handle.
    /// </summary>
    public struct InstanceHandle : IEquatable<InstanceHandle>
    {
        #region Constants
        public static readonly InstanceHandle HandleNil = 0;
        #endregion

        #region Fields
        private readonly int _value;
        #endregion

        #region Constructors
        internal InstanceHandle(int value)
        {
            _value = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="InstanceHandle"/> from an <see cref="int"/> value.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value.</param>
        /// <returns>A newly created <see cref="InstanceHandle"/> object.</returns>
        public static InstanceHandle FromInt32(int value)
        {
            InstanceHandle r = new InstanceHandle(value);
            return r;
        }

        /// <summary>
        /// Gets the <see cref="int"/> value of the <see cref="InstanceHandle"/>.
        /// </summary>
        /// <returns>The <see cref="int"/> value.</returns>
        public int ToInt32()
        {
            return _value;
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        /// <returns>The string representation of the instance.</returns>
        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
        #endregion

        #region IEquatable<InstanceHandle> Members
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

            if (obj is int integer)
            {
                return _value == integer;
            }

            if (obj is InstanceHandle handle)
            {
                return Equals(handle);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other"> An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(InstanceHandle other)
        {
            return _value.Equals(other._value);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return _value;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="InstanceHandle" /> to <see cref="int" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="int" /> value.</returns>
        public static implicit operator int(InstanceHandle value)
        {
            return value.ToInt32();
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="int" /> to <see cref="InstanceHandle" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="InstanceHandle" /> value.</returns>
        public static implicit operator InstanceHandle(int value)
        {
            return FromInt32(value);
        }

        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="x">The left value for the comparison.</param>
        /// <param name="y">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(InstanceHandle x, InstanceHandle y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="x">The left value for the comparison.</param>
        /// <param name="y">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(InstanceHandle x, InstanceHandle y)
        {
            return !x.Equals(y);
        }
        #endregion
    }
}
