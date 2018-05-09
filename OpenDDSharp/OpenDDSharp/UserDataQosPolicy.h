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

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {

		ref class Entity;

		/// <summary>
		/// The purpose of this QoS is to allow the application to attach additional information to the created <see cref="Entity" /> objects such that when
		/// a remote application discovers their existence it can access that information and use it for its own purposes.
		/// </summary>
		public ref class UserDataQosPolicy {
		
		private:
			IEnumerable<System::Byte>^ m_value;					

		public:
			/// <summary>
			/// Gets or sets the bytes assigned to the <see cref="UserDataQosPolicy" />
			/// </summary>
			property IEnumerable<System::Byte>^ Value {
				IEnumerable<System::Byte>^ get();
				void set(IEnumerable<System::Byte>^ value);
			};

		internal:
			UserDataQosPolicy();						
		
		internal:
			::DDS::UserDataQosPolicy ToNative();
			void FromNative(::DDS::UserDataQosPolicy qos);

		};
	};
};
