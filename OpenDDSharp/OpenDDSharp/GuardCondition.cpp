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