#include "DeadlineQosPolicy.h"

::OpenDDSharp::DDS::DeadlineQosPolicy::DeadlineQosPolicy() {
	period.Seconds = OpenDDSharp::DDS::Duration::InfiniteSeconds;
	period.NanoSeconds = OpenDDSharp::DDS::Duration::InfiniteNanoseconds;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::DeadlineQosPolicy::Period::get() {
	return period;
};

void OpenDDSharp::DDS::DeadlineQosPolicy::Period::set(::OpenDDSharp::DDS::Duration value) {
	period = value;
};

::DDS::DeadlineQosPolicy OpenDDSharp::DDS::DeadlineQosPolicy::ToNative() {
	::DDS::DeadlineQosPolicy* qos = new ::DDS::DeadlineQosPolicy();

	qos->period = period.ToNative();

	return *qos;
};

void OpenDDSharp::DDS::DeadlineQosPolicy::FromNative(::DDS::DeadlineQosPolicy qos) {
	period.FromNative(qos.period);
};