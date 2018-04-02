#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public ref class Condition {

		internal:
			::DDS::Condition_ptr impl_entity;

		internal:
			Condition(::DDS::Condition_ptr condition);

		public:
			System::Boolean GetTriggerValue();

		};
	};
};