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
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "InstanceHandle.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataWriter;
		ref class DataReader;

		/// <summary>
		/// The SubscriptionMatched status indicates that either a compatible <see cref="DataWriter" /> has been matched or a previously matched data writer has ceased to be matched.
		/// </summary>
		public value struct SubscriptionMatchedStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			System::Int32 current_count;
			System::Int32 current_count_change;
			OpenDDSharp::DDS::InstanceHandle last_publication_handle;

		public:
			/// <summary>
			/// Gets the cumulative count of data writers that have compatibly matched this <see cref="DataReader" />.
			/// </summary>
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the incremental change in the total count since the last time this status was accessed.
			/// </summary>
			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the current number of data writers matched to this <see cref="DataReader" />.
			/// </summary>
			property System::Int32 CurrentCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the change in the current count since the last time this status was accessed.
			/// </summary>
			property System::Int32 CurrentCountChange {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the handle for the last <see cref="DataWriter" /> matched.
			/// </summary>
			property OpenDDSharp::DDS::InstanceHandle LastPublicationHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			};

		internal:
			SubscriptionMatchedStatus(::DDS::SubscriptionMatchedStatus status);			
			void FromNative(::DDS::SubscriptionMatchedStatus native);
		};
	};
};