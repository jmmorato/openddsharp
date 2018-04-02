#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public enum class DestinationOrderQosPolicyKind : System::Int32 {
			ByReceptionTimestampDestinationOrderQos = ::DDS::BY_RECEPTION_TIMESTAMP_DESTINATIONORDER_QOS,
			BySourceTimestampDestinationOrderQos = ::DDS::BY_SOURCE_TIMESTAMP_DESTINATIONORDER_QOS
		};

	};
};