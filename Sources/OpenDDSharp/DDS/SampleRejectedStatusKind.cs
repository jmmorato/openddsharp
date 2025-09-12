/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
namespace OpenDDSharp.DDS;

/// <summary>
/// This enumeration defines the valid values of the <see cref="SampleRejectedStatus" /> LastReason.
/// </summary>
public enum SampleRejectedStatusKind
{
    /// <summary>
    /// No sample has been rejected yet.
    /// </summary>
    NotRejected = 0,

    /// <summary>
    /// The sample was rejected because it would exceed the maximum number of instances set by the
    /// <see cref="ResourceLimitsQosPolicy" />.
    /// </summary>
    RejectedByInstancesLimit = 1,

    /// <summary>
    /// The sample was rejected because it would exceed the maximum number of samples set by the
    /// <see cref="ResourceLimitsQosPolicy" />.
    /// </summary>
    RejectedBySamplesLimit = 2,

    /// <summary>
    /// The sample was rejected because it would exceed the maximum number of samples per instance set by the
    /// <see cref="ResourceLimitsQosPolicy" />.
    /// </summary>
    RejectedBySamplesPerInstanceLimit = 3,
}