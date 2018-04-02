#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public value struct InconsistentTopicStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;

		public:
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

		internal:
			InconsistentTopicStatus(::DDS::InconsistentTopicStatus status);
			::DDS::InconsistentTopicStatus ToNative();
			void FromNative(::DDS::InconsistentTopicStatus status);
		};
	};
};
