#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public enum class DurabilityQosPolicyKind : System::Int32 {
			VolatileDurabilityQos = ::DDS::VOLATILE_DURABILITY_QOS,
			TransientLocalDurabilityQos = ::DDS::TRANSIENT_LOCAL_DURABILITY_QOS,
			TransientDurabilityQos = ::DDS::TRANSIENT_DURABILITY_QOS,
			PersistentDurabilityQos = ::DDS::PERSISTENT_DURABILITY_QOS
		};
	};
};