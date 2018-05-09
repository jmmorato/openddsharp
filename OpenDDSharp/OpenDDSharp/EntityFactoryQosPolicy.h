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

namespace OpenDDSharp {
	namespace DDS {
		
		ref class Entity;
		ref class DomainParticipant;
		ref class Publisher;
		ref class Subscriber;
		ref class Topic;
		ref class DataWriter;
		ref class DataReader;

		/// <summary>
		/// This policy controls the behavior of the <see cref="Entity" /> as a factory for other entities.
		/// </summary>
		/// <remarks>
		/// <para>This policy concerns only <see cref="DomainParticipant" /> (as factory for <see cref="Publisher" />, <see cref="Subscriber" />, and <see cref="Topic" />), 
		/// <see cref="Publisher" /> (as factory for <see cref="DataWriter" />), and <see cref="Subscriber" /> (as factory for <see cref="DataReader" />).</para>
		/// <para>This policy is mutable. A change in the policy affects only the entities created after the change; not the previously created entities.</para>
		/// </remarks>
		public ref class EntityFactoryQosPolicy {

		private:
			::System::Boolean autoenable_created_entities;

		public:
			/// <summary>
			/// Gets or sets the value for the autoenable created entities.
			/// A value equals <see langword="true" /> indicates that the factory create operation will automatically invoke the enable operation each time a new <see cref="Entity" /> is created.
			/// A value equals <see langword="false" /> indicates that the <see cref="Entity" /> will not be automatically enabled. The application will need to enable it explicitly by means of the enable operation.
			/// The default value for this property is <see langword="true" />
			/// </summary>
			property System::Boolean AutoenableCreatedEntities {
				System::Boolean get();
				void set(System::Boolean value);
			}

		internal:
			EntityFactoryQosPolicy();			

		internal:
			::DDS::EntityFactoryQosPolicy ToNative();
			void FromNative(::DDS::EntityFactoryQosPolicy qos);
		};
	};
};
