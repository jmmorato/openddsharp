/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
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
	namespace DDS {
		ref class DataReader;
	}
}

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			/// <summary>
			/// The PublicationDisconnected status indicates that a <see cref="OpenDDSharp::DDS::DataReader" /> has been disconnected.
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