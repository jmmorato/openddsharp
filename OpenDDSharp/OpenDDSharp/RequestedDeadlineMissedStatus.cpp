#include "RequestedDeadlineMissedStatus.h"

OpenDDSharp::DDS::RequestedDeadlineMissedStatus::RequestedDeadlineMissedStatus(::DDS::RequestedDeadlineMissedStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
	last_instance_handle = status.last_instance_handle;
};

System::Int32 OpenDDSharp::DDS::RequestedDeadlineMissedStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::RequestedDeadlineMissedStatus::TotalCountChange::get() {
	return total_count_change;
};

System::Int32 OpenDDSharp::DDS::RequestedDeadlineMissedStatus::LastInstanceHandle::get() {
	return last_instance_handle;
};

::DDS::RequestedDeadlineMissedStatus OpenDDSharp::DDS::RequestedDeadlineMissedStatus::ToNative() {
	::DDS::RequestedDeadlineMissedStatus ret;

	ret.last_instance_handle = last_instance_handle;
	ret.total_count = total_count;
	ret.total_count_change = total_count_change;	

	return ret;
};

void OpenDDSharp::DDS::RequestedDeadlineMissedStatus::FromNative(::DDS::RequestedDeadlineMissedStatus native) {
	last_instance_handle = native.last_instance_handle;
	total_count = native.total_count;
	total_count_change = native.total_count_change;
};
