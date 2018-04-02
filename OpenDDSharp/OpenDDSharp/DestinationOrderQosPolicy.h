#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "DestinationOrderQosPolicyKind.h"
#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class DestinationOrderQosPolicy {

		private:
			OpenDDSharp::DDS::DestinationOrderQosPolicyKind kind;			

		public:
			property ::OpenDDSharp::DDS::DestinationOrderQosPolicyKind Kind {
				::OpenDDSharp::DDS::DestinationOrderQosPolicyKind get();
				void set(::OpenDDSharp::DDS::DestinationOrderQosPolicyKind value);
			};

		internal:
			DestinationOrderQosPolicy();			

		internal:
			::DDS::DestinationOrderQosPolicy ToNative();
			void FromNative(::DDS::DestinationOrderQosPolicy qos);
		};
	};
};

