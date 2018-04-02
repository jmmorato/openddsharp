#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "LivelinessQosPolicyKind.h"
#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class LivelinessQosPolicy {

		private:
			OpenDDSharp::DDS::LivelinessQosPolicyKind kind;
			OpenDDSharp::DDS::Duration lease_duration;

		public:
			property ::OpenDDSharp::DDS::LivelinessQosPolicyKind Kind {
				::OpenDDSharp::DDS::LivelinessQosPolicyKind get();
				void set(::OpenDDSharp::DDS::LivelinessQosPolicyKind value);
			};

			property ::OpenDDSharp::DDS::Duration LeaseDuration {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			LivelinessQosPolicy();			

		internal:
			::DDS::LivelinessQosPolicy ToNative();
			void FromNative(::DDS::LivelinessQosPolicy qos);
		};
	};
};

