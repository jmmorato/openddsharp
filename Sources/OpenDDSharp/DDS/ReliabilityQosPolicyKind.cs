/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
namespace OpenDDSharp.DDS;

/// <summary>
/// This enumeration defines the valid kinds of the <see cref="ReliabilityQosPolicy.Kind" />.
/// </summary>
public enum ReliabilityQosPolicyKind
{
    /// <summary>
    /// Makes no promises as to the reliability of the samples and could be expected to drop samples under some circumstances.
    /// </summary>
    BestEffortReliabilityQos = 0,

    /// <summary>
    /// Indicates that the service should eventually deliver all values to eligible <see cref="DataReader">DataReaders</see>.
    /// </summary>
    ReliableReliabilityQos = 1,
}