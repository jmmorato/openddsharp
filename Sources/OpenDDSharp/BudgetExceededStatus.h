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

namespace OpenDDSharp {
	namespace DDS {		
		ref class LatencyBudgetQosPolicy;
	}
}
namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			/// <summary>
			/// The BudgetExceeded status indicates delays in excess of the <see cref="OpenDDSharp::DDS::LatencyBudgetQosPolicy" /> duration.
			/// </summary>
			public value struct BudgetExceededStatus {

			private:
				System::Int32 total_count;
				System::Int32 total_count_change;
				System::Int32 last_instance_handle;

			public:
				/// <summary>
				/// Gets the cumulative count of reported latency budget exceeded.
				/// </summary>
				property System::Int32 TotalCount {
					System::Int32 get();
				};

				/// <summary>
				/// Gets the incremental count of latency budget exceeded reported since the last time this status was accessed.
				/// </summary>
				property System::Int32 TotalCountChange {
					System::Int32 get();
				};

				/// <summary>
				/// Gets the instance handle of the last latency budged exceeded.
				/// </summary>
				property OpenDDSharp::DDS::InstanceHandle LastInstanceHandle {
					OpenDDSharp::DDS::InstanceHandle get();
				};

			internal:
				BudgetExceededStatus(::OpenDDS::DCPS::BudgetExceededStatus status);
			};

		};
	};
};