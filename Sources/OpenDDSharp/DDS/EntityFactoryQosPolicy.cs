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

namespace OpenDDSharp.DDS;

/// <summary>
/// This policy controls the behavior of the <see cref="Entity" /> as a factory for other entities.
/// </summary>
/// <remarks>
/// <para>This policy concerns only <see cref="DomainParticipant" /> (as factory for <see cref="Publisher" />,
/// <see cref="Subscriber" />, and <see cref="Topic" />), <see cref="Publisher" />
/// (as factory for <see cref="DataWriter" />), and <see cref="Subscriber" />
/// (as factory for <see cref="DataReader" />).</para>
/// <para>This policy is mutable. A change in the policy affects only the entities created after the change;
/// not the previously created entities.</para>
/// </remarks>
public sealed class EntityFactoryQosPolicy : IEquatable<EntityFactoryQosPolicy>
{
    #region Properties
    /// <summary>
    /// Gets or sets a value indicating whether the autoenable created entities is active or not.
    /// A value equals <see langword="true" /> indicates that the factory create operation will automatically invoke
    /// the enable operation each time a new <see cref="Entity" /> is created. A value equals <see langword="false" />
    /// indicates that the <see cref="Entity" /> will not be automatically enabled. The application will need to
    /// enable it explicitly by means of the enable operation.
    /// The default value for this property is <see langword="true" />.
    /// </summary>
    public bool AutoenableCreatedEntities { get; set; }
    #endregion

    #region Constructors
    internal EntityFactoryQosPolicy()
    {
        AutoenableCreatedEntities = true;
    }
    #endregion

    #region IEquatable<EntityFactoryQosPolicy> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the other parameter;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(EntityFactoryQosPolicy other)
    {
        if (other == null)
        {
            return false;
        }

        return AutoenableCreatedEntities == other.AutoenableCreatedEntities;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object is equal to the current object;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public override bool Equals(object obj)
    {
        return (obj is EntityFactoryQosPolicy other) && Equals(other);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return -2026186021 + AutoenableCreatedEntities.GetHashCode();
    }
    #endregion

    #region Operators
    /// <summary>
    /// Equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(EntityFactoryQosPolicy left, EntityFactoryQosPolicy right)
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
    /// <returns>
    /// <see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.
    /// </returns>
    public static bool operator !=(EntityFactoryQosPolicy left, EntityFactoryQosPolicy right)
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
internal struct EntityFactoryQosPolicyWrapper
{
    #region Fields
    [MarshalAs(UnmanagedType.I1)]
    public bool AutoenableCreatedEntities;
    #endregion

    #region Operators
    /// <summary>
    /// Implicit conversion operator from <see cref="EntityFactoryQosPolicyWrapper" /> to
    /// <see cref="EntityFactoryQosPolicy" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="EntityFactoryQosPolicy" /> object.</returns>
    public static implicit operator EntityFactoryQosPolicy(EntityFactoryQosPolicyWrapper value)
    {
        return new EntityFactoryQosPolicy
        {
            AutoenableCreatedEntities = value.AutoenableCreatedEntities,
        };
    }

    /// <summary>
    /// Implicit conversion operator from <see cref="EntityFactoryQosPolicy" /> to
    /// <see cref="EntityFactoryQosPolicyWrapper" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="EntityFactoryQosPolicy" /> object.</returns>
    public static implicit operator EntityFactoryQosPolicyWrapper(EntityFactoryQosPolicy value)
    {
        return new EntityFactoryQosPolicyWrapper
        {
            AutoenableCreatedEntities = value.AutoenableCreatedEntities,
        };
    }
    #endregion
}