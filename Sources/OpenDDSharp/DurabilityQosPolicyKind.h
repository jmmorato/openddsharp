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

		ref class DurabilityQosPolicy;

		/// <summary>
		/// This enumeration defines the valid kinds of the <see cref="DurabilityQosPolicy" /> Kind.
		/// </summary>
		public enum class DurabilityQosPolicyKind : System::Int32 {
			/// <summary>
			/// Samples are discarded after being sent to all known subscribers. 
			/// As a side effect, subscribers cannot recover samples sent before they connect.
			/// </summary>
			VolatileDurabilityQos = ::DDS::VOLATILE_DURABILITY_QOS,

			/// <summary>
			/// Data readers that are associated/connected with a data writer will be sent all of the samples
			/// in the data writer's history.
			/// </summary>
			TransientLocalDurabilityQos = ::DDS::TRANSIENT_LOCAL_DURABILITY_QOS,

			/// <summary>
			/// Samples outlive a data writer and last as long as the process is alive. The samples are kept in memory, but are not
			/// persisted to permanent storage. A data reader subscribed to the same topic and partition
			/// within the same domain will be sent all of the cached samples that belong to the same topic/partition.
			/// </summary>
			TransientDurabilityQos = ::DDS::TRANSIENT_DURABILITY_QOS,

			/// <summary>
			/// Provides basically the same functionality as transient durability except the cached samples are persisted and 
			/// will survive process destruction.
			/// </summary>
			PersistentDurabilityQos = ::DDS::PERSISTENT_DURABILITY_QOS
		};
	};
};