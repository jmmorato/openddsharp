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

#include "OwnershipStrengthQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataWriter;

		/// <summary>
		/// This QoS policy should be used in combination with the Ownership policy. It only applies to the situation case where
		/// ownership kind is set to Exclusive.
		/// </summary>
		public ref class OwnershipStrengthQosPolicy {

		private:
			System::Int32 m_value;

		public:
			/// <summary>
			/// Get or sets the value of the ownership strength. The value member is used to determine which <see cref="DataWriter" /> is the owner of the data-object
			/// instance. The default value is zero.
			/// </summary>
			property System::Int32 Value {
				System::Int32 get();
				void set(System::Int32 value);
			};

		internal:
			OwnershipStrengthQosPolicy();

		internal:
			::DDS::OwnershipStrengthQosPolicy ToNative();
			void FromNative(::DDS::OwnershipStrengthQosPolicy qos);
		};
	};
};

