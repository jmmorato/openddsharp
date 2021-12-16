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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "StatusCondition.h"

::DDS::Condition_ptr StatusCondition_NarrowBase(::DDS::StatusCondition_ptr status_condition) {
	return static_cast<::DDS::Condition_ptr>(status_condition);
}

::DDS::StatusMask StatusCondition_GetEnabledStatuses(::DDS::StatusCondition_ptr status_condition) {
	return status_condition->get_enabled_statuses();
}

void StatusCondition_SetEnabledStatuses(::DDS::StatusCondition_ptr status_condition, ::DDS::StatusMask value) {
	status_condition->set_enabled_statuses(value);
}