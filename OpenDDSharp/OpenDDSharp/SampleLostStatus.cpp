#include "SampleLostStatus.h"

OpenDDSharp::DDS::SampleLostStatus::SampleLostStatus(::DDS::SampleLostStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;	
};

System::Int32 OpenDDSharp::DDS::SampleLostStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::SampleLostStatus::TotalCountChange::get() {
	return total_count_change;
};

::DDS::SampleLostStatus OpenDDSharp::DDS::SampleLostStatus::ToNative() {
	::DDS::SampleLostStatus ret;

	ret.total_count = total_count;
	ret.total_count_change = total_count_change;

	return ret;
}

void OpenDDSharp::DDS::SampleLostStatus::FromNative(::DDS::SampleLostStatus native) {
	total_count = native.total_count;
	total_count_change = native.total_count_change;
}