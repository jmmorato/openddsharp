#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "ReliabilityQosPolicyKind.h"
#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class ReliabilityQosPolicy {

		private:
			OpenDDSharp::DDS::ReliabilityQosPolicyKind kind;
			OpenDDSharp::DDS::Duration max_blocking_time;

		public:
			property ::OpenDDSharp::DDS::ReliabilityQosPolicyKind Kind {
				::OpenDDSharp::DDS::ReliabilityQosPolicyKind get();
				void set(::OpenDDSharp::DDS::ReliabilityQosPolicyKind value);
			};

			property ::OpenDDSharp::DDS::Duration MaxBlockingTime {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			ReliabilityQosPolicy();			

		internal:
			::DDS::ReliabilityQosPolicy ToNative();
			void FromNative(::DDS::ReliabilityQosPolicy qos);
		};
	};
};

