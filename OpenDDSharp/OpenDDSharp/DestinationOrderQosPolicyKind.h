#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		
		ref class DestinationOrderQosPolicy;

		/// <summary>
		/// This enumeration defines the valid kinds of the <see cref="DestinationOrderQosPolicy" /> Kind.
		/// </summary>
		public enum class DestinationOrderQosPolicyKind : System::Int32 {
			/// <summary>
			/// Indicates that, assuming the Ownership policy allows it, the latest received value for the instance should be the one whose value is kept.
			/// </summary>
			ByReceptionTimestampDestinationOrderQos = ::DDS::BY_RECEPTION_TIMESTAMP_DESTINATIONORDER_QOS,

			/// <summary>
			/// Indicates that, assuming the Ownership policy allows it, a timestamp placed at the source should be used. This is the only setting that, 
			/// in the case of concurrent same-strength DataWriter objects updating the same instance, ensures all subscribers will end up with the same final value for the instance.
			/// </summary>
			BySourceTimestampDestinationOrderQos = ::DDS::BY_SOURCE_TIMESTAMP_DESTINATIONORDER_QOS
		};

	};
};