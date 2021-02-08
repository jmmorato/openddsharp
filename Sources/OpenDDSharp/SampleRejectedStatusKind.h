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

		ref class ResourceLimitsQosPolicy;
		value struct SampleRejectedStatus;

		/// <summary>
		/// This enumeration defines the valid values of the <see cref="SampleRejectedStatus" /> LastReason.
		/// </summary>
		public enum class SampleRejectedStatusKind : System::Int32 {
			/// <summary>
			/// No sample has been rejected yet.
			/// </summary>
			NotRejected = ::DDS::NOT_REJECTED,

			/// <summary>
			/// The sample was rejected because it would exceed the maximum number of instances set by the <see cref="ResourceLimitsQosPolicy />.
			/// </summary>
			RejectedByInstancesLimit = ::DDS::REJECTED_BY_INSTANCES_LIMIT,

			/// <summary>
			/// The sample was rejected because it would exceed the maximum number of samples set by the <see cref="ResourceLimitsQosPolicy />.
			/// </summary>
			RejectedBySamplesLimit = ::DDS::REJECTED_BY_SAMPLES_LIMIT,

			/// <summary>
			/// The sample was rejected because it would exceed the maximum number of samples per instance set by the <see cref="ResourceLimitsQosPolicy />.
			/// </summary>
			RejectedBySamplesPerInstanceLimit = ::DDS::REJECTED_BY_SAMPLES_PER_INSTANCE_LIMIT
		};

	};
};
