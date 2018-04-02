#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "DurabilityQosPolicyKind.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class DurabilityQosPolicy {

		private:
			::OpenDDSharp::DDS::DurabilityQosPolicyKind kind;

		public:
			property ::OpenDDSharp::DDS::DurabilityQosPolicyKind Kind {
				::OpenDDSharp::DDS::DurabilityQosPolicyKind get();
				void set(::OpenDDSharp::DDS::DurabilityQosPolicyKind value);
			}			

		internal:
			DurabilityQosPolicy();			

		internal:
			::DDS::DurabilityQosPolicy ToNative();
			void FromNative(::DDS::DurabilityQosPolicy qos);
		};
	};
};

