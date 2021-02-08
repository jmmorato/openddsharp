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
#include "GuardCondition.h"

OpenDDSharp::DDS::GuardCondition::GuardCondition(::DDS::GuardCondition_ptr native) : OpenDDSharp::DDS::Condition(static_cast<::DDS::Condition_ptr>(native)) {
	impl_entity = ::DDS::GuardCondition::_duplicate(native);
}

OpenDDSharp::DDS::GuardCondition::GuardCondition() : GuardCondition(new ::DDS::GuardCondition()) { }

OpenDDSharp::DDS::GuardCondition::!GuardCondition() {
    impl_entity = NULL;
}

System::Boolean OpenDDSharp::DDS::GuardCondition::TriggerValue::get() {
	return impl_entity->get_trigger_value();
}

void OpenDDSharp::DDS::GuardCondition::TriggerValue::set(System::Boolean value) {
	impl_entity->set_trigger_value(value);
}
