#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class ReaderDataLifecycleQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration autopurge_nowriter_samples_delay;
			::OpenDDSharp::DDS::Duration autopurge_disposed_samples_delay;

		public:
			property ::OpenDDSharp::DDS::Duration AutopurgeNowriterSamplesDelay {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

			property ::OpenDDSharp::DDS::Duration AutopurgeDisposedSamplesDelay {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			ReaderDataLifecycleQosPolicy();

		internal:
			::DDS::ReaderDataLifecycleQosPolicy ToNative();
			void FromNative(::DDS::ReaderDataLifecycleQosPolicy qos);
		};
	};
};
