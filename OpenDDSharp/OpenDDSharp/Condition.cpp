#include "Condition.h"

OpenDDSharp::DDS::Condition::Condition(::DDS::Condition_ptr condition) {
	impl_entity = condition;
};

System::Boolean OpenDDSharp::DDS::Condition::GetTriggerValue() {
	return impl_entity->get_trigger_value();
}