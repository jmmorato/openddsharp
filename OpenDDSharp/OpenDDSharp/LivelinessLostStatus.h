#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public value struct LivelinessLostStatus {

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
			LivelinessLostStatus(::DDS::LivelinessLostStatus status);
			::DDS::LivelinessLostStatus ToNative();
			void FromNative(::DDS::LivelinessLostStatus native);
		};
	};
};
