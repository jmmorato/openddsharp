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
			/// The PublicationDisconnected status indicates that a <see cref="DataReader" /> has been disconnected.
			/// </summary>
			public value struct PublicationDisconnectedStatus {

			private:
				IEnumerable<OpenDDSharp::DDS::InstanceHandle>^ subscription_handles;

			public:
				/// <summary>
				/// Gets the sequence of disconnected subscription handles.
				/// </summary>
				property IEnumerable<OpenDDSharp::DDS::InstanceHandle>^ SubscriptionHandles {
					IEnumerable<OpenDDSharp::DDS::InstanceHandle>^ get();
				};

			internal:
				PublicationDisconnectedStatus(::OpenDDS::DCPS::PublicationDisconnectedStatus status);
			};
		};
	};
};