#include "LatencyBudgetQosPolicy.h"

::OpenDDSharp::DDS::LatencyBudgetQosPolicy::LatencyBudgetQosPolicy() {
	duration.Seconds = OpenDDSharp::DDS::Duration::DurationInfiniteSec;
	duration.NanoSeconds = OpenDDSharp::DDS::Duration::DurationInfiniteNsec;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::LatencyBudgetQosPolicy::Duration::get() {
	return duration;
};

void OpenDDSharp::DDS::LatencyBudgetQosPolicy::Duration::set(::OpenDDSharp::DDS::Duration value) {
	duration = value;
};

::DDS::LatencyBudgetQosPolicy OpenDDSharp::DDS::LatencyBudgetQosPolicy::ToNative() {
	::DDS::LatencyBudgetQosPolicy* qos = new ::DDS::LatencyBudgetQosPolicy();

	qos->duration = duration.ToNative();

	return *qos;
};

void OpenDDSharp::DDS::LatencyBudgetQosPolicy::FromNative(::DDS::LatencyBudgetQosPolicy qos) {	
	duration.FromNative(qos.duration);
};