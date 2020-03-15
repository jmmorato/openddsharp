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
    /// This policy controls the behavior of DDS when the value of an instance changes before it is finally communicated to some of its existing <see cref="DataReader" /> entities.
    /// </summary>
    /// <remarks>
    /// <para>This policy defaults to a <see cref="HistoryQosPolicyKind.KeepLastHistoryQos"/> with a <see cref="HistoryQosPolicy.Depth"/> of one.</para>
    /// <para>The setting of <see cref="HistoryQosPolicy.Depth"/> must be consistent with the <see cref="ResourceLimitsQosPolicy.MaxSamplesPerInstance"/> .
    /// For these two QoS to be consistent, they must verify that Depth &lt;= MaxSamplesPerInstance.</para>
    /// </remarks>
    public sealed class HistoryQosPolicy : IEquatable<HistoryQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the history kind applied to the <see cref="Entity" />.
        /// </summary>
        public HistoryQosPolicyKind Kind { get; set; }

        /// <summary>
        /// Gets or sets the history maximum depth.
        /// </summary>
        public int Depth { get; set; }
        #endregion

        #region Constructors
        internal HistoryQosPolicy()
        {
            Kind = HistoryQosPolicyKind.KeepLastHistoryQos;
            Depth = 1;
        }
        #endregion

        #region IEquatable<HistoryQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(HistoryQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return Kind == other.Kind &&
                   Depth == other.Depth;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is HistoryQosPolicy other) && Equals(other);

        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1470823525;
            hashCode = (hashCode * -1521134295) + Kind.GetHashCode();
            hashCode = (hashCode * -1521134295) + Depth.GetHashCode();
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
        public static bool operator ==(HistoryQosPolicy left, HistoryQosPolicy right)
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
        public static bool operator !=(HistoryQosPolicy left, HistoryQosPolicy right)
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
    internal struct HistoryQosPolicyWrapper
    {
        #region Fields
        public HistoryQosPolicyKind Kind;
        public int Depth;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="HistoryQosPolicyWrapper" /> to <see cref="HistoryQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="HistoryQosPolicy" /> object.</returns>
        public static implicit operator HistoryQosPolicy(HistoryQosPolicyWrapper value)
        {
            return new HistoryQosPolicy
            {
                Kind = value.Kind,
                Depth = value.Depth,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="HistoryQosPolicy" /> to <see cref="HistoryQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="HistoryQosPolicyWrapper" /> object.</returns>
        public static implicit operator HistoryQosPolicyWrapper(HistoryQosPolicy value)
        {
            return new HistoryQosPolicyWrapper
            {
                Kind = value.Kind,
                Depth = value.Depth,
            };
        }
        #endregion
    }
}