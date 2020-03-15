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
    /// The Presentation QoS policy controls how changes to instances by publishers are presented to data readers. It affects the relative ordering of these changes and 
    /// the scope of this ordering. Additionally, this policy introduces the concept of coherent change sets.
    /// </summary>
    /// <remarks>
    /// This policy controls the ordering and scope of samples made available to the subscriber, but the subscriber application must use the proper logic in reading samples 
    /// to guarantee the requested behavior.
    /// </remarks>
    public sealed class PresentationQosPolicy : IEquatable<PresentationQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets a value that specifies how the samples representing changes to data instances are presented to a subscribing application.
        /// </summary>
        public PresentationQosPolicyKind AccessScope { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether allow one or more changes to an instance be made available to an associated data reader as a single change or not. If a data reader does not receive
        /// the entire set of coherent changes made by a publisher, then none of the changes are made available. The semantics of coherent changes are similar in nature
        /// to those found in transactions provided by many relational databases. By default, CoherentAccess is <see langword="false" />.
        /// </summary>
        public bool CoherentAccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether preserve the order of changes or not. By default, OrderedAccess is <see langword="false" />.
        /// </summary>
        public bool OrderedAccess { get; set; }
        #endregion

        #region Constructors
        internal PresentationQosPolicy()
        {
            AccessScope = PresentationQosPolicyKind.InstancePresentationQos;
            CoherentAccess = false;
            OrderedAccess = false;
        }
        #endregion

        #region IEquatable<PresentationQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(PresentationQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return AccessScope == other.AccessScope &&
                   CoherentAccess == other.CoherentAccess &&
                   OrderedAccess == other.OrderedAccess;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is PresentationQosPolicy other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 47819098;
            hashCode = (hashCode * -1521134295) + AccessScope.GetHashCode();
            hashCode = (hashCode * -1521134295) + CoherentAccess.GetHashCode();
            hashCode = (hashCode * -1521134295) + OrderedAccess.GetHashCode();
            return hashCode;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(PresentationQosPolicy left, PresentationQosPolicy right)
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
        public static bool operator !=(PresentationQosPolicy left, PresentationQosPolicy right)
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
    internal struct PresentationQosPolicyWrapper
    {
        #region Fields
        public PresentationQosPolicyKind AccessScope;
        public bool CoherentAccess;
        public bool OrderedAccess;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="PresentationQosPolicyWrapper" /> to <see cref="PresentationQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="PresentationQosPolicy" /> object.</returns>
        public static implicit operator PresentationQosPolicy(PresentationQosPolicyWrapper value)
        {
            return new PresentationQosPolicy
            {
                AccessScope = value.AccessScope,
                CoherentAccess = value.CoherentAccess,
                OrderedAccess = value.OrderedAccess,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="PresentationQosPolicy" /> to <see cref="PresentationQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="PresentationQosPolicy" /> object.</returns>
        public static implicit operator PresentationQosPolicyWrapper(PresentationQosPolicy value)
        {
            return new PresentationQosPolicyWrapper
            {
                AccessScope = value.AccessScope,
                CoherentAccess = value.CoherentAccess,
                OrderedAccess = value.OrderedAccess,
            };
        }
        #endregion
    }
}