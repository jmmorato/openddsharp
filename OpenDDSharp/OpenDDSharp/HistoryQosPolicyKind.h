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

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		ref class HistoryQosPolicy;

		/// <summary>
		/// This enumeration defines the valid kinds of the <see cref="HistoryQosPolicy" /> Kind.
		/// </summary>
		public enum class HistoryQosPolicyKind : System::Int32 {
			/// <summary>
			/// Specifies that only the last depth values should be kept. When a data writer contains depth samples of a given instance, a write of
			/// new samples for that instance are queued for delivery and the oldest unsent samples are discarded. When a data reader contains depth samples of a given instance, 
			/// any incoming samples for that instance are kept and the oldest samples are discarded.
			/// </summary>
			KeepLastHistoryQos = ::DDS::KEEP_LAST_HISTORY_QOS,

			/// <summary>
			/// specifies that all possible samples for that instance should be kept. When "keep all" is specified and the number of unread samples is
			/// equal to the "resource limits" property of MaxSamplesPerInstance then any incoming samples are rejected.
			/// </summary>
			KeepAllHistoryQos = ::DDS::KEEP_ALL_HISTORY_QOS
		};
	};
};