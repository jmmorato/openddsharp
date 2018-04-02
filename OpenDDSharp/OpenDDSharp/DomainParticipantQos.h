#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "UserDataQosPolicy.h"
#include "EntityFactoryQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class DomainParticipantQos {		

		private:
			OpenDDSharp::DDS::UserDataQosPolicy^ user_data;
			OpenDDSharp::DDS::EntityFactoryQosPolicy^ entity_factory;
	
		public:
			property OpenDDSharp::DDS::UserDataQosPolicy^ UserData {
				OpenDDSharp::DDS::UserDataQosPolicy^ get();				
			};

			property OpenDDSharp::DDS::EntityFactoryQosPolicy^ EntityFactory {
				OpenDDSharp::DDS::EntityFactoryQosPolicy^ get();			
			};

		public:
			DomainParticipantQos();						

		internal:
			::DDS::DomainParticipantQos ToNative();
			void FromNative(::DDS::DomainParticipantQos qos);

		};
	};
};
