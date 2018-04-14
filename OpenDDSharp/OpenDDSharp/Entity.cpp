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