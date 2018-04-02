#include "SubscriptionMatchedStatus.h"

OpenDDSharp::DDS::SubscriptionMatchedStatus::SubscriptionMatchedStatus(::DDS::SubscriptionMatchedStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
	current_count = status.current_count;
	current_count_change = status.current_count_change;
	last_publication_handle = status.last_publication_handle;
};

System::Int32 OpenDDSharp::DDS::SubscriptionMatchedStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::SubscriptionMatchedStatus::TotalCountChange::get() {
	return total_count_change;
};

System::Int32 OpenDDSharp::DDS::SubscriptionMatchedStatus::CurrentCount::get() {
	return current_count;
};

System::Int32 OpenDDSharp::DDS::SubscriptionMatchedStatus::CurrentCountChange::get() {
	return current_count_change;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::SubscriptionMatchedStatus::LastPublicationHandle::get() {
	return last_publication_handle;
};


::DDS::SubscriptionMatchedStatus OpenDDSharp::DDS::SubscriptionMatchedStatus::ToNative() {
	::DDS::SubscriptionMatchedStatus ret;

	ret.current_count = current_count;
	ret.current_count_change = current_count_change;
	ret.last_publication_handle = last_publication_handle;
	ret.total_count = total_count;
	ret.total_count_change = total_count_change;

	return ret;
}

void OpenDDSharp::DDS::SubscriptionMatchedStatus::FromNative(::DDS::SubscriptionMatchedStatus native) {
	current_count = native.current_count;
	current_count_change = native.current_count_change;
	last_publication_handle = native.last_publication_handle;
	total_count = native.total_count;
	total_count_change = native.total_count_change;
}