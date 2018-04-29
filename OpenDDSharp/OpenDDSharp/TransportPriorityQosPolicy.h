#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// The purpose of this QoS is to allow the application to take advantage of transports capable of sending messages with different priorities.
		/// </summary>
		/// <remarks>
		/// This policy is considered a hint. The policy depends on the ability of the underlying transports to set a priority on the messages
		/// they send. Any value within the range of a 32-bit signed integer may be chosen; higher values indicate higher priority.
		/// </remarks>
		public ref class TransportPriorityQosPolicy {

		private:
			System::Int32 m_value;

		public:
			/// <summary>
			/// Gets or sets the transport priority value.
			/// </summary>
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

