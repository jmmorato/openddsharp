#include "TimeBasedFilterQosPolicy.h"

::OpenDDSharp::DDS::TimeBasedFilterQosPolicy::TimeBasedFilterQosPolicy() {
	minimum_separation.Seconds = OpenDDSharp::DDS::Duration::DurationZeroSec;
	minimum_separation.NanoSeconds = OpenDDSharp::DDS::Duration::DurationZeraoNsec;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::TimeBasedFilterQosPolicy::MinimumSeparation::get() {
	return minimum_separation;
};

void OpenDDSharp::DDS::TimeBasedFilterQosPolicy::MinimumSeparation::set(::OpenDDSharp::DDS::Duration value) {
	minimum_separation = value;
};

::DDS::TimeBasedFilterQosPolicy OpenDDSharp::DDS::TimeBasedFilterQosPolicy::ToNative() {
	::DDS::TimeBasedFilterQosPolicy* qos = new ::DDS::TimeBasedFilterQosPolicy();

	qos->minimum_separation = minimum_separation.ToNative();

	return *qos;
};

void OpenDDSharp::DDS::TimeBasedFilterQosPolicy::FromNative(::DDS::TimeBasedFilterQosPolicy qos) {
	minimum_separation.FromNative(qos.minimum_separation);
};