/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

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
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "InstanceHandle.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DeadlineQosPolicy;

		/// <summary>
		/// The RequestedDeadlineMissed status indicates that the deadline requested via the <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.
		/// </summary>
		public value struct RequestedDeadlineMissedStatus {
			
		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			System::Int32 last_instance_handle;

		public:
			/// <summary>
			/// Gets the cumulative count of missed requested deadlines that have been reported.
			/// </summary>
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the incremental count of missed requested deadlines since the last time this status was accessed.
			/// </summary>
			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the instance handle of the last missed deadline.
			/// </summary>
			property OpenDDSharp::DDS::InstanceHandle LastInstanceHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			};

		internal:
			RequestedDeadlineMissedStatus(::DDS::RequestedDeadlineMissedStatus status);
			::DDS::RequestedDeadlineMissedStatus ToNative();
			void FromNative(::DDS::RequestedDeadlineMissedStatus native);
		};
	};
};