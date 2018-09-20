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

OpenDDSharp::DDS::GuardCondition::GuardCondition(::DDS::GuardCondition_ptr native) : OpenDDSharp::DDS::Condition(native) {
	impl_entity = native;	
}

OpenDDSharp::DDS::GuardCondition::GuardCondition() : GuardCondition(new ::DDS::GuardCondition()) {

}

System::Boolean OpenDDSharp::DDS::GuardCondition::TriggerValue::get() {
	return impl_entity->get_trigger_value();
}

void OpenDDSharp::DDS::GuardCondition::TriggerValue::set(System::Boolean value) {
	impl_entity->set_trigger_value(value);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::GuardCondition::AttachToWaitSet(OpenDDSharp::DDS::WaitSet^ ws) {
	if (ws == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->attach_to_ws(ws->impl_entity);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::GuardCondition::DetachFromWaitSet(OpenDDSharp::DDS::WaitSet^ ws) {
	if (ws == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->detach_from_ws(ws->impl_entity);
}

void OpenDDSharp::DDS::GuardCondition::SignalAll() {
	impl_entity->signal_all();
}