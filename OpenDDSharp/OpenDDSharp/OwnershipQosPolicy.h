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

#include "OwnershipQosPolicyKind.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class Entity;

		/// <summary>
		/// This policy controls whether DDS allows multiple <see cref="DataWriter" /> objects to update the same instance (identified by Topic + key) of a data-object.
		/// </summary>
		public ref class OwnershipQosPolicy {

		private:
			OpenDDSharp::DDS::OwnershipQosPolicyKind kind;

		public:
			/// <summary>
			/// Gets or sets the ownership kind applied to the <see cref="Entity" />
			/// </summary>
			property ::OpenDDSharp::DDS::OwnershipQosPolicyKind Kind {
				::OpenDDSharp::DDS::OwnershipQosPolicyKind get();
				void set(::OpenDDSharp::DDS::OwnershipQosPolicyKind value);
			};

		internal:
			OwnershipQosPolicy();			

		internal:
			::DDS::OwnershipQosPolicy ToNative();
			void FromNative(::DDS::OwnershipQosPolicy qos);
		};
	};
};

