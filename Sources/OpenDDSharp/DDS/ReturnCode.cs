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

namespace OpenDDSharp.DDS;

/// <summary>
/// Represents the various return code values that DDS operations return.
/// </summary>
public enum ReturnCode
{
    /// <summary>
    /// Successful return.
    /// </summary>
    Ok = 0,

    /// <summary>
    /// Generic, unspecified error
    /// </summary>
    Error = 1,

    /// <summary>
    /// Unsupported operation or QoS policy setting. Can only be returned by operations that are optional or
    /// operations that uses an optional &lt;Entity&gt;QoS as a parameter.
    /// </summary>
    Unsupported = 2,

    /// <summary>
    /// Illegal parameter value.
    /// </summary>
    BadParameter = 3,

    /// <summary>
    /// A pre-condition for the operation was not met.
    /// </summary>
    PreconditionNotMet = 4,

    /// <summary>
    /// Service ran out of the resources needed to complete the operation.
    /// </summary>
    OutOfResources = 5,

    /// <summary>
    /// Operation invoked on an <see cref="Entity" /> that is not yet enabled.
    /// </summary>
    NotEnabled = 6,

    /// <summary>
    /// Application attempted to modify an immutable QoS policy.
    /// </summary>
    ImmutablePolicy = 7,

    /// <summary>
    /// Application specified a set of policies that are not consistent with each other.
    /// </summary>
    InconsistentPolicy = 8,

    /// <summary>
    /// The object target of this operation has already been deleted.
    /// </summary>
    AlreadyDeleted = 9,

    /// <summary>
    /// The operation timed out.
    /// </summary>
    Timeout = 10,

    /// <summary>
    /// Indicates a situation where the operation did not return any data.
    /// </summary>
    NoData = 11,

    /// <summary>
    /// An operation was invoked on an inappropriate object or at an inappropriate time (as determined by QoS policies
    /// that control the behaviour of the object in question). There is no precondition that could be changed to
    /// make the operation succeed.
    /// </summary>
    IllegalOperation = 12,
}