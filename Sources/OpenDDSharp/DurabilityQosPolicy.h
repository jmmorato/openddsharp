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
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "DurabilityQosPolicyKind.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class Entity;
		ref class Topic;
		ref class DataReader;
		ref class DataWriter;

		/// <summary>
		/// The durability policy controls whether data writers should maintain samples after they
		/// have been sent to known subscribers. This policy applies to the <see cref="Topic" />, <see cref="DataReader" />, and 
		/// <see cref="DataWriter" /> entities via the durability member of their respective QoS structures.
		/// </summary>
		/// <remarks>
		/// <para>The value offered is considered compatible with the value requested if and only if the inequality "offered kind >= requested
		/// kind" evaluates to 'true'. For the purposes of this inequality, the values of Durability kind are considered ordered such
		/// that Volatile &lt; TransientLocal &lt; Transient &lt; Persistent.</para>
		/// </remarks>
		public ref class DurabilityQosPolicy {

		private:
			::OpenDDSharp::DDS::DurabilityQosPolicyKind kind;

		public:
			/// <summary>
			/// Gets or sets the <see cref="DurabilityQosPolicyKind" /> assigned to the related <see cref="Entity" />
			/// </summary>
			property ::OpenDDSharp::DDS::DurabilityQosPolicyKind Kind {
				::OpenDDSharp::DDS::DurabilityQosPolicyKind get();
				void set(::OpenDDSharp::DDS::DurabilityQosPolicyKind value);
			}			

		internal:
			DurabilityQosPolicy();			

		internal:
			::DDS::DurabilityQosPolicy ToNative();
			void FromNative(::DDS::DurabilityQosPolicy qos);
		};
	};
};

