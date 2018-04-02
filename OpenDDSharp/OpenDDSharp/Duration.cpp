#include "Duration.h"

System::Int32 OpenDDSharp::DDS::Duration::Seconds::get() {
	return sec;
};

void OpenDDSharp::DDS::Duration::Seconds::set(System::Int32 value) {
	sec = value;
};

System::UInt32 OpenDDSharp::DDS::Duration::NanoSeconds::get() {
	return nanosec;
};

void OpenDDSharp::DDS::Duration::NanoSeconds::set(System::UInt32 value) {
	nanosec = value;
};

::DDS::Duration_t OpenDDSharp::DDS::Duration::ToNative() {
	::DDS::Duration_t duration;

	duration.sec = sec;
	duration.nanosec = nanosec;

	return duration;
};

void OpenDDSharp::DDS::Duration::FromNative(::DDS::Duration_t duration) {
	sec = duration.sec;
	nanosec = duration.nanosec;
};