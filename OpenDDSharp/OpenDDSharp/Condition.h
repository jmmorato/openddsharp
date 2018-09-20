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

		ref class GuardCondition;
		ref class StatusCondition;
		ref class ReadCondition;

		/// <summary>
		/// A Condition is a root class for all the conditions that may be attached to a WaitSet. This basic class is specialized in three
		///	classes that are known by the middleware: <see cref="GuardCondition" />, <see cref="StatusCondition" />, and <see cref="ReadCondition" />
		/// </summary>
		public ref class Condition {

		internal:
			::DDS::Condition_ptr impl_entity;

		public:
			/// <summary>
			/// Gets the trigger value of the <see cref="Condition" />
			/// </summary>
			property System::Boolean TriggerValue {
				System::Boolean get();
			}

		internal:
			Condition(::DDS::Condition_ptr condition);

		private:
			System::Boolean GetTriggerValue();

		};
	};
};