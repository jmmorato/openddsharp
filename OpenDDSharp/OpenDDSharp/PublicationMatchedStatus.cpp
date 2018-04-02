#include "PublicationMatchedStatus.h"

OpenDDSharp::DDS::PublicationMatchedStatus::PublicationMatchedStatus(::DDS::PublicationMatchedStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
	current_count = status.current_count;
	current_count_change = status.current_count_change;
	last_subscription_handle = status.last_subscription_handle;
};

System::Int32 OpenDDSharp::DDS::PublicationMatchedStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::PublicationMatchedStatus::TotalCountChange::get() {
	return total_count_change;
};

System::Int32 OpenDDSharp::DDS::PublicationMatchedStatus::CurrentCount::get() {
	return current_count;
};

System::Int32 OpenDDSharp::DDS::PublicationMatchedStatus::CurrentCountChange::get() {
	return current_count_change;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::PublicationMatchedStatus::LastSubscriptionHandle::get() {
	return last_subscription_handle;
};

::DDS::PublicationMatchedStatus OpenDDSharp::DDS::PublicationMatchedStatus::ToNative() {
	::DDS::PublicationMatchedStatus ret;

	ret.total_count = total_count;
	ret.total_count_change = total_count_change;
	ret.current_count = current_count;
	ret.current_count_change = current_count_change;
	ret.last_subscription_handle = last_subscription_handle;

	return ret;
}

void OpenDDSharp::DDS::PublicationMatchedStatus::FromNative(::DDS::PublicationMatchedStatus native) {
	total_count = native.total_count;
	total_count_change = native.total_count_change;
	current_count = native.current_count;
	current_count_change = native.current_count_change;
	last_subscription_handle = native.last_subscription_handle;
}
