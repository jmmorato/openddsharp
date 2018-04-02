#include "Timestamp.h"

System::Int32 OpenDDSharp::DDS::Timestamp::Seconds::get() {
	return sec;
};

void OpenDDSharp::DDS::Timestamp::Seconds::set(System::Int32 value) {
	sec = value;
};

System::UInt32 OpenDDSharp::DDS::Timestamp::NanoSeconds::get() {
	return nanosec;
};

void OpenDDSharp::DDS::Timestamp::NanoSeconds::set(System::UInt32 value) {
	nanosec = value;
};

::DDS::Time_t OpenDDSharp::DDS::Timestamp::ToNative() {
	::DDS::Time_t time;

	time.sec = sec;
	time.nanosec = nanosec;

	return time;
};

void OpenDDSharp::DDS::Timestamp::FromNative(::DDS::Time_t time) {
	sec = time.sec;
	nanosec = time.nanosec;
};