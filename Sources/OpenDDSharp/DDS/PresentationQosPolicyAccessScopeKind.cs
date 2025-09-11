/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
namespace OpenDDSharp.DDS;

/// <summary>
/// This enumeration defines the valid kinds of the <see cref="PresentationQosPolicy.AccessScope" />.
/// </summary>
public enum PresentationQosPolicyAccessScopeKind
{
    /// <summary>
    /// Indicates that changes occur to instances independently. Instance access essentially acts as
    /// a no-op with respect to CoherentAccess and OrderedAccess. Setting either of these values to true
    /// has no observable affect within the subscribing application.
    /// </summary>
    InstancePresentationQos = 0,

    /// <summary>
    /// Indicates that accepted changes are limited to all instances within the same
    /// <see cref="DataReader" /> or <see cref="DataWriter" />.
    /// </summary>
    TopicPresentationQos = 1,

    /// <summary>
    /// Indicates that accepted changes are limited to all instances within the same
    /// <see cref="Publisher" /> or <see cref="Subscriber" />.
    /// </summary>
    GroupPresentationQos = 2,
}