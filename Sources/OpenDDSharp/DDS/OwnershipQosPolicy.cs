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
    /// This policy controls whether DDS allows multiple <see cref="DataWriter" /> objects to update the same instance (identified by Topic + key) of a data-object.
    /// </summary>
    public sealed class OwnershipQosPolicy : IEquatable<OwnershipQosPolicy>
    {
        #region Propeties
        /// <summary>
        /// Gets or sets the ownership kind applied to the <see cref="Entity" />.
        /// </summary>
        public OwnershipQosPolicyKind Kind { get; set; }
        #endregion

        #region Constructors
        internal OwnershipQosPolicy()
        {
            Kind = OwnershipQosPolicyKind.SharedOwnershipQos;
        }
        #endregion

        #region IEquatable<OwnershipQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(OwnershipQosPolicy other)
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
            return (obj is OwnershipQosPolicy other) && Equals(other);
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
        public static bool operator ==(OwnershipQosPolicy left, OwnershipQosPolicy right)
        {
            if (left is null && right is null)
            {
                return true;
            }

            if (left is null || right is null)
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
        public static bool operator !=(OwnershipQosPolicy left, OwnershipQosPolicy right)
        {
            if (left is null && right is null)
            {
                return false;
            }

            if (left is null || right is null)
            {
                return true;
            }

            return !left.Equals(right);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct OwnershipQosPolicyWrapper
    {
        #region Fields
        public OwnershipQosPolicyKind Kind;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="OwnershipQosPolicyWrapper" /> to <see cref="OwnershipQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="OwnershipQosPolicy" /> object.</returns>
        public static implicit operator OwnershipQosPolicy(OwnershipQosPolicyWrapper value)
        {
            return new OwnershipQosPolicy
            {
                Kind = value.Kind,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="OwnershipQosPolicy" /> to <see cref="OwnershipQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="OwnershipQosPolicy" /> object.</returns>
        public static implicit operator OwnershipQosPolicyWrapper(OwnershipQosPolicy value)
        {
            return new OwnershipQosPolicyWrapper
            {
                Kind = value.Kind,
            };
        }
        #endregion
    }
}