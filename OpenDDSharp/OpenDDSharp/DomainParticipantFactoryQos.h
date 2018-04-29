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
