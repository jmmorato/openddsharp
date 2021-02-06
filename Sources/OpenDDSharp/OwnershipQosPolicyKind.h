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

		ref class OwnershipQosPolicy;
		ref class Topic;
		ref class DataWriter;
		ref class DataReader;

		/// <summary>
		/// This enumeration defines the valid kinds of the <see cref="OwnershipQosPolicy" /> Kind.
		/// </summary>
		public enum class OwnershipQosPolicyKind : System::Int32 {
			/// <summary>
			/// Indicates that DDS does not enforce unique ownership for each instance. In this case, multiple writers can
			/// update the same data-object instance. The subscriber to the <see cref="Topic" /> will be able to access modifications from all <see cref="DataWriter" />
			/// objects, subject to the settings of other QoS that may filter particular samples (e.g., the TimeBasedFilter or History
			/// QoS policy). In any case there is no "filtering" of modifications made based on the identity of the <see cref="DataWriter" /> that causes the modification.
			/// </summary>
			SharedOwnershipQos = ::DDS::SHARED_OWNERSHIP_QOS,

			/// <summary>
			/// Indicates that each instance of a data-object can only be modified by one <see cref="DataWriter" />. In other words, at any point
			/// in time a single <see cref="DataWriter" /> "owns" each instance and is the only one whose modifications will be visible to 
			/// the <see cref="DataReader" /> objects.
			/// </summary>
			ExclusiveOwnershipQos = ::DDS::EXCLUSIVE_OWNERSHIP_QOS
		};
	};
};