/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using System;
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS;

/// <summary>
/// Defines the rules that determine whether the type used to publish a given topic is
/// consistent with the type used to subscribe to it.
/// </summary>
public sealed class TypeConsistencyEnforcementQosPolicy : IEquatable<TypeConsistencyEnforcementQosPolicy>
{
    #region Properties
    /// <summary>
    /// Gets or sets the type consistency enforcement policy kind.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="TypeConsistencyEnforcementQosPolicyKind.AllowTypeCoercion" />.
    /// </remarks>
    public TypeConsistencyEnforcementQosPolicyKind Kind { get; set; }

    /// <summary>
    /// Controls whether sequence bounds are taken into consideration for type assignability.
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="true" />.
    /// </remarks>
    public bool IgnoreSequenceBounds { get; set; }

    /// <summary>
    /// Controls whether string bounds are taken into consideration for type assignability.
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="true" />.
    /// </remarks>
    public bool IgnoreStringBounds { get; set; }

    /// <summary>
    /// Controls whether member names are taken into consideration for type assignability.
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="false" />.
    /// </remarks>
    public bool IgnoreMemberNames { get; set; }

    /// <summary>
    /// Controls whether type widening is allowed.
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="false" />.
    /// </remarks>
    public bool PreventTypeWidening { get; set; }

    /// <summary>
    /// Controls whether type information must be available to complete matching between a DataWriter and this DataReader.
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="false" />.
    /// </remarks>
    public bool ForceTypeValidation { get; set; }
    #endregion

    #region Constructors
    internal TypeConsistencyEnforcementQosPolicy()
    {
        Kind = TypeConsistencyEnforcementQosPolicyKind.AllowTypeCoercion;
        IgnoreSequenceBounds = true;
        IgnoreStringBounds = true;
        IgnoreMemberNames = false;
        PreventTypeWidening = false;
        ForceTypeValidation = false;
    }
    #endregion

    #region IEquatable<TypeConsistencyEnforcementQosPolicy> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the other parameter;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(TypeConsistencyEnforcementQosPolicy other)
    {
        if (other == null)
        {
            return false;
        }

        return Kind == other.Kind &&
               IgnoreSequenceBounds == other.IgnoreSequenceBounds &&
               IgnoreStringBounds == other.IgnoreStringBounds &&
               IgnoreMemberNames == other.IgnoreMemberNames &&
               PreventTypeWidening == other.PreventTypeWidening &&
               ForceTypeValidation == other.ForceTypeValidation;
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
        var hashCode = 1470823525;
        hashCode = (hashCode * -2026186021) + Kind.GetHashCode();
        hashCode = (hashCode * -2026186021) + IgnoreSequenceBounds.GetHashCode();
        hashCode = (hashCode * -2026186021) + IgnoreStringBounds.GetHashCode();
        hashCode = (hashCode * -2026186021) + IgnoreMemberNames.GetHashCode();
        hashCode = (hashCode * -2026186021) + PreventTypeWidening.GetHashCode();
        hashCode = (hashCode * -2026186021) + ForceTypeValidation.GetHashCode();
        return hashCode;
    }
    #endregion

    #region Operators
    /// <summary>
    /// Equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns>
    /// <see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(TypeConsistencyEnforcementQosPolicy left, TypeConsistencyEnforcementQosPolicy right)
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
    public static bool operator !=(TypeConsistencyEnforcementQosPolicy left, TypeConsistencyEnforcementQosPolicy right)
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
internal struct TypeConsistencyEnforcementQosPolicyWrapper
{
    #region Fields
    [MarshalAs(UnmanagedType.I2)]
    public TypeConsistencyEnforcementQosPolicyKind Kind;

    [MarshalAs(UnmanagedType.I1)]
    public bool IgnoreSequenceBounds;

    [MarshalAs(UnmanagedType.I1)]
    public bool IgnoreStringBounds;

    [MarshalAs(UnmanagedType.I1)]
    public bool IgnoreMemberNames;

    [MarshalAs(UnmanagedType.I1)]
    public bool PreventTypeWidening;

    [MarshalAs(UnmanagedType.I1)]
    public bool ForceTypeValidation;
    #endregion

    #region Operators
    /// <summary>
    /// Implicit conversion operator from <see cref="TypeConsistencyEnforcementQosPolicyWrapper" /> to
    /// <see cref="TypeConsistencyEnforcementQosPolicy" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="TypeConsistencyEnforcementQosPolicy" /> object.</returns>
    public static implicit operator TypeConsistencyEnforcementQosPolicy(TypeConsistencyEnforcementQosPolicyWrapper value)
    {
        return new TypeConsistencyEnforcementQosPolicy
        {
            Kind = value.Kind,
            IgnoreSequenceBounds = value.IgnoreSequenceBounds,
            IgnoreStringBounds = value.IgnoreStringBounds,
            IgnoreMemberNames = value.IgnoreMemberNames,
            PreventTypeWidening = value.PreventTypeWidening,
            ForceTypeValidation = value.ForceTypeValidation,
        };
    }

    /// <summary>
    /// Implicit conversion operator from <see cref="TypeConsistencyEnforcementQosPolicy" /> to
    /// <see cref="TypeConsistencyEnforcementQosPolicyWrapper" />.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The <see cref="TypeConsistencyEnforcementQosPolicy" /> object.</returns>
    public static implicit operator TypeConsistencyEnforcementQosPolicyWrapper(TypeConsistencyEnforcementQosPolicy value)
    {
        return new TypeConsistencyEnforcementQosPolicyWrapper
        {
            Kind = value.Kind,
            IgnoreSequenceBounds = value.IgnoreSequenceBounds,
            IgnoreStringBounds = value.IgnoreStringBounds,
            IgnoreMemberNames = value.IgnoreMemberNames,
            PreventTypeWidening = value.PreventTypeWidening,
            ForceTypeValidation = value.ForceTypeValidation,
        };
    }
    #endregion
}