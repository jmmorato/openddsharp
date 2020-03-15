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
    /// This policy controls the resources that DDS can use in order to meet the requirements imposed by the application and other QoS settings.
    /// </summary>
    /// <remarks>
    /// <para>If the <see cref="DataWriter" /> objects are communicating samples faster than they are ultimately taken by the <see cref="DataReader" /> objects, the
    /// middleware will eventually hit against some of the QoS-imposed resource limits. Note that this may occur when just a single
    /// <see cref="DataReader" /> cannot keep up with its corresponding <see cref="DataWriter" />. The behavior in this case depends on the setting for the
    /// Reliability QoS. If reliability is BestEffort then DDS is allowed to drop samples. If the reliability is Reliable, DDS will block the <see cref="DataWriter" />
    /// or discard the sample at the <see cref="DataReader" /> in order not to lose existing samples.</para>
    /// <para>The setting of ResourceLimits MaxSamples must be consistent with the MaxSamplesPerInstance. For these two
    /// values to be consistent they must verify that “MaxSamples &gt;= MaxSamplesPerInstance.”</para>
    /// <para>The setting of ResourceLimits MaxSamplesPerInstance must be consistent with the History Depth. For these two
    /// QoS to be consistent, they must verify that "depth &lt;= MaxSamplesPerInstance".</para>
    /// <para>An attempt to set this policy to inconsistent values when an entity is created of via a SetQos operation will cause the operation to fail.</para>
    /// </remarks>
    public sealed class ResourceLimitsQosPolicy : IEquatable<ResourceLimitsQosPolicy>
    {
        #region Constants
        /// <summary>
        /// Used to indicate the absence of a particular limit.
        /// </summary>
        public const int LengthUnlimited = -1;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the maximum number of samples a single <see cref="DataWriter" /> or <see cref="DataReader" /> can manage across all of its instances.
        /// The default value is <see cref="LengthUnlimited" />.
        /// </summary>
        public int MaxSamples { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of instances that a <see cref="DataWriter" /> or <see cref="DataReader" /> can manage.
        /// The default value is <see cref="LengthUnlimited" />.
        /// </summary>
        public int MaxInstances { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of samples that can be managed for an individual instance in a single <see cref="DataWriter" /> or <see cref="DataReader" />.
        /// The default value is <see cref="LengthUnlimited" />.
        /// </summary>
        public int MaxSamplesPerInstance { get; set; }
        #endregion

        #region Constructors
        internal ResourceLimitsQosPolicy()
        {
            MaxSamples = LengthUnlimited;
            MaxInstances = LengthUnlimited;
            MaxSamplesPerInstance = LengthUnlimited;
        }
        #endregion

        #region IEquatable<ResourceLimitsQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(ResourceLimitsQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return MaxInstances == other.MaxInstances &&
                   MaxSamples == other.MaxSamples &&
                   MaxSamplesPerInstance == other.MaxSamplesPerInstance;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is ResourceLimitsQosPolicy other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1217544731;
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
        public static bool operator ==(ResourceLimitsQosPolicy left, ResourceLimitsQosPolicy right)
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
        public static bool operator !=(ResourceLimitsQosPolicy left, ResourceLimitsQosPolicy right)
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
    internal struct ResourceLimitsQosPolicyWrapper
    {
        #region Fields
        public int MaxSamples;
        public int MaxInstances;
        public int MaxSamplesPerInstance;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="ResourceLimitsQosPolicyWrapper" /> to <see cref="ResourceLimitsQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="ResourceLimitsQosPolicy" /> object.</returns>
        public static implicit operator ResourceLimitsQosPolicy(ResourceLimitsQosPolicyWrapper value)
        {
            return new ResourceLimitsQosPolicy
            {
                MaxSamples = value.MaxSamples,
                MaxInstances = value.MaxInstances,
                MaxSamplesPerInstance = value.MaxSamplesPerInstance,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="ResourceLimitsQosPolicy" /> to <see cref="ResourceLimitsQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="ResourceLimitsQosPolicyWrapper" /> object.</returns>
        public static implicit operator ResourceLimitsQosPolicyWrapper(ResourceLimitsQosPolicy value)
        {
            return new ResourceLimitsQosPolicyWrapper
            {
                MaxSamples = value.MaxSamples,
                MaxInstances = value.MaxInstances,
                MaxSamplesPerInstance = value.MaxSamplesPerInstance,
            };
        }
        #endregion
    }
}
