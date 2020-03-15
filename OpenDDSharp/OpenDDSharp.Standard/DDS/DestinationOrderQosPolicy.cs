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
    /// This policy controls how each <see cref="Subscriber" /> resolves the final value of a data instance that is written by multiple <see cref="DataWriter" />
    /// objects (which may be associated with different <see cref="Publisher" /> objects) running on different nodes.
    /// </summary>
    /// <remarks>
    /// The value offered is considered compatible with the value requested if and only if the inequality "offered kind &gt;= requested
    /// kind" evaluates to '<see langword="true" />'. For the purposes of this inequality, the values of <see cref="DestinationOrderQosPolicyKind" /> are considered
    /// ordered such that <see cref="DestinationOrderQosPolicyKind.ByReceptionTimestampDestinationOrderQos" /> &lt; <see cref="DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos" />.
    /// </remarks>
    public sealed class DestinationOrderQosPolicy : IEquatable<DestinationOrderQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the destination order kind applied to the <see cref="Entity" />.
        /// </summary>
        public DestinationOrderQosPolicyKind Kind { get; set; }
        #endregion

        #region Constructors
        internal DestinationOrderQosPolicy()
        {
            Kind = DestinationOrderQosPolicyKind.ByReceptionTimestampDestinationOrderQos;
        }
        #endregion

        #region IEquatable<DestinationOrderQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(DestinationOrderQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return Kind == other.Kind;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is DestinationOrderQosPolicy other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return -2026186021 + Kind.GetHashCode();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(DestinationOrderQosPolicy left, DestinationOrderQosPolicy right)
        {
            if (left == null && right == null)
            {
                return true;
            }

            if (left == null || right == null)
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
        public static bool operator !=(DestinationOrderQosPolicy left, DestinationOrderQosPolicy right)
        {
            if (left == null && right == null)
            {
                return false;
            }

            if (left == null || right == null)
            {
                return true;
            }

            return !left.Equals(right);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DestinationOrderQosPolicyWrapper
    {
        #region Fields
        public DestinationOrderQosPolicyKind Kind;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="DestinationOrderQosPolicyWrapper" /> to <see cref="DestinationOrderQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="DestinationOrderQosPolicy" /> object.</returns>
        public static implicit operator DestinationOrderQosPolicy(DestinationOrderQosPolicyWrapper value)
        {
            return new DestinationOrderQosPolicy
            {
                Kind = value.Kind,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="DestinationOrderQosPolicy" /> to <see cref="DestinationOrderQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="DestinationOrderQosPolicy" /> object.</returns>
        public static implicit operator DestinationOrderQosPolicyWrapper(DestinationOrderQosPolicy value)
        {
            return new DestinationOrderQosPolicyWrapper
            {
                Kind = value.Kind,
            };
        }
        #endregion
    }
}