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

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {

		ref class Publisher;
		ref class Subscriber;
		ref class DataReader;
		ref class DataWriter;

		/// <summary>
		/// The purpose of this QoS is to allow the application to attach additional information to the created <see cref="Publisher" /> or <see cref="Subscriber" />.
		/// The value of the GroupData is available to the application on the <see cref="DataReader" /> and <see cref="DataWriter" /> entities and is propagated by
		/// means of the built-in topics.
		/// </summary>
		public ref class GroupDataQosPolicy {

		private:
			IEnumerable<System::Byte>^ m_value;

		public:
			/// <summary>
			/// Gets or sets the bytes assigned to the <see cref="GroupDataQosPolicy" />
			/// </summary>
			property IEnumerable<System::Byte>^ Value {
				IEnumerable<System::Byte>^ get();
				void set(IEnumerable<System::Byte>^ value);
			};

		internal:
			GroupDataQosPolicy();					

		internal:
			::DDS::GroupDataQosPolicy ToNative();
			void FromNative(::DDS::GroupDataQosPolicy qos);

		};
	};
};