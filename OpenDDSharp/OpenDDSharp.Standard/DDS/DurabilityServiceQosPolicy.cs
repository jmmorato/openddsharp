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
    /// This policy is used to configure the history QoS and the resource limits QoS used by the fictitious <see cref="DataReader" /> and
    /// <see cref="DataWriter" /> used by the "persistence service".
    /// </summary>
    public sealed class DurabilityServiceQosPolicy : IEquatable<DurabilityServiceQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets a value that specifies how long the durability service must wait before it is allowed to remove the information on
        /// the transient or persistent topic data-instances as a result of incoming dispose messages.
        /// </summary>
        public Duration ServiceCleanupDelay { get; set; }

        /// <summary>
        /// Gets or sets the type of history the durability service must apply for the transient or
        /// persistent topic data-instances.
        /// </summary>
        public HistoryQosPolicyKind HistoryKind { get; set; }

        /// <summary>
        /// Gets or sets the number of samples of each instance of data (identified by its key) that is managed by the durability service 
        /// for the transient or persistent topic data-instances.
        /// </summary>
        public int HistoryDepth { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of data-samples the <see cref="DataWriter" /> (or <see cref="DataReader" />) can manage across all the instances associated with it.
        /// Represents the maximum samples the middleware can store for any one <see cref="DataWriter" /> (or <see cref="DataReader" />).
        /// </summary>
        public int MaxSamples { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of instances the <see cref="DataWriter" /> (or <see cref="DataReader" />) can manage.
        /// </summary>
        public int MaxInstances { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of samples of any one instance a <see cref="DataWriter" /> (or <see cref="DataReader" />) can manage.
        /// </summary>
        public int MaxSamplesPerInstance { get; set; }
        #endregion

        #region Constructors
        internal DurabilityServiceQosPolicy()
        {
            ServiceCleanupDelay = new Duration
            {
                Seconds = Duration.ZeroSeconds,
                NanoSeconds = Duration.ZeroNanoseconds,
            };
            HistoryKind = HistoryQosPolicyKind.KeepLastHistoryQos;
            HistoryDepth = 1;
            MaxSamples = ResourceLimitsQosPolicy.LengthUnlimited;
            MaxInstances = ResourceLimitsQosPolicy.LengthUnlimited;
            MaxSamplesPerInstance = ResourceLimitsQosPolicy.LengthUnlimited;
        }
        #endregion

        #region IEquatable<DurabilityServiceQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(DurabilityServiceQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return ServiceCleanupDelay == other.ServiceCleanupDelay &&
                   HistoryKind == other.HistoryKind &&
                   HistoryDepth == other.HistoryDepth &&
                   MaxSamples == other.MaxSamples &&
                   MaxInstances == other.MaxInstances &&
                   MaxSamplesPerInstance == other.MaxSamplesPerInstance;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is DurabilityServiceQosPolicy other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = -1955302307;
            hashCode = (hashCode * -1521134295) + ServiceCleanupDelay.GetHashCode();
            hashCode = (hashCode * -1521134295) + HistoryKind.GetHashCode();
            hashCode = (hashCode * -1521134295) + HistoryDepth.GetHashCode();
            hashCode = (hashCode * -1521134295) + MaxSamples.GetHashCode();
            hashCode = (hashCode * -1521134295) + MaxInstances.GetHashCode();
            hashCode = (hashCode * -1521134295) + MaxSamplesPerInstance.GetHashCode();
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
        public static bool operator ==(DurabilityServiceQosPolicy left, DurabilityServiceQosPolicy right)
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
        public static bool operator !=(DurabilityServiceQosPolicy left, DurabilityServiceQosPolicy right)
        {
            if (left == null && right == null)
            {
                return true;
            }

            if (left == null || right == null)
            {
                return false;
            }

            return !left.Equals(right);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DurabilityServiceQosPolicyWrapper
    {
        #region Fields
        public Duration ServiceCleanupDelay;
        public HistoryQosPolicyKind HistoryKind;
        public int HistoryDepth;
        public int MaxSamples;
        public int MaxInstances;
        public int MaxSamplesPerInstance;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="DurabilityServiceQosPolicyWrapper" /> to <see cref="DurabilityServiceQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="DurabilityServiceQosPolicy" /> object.</returns>
        public static implicit operator DurabilityServiceQosPolicy(DurabilityServiceQosPolicyWrapper value)
        {
            return new DurabilityServiceQosPolicy
            {
                ServiceCleanupDelay = value.ServiceCleanupDelay,
                HistoryKind = value.HistoryKind,
                HistoryDepth = value.HistoryDepth,
                MaxSamples = value.MaxSamples,
                MaxInstances = value.MaxInstances,
                MaxSamplesPerInstance = value.MaxSamplesPerInstance,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="DurabilityServiceQosPolicy" /> to <see cref="DurabilityServiceQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="DurabilityQosPolicyWrapper" /> object.</returns>
        public static implicit operator DurabilityServiceQosPolicyWrapper(DurabilityServiceQosPolicy value)
        {
            return new DurabilityServiceQosPolicyWrapper
            {
                ServiceCleanupDelay = value.ServiceCleanupDelay,
                HistoryKind = value.HistoryKind,
                HistoryDepth = value.HistoryDepth,
                MaxSamples = value.MaxSamples,
                MaxInstances = value.MaxInstances,
                MaxSamplesPerInstance = value.MaxSamplesPerInstance,
            };
        }
        #endregion
    }
}
