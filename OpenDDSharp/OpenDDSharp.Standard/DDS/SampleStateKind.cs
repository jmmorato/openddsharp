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
    /// Indicates whether or not a sample has ever been read.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SampleStateKind : IEquatable<SampleStateKind>
    {
        #region Constants
        /// <summary>
        /// New view state.
        /// </summary>
        public static readonly SampleStateKind ReadSampleState = 1U;

        /// <summary>
        /// Not new view state.
        /// </summary>
        public static readonly SampleStateKind NotReadSampleState = 2U;
        #endregion

        #region Fields
        private readonly uint _value;
        #endregion

        #region Constructors
        internal SampleStateKind(uint value)
        {
            _value = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the bitwise <see cref="SampleStateMask"/> value from the <see cref="SampleStateKind"/> entry parameters.
        /// </summary>
        /// <param name="left">The left value of the operator.</param>
        /// <param name="right">The right value of the operator.</param>
        /// <returns>The <see cref="SampleStateMask"/> value.</returns>
        public static SampleStateMask BitwiseOr(SampleStateKind left, SampleStateKind right)
        {
            return left | (SampleStateMask)right;
        }

        /// <summary>
        /// Creates a new <see cref="SampleStateKind"/> from an <see cref="uint"/> value.
        /// </summary>
        /// <param name="value">The <see cref="uint"/> value.</param>
        /// <returns>A newly created <see cref="SampleStateKind"/> object.</returns>
        public static SampleStateKind FromUInt32(uint value)
        {
            return new SampleStateKind(value);
        }

        /// <summary>
        /// Gets the <see cref="uint"/> value of the <see cref="SampleStateKind"/>.
        /// </summary>
        /// <returns>The <see cref="uint"/> value.</returns>
        public uint ToUInt32()
        {
            return _value;
        }

        /// <summary>
        /// Gets the <see cref="SampleStateMask"/> value of the <see cref="StatusKind"/>.
        /// </summary>
        /// <returns>The <see cref="SampleStateMask"/> value.</returns>
        public SampleStateMask ToSampleStateMask()
        {
            return _value;
        }
        #endregion

        #region IEquatable<SampleStateKind> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(SampleStateKind other)
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

            SampleStateKind aux;
            if (obj is uint)
            {
                aux = (uint)obj;
            }
            else
            {
                aux = (SampleStateKind)obj;
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
        /// Implicit conversion operator from <see cref="SampleStateKind" /> to <see cref="uint" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="uint" /> value.</returns>
        public static implicit operator uint(SampleStateKind value)
        {
            return value.ToUInt32();
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="uint" /> to <see cref="SampleStateKind" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="SampleStateKind" /> value.</returns>
        public static implicit operator SampleStateKind(uint value)
        {
            return FromUInt32(value);
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="StatusKind" /> to <see cref="StatusMask" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="SampleStateMask" /> value.</returns>
        public static implicit operator SampleStateMask(SampleStateKind value)
        {
            return value.ToSampleStateMask();
        }

        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="x">The left value for the comparison.</param>
        /// <param name="y">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(SampleStateKind x, SampleStateKind y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="x">The left value for the comparison.</param>
        /// <param name="y">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(SampleStateKind x, SampleStateKind y)
        {
            return !x.Equals(y);
        }
        #endregion
    }
}