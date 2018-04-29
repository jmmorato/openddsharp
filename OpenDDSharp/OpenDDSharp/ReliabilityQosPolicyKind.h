#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		ref class ReliabilityQosPolicy;

		/// <summary>
		/// This enumeration defines the valid kinds of the <see cref="ReliabilityQosPolicy" /> Kind.
		/// </summary>
		public enum class ReliabilityQosPolicyKind : System::Int32 {
			/// <summary>
			/// Makes no promises as to the reliability of the samples and could be expected to drop samples under some circumstances.
			/// </summary>
			BestEffortReliabilityQos = ::DDS::BEST_EFFORT_RELIABILITY_QOS,

			/// <summary>
			/// Indicates that the service should eventually deliver all values to eligible data readers.
			/// </summary>
			ReliableReliabilityQos = ::DDS::RELIABLE_RELIABILITY_QOS
		};
	};
};