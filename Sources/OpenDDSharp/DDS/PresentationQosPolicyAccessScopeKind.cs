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