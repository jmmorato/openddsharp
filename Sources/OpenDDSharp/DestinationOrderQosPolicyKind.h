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
		
		ref class DestinationOrderQosPolicy;

		/// <summary>
		/// This enumeration defines the valid kinds of the <see cref="DestinationOrderQosPolicy" /> Kind.
		/// </summary>
		public enum class DestinationOrderQosPolicyKind : System::Int32 {
			/// <summary>
			/// Indicates that, assuming the Ownership policy allows it, the latest received value for the instance should be the one whose value is kept.
			/// </summary>
			ByReceptionTimestampDestinationOrderQos = ::DDS::BY_RECEPTION_TIMESTAMP_DESTINATIONORDER_QOS,

			/// <summary>
			/// Indicates that, assuming the Ownership policy allows it, a timestamp placed at the source should be used. This is the only setting that, 
			/// in the case of concurrent same-strength DataWriter objects updating the same instance, ensures all subscribers will end up with the same final value for the instance.
			/// </summary>
			BySourceTimestampDestinationOrderQos = ::DDS::BY_SOURCE_TIMESTAMP_DESTINATIONORDER_QOS
		};

	};
};