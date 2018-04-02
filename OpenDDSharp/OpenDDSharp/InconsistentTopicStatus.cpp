#include "InconsistentTopicStatus.h"

OpenDDSharp::DDS::InconsistentTopicStatus::InconsistentTopicStatus(::DDS::InconsistentTopicStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
};

System::Int32 OpenDDSharp::DDS::InconsistentTopicStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::InconsistentTopicStatus::TotalCountChange::get() {
	return total_count_change;
};

::DDS::InconsistentTopicStatus OpenDDSharp::DDS::InconsistentTopicStatus::ToNative() {
	::DDS::InconsistentTopicStatus ret;

	ret.total_count = total_count;
	ret.total_count_change = total_count_change;

	return ret;
};

void OpenDDSharp::DDS::InconsistentTopicStatus::FromNative(::DDS::InconsistentTopicStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
}
