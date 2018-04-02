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

		public ref class TopicDataQosPolicy {

		private:
			IEnumerable<System::Byte>^ m_value;

		public:
			virtual property IEnumerable<System::Byte>^ Value {
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
