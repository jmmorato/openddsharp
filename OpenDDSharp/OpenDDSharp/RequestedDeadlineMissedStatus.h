#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "InstanceHandle.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DeadlineQosPolicy;

		/// <summary>
		/// The RequestedDeadlineMissed status indicates that the deadline requested via the <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.
		/// </summary>
		public value struct RequestedDeadlineMissedStatus {
			
		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			System::Int32 last_instance_handle;

		public:
			/// <summary>
			/// Gets the cumulative count of missed requested deadlines that have been reported.
			/// </summary>
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the incremental count of missed requested deadlines since the last time this status was accessed.
			/// </summary>
			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the instance handle of the last missed deadline.
			/// </summary>
			property OpenDDSharp::DDS::InstanceHandle LastInstanceHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			};

		internal:
			RequestedDeadlineMissedStatus(::DDS::RequestedDeadlineMissedStatus status);
			::DDS::RequestedDeadlineMissedStatus ToNative();
			void FromNative(::DDS::RequestedDeadlineMissedStatus native);
		};
	};
};