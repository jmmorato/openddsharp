#pragma once

#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public value struct SampleLostStatus {

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
			SampleLostStatus(::DDS::SampleLostStatus status);
			::DDS::SampleLostStatus ToNative();
			void FromNative(::DDS::SampleLostStatus native);
		};
	};
};
