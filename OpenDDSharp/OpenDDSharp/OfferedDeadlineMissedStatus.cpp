#include "OfferedDeadlineMissedStatus.h"

OpenDDSharp::DDS::OfferedDeadlineMissedStatus::OfferedDeadlineMissedStatus(::DDS::OfferedDeadlineMissedStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
	last_instance_handle = status.last_instance_handle;
};

System::Int32 OpenDDSharp::DDS::OfferedDeadlineMissedStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::OfferedDeadlineMissedStatus::TotalCountChange::get() {
	return total_count_change;
};

System::Int32 OpenDDSharp::DDS::OfferedDeadlineMissedStatus::LastInstanceHandle::get() {
	return last_instance_handle;
};

::DDS::OfferedDeadlineMissedStatus OpenDDSharp::DDS::OfferedDeadlineMissedStatus::ToNative() {
	::DDS::OfferedDeadlineMissedStatus ret;

	ret.total_count = total_count;
	ret.total_count_change = total_count_change;
	ret.last_instance_handle = last_instance_handle;

	return ret;
}

void OpenDDSharp::DDS::OfferedDeadlineMissedStatus::FromNative(::DDS::OfferedDeadlineMissedStatus native) {
	total_count = native.total_count;
	total_count_change = native.total_count_change;
	last_instance_handle = native.last_instance_handle;
}
