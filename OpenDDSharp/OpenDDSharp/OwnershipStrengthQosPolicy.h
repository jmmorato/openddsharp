#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "OwnershipStrengthQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class OwnershipStrengthQosPolicy {

		private:
			System::Int32 m_value;

		public:
			property System::Int32 Value {
				System::Int32 get();
				void set(System::Int32 value);
			};

		internal:
			OwnershipStrengthQosPolicy();

		internal:
			::DDS::OwnershipStrengthQosPolicy ToNative();
			void FromNative(::DDS::OwnershipStrengthQosPolicy qos);
		};
	};
};

