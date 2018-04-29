#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "OwnershipStrengthQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// This QoS policy should be used in combination with the OWNERSHIP policy. It only applies to the situation case where
		/// ownership kind is set to Exclusive.
		/// </summary>
		public ref class OwnershipStrengthQosPolicy {

		private:
			System::Int32 m_value;

		public:
			/// <summary>
			/// Get or sets the value of the ownership strength. The value member is used to determine which <see cref="DataWriter" /> is the owner of the data-object
			/// instance. The default value is zero.
			/// </summary>
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

