#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"
#include "HistoryQosPolicyKind.h"
#include "ResourceLimitsQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class DurabilityServiceQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration service_cleanup_delay;
			::OpenDDSharp::DDS::HistoryQosPolicyKind history_kind;
			System::Int32 history_depth;
			System::Int32 max_samples;
			System::Int32 max_instances;
			System::Int32 max_samples_per_instance;

		public:
			property OpenDDSharp::DDS::Duration ServiceCleanupDelay {
				OpenDDSharp::DDS::Duration get();
				void set(OpenDDSharp::DDS::Duration value);
			};

			property ::OpenDDSharp::DDS::HistoryQosPolicyKind HistoryKind {
				::OpenDDSharp::DDS::HistoryQosPolicyKind get();
				void set(::OpenDDSharp::DDS::HistoryQosPolicyKind value);
			};

			property System::Int32 HistoryDepth {
				System::Int32 get();
				void set(System::Int32 value);
			};

			property System::Int32 MaxSamples {
				System::Int32 get();
				void set(System::Int32 value);
			};

			property System::Int32 MaxInstances {
				System::Int32 get();
				void set(System::Int32 value);
			};

			property System::Int32 MaxSamplesPerInstance {
				System::Int32 get();
				void set(System::Int32 value);
			};

		internal:
			DurabilityServiceQosPolicy();			

		internal:
			::DDS::DurabilityServiceQosPolicy ToNative();
			void FromNative(::DDS::DurabilityServiceQosPolicy qos);
		};
	};
};

