#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		ref class ResourceLimitsQosPolicy;
		value struct SampleRejectedStatus;

		/// <summary>
		/// This enumeration defines the valid values of the <see cref="SampleRejectedStatus" /> LastReason.
		/// </summary>
		public enum class SampleRejectedStatusKind : System::Int32 {
			/// <summary>
			/// No sample has been rejected yet.
			/// </summary>
			NotRejected = ::DDS::NOT_REJECTED,

			/// <summary>
			/// The sample was rejected because it would exceed the maximum number of instances set by the <see cref="ResourceLimitsQosPolicy />.
			/// </summary>
			RejectedByInstancesLimit = ::DDS::REJECTED_BY_INSTANCES_LIMIT,

			/// <summary>
			/// The sample was rejected because it would exceed the maximum number of samples set by the <see cref="ResourceLimitsQosPolicy />.
			/// </summary>
			RejectedBySamplesLimit = ::DDS::REJECTED_BY_SAMPLES_LIMIT,

			/// <summary>
			/// The sample was rejected because it would exceed the maximum number of samples per instance set by the <see cref="ResourceLimitsQosPolicy />.
			/// </summary>
			RejectedBySamplesPerInstanceLimit = ::DDS::REJECTED_BY_SAMPLES_PER_INSTANCE_LIMIT
		};

	};
};
