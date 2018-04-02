#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public enum class SampleRejectedStatusKind : System::Int32 {
			NotRejected = ::DDS::NOT_REJECTED,
			RejectedByInstancesLimit = ::DDS::REJECTED_BY_INSTANCES_LIMIT,
			RejectedBySamplesLimit = ::DDS::REJECTED_BY_SAMPLES_LIMIT,
			RejectedBySamplesPerInstanceLimit = ::DDS::REJECTED_BY_SAMPLES_PER_INSTANCE_LIMIT
		};

	};
};
