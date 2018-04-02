#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class LifespanQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration duration;

		public:
			property ::OpenDDSharp::DDS::Duration Duration {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			LifespanQosPolicy();			

		internal:
			::DDS::LifespanQosPolicy ToNative();
			void FromNative(::DDS::LifespanQosPolicy qos);
		};
	};
};
