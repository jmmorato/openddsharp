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
#include "Entity.h"

OpenDDSharp::DDS::Entity::Entity(::DDS::Entity_ptr entity) {
	impl_entity = entity;
	contained_entities = gcnew List<Entity^>();
}

OpenDDSharp::DDS::StatusCondition^ OpenDDSharp::DDS::Entity::StatusCondition::get() {
	return GetStatusCondition();
}

OpenDDSharp::DDS::StatusMask OpenDDSharp::DDS::Entity::StatusChanges::get() {
	return GetStatusChanges();
}

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::Entity::InstanceHandle::get() {
	return GetInstanceHandle();
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Entity::Enable() {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->enable();
}

OpenDDSharp::DDS::StatusCondition^ OpenDDSharp::DDS::Entity::GetStatusCondition() {
	::DDS::StatusCondition_ptr native = impl_entity->get_statuscondition();
	if (native != NULL) {
		return gcnew OpenDDSharp::DDS::StatusCondition(native, this);
	} 
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::StatusMask OpenDDSharp::DDS::Entity::GetStatusChanges() {
	return (OpenDDSharp::DDS::StatusMask)impl_entity->get_status_changes();
}

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::Entity::GetInstanceHandle() {
	return impl_entity->get_instance_handle();	
}

ICollection<OpenDDSharp::DDS::Entity^>^ OpenDDSharp::DDS::Entity::GetContainedEntities() {
	List<Entity^>^ ret = gcnew List<Entity^>();

	for each (Entity^ e in contained_entities)
	{
		ret = Enumerable::ToList(Enumerable::Concat(ret, e->GetContainedEntities()));
		ret->Add(e);
	}

	return ret;
}