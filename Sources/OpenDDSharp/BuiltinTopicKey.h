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

namespace OpenDDSharp {
	namespace DDS {
		 
		/// <summary>
		/// Global unique identifier of the built-in topics.
		/// </summary>
		public value struct BuiltinTopicKey {

		private:
			array<System::Int32, 1>^ m_value;

		public:
			/// <summary>
			/// Gets the value of the <see cref="BuiltinTopicKey" />
			/// </summary>
			property array<System::Int32, 1>^ Value {
				array<System::Int32, 1>^ get();
			};

		internal:
			void FromNative(::DDS::BuiltinTopicKey_t native);
			::DDS::BuiltinTopicKey_t ToNative();

		};
	};
};