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
    /// The durability policy controls whether data writers should maintain samples after they
    /// have been sent to known subscribers. This policy applies to the <see cref="Topic" />, <see cref="DataReader" />, and
    /// <see cref="DataWriter" /> entities via the durability member of their respective QoS structures.
    /// </summary>
    /// <remarks>
    /// <para>The value offered is considered compatible with the value requested if and only if the inequality "offered kind &gt;= requested
    /// kind" evaluates to '<see langword="true" />'. For the purposes of this inequality, the values of <see cref="DurabilityQosPolicy.Kind"/> are considered ordered such
    /// that Volatile &lt; TransientLocal &lt; Transient &lt; Persistent.</para>
    /// </remarks>
    public sealed class DurabilityQosPolicy : IEquatable<DurabilityQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="DurabilityQosPolicyKind" /> assigned to the related <see cref="Entity" />.
        /// </summary>
        public DurabilityQosPolicyKind Kind { get; set; }
        #endregion

        #region Constructors
        internal DurabilityQosPolicy()
        {
            Kind = DurabilityQosPolicyKind.VolatileDurabilityQos;
        }
        #endregion

        #region IEquatable<DurabilityQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(DurabilityQosPolicy other)
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
            return (obj is DurabilityQosPolicy other) && Equals(other);
          
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Kind.GetHashCode();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(DurabilityQosPolicy left, DurabilityQosPolicy right)
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
        public static bool operator !=(DurabilityQosPolicy left, DurabilityQosPolicy right)
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
    internal struct DurabilityQosPolicyWrapper
    {
        #region Fields
        public DurabilityQosPolicyKind Kind;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="DurabilityQosPolicyWrapper" /> to <see cref="DurabilityQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="DurabilityQosPolicy" /> object.</returns>
        public static implicit operator DurabilityQosPolicy(DurabilityQosPolicyWrapper value)
        {
            return new DurabilityQosPolicy
            {
                Kind = value.Kind,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="DurabilityQosPolicy" /> to <see cref="DurabilityQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="DurabilityQosPolicyWrapper" /> object.</returns>
        public static implicit operator DurabilityQosPolicyWrapper(DurabilityQosPolicy value)
        {
            return new DurabilityQosPolicyWrapper
            {
                Kind = value.Kind,
            };
        }
        #endregion
    }
}
