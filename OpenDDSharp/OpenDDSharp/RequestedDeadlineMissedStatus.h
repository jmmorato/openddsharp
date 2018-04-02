#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public value struct RequestedDeadlineMissedStatus {
			
		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			System::Int32 last_instance_handle;

		public:
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

			property System::Int32 LastInstanceHandle {
				System::Int32 get();
			};

		internal:
			RequestedDeadlineMissedStatus(::DDS::RequestedDeadlineMissedStatus status);
			::DDS::RequestedDeadlineMissedStatus ToNative();
			void FromNative(::DDS::RequestedDeadlineMissedStatus native);
		};
	};
};