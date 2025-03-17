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
    /// This policy controls the behavior of the <see cref="DataReader" /> with regards to the lifecycle of the data-instances it manages, that is, the
    /// data-instances that have been received and for which the <see cref="DataReader" /> maintains some internal resources.
    /// </summary>
    public sealed class ReaderDataLifecycleQosPolicy : IEquatable<ReaderDataLifecycleQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the maximum duration for which the <see cref="DataReader" /> will maintain information
        /// regarding an instance once its InstanceState becomes NotAliveNoWriters. After this time elapses, the <see cref="DataReader" />
        /// will purge all internal information regarding the instance, any untaken samples will also be lost.
        /// </summary>
        public Duration AutopurgeNowriterSamplesDelay { get; set; }

        /// <summary>
        /// Gets or sets the maximum duration for which the <see cref="DataReader" /> will maintain samples for
        /// an instance once its InstanceState becomes NotAliveDisposed. After this time elapses, the <see cref="DataReader" /> will purge all
        /// samples for the instance.
        /// </summary>
        public Duration AutopurgeDisposedSamplesDelay { get; set; }
        #endregion

        #region Constructors
        internal ReaderDataLifecycleQosPolicy()
        {
            AutopurgeNowriterSamplesDelay = new Duration
            {
                Seconds = Duration.InfiniteSeconds,
                NanoSeconds = Duration.InfiniteNanoseconds,
            };

            AutopurgeDisposedSamplesDelay = new Duration
            {
                Seconds = Duration.InfiniteSeconds,
                NanoSeconds = Duration.InfiniteNanoseconds,
            };
        }
        #endregion

        #region IEquatable<ReaderDataLifecycleQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(ReaderDataLifecycleQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return AutopurgeDisposedSamplesDelay == other.AutopurgeDisposedSamplesDelay &&
                   AutopurgeNowriterSamplesDelay == other.AutopurgeNowriterSamplesDelay;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is ReaderDataLifecycleQosPolicy other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = -910965429;
            hashCode = (hashCode * -1521134295) + AutopurgeNowriterSamplesDelay.GetHashCode();
            hashCode = (hashCode * -1521134295) + AutopurgeDisposedSamplesDelay.GetHashCode();
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
        public static bool operator ==(ReaderDataLifecycleQosPolicy left, ReaderDataLifecycleQosPolicy right)
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
        public static bool operator !=(ReaderDataLifecycleQosPolicy left, ReaderDataLifecycleQosPolicy right)
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
    internal struct ReaderDataLifecycleQosPolicyWrapper
    {
        #region Fields
        public Duration AutopurgeNowriterSamplesDelay;
        public Duration AutopurgeDisposedSamplesDelay;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="ReaderDataLifecycleQosPolicyWrapper" /> to <see cref="ReaderDataLifecycleQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="ReaderDataLifecycleQosPolicy" /> object.</returns>
        public static implicit operator ReaderDataLifecycleQosPolicy(ReaderDataLifecycleQosPolicyWrapper value)
        {
            return new ReaderDataLifecycleQosPolicy
            {
                AutopurgeNowriterSamplesDelay = value.AutopurgeNowriterSamplesDelay,
                AutopurgeDisposedSamplesDelay = value.AutopurgeDisposedSamplesDelay,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="ReaderDataLifecycleQosPolicy" /> to <see cref="ReaderDataLifecycleQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="ReaderDataLifecycleQosPolicyWrapper" /> object.</returns>
        public static implicit operator ReaderDataLifecycleQosPolicyWrapper(ReaderDataLifecycleQosPolicy value)
        {
            return new ReaderDataLifecycleQosPolicyWrapper
            {
                AutopurgeNowriterSamplesDelay = value.AutopurgeNowriterSamplesDelay,
                AutopurgeDisposedSamplesDelay = value.AutopurgeDisposedSamplesDelay,
            };
        }
        #endregion
    }
}