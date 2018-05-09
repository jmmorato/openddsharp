#include "LifespanQosPolicy.h"

OpenDDSharp::DDS::LifespanQosPolicy::LifespanQosPolicy() {		
	duration.Seconds = OpenDDSharp::DDS::Duration::InfiniteSeconds;
	duration.NanoSeconds = OpenDDSharp::DDS::Duration::InfiniteNanoseconds;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::LifespanQosPolicy::Duration::get() {
	return duration;
};

void OpenDDSharp::DDS::LifespanQosPolicy::Duration::set(::OpenDDSharp::DDS::Duration value) {
	duration = value;
};

::DDS::LifespanQosPolicy OpenDDSharp::DDS::LifespanQosPolicy::ToNative() {
	::DDS::LifespanQosPolicy* qos = new ::DDS::LifespanQosPolicy();

	qos->duration.sec = duration.Seconds;
	qos->duration.nanosec = duration.NanoSeconds;

	return *qos;
};

void OpenDDSharp::DDS::LifespanQosPolicy::FromNative(::DDS::LifespanQosPolicy qos) {
	duration.Seconds = qos.duration.sec;
	duration.NanoSeconds = qos.duration.nanosec;
};