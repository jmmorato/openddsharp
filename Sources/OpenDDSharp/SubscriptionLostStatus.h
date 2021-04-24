/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
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
	namespace DDS {
		ref class DataWriter;
	}
}

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			/// <summary>
			/// The SubscriptionLost status indicates that a <see cref="OpenDDSharp::DDS::DataWriter" /> has been lost.
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