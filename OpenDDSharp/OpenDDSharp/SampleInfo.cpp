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
#include "SampleInfo.h"

OpenDDSharp::DDS::SampleInfo::SampleInfo() {
};

System::Boolean OpenDDSharp::DDS::SampleInfo::ValidData::get() {
	return valid_data;
};

OpenDDSharp::DDS::SampleStateKind OpenDDSharp::DDS::SampleInfo::SampleState::get() {
	return sample_state;
};

OpenDDSharp::DDS::ViewStateKind OpenDDSharp::DDS::SampleInfo::ViewState::get() {
	return view_state;
};

OpenDDSharp::DDS::InstanceStateKind OpenDDSharp::DDS::SampleInfo::InstanceState::get() {
	return instance_state;
};

OpenDDSharp::DDS::Timestamp OpenDDSharp::DDS::SampleInfo::SourceTimestamp::get() {
	return source_timestamp;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::SampleInfo::InstanceHandle::get() {
	return instance_handle;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::SampleInfo::PublicationHandle::get() {
	return publication_handle;
};

System::Int32 OpenDDSharp::DDS::SampleInfo::DisposedGenerationCount::get() {
	return disposed_generation_count;
};

System::Int32 OpenDDSharp::DDS::SampleInfo::NoWritersGenerationCount::get() {
	return no_writers_generation_count;
};

System::Int32 OpenDDSharp::DDS::SampleInfo::SampleRank::get() {
	return sample_rank;
};

System::Int32 OpenDDSharp::DDS::SampleInfo::GenerationRank::get() {
	return generation_rank;
};

System::Int32 OpenDDSharp::DDS::SampleInfo::AbsoluteGenerationRank::get() {
	return absolute_generation_rank;
};

void OpenDDSharp::DDS::SampleInfo::FromNative(::DDS::SampleInfo native) {	
	valid_data = native.valid_data;
	sample_state = native.sample_state;
	view_state = native.view_state;
	instance_state = native.instance_state;
	source_timestamp.FromNative(native.source_timestamp);
	instance_handle = native.instance_handle;
	publication_handle = native.publication_handle;
	disposed_generation_count = native.disposed_generation_count;
	no_writers_generation_count = native.no_writers_generation_count;
	sample_rank = native.sample_rank;
	generation_rank = native.generation_rank;
	absolute_generation_rank = native.absolute_generation_rank;
}

::DDS::SampleInfo OpenDDSharp::DDS::SampleInfo::ToNative() {
	::DDS::SampleInfo ret;

	ret.valid_data = valid_data;
	ret.sample_state = sample_state;
	ret.view_state = view_state;
	ret.instance_state = instance_state;
	ret.source_timestamp = source_timestamp.ToNative();
	ret.instance_handle = instance_handle;
	ret.publication_handle = publication_handle;
	ret.disposed_generation_count = disposed_generation_count;
	ret.no_writers_generation_count = no_writers_generation_count;
	ret.sample_rank = sample_rank;
	ret.generation_rank = generation_rank;
	ret.absolute_generation_rank = absolute_generation_rank;

	return ret;
}