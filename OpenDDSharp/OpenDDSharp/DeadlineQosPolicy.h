#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class DeadlineQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration period;

		public:
			property ::OpenDDSharp::DDS::Duration Period {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			DeadlineQosPolicy();			

		internal:
			::DDS::DeadlineQosPolicy ToNative();
			void FromNative(::DDS::DeadlineQosPolicy qos);
		};
	};
};
