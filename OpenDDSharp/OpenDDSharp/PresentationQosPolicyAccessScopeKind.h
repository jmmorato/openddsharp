/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		
		ref class DataReader;
		ref class DataWriter;
		ref class Publisher;
		ref class Subscriber;
		ref class PresentationQosPolicy;

		/// <summary>
		/// This enumeration defines the valid kinds of the <see cref="PresentationQosPolicy" /> AccessScope.
		/// </summary>
		public enum class PresentationQosPolicyAccessScopeKind : System::Int32 {

			/// <summary>
			/// Indicates that changes occur to instances independently. Instance access essentially acts as a no-op with respect to
			/// CoherentAccess and OrderedAccess. Setting either of these values to true has no observable affect within the subscribing application.
			/// </summary>
			InstancePresentationQos = ::DDS::INSTANCE_PRESENTATION_QOS,

			/// <summary>
			/// Indicates that accepted changes are limited to all instances within the same <see cref="DataReader" /> or <see cref="DataWriter" />.
			/// </summary>
			TopicPresentationQos = ::DDS::TOPIC_PRESENTATION_QOS,

			/// <summary>
			/// Indicates that accepted changes are limited to all instances within the same <see cref="Publisher" /> or <see cref="Subscriber" />.
			/// </summary>
			GroupPresentationQos = ::DDS::GROUP_PRESENTATION_QOS
		};
	};
};