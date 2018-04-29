#pragma once

#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsPublicationC.h>
#pragma managed

#include "InstanceHandle.h"

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {
			
			ref class DataReader;

			/// <summary>
			/// The PublicationReconnected status indicates that a <see cref="DataReader" /> has been reconnected.
			/// </summary>
			public value struct PublicationReconnectedStatus {

			private:
				IEnumerable<OpenDDSharp::DDS::InstanceHandle>^ subscription_handles;

			public:
				/// <summary>
				/// Gets the sequence of reconnected subscription handles.
				/// </summary>
				property IEnumerable<OpenDDSharp::DDS::InstanceHandle>^ SubscriptionHandles {
					IEnumerable<OpenDDSharp::DDS::InstanceHandle>^ get();
				};

			internal:
				PublicationReconnectedStatus(::OpenDDS::DCPS::PublicationReconnectedStatus status);
			};
		};
	};
};