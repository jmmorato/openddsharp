/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
namespace OpenDDSharp.DDS;

/// <summary>
/// This enumeration defines the valid kinds of the <see cref="OwnershipQosPolicy" /> Kind.
/// </summary>
public enum OwnershipQosPolicyKind
{
    /// <summary>
    /// Indicates that DDS does not enforce unique ownership for each instance. In this case, multiple writers can
    /// update the same data-object instance. The subscriber to the <see cref="Topic" /> will be able to access
    /// modifications from all <see cref="DataWriter" /> objects, subject to the settings of other QoS that may
    /// filter particular samples (e.g., the TimeBasedFilter or History QoS policy). In any case there is no
    /// "filtering" of modifications made based on the identity of the <see cref="DataWriter" />
    /// that causes the modification.
    /// </summary>
    SharedOwnershipQos = 0,

    /// <summary>
    /// Indicates that each instance of a data-object can only be modified by one <see cref="DataWriter" />.
    /// In other words, at any point in time a single <see cref="DataWriter" /> "owns" each instance and is the
    /// only one whose modifications will be visible to
    /// the <see cref="DataReader" /> objects.
    /// </summary>
    ExclusiveOwnershipQos = 1,
}