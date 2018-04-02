#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public ref class QosPolicyCount {

		private:
			System::Int32 policy_id;
			System::Int32 count;

		public:
			property System::Int32 PolicyId {
				System::Int32 get();
			};
			
			property System::Int32 Count {
				System::Int32 get();
			};

		internal:		
			QosPolicyCount(::DDS::QosPolicyCount native);
			::DDS::QosPolicyCount ToNative();
			void FromNative(::DDS::QosPolicyCount native);
		};
	};
};