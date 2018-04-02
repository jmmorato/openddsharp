#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionExtC.h>
#pragma managed

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {
			public value struct SubscriptionDisconnectedStatus {

			private:
				IEnumerable<System::Int32>^ publication_handles;

			public:
				property IEnumerable<System::Int32>^ PublicationHandles {
					IEnumerable<System::Int32>^ get();
				};

			internal:
				SubscriptionDisconnectedStatus(::OpenDDS::DCPS::SubscriptionDisconnectedStatus status);
			};
		};
	};
};