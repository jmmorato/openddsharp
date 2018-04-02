#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "SampleRejectedStatusKind.h"

namespace OpenDDSharp {
	namespace DDS {

		public value struct SampleRejectedStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			OpenDDSharp::DDS::SampleRejectedStatusKind last_reason;
			System::Int32 last_instance_handle;

		public:
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

			property OpenDDSharp::DDS::SampleRejectedStatusKind LastReason {
				OpenDDSharp::DDS::SampleRejectedStatusKind get();
			};

			property System::Int32 LastInstanceHandle {
				System::Int32 get();
			};

		internal:
			SampleRejectedStatus(::DDS::SampleRejectedStatus status);
			::DDS::SampleRejectedStatus ToNative();
			void FromNative(::DDS::SampleRejectedStatus native);
		};
	};
};