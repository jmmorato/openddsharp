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
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "Condition.h"
#include "StatusMask.h"
#include "ReturnCode.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class WaitSet;
		ref class Entity;

		/// <summary>
		/// A StatusCondition object is a specific <see cref="Condition" /> that is associated with each <see cref="Entity" />.
		/// The <see cref="Condition::TriggerValue" /> of the StatusCondition depends on the communication status of that entity (e.g., arrival of data, loss of
		/// information, etc.), 'filtered' by the set of <see cref="EnabledStatuses" /> on the StatusCondition.
		/// </summary>
		public ref class StatusCondition : public Condition {

		internal:
			::DDS::StatusCondition_ptr impl_entity;
			OpenDDSharp::DDS::Entity^ m_entity;

		public:
			/// <summary>
			/// Gets the <see cref="Entity" /> associated with the <see cref="StatusCondition" />.
			/// </summary>
			/// <remarks>
			/// Note that there is exactly one <see cref="Entity" /> associated with each <see cref="StatusCondition" />.
			/// </remarks>
			property OpenDDSharp::DDS::Entity^ Entity{
				OpenDDSharp::DDS::Entity^ get();
			}

			/// <summary>
			/// Gets or sets the <see cref="StatusMask" /> that is taken into account to determine the <see cref="Condition::TriggerValue" /> of the <see cref="StatusCondition" />.
			/// </summary>
			/// <remarks>
			/// <para>Set a new value for the property may change the <see cref="Condition::TriggerValue" /> of the <see cref="StatusCondition" />.</para>
			/// <para><see cref="WaitSet" /> objects behavior depend on the changes of the <see cref="Condition::TriggerValue" /> of their attached conditions. 
			/// Therefore, any <see cref="WaitSet" /> to which the <see cref="StatusCondition" /> is attached is potentially affected by this operation.</para>
			/// <para>If the setter is not invoked, the default mask of enabled statuses includes all the statuses.</para>
			/// </remarks>
			property OpenDDSharp::DDS::StatusMask EnabledStatuses {
				OpenDDSharp::DDS::StatusMask get();
				void set(OpenDDSharp::DDS::StatusMask value);
			}

		internal:
			StatusCondition(::DDS::StatusCondition_ptr status_condition, OpenDDSharp::DDS::Entity^ entity);

		};
	};
};
