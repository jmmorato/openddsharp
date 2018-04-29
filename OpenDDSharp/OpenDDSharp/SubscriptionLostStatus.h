#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionExtC.h>
#pragma managed

#include "InstanceHandle.h"

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			ref class DataWriter;

			/// <summary>
			/// The SubscriptionLost status indicates that a <see cref="DataWriter" /> has been lost.
			/// </summary>
			public value struct SubscriptionLostStatus {

			private:
				IEnumerable<OpenDDSharp::DDS::InstanceHandle>^ publication_handles;

			public:
				/// <summary>
				/// Gets the sequence of lost publication handles.
				/// </summary>
				property IEnumerable<OpenDDSharp::DDS::InstanceHandle>^ PublicationHandles {
					IEnumerable<OpenDDSharp::DDS::InstanceHandle>^ get();
				};

			internal:
				SubscriptionLostStatus(::OpenDDS::DCPS::SubscriptionLostStatus status);
			};
		};
	};
};