#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public ref class TransportPriorityQosPolicy {

		private:
			System::Int32 m_value;

		public:
			property System::Int32 Value {
				System::Int32 get();
				void set(System::Int32 value);
			};

		internal:
			TransportPriorityQosPolicy();			

		internal:
			::DDS::TransportPriorityQosPolicy ToNative();
			void FromNative(::DDS::TransportPriorityQosPolicy qos);
		};
	};
};

