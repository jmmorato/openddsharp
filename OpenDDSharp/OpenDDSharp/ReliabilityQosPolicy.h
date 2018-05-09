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
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "ReliabilityQosPolicyKind.h"
#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataReader;
		ref class DataWriter;
		ref class Entity;

		/// <summary>
		/// This policy indicates the level of reliability requested by a <see cref="DataReader" /> or offered by a <see cref="DataWriter" />.
		/// </summary>
		/// <remarks>
		/// This policy is considered during the creation of associations between data writers and data readers. The value of both sides of the association must be compatible in order for an
		/// association to be created. The value offered is considered compatible with the value requested if and only if the inequality "offered kind >= requested kind" evaluates to 'true'. 
		/// For the purposes of this inequality, the values of Reliability kind are considered ordered such that BestEffort &lt; Reliable.
		/// </remarks>
		public ref class ReliabilityQosPolicy {

		private:
			OpenDDSharp::DDS::ReliabilityQosPolicyKind kind;
			OpenDDSharp::DDS::Duration max_blocking_time;

		public:
			/// <summary>
			/// Gets or sets the reliability kind applied to the <see cref="Entity" />
			/// </summary>
			property ::OpenDDSharp::DDS::ReliabilityQosPolicyKind Kind {
				::OpenDDSharp::DDS::ReliabilityQosPolicyKind get();
				void set(::OpenDDSharp::DDS::ReliabilityQosPolicyKind value);
			};

			/// <summary>
			/// Gets or sets the maximum blocking time when the history QoS policy is set to
			/// "keep all" and the writer is unable to proceed because of resource limits.
			/// </summary>
			property ::OpenDDSharp::DDS::Duration MaxBlockingTime {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			ReliabilityQosPolicy();			

		internal:
			::DDS::ReliabilityQosPolicy ToNative();
			void FromNative(::DDS::ReliabilityQosPolicy qos);
		};
	};
};

