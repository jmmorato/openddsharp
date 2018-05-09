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
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		
		/// <summary>
		/// Type definition for an instance handle.
		/// </summary>
		public value struct InstanceHandle {

		public:
			/// <summary>
			/// Represent a nil instance handle
			/// </summary>
			static const InstanceHandle HandleNil = ::DDS::HANDLE_NIL;

		private:
			System::Int32 m_value;

		internal:
			InstanceHandle(System::Int32 value);

		public:
			static operator System::Int32(InstanceHandle self) {
				return self.m_value;
			}

			static operator InstanceHandle(System::Int32 value) {
				InstanceHandle r(value);
				return r;
			}

		};
	}
};

