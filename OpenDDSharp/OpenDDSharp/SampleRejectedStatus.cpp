#include "SampleRejectedStatus.h"

OpenDDSharp::DDS::SampleRejectedStatus::SampleRejectedStatus(::DDS::SampleRejectedStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
	last_reason = (OpenDDSharp::DDS::SampleRejectedStatusKind)status.last_reason;
	last_instance_handle = status.last_instance_handle;
};

System::Int32 OpenDDSharp::DDS::SampleRejectedStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::SampleRejectedStatus::TotalCountChange::get() {
	return total_count_change;
};

OpenDDSharp::DDS::SampleRejectedStatusKind OpenDDSharp::DDS::SampleRejectedStatus::LastReason::get() {
	return last_reason;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::SampleRejectedStatus::LastInstanceHandle::get() {
	return last_instance_handle;
};

::DDS::SampleRejectedStatus OpenDDSharp::DDS::SampleRejectedStatus::ToNative() {
	::DDS::SampleRejectedStatus ret;

	ret.last_instance_handle = last_instance_handle;
	ret.last_reason = (::DDS::SampleRejectedStatusKind)last_reason;
	ret.total_count = total_count;
	ret.total_count_change = total_count_change;

	return ret;
}

void OpenDDSharp::DDS::SampleRejectedStatus::FromNative(::DDS::SampleRejectedStatus native) {
	last_instance_handle = native.last_instance_handle;
	last_reason = (OpenDDSharp::DDS::SampleRejectedStatusKind)native.last_reason;
	total_count = native.total_count;
	total_count_change = native.total_count_change;
}
