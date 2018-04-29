#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {

		ref class Entity;

		/// <summary>
		/// The purpose of this QoS is to allow the application to attach additional information to the created <see cref="Entity" /> objects such that when
		/// a remote application discovers their existence it can access that information and use it for its own purposes.
		/// </summary>
		public ref class UserDataQosPolicy {
		
		private:
			IEnumerable<System::Byte>^ m_value;					

		public:
			/// <summary>
			/// Gets or sets the bytes assigned to the <see cref="UserDataQosPolicy" />
			/// </summary>
			property IEnumerable<System::Byte>^ Value {
				IEnumerable<System::Byte>^ get();
				void set(IEnumerable<System::Byte>^ value);
			};

		internal:
			UserDataQosPolicy();						
		
		internal:
			::DDS::UserDataQosPolicy ToNative();
			void FromNative(::DDS::UserDataQosPolicy qos);

		};
	};
};
