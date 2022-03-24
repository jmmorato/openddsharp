/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018-2022 Jose Morato

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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// Holds the <see cref="DomainParticipantFactory" /> Quality of Service policies.
    /// </summary>
    public sealed class DomainParticipantFactoryQos : IEquatable<DomainParticipantFactoryQos>
    {
        #region Properties
        /// <summary>
        /// Gets the <see cref="EntityFactoryQosPolicy"/>.
        /// </summary>
        public EntityFactoryQosPolicy EntityFactory { get; internal set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainParticipantFactoryQos"/> class.
        /// </summary>
        public DomainParticipantFactoryQos()
        {
            EntityFactory = new EntityFactoryQosPolicy();
        }
        #endregion

        #region Methods
        internal DomainParticipantFactoryQosWrapper ToNative()
        {
            var data = new DomainParticipantFactoryQosWrapper
            {
                EntityFactory = EntityFactory,
            };

            return data;
        }

        internal void FromNative(DomainParticipantFactoryQosWrapper wrapper)
        {
            EntityFactory = wrapper.EntityFactory;
        }
        #endregion

        #region IEquatable<DomainParticipantFactoryQos> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(DomainParticipantFactoryQos other)
        {
            if (other == null)
            {
                return false;
            }

            return EntityFactory == other.EntityFactory;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is DomainParticipantFactoryQos other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1476352029;
            hashCode = (hashCode * -1521134295) + EqualityComparer<EntityFactoryQosPolicy>.Default.GetHashCode(EntityFactory);
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
        public static bool operator ==(DomainParticipantFactoryQos left, DomainParticipantFactoryQos right)
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
        public static bool operator !=(DomainParticipantFactoryQos left, DomainParticipantFactoryQos right)
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
    internal struct DomainParticipantFactoryQosWrapper
    {
        #region Fields
        [MarshalAs(UnmanagedType.Struct)]
        public EntityFactoryQosPolicyWrapper EntityFactory;
        #endregion
    }
}
