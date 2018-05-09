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
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "UserDataQosPolicy.h"
#include "EntityFactoryQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DomainParticipantFactory;

		/// <summary>
		/// Holds the <see cref="DomainParticipantFactory" /> Quality of Service policies.
		/// </summary>
		public ref class DomainParticipantFactoryQos {

		private:			
			OpenDDSharp::DDS::EntityFactoryQosPolicy^ entity_factory;

		public:
			/// <summary>
			/// Gets the entity factory QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::EntityFactoryQosPolicy^ EntityFactory {
				OpenDDSharp::DDS::EntityFactoryQosPolicy^ get();
			};

		public:
			/// <summary>
			/// Creates a new instance of <see cref="DomainParticipantFactoryQos" />.
			/// </summary>
			DomainParticipantFactoryQos();

		internal:
			::DDS::DomainParticipantFactoryQos ToNative();
			void FromNative(::DDS::DomainParticipantFactoryQos qos);

		};
	};
};
