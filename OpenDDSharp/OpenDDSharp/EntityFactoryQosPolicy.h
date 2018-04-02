#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public ref class EntityFactoryQosPolicy {

		private:
			::System::Boolean autoenable_created_entities;

		public:
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
