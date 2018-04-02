#include "ReadCondition.h"

OpenDDSharp::DDS::ReadCondition::ReadCondition(::DDS::ReadCondition_ptr read_condition, OpenDDSharp::DDS::DataReader^ reader) : Condition(read_condition) {
	impl_entity = read_condition;
	data_reader = reader;
}

OpenDDSharp::DDS::SampleStateMask OpenDDSharp::DDS::ReadCondition::GetSampleStateMask() {
	return impl_entity->get_sample_state_mask();
}

OpenDDSharp::DDS::ViewStateMask OpenDDSharp::DDS::ReadCondition::GetViewStateMask() {
	return impl_entity->get_view_state_mask();
}

OpenDDSharp::DDS::InstanceStateMask OpenDDSharp::DDS::ReadCondition::GetInstanceStateMask() {
	return impl_entity->get_instance_state_mask();
}

OpenDDSharp::DDS::DataReader^ OpenDDSharp::DDS::ReadCondition::GetDatareader() {
	return data_reader;
}