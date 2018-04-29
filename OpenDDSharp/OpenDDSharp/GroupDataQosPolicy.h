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

		ref class Publisher;
		ref class Subscriber;
		ref class DataReader;
		ref class DataWriter;

		/// <summary>
		/// The purpose of this QoS is to allow the application to attach additional information to the created <see cref="Publisher" /> or <see cref="Subscriber" />.
		/// The value of the GroupData is available to the application on the <see cref="DataReader" /> and <see cref="DataWriter" /> entities and is propagated by
		/// means of the built-in topics.
		/// </summary>
		public ref class GroupDataQosPolicy {

		private:
			IEnumerable<System::Byte>^ m_value;

		public:
			/// <summary>
			/// Gets or sets the bytes assigned to the <see cref="GroupDataQosPolicy" />
			/// </summary>
			property IEnumerable<System::Byte>^ Value {
				IEnumerable<System::Byte>^ get();
				void set(IEnumerable<System::Byte>^ value);
			};

		internal:
			GroupDataQosPolicy();					

		internal:
			::DDS::GroupDataQosPolicy ToNative();
			void FromNative(::DDS::GroupDataQosPolicy qos);

		};
	};
};