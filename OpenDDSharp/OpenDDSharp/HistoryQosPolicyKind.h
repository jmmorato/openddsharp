#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public enum class HistoryQosPolicyKind : System::Int32 {
			KeepLastHistoryQos = ::DDS::KEEP_LAST_HISTORY_QOS,
			KeepAllHistoryQos = ::DDS::KEEP_ALL_HISTORY_QOS
		};
	};
};