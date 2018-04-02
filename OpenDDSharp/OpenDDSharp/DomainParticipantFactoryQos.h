#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "UserDataQosPolicy.h"
#include "EntityFactoryQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class DomainParticipantFactoryQos {

		private:			
			OpenDDSharp::DDS::EntityFactoryQosPolicy^ entity_factory;

		public:
			property OpenDDSharp::DDS::EntityFactoryQosPolicy^ EntityFactory {
				OpenDDSharp::DDS::EntityFactoryQosPolicy^ get();
			};

		public:
			DomainParticipantFactoryQos();

		internal:
			::DDS::DomainParticipantFactoryQos ToNative();
			void FromNative(::DDS::DomainParticipantFactoryQos qos);

		};
	};
};
