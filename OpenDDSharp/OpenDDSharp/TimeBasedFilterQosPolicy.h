#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class TimeBasedFilterQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration minimum_separation;

		public:
			property ::OpenDDSharp::DDS::Duration MinimumSeparation {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			TimeBasedFilterQosPolicy();

		internal:
			::DDS::TimeBasedFilterQosPolicy ToNative();
			void FromNative(::DDS::TimeBasedFilterQosPolicy qos);
		};
	};
};
