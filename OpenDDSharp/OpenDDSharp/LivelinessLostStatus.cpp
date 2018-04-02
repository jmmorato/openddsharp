#include "LivelinessLostStatus.h"

OpenDDSharp::DDS::LivelinessLostStatus::LivelinessLostStatus(::DDS::LivelinessLostStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
};

System::Int32 OpenDDSharp::DDS::LivelinessLostStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::LivelinessLostStatus::TotalCountChange::get() {
	return total_count_change;
};

::DDS::LivelinessLostStatus OpenDDSharp::DDS::LivelinessLostStatus::ToNative() {
	::DDS::LivelinessLostStatus ret;

	ret.total_count = total_count;
	ret.total_count_change = total_count_change;

	return ret;
}

void OpenDDSharp::DDS::LivelinessLostStatus::FromNative(::DDS::LivelinessLostStatus native) {
	total_count = native.total_count;
	total_count_change = native.total_count_change;
}
