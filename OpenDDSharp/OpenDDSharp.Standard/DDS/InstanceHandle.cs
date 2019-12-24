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

namespace OpenDDSharp.DDS
{
    public struct InstanceHandle
    {
        #region Constants
        public static readonly InstanceHandle HandleNil = 0;
        #endregion

        #region Fields
        private int _value;
        #endregion

        #region Constructors
        internal InstanceHandle(int value)
        {
            _value = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Implicit conversion operator from <see cref="InstanceHandle" /> to <see cref="int" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="int" /> value.</returns>
        public static implicit operator int (InstanceHandle value)
        {
            return value._value;
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="int" /> to <see cref="InstanceHandle" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="InstanceHandle" /> value.</returns>
        public static implicit operator InstanceHandle(int value)
        {
            InstanceHandle r = new InstanceHandle(value);
            return r;
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

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType() && obj.GetType() != typeof(int))
            {
                return false;
            }

            InstanceHandle aux = InstanceHandle.HandleNil;
            if (obj.GetType() == typeof(int))
            {
                aux = (int)obj;
            }
            else
            {
                aux = (InstanceHandle)obj;
            }

            return (_value == aux._value);
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
    }
}
