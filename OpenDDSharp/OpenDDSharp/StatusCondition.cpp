#include "StatusCondition.h"
#include "Entity.h"

OpenDDSharp::DDS::StatusCondition::StatusCondition(::DDS::StatusCondition_ptr status_condition, OpenDDSharp::DDS::Entity^ entity) : Condition(status_condition) {
	impl_entity = status_condition;
	m_entity = entity;
}

OpenDDSharp::DDS::StatusMask OpenDDSharp::DDS::StatusCondition::GetEnabledStatuses() {
	return impl_entity->get_enabled_statuses();
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::StatusCondition::SetEnabledStatuses(OpenDDSharp::DDS::StatusMask mask) {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_enabled_statuses(mask);
}

OpenDDSharp::DDS::Entity^ OpenDDSharp::DDS::StatusCondition::GetEntity() {
	return m_entity;
}