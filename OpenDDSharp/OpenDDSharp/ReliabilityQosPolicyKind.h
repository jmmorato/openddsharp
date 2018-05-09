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

		ref class ReliabilityQosPolicy;

		/// <summary>
		/// This enumeration defines the valid kinds of the <see cref="ReliabilityQosPolicy" /> Kind.
		/// </summary>
		public enum class ReliabilityQosPolicyKind : System::Int32 {
			/// <summary>
			/// Makes no promises as to the reliability of the samples and could be expected to drop samples under some circumstances.
			/// </summary>
			BestEffortReliabilityQos = ::DDS::BEST_EFFORT_RELIABILITY_QOS,

			/// <summary>
			/// Indicates that the service should eventually deliver all values to eligible data readers.
			/// </summary>
			ReliableReliabilityQos = ::DDS::RELIABLE_RELIABILITY_QOS
		};
	};
};