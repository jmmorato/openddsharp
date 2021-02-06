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
#include "StatusCondition.h"
#include "Entity.h"

OpenDDSharp::DDS::StatusCondition::StatusCondition(::DDS::StatusCondition_ptr status_condition, OpenDDSharp::DDS::Entity^ entity) : Condition(status_condition) {
	impl_entity = status_condition;
	m_entity = entity;
}

OpenDDSharp::DDS::StatusMask OpenDDSharp::DDS::StatusCondition::EnabledStatuses::get() {
	return impl_entity->get_enabled_statuses();
}

void OpenDDSharp::DDS::StatusCondition::EnabledStatuses::set(OpenDDSharp::DDS::StatusMask value) {
	impl_entity->set_enabled_statuses(value);
}

OpenDDSharp::DDS::Entity^ OpenDDSharp::DDS::StatusCondition::Entity::get() {
	return m_entity;
}