#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "SampleRejectedStatusKind.h"
#include "InstanceHandle.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// The SampleRejected status indicates that a sample received by the data reader has been rejected.
		/// </summary>
		public value struct SampleRejectedStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			OpenDDSharp::DDS::SampleRejectedStatusKind last_reason;
			System::Int32 last_instance_handle;

		public:
			/// <summary>
			/// Gets the cumulative count of samples that have been reported as rejected.
			/// </summary>
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the incremental count of rejected samples since the last time this status was accessed.
			/// </summary>
			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the reason the most recently rejected sample was rejected.
			/// </summary>
			property OpenDDSharp::DDS::SampleRejectedStatusKind LastReason {
				OpenDDSharp::DDS::SampleRejectedStatusKind get();
			};

			/// <summary>
			/// Gets the instance handle of the last rejected sample.
			/// </summary>
			property OpenDDSharp::DDS::InstanceHandle LastInstanceHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			};

		internal:
			SampleRejectedStatus(::DDS::SampleRejectedStatus status);
			::DDS::SampleRejectedStatus ToNative();
			void FromNative(::DDS::SampleRejectedStatus native);
		};
	};
};