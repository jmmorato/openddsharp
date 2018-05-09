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

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// The purpose of this QoS is to allow the application to take advantage of transports capable of sending messages with different priorities.
		/// </summary>
		/// <remarks>
		/// This policy is considered a hint. The policy depends on the ability of the underlying transports to set a priority on the messages
		/// they send. Any value within the range of a 32-bit signed integer may be chosen; higher values indicate higher priority.
		/// </remarks>
		public ref class TransportPriorityQosPolicy {

		private:
			System::Int32 m_value;

		public:
			/// <summary>
			/// Gets or sets the transport priority value.
			/// </summary>
			property System::Int32 Value {
				System::Int32 get();
				void set(System::Int32 value);
			};

		internal:
			TransportPriorityQosPolicy();			

		internal:
			::DDS::TransportPriorityQosPolicy ToNative();
			void FromNative(::DDS::TransportPriorityQosPolicy qos);
		};
	};
};

