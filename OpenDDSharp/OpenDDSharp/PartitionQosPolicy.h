#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include <vcclr.h>
#include <msclr/marshal.h>

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {

		public ref class PartitionQosPolicy {

		private:
			IEnumerable<System::String^>^ name;

		public:
			virtual property IEnumerable<System::String^>^ Name {
				IEnumerable<System::String^>^ get();
				void set(IEnumerable<System::String^>^ value);
			};

		internal:
			PartitionQosPolicy();						

		internal:
			::DDS::PartitionQosPolicy ToNative();
			void FromNative(::DDS::PartitionQosPolicy qos);

		};
	};
};
