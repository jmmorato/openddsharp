/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/

using System.Diagnostics.CodeAnalysis;

namespace OpenDDSharp.DDS;

/// <summary>
/// This enumeration defines the valid kinds of the <see cref="TypeConsistencyEnforcementQosPolicy" /> Kind.
/// </summary>
[SuppressMessage("Design", "CA1008:Enums should have zero value", Justification = "Following OpenDDS specification")]
public enum TypeConsistencyEnforcementQosPolicyKind : short
{
    /// <summary>
    /// Specifies that type coercion is disallowed when applying the
    /// <see cref="TypeConsistencyEnforcementQosPolicy" />. This enforces strict
    /// type consistency, ensuring that data readers and data writers have exactly
    /// matching types without allowing for any implicit coercion or conversions.
    /// </summary>
    DisallowTypeCoercion = 1,

    /// <summary>
    /// Specifies that type coercion is allowed when applying the
    /// <see cref="TypeConsistencyEnforcementQosPolicy" />. This enables relaxed
    /// type consistency, permitting data readers and data writers to communicate
    /// even if their types do not strictly match, as long as the coercion is valid.
    /// </summary>
    AllowTypeCoercion = 2,
}