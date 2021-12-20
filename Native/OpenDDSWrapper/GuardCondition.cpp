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
#include "GuardCondition.h"

::DDS::GuardCondition_ptr GuardCondition_CreateGuardCondition() {
	return new ::DDS::GuardCondition();
}

::DDS::Condition_ptr GuardCondition_NarrowBase(::DDS::GuardCondition_ptr gc) {
	return static_cast< ::DDS::Condition_ptr>(gc);
}

::CORBA::Boolean GuardCondition_GetTriggerValue(::DDS::GuardCondition_ptr gc) {
	return gc->get_trigger_value();
}

void GuardCondition_SetTriggerValue(::DDS::GuardCondition_ptr gc, ::CORBA::Boolean value) {
	gc->set_trigger_value(value);
}
