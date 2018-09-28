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
#include "ReadCondition.h"

OpenDDSharp::DDS::ReadCondition::ReadCondition(::DDS::ReadCondition_ptr read_condition, OpenDDSharp::DDS::DataReader^ reader) : Condition(static_cast<::DDS::Condition_ptr>(read_condition)) {
	impl_entity = ::DDS::ReadCondition::_duplicate(read_condition);
	data_reader = reader;
}

OpenDDSharp::DDS::ReadCondition::!ReadCondition() {
    impl_entity = NULL;
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