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
    /// Indicates if the samples are from a live <see cref="DataWriter" /> or not.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct InstanceStateKind : IEquatable<InstanceStateKind>
    {
        #region Constants
        /// <summary>
        /// Indicates that:
        /// <list type="bullet">
        ///     <item><description>Samples have been received for the instance</description></item>
        ///     <item><description>There are live <see cref="DataWriter" /> entities writing the instance</description></item>
        ///     <item><description>The instance has not been explicitly disposed (or else more samples have been received after it was disposed)</description></item>
        /// </list>
        /// </summary>
        public static readonly InstanceStateKind AliveInstanceState = 1U;

        /// <summary>
        /// Indicates the instance was explicitly disposed by a <see cref="DataWriter" /> by means of the Delete operation.
        /// </summary>
        public static readonly InstanceStateKind NotAliveDisposedInstanceState = 2U;

        /// <summary>
        /// Indicates the instance has been declared as not-alive by the <see cref="DataReader" /> because it detected that
        /// there are no live <see cref="DataWriter" /> entities writing that instance.
        /// </summary>
        public static readonly InstanceStateKind NotAliveNoWritersInstanceState = 4U;
        #endregion

        #region Fields
        private readonly uint _value;
        #endregion

        #region Constructors
        internal InstanceStateKind(uint value)
        {
            _value = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the bitwise <see cref="InstanceStateMask"/> value from the <see cref="InstanceStateKind"/> entry parameters.
        /// </summary>
        /// <param name="left">The left value of the operator.</param>
        /// <param name="right">The right value of the operator.</param>
        /// <returns>The <see cref="InstanceStateMask"/> value.</returns>
        public static InstanceStateMask BitwiseOr(InstanceStateKind left, InstanceStateKind right)
        {
            return left | (InstanceStateMask)right;
        }

        /// <summary>
        /// Creates a new <see cref="InstanceStateKind"/> from an <see cref="uint"/> value.
        /// </summary>
        /// <param name="value">The <see cref="uint"/> value.</param>
        /// <returns>A newly created <see cref="InstanceStateKind"/> object.</returns>
        public static InstanceStateKind FromUInt32(uint value)
        {
            return new InstanceStateKind(value);
        }

        /// <summary>
        /// Gets the <see cref="uint"/> value of the <see cref="InstanceStateKind"/>.
        /// </summary>
        /// <returns>The <see cref="uint"/> value.</returns>
        public uint ToUInt32()
        {
            return _value;
        }

        /// <summary>
        /// Gets the <see cref="InstanceStateMask"/> value of the <see cref="InstanceStateKind"/>.
        /// </summary>
        /// <returns>The <see cref="InstanceStateMask"/> value.</returns>
        public InstanceStateMask ToInstanceStateMask()
        {
            return _value;
        }
        #endregion

        #region IEquatable<InstanceStateKind> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(InstanceStateKind other)
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

            InstanceStateKind aux;
            if (obj is uint)
            {
                aux = (uint)obj;
            }
            else
            {
                aux = (InstanceStateKind)obj;
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
        /// Implicit conversion operator from <see cref="InstanceStateKind" /> to <see cref="uint" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="uint" /> value.</returns>
        public static implicit operator uint(InstanceStateKind value)
        {
            return value.ToUInt32();
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="uint" /> to <see cref="InstanceStateKind" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="InstanceStateKind" /> value.</returns>
        public static implicit operator InstanceStateKind(uint value)
        {
            return FromUInt32(value);
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="InstanceStateKind" /> to <see cref="InstanceStateMask" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="StatusMask" /> value.</returns>
        public static implicit operator InstanceStateMask(InstanceStateKind value)
        {
            return value.ToInstanceStateMask();
        }

        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(InstanceStateKind left, InstanceStateKind right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(InstanceStateKind left, InstanceStateKind right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Bit-wise operator.
        /// </summary>
        /// <param name="left">The left value of the operator.</param>
        /// <param name="right">The right value of the operator.</param>
        /// <returns>The resulting <see cref="InstanceStateMask" />.</returns>
        public static InstanceStateMask operator |(InstanceStateKind left, InstanceStateKind right)
        {
            return BitwiseOr(left, right);
        }
        #endregion
    }
}
