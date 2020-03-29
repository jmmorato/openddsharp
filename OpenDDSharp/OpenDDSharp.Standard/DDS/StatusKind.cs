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
    /// Kinds of communication status.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct StatusKind : IEquatable<StatusKind>
    {
        #region Constants
        /// <summary>
        /// Another topic exists with the same name but different characteristics.
        /// </summary>
        public static readonly StatusKind InconsistentTopicStatus = 1U;

        /// <summary>
        /// The deadline that the <see cref="DataWriter" /> has committed through its <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.
        /// </summary>
        public static readonly StatusKind OfferedDeadlineMissedStatus = 2U;

        /// <summary>
        /// The deadline that the <see cref="DataReader" /> was expecting through its <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.
        /// </summary>
        public static readonly StatusKind RequestedDeadlineMissedStatus = 4U;

        /// <summary>
        /// A QoS policy value was incompatible with what was requested.
        /// </summary>
        public static readonly StatusKind OfferedIncompatibleQosStatus = 32U;

        /// <summary>
        /// A QoS policy value was incompatible with what is offered.
        /// </summary>
        public static readonly StatusKind RequestedIncompatibleQosStatus = 64U;

        /// <summary>
        /// A sample has been lost (i.e. was never received).
        /// </summary>
        public static readonly StatusKind SampleLostStatus = 128U;

        /// <summary>
        /// A (received) sample has been rejected.
        /// </summary>
        public static readonly StatusKind SampleRejectedStatus = 256U;

        /// <summary>
        /// New data is available.
        /// </summary>
        public static readonly StatusKind DataOnReadersStatus = 512U;

        /// <summary>
        /// One or more new data samples have been received.
        /// </summary>
        public static readonly StatusKind DataAvailableStatus = 1024U;

        /// <summary>
        /// The liveliness that the <see cref="DataWriter" /> has committed to through its  <see cref="LivelinessQosPolicy" /> was not respected, 
        /// thus <see cref="DataReader" /> entities will consider the <see cref="DataWriter" /> as no longer alive.
        /// </summary>
        public static readonly StatusKind LivelinessLostStatus = 2048U;

        /// <summary>
        /// The liveliness of one or more <see cref="DataWriter" /> that were writing instances read through the <see cref="DataReader" /> has changed. 
        /// Some <see cref="DataWriter" /> have become alive or not alive.
        /// </summary>
        public static readonly StatusKind LivelinessChangedStatus = 4096U;

        /// <summary>
        /// The <see cref="DataWriter" /> has found <see cref="DataReader" /> that matches the <see cref="Topic" /> and has compatible QoS.
        /// </summary>
        public static readonly StatusKind PublicationMatchedStatus = 8192U;

        /// <summary>
        /// The <see cref="DataReader" /> has found <see cref="DataWriter" /> that matches the <see cref="Topic" /> and has compatible QoS.
        /// </summary>
        public static readonly StatusKind SubscriptionMatchedStatus = 16384U;
        #endregion

        #region Fields
        private readonly uint _value;
        #endregion

        #region Constructors
        internal StatusKind(uint value)
        {
            _value = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the bitwise <see cref="StatusMask"/> value from the <see cref="StatusKind"/> entry parameters.
        /// </summary>
        /// <param name="left">The left value of the operator.</param>
        /// <param name="right">The right value of the operator.</param>
        /// <returns>The <see cref="StatusMask"/> value.</returns>
        public static StatusMask BitwiseOr(StatusKind left, StatusKind right)
        {
            return left | right;
        }

        /// <summary>
        /// Creates a new <see cref="StatusKind"/> from an <see cref="uint"/> value.
        /// </summary>
        /// <param name="value">The <see cref="uint"/> value.</param>
        /// <returns>A newly created <see cref="StatusKind"/> object.</returns>
        public static StatusKind FromUInt32(uint value)
        {
            return new StatusKind(value);
        }

        /// <summary>
        /// Gets the <see cref="uint"/> value of the <see cref="StatusKind"/>.
        /// </summary>
        /// <returns>The <see cref="uint"/> value.</returns>
        public uint ToUInt32()
        {
            return _value;
        }

        /// <summary>
        /// Gets the <see cref="StatusMask"/> value of the <see cref="StatusKind"/>.
        /// </summary>
        /// <returns>The <see cref="StatusMask"/> value.</returns>
        public StatusMask ToStatusMask()
        {
            return _value;
        }
        #endregion

        #region IEquatable<StatusKind> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(StatusKind other)
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

            StatusKind aux;
            if (obj is uint)
            {
                aux = (uint)obj;
            }
            else
            {
                aux = (StatusKind)obj;
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
        /// Implicit conversion operator from <see cref="StatusKind" /> to <see cref="uint" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="uint" /> value.</returns>
        public static implicit operator uint(StatusKind value)
        {
            return value.ToUInt32();
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="uint" /> to <see cref="StatusKind" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="StatusKind" /> value.</returns>
        public static implicit operator StatusKind(uint value)
        {
            return FromUInt32(value);
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="StatusKind" /> to <see cref="StatusMask" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="StatusMask" /> value.</returns>
        public static implicit operator StatusMask(StatusKind value)
        {
            return value.ToStatusMask();
        }

        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(StatusKind left, StatusKind right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(StatusKind left, StatusKind right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Bit-wise operator.
        /// </summary>
        /// <param name="left">The left value of the operator.</param>
        /// <param name="right">The right value of the operator.</param>
        /// <returns>The resulting <see cref="StatusMask" />.</returns>
        public static StatusMask operator |(StatusKind left, StatusKind right)
        {
            return BitwiseOr(left, right);
        }
        #endregion
    }
}
