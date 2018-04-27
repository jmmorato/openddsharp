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