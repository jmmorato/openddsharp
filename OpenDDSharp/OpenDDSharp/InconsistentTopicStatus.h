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

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// The InconsistentTopic status indicates that a topic was attempted to be registered that already exists with different characteristics.
		/// Typically, the existing topic may have a different type associated with it.
		/// </summary>
		public value struct InconsistentTopicStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;

		public:
			/// <summary>
			/// Gets the cumulative count of topics that have been reported as inconsistent.
			/// </summary>
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the incremental count of inconsistent topics since the last time this status was accessed.
			/// </summary>
			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

		internal:
			InconsistentTopicStatus(::DDS::InconsistentTopicStatus status);			
			void FromNative(::DDS::InconsistentTopicStatus status);
		};
	};
};
