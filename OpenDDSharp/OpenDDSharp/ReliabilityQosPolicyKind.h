#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public enum class ReliabilityQosPolicyKind : System::Int32 {
			BestEffortReliabilityQos = ::DDS::BEST_EFFORT_RELIABILITY_QOS,
			ReliableReliabilityQos = ::DDS::RELIABLE_RELIABILITY_QOS
		};
	};
};