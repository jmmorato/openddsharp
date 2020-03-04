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
    /// This policy controls the mechanism and parameters used by DDS to ensure that particular entities on the network are still "alive".
    /// </summary>
    /// <remarks>
    /// The value offered is considered compatible with the value requested if and only if the following conditions are met:
    /// <list type="bullet">
    ///     <item><description>The inequality "offered Kind &gt;= requested Kind" evaluates to 'true'. For the purposes of this inequality, the values of Liveliness kind are considered ordered such that: Automatic &lt; ManualByParticipant &lt; ManualByTopic.</description></item>
    ///     <item><description>The inequality "offered LeaseDuration &lt;= requested LeaseDuration" evaluates to 'true'.</description></item>
    /// </list>
    /// </remarks>
    public sealed class LivelinessQosPolicy : IEquatable<LivelinessQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the liveliness kind applied to the <see cref="Entity" />.
        /// </summary>
        public LivelinessQosPolicyKind Kind { get; set; }

        /// <summary>
        /// Gets or sets the liveliness lease duration.
        /// </summary>
        public Duration LeaseDuration { get; set; }
        #endregion

        #region Constructors
        internal LivelinessQosPolicy()
        {
            Kind = LivelinessQosPolicyKind.AutomaticLivelinessQos;
            LeaseDuration = new Duration
            {
                Seconds = Duration.InfiniteSeconds,
                NanoSeconds = Duration.InfiniteNanoseconds,
            };
        }
        #endregion

        #region IEquatable<LivelinessQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(LivelinessQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return Kind == other.Kind &&
                   LeaseDuration == other.LeaseDuration;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is LivelinessQosPolicy other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1464949012;
            hashCode = (hashCode * -1521134295) + Kind.GetHashCode();
            hashCode = (hashCode * -1521134295) + LeaseDuration.GetHashCode();
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
        public static bool operator ==(LivelinessQosPolicy left, LivelinessQosPolicy right)
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
        public static bool operator !=(LivelinessQosPolicy left, LivelinessQosPolicy right)
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
    internal struct LivelinessQosPolicyWrapper
    {
        #region Fields
        public LivelinessQosPolicyKind Kind;
        public Duration LeaseDuration;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="LivelinessQosPolicyWrapper" /> to <see cref="LivelinessQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="LivelinessQosPolicy" /> object.</returns>
        public static implicit operator LivelinessQosPolicy(LivelinessQosPolicyWrapper value)
        {
            return new LivelinessQosPolicy
            {
                Kind = value.Kind,
                LeaseDuration = value.LeaseDuration,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="LivelinessQosPolicy" /> to <see cref="LivelinessQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="LivelinessQosPolicyWrapper" /> object.</returns>
        public static implicit operator LivelinessQosPolicyWrapper(LivelinessQosPolicy value)
        {
            return new LivelinessQosPolicyWrapper
            {
                Kind = value.Kind,
                LeaseDuration = value.LeaseDuration,
            };
        }
        #endregion
    }
}