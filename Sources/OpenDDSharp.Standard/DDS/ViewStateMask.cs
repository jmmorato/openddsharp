﻿/*********************************************************************
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
    /// Represent a bit-mask of <see cref="ViewStateKind" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ViewStateMask : IEquatable<ViewStateMask>
    {
        #region Constants
        /// <summary>
        /// A mask containing any <see cref="ViewStateKind" />.
        /// </summary>
        public static readonly ViewStateMask AnyViewState = 65535U;
        #endregion

        #region Fields
        private readonly uint _value;
        #endregion

        #region Constructors
        internal ViewStateMask(uint value)
        {
            _value = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="ViewStateMask"/> from an <see cref="uint"/> value.
        /// </summary>
        /// <param name="value">The <see cref="uint"/> value.</param>
        /// <returns>A newly created <see cref="ViewStateMask"/> object.</returns>
        public static ViewStateMask FromUInt32(uint value)
        {
            return new ViewStateMask(value);
        }

        /// <summary>
        /// Gets the <see cref="uint"/> value of the <see cref="ViewStateMask"/>.
        /// </summary>
        /// <returns>The <see cref="uint"/> value.</returns>
        public uint ToUInt32()
        {
            return _value;
        }
        #endregion

        #region IEquatable<ViewStateMask> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(ViewStateMask other)
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

            ViewStateMask aux;
            if (obj is uint)
            {
                aux = (uint)obj;
            }
            else
            {
                aux = (ViewStateMask)obj;
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
        /// Implicit conversion operator from <see cref="ViewStateMask" /> to <see cref="uint" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="uint" /> value.</returns>
        public static implicit operator uint(ViewStateMask value)
        {
            return value.ToUInt32();
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="uint" /> to <see cref="ViewStateMask" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="ViewStateMask" /> value.</returns>
        public static implicit operator ViewStateMask(uint value)
        {
            return FromUInt32(value);
        }

        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="x">The left value for the comparison.</param>
        /// <param name="y">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(ViewStateMask x, ViewStateMask y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="x">The left value for the comparison.</param>
        /// <param name="y">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(ViewStateMask x, ViewStateMask y)
        {
            return !x.Equals(y);
        }
        #endregion
    }
}
