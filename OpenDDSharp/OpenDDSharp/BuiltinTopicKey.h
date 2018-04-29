#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		 
		/// <summary>
		/// Global unique identifier of the built-in topics.
		/// </summary>
		public value struct BuiltinTopicKey {

		private:
			array<System::Int32, 1>^ m_value;

		public:
			/// <summary>
			/// Gets the value of the <see cref="BuiltinTopicKey" />
			/// </summary>
			property array<System::Int32, 1>^ Value {
				array<System::Int32, 1>^ get();
			};		

		internal:
			void FromNative(::DDS::BuiltinTopicKey_t native);

		};
	};
};