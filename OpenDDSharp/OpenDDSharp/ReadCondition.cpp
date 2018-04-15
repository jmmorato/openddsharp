#include "ReadCondition.h"

OpenDDSharp::DDS::ReadCondition::ReadCondition(::DDS::ReadCondition_ptr read_condition, OpenDDSharp::DDS::DataReader^ reader) : Condition(read_condition) {
	impl_entity = read_condition;
	data_reader = reader;
}

OpenDDSharp::DDS::SampleStateMask OpenDDSharp::DDS::ReadCondition::SampleStateMask::get() {
	return impl_entity->get_sample_state_mask();
}

OpenDDSharp::DDS::ViewStateMask OpenDDSharp::DDS::ReadCondition::ViewStateMask::get() {
	return impl_entity->get_view_state_mask();
}

OpenDDSharp::DDS::InstanceStateMask OpenDDSharp::DDS::ReadCondition::InstanceStateMask::get() {
	return impl_entity->get_instance_state_mask();
}

OpenDDSharp::DDS::DataReader^ OpenDDSharp::DDS::ReadCondition::DataReader::get() {
	return data_reader;
}