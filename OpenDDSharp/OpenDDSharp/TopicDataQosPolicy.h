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

		ref class Topic;

		/// <summary>
		/// The purpose of this QoS is to allow the application to attach additional information to the created <see cref="Topic" /> such that when a
		/// remote application discovers their existence it can examine the information and use it in an application-defined way.
		/// </summary>
		public ref class TopicDataQosPolicy {

		private:
			IEnumerable<System::Byte>^ m_value;

		public:
			/// <summary>
			/// Gets or sets the bytes assigned to the <see cref="TopicDataQosPolicy" />
			/// </summary>
			property IEnumerable<System::Byte>^ Value {
				IEnumerable<System::Byte>^ get();
				void set(IEnumerable<System::Byte>^ value);
			};

		internal:
			TopicDataQosPolicy();						

		internal:
			::DDS::TopicDataQosPolicy ToNative();
			void FromNative(::DDS::TopicDataQosPolicy qos);

		};
	};
};
