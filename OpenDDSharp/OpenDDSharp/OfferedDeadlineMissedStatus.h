#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "InstanceHandle.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataWriter;

		/// <summary>
		/// The OfferedDeadlineMissed status indicates that the deadline offered by the <see cref="DataWriter" /> has been missed for one or more instances.
		/// </summary>
		public value struct OfferedDeadlineMissedStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			System::Int32 last_instance_handle;

		public:
			/// <summary>
			/// Gets the cumulative count of times that deadlines have been missed for an instance.
			/// </summary>
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the incremental change in the total count since the last time this status was accessed.
			/// </summary>
			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the last instance handle that has missed a deadline.
			/// </summary>
			property OpenDDSharp::DDS::InstanceHandle LastInstanceHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			};

		internal:
			OfferedDeadlineMissedStatus(::DDS::OfferedDeadlineMissedStatus status);
			::DDS::OfferedDeadlineMissedStatus ToNative();
			void FromNative(::DDS::OfferedDeadlineMissedStatus native);
		};
	};
};
