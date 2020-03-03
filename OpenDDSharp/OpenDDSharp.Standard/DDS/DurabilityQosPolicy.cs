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
    /// <para>The value offered is considered compatible with the value requested if and only if the inequality "offered kind >= requested
    /// kind" evaluates to 'true'. For the purposes of this inequality, the values of Durability kind are considered ordered such
    /// that Volatile &lt; TransientLocal &lt; Transient &lt; Persistent.</para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct DurabilityQosPolicy : IEquatable<DurabilityQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="DurabilityQosPolicyKind" /> assigned to the related <see cref="Entity" />.
        /// </summary>
        public DurabilityQosPolicyKind Kind { get; set; }
        #endregion

        #region IEquatable<DurabilityQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(DurabilityQosPolicy other)
        {
            return Kind == other.Kind;
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

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((DurabilityQosPolicy)obj);
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
            return !left.Equals(right);
        }
        #endregion
    }
}
