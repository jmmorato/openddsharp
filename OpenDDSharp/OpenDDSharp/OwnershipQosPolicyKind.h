#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public enum class OwnershipQosPolicyKind : System::Int32 {
			SharedOwnershipQos = ::DDS::SHARED_OWNERSHIP_QOS,
			ExclusiveOwnershipQos = ::DDS::EXCLUSIVE_OWNERSHIP_QOS
		};
	};
};