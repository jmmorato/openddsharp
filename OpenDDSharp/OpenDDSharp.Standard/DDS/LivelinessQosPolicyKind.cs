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
namespace OpenDDSharp.DDS
{
    /// <summary>
    /// This enumeration defines the valid kinds of the <see cref="LivelinessQosPolicy" /> Kind.
    /// </summary>
    public enum LivelinessQosPolicyKind
    {
        /// <summary>
        /// Means that the service will send a liveliness indication if the participant has not sent any network traffic for the LeaseDuration
        /// </summary>
        AutomaticLivelinessQos = 0,

        /// <summary>
        /// requires only that one Entity within the publisher is asserted to be alive to deduce all other <see cref="Entity" /> objects within the
        /// same <see cref="DomainParticipant" /> are also alive.
        /// </summary>
        ManualByParticipantLivelinessQos = 1,

        /// <summary>
        /// Requires that at least one instance within the <see cref="DataWriter" /> is asserted.
        /// </summary>
        ManualByTopicLivelinessQos = 2,
    }
}