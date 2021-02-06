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

#include "DestinationOrderQosPolicyKind.h"
#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class Entity;
		ref class Subscriber;
		ref class Publisher;
		ref class DataWriter;

		/// <summary>
		/// This policy controls how each <see cref="Subscriber" /> resolves the final value of a data instance that is written by multiple <see cref="DataWriter" />
		/// objects (which may be associated with different <see cref="Publisher" /> objects) running on different nodes.
		/// </summary>
		/// <remarks>
		/// The value offered is considered compatible with the value requested if and only if the inequality "offered kind &gt;= requested
		/// kind" evaluates to 'true'. For the purposes of this inequality, the values of DestinationOrder kind are considered
		/// ordered such that ByReceptionTimestamp &lt; BySourceTimestamp.
		/// </remarks>
		public ref class DestinationOrderQosPolicy {

		private:
			OpenDDSharp::DDS::DestinationOrderQosPolicyKind kind;			

		public:
			/// <summary>
			/// Gets or sets the destination order kind applied to the <see cref="Entity" />
			/// </summary>
			property ::OpenDDSharp::DDS::DestinationOrderQosPolicyKind Kind {
				::OpenDDSharp::DDS::DestinationOrderQosPolicyKind get();
				void set(::OpenDDSharp::DDS::DestinationOrderQosPolicyKind value);
			};

		internal:
			DestinationOrderQosPolicy();			

		internal:
			::DDS::DestinationOrderQosPolicy ToNative();
			void FromNative(::DDS::DestinationOrderQosPolicy qos);
		};
	};
};

