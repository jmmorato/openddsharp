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
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// This policy provides a means for the application to indicate to the middleware the “urgency” of the data-communication. By
		/// having a non-zero duration DDS can optimize its internal operation. This policy is considered a hint. There is no specified mechanism as 
		/// to how the service should take advantage of this hint.
		/// </summary>
		public ref class LatencyBudgetQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration duration;

		public:
			/// <summary>
			/// Gets or sets the maximum duration for the latency budget. 
			/// </summary>
			/// <remarks>
			/// The value offered is considered compatible with the value requested if and only if the inequality 
			/// "offered duration &lt;= requested duration" evaluates to 'true'.
			/// </remarks>
			property ::OpenDDSharp::DDS::Duration Duration {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			LatencyBudgetQosPolicy();			

		internal:
			::DDS::LatencyBudgetQosPolicy ToNative();
			void FromNative(::DDS::LatencyBudgetQosPolicy qos);
		};
	};
};
