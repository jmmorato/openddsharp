#include "LivelinessChangedStatus.h"

OpenDDSharp::DDS::LivelinessChangedStatus::LivelinessChangedStatus(::DDS::LivelinessChangedStatus status) {
	alive_count = status.alive_count;
	not_alive_count = status.not_alive_count;
	alive_count_change = status.alive_count_change;
	not_alive_count_change = status.not_alive_count_change;
	last_publication_handle = status.last_publication_handle;
};

System::Int32 OpenDDSharp::DDS::LivelinessChangedStatus::AliveCount::get() {
	return alive_count;
};

System::Int32 OpenDDSharp::DDS::LivelinessChangedStatus::NotAliveCount::get() {
	return not_alive_count;
};

System::Int32 OpenDDSharp::DDS::LivelinessChangedStatus::AliveCountChange::get() {
	return alive_count_change;
};

System::Int32 OpenDDSharp::DDS::LivelinessChangedStatus::NotAliveCountChange::get() {
	return not_alive_count_change;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::LivelinessChangedStatus::LastPublicationHandle::get() {
	return last_publication_handle;
};

::DDS::LivelinessChangedStatus OpenDDSharp::DDS::LivelinessChangedStatus::ToNative() {
	::DDS::LivelinessChangedStatus ret;

	ret.alive_count = alive_count;
	ret.alive_count_change = alive_count_change;
	ret.last_publication_handle = last_publication_handle;
	ret.not_alive_count = not_alive_count;
	ret.not_alive_count_change = not_alive_count_change;

	return ret;
}

void OpenDDSharp::DDS::LivelinessChangedStatus::FromNative(::DDS::LivelinessChangedStatus native) {
	alive_count = native.alive_count;
	alive_count_change = native.alive_count_change;
	last_publication_handle = native.last_publication_handle;
	not_alive_count = native.not_alive_count;
	not_alive_count_change = native.not_alive_count_change;
}