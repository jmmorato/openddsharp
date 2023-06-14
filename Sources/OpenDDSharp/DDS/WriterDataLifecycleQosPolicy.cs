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
    /// This policy controls the behavior of the <see cref="DataWriter" /> with regards to the lifecycle of the data-instances it manages, that is, the
    /// data-instances that have been either explicitly registered with the <see cref="DataWriter" /> using the register operations or implicitly by directly writing the data.
    /// </summary>
    public sealed class WriterDataLifecycleQosPolicy : IEquatable<WriterDataLifecycleQosPolicy>
    {
        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether auto-dispo the unregistered instances or not.
        /// Controls the behavior when the <see cref="DataWriter" /> unregisters an instance by means of the unregister operations.
        /// If the value of this property is equals <see langword="true" />, causes the DataWriter to dispose the instance each time it
        /// is unregistered. The behavior is identical to explicitly calling one of the dispose operations on the instance prior to calling the unregister operation.
        /// Otherwise, if the value of this property is equals <see langword="false" />, will not cause this automatic disposition upon
        /// unregistering. The application can still call one of the dispose operations prior to unregistering the instance and accomplish the same effect.
        /// </summary>
        /// <remarks>
        /// Note that the deletion of a <see cref="DataWriter" /> automatically unregisters all data-instances it manages. Therefore the
        /// setting of the AutodisposeUnregisteredInstances flag will determine whether instances are ultimately disposed when the
        /// <see cref="DataWriter" /> is deleted either directly by means of the <see cref="Publisher" /> DeleteDataWriter operation or indirectly as a consequence of
        /// calling DeleteContainedEntities on the <see cref="Publisher" /> or the <see cref="DomainParticipant" /> that contains the <see cref="DataWriter" />.
        /// </remarks>
        public bool AutodisposeUnregisteredInstances { get; set; }
        #endregion

        #region Constructors
        internal WriterDataLifecycleQosPolicy()
        {
            AutodisposeUnregisteredInstances = true;
        }
        #endregion

        #region IEquatable<WriterDataLifecycleQosPolicy> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(WriterDataLifecycleQosPolicy other)
        {
            if (other == null)
            {
                return false;
            }

            return AutodisposeUnregisteredInstances == other.AutodisposeUnregisteredInstances;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is WriterDataLifecycleQosPolicy other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = -910965429;
            hashCode = (hashCode * -1521134295) + AutodisposeUnregisteredInstances.GetHashCode();
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
        public static bool operator ==(WriterDataLifecycleQosPolicy left, WriterDataLifecycleQosPolicy right)
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
        public static bool operator !=(WriterDataLifecycleQosPolicy left, WriterDataLifecycleQosPolicy right)
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
    internal struct WriterDataLifecycleQosPolicyWrapper
    {
        #region Fields
        [MarshalAs(UnmanagedType.I1)]
        public bool AutodisposeUnregisteredInstances;
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion operator from <see cref="WriterDataLifecycleQosPolicyWrapper" /> to <see cref="WriterDataLifecycleQosPolicy" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="WriterDataLifecycleQosPolicy" /> object.</returns>
        public static implicit operator WriterDataLifecycleQosPolicy(WriterDataLifecycleQosPolicyWrapper value)
        {
            return new WriterDataLifecycleQosPolicy
            {
                AutodisposeUnregisteredInstances = value.AutodisposeUnregisteredInstances,
            };
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="WriterDataLifecycleQosPolicy" /> to <see cref="WriterDataLifecycleQosPolicyWrapper" />.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The <see cref="WriterDataLifecycleQosPolicyWrapper" /> object.</returns>
        public static implicit operator WriterDataLifecycleQosPolicyWrapper(WriterDataLifecycleQosPolicy value)
        {
            return new WriterDataLifecycleQosPolicyWrapper
            {
                AutodisposeUnregisteredInstances = value.AutodisposeUnregisteredInstances,
            };
        }
        #endregion
    }
}