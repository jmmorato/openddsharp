#include "ReliabilityQosPolicy.h"

OpenDDSharp::DDS::ReliabilityQosPolicy::ReliabilityQosPolicy() {
	kind = OpenDDSharp::DDS::ReliabilityQosPolicyKind::BestEffortReliabilityQos;
};

OpenDDSharp::DDS::ReliabilityQosPolicyKind OpenDDSharp::DDS::ReliabilityQosPolicy::Kind::get() {
	return kind;
};

void OpenDDSharp::DDS::ReliabilityQosPolicy::Kind::set(OpenDDSharp::DDS::ReliabilityQosPolicyKind value) {
	kind = value;
};

OpenDDSharp::DDS::Duration OpenDDSharp::DDS::ReliabilityQosPolicy::MaxBlockingTime::get() {
	return max_blocking_time;
};

void OpenDDSharp::DDS::ReliabilityQosPolicy::MaxBlockingTime::set(OpenDDSharp::DDS::Duration value) {
	max_blocking_time = value;
};

::DDS::ReliabilityQosPolicy OpenDDSharp::DDS::ReliabilityQosPolicy::ToNative() {
	::DDS::ReliabilityQosPolicy* qos = new ::DDS::ReliabilityQosPolicy();

	qos->kind = (::DDS::ReliabilityQosPolicyKind)kind;
	qos->max_blocking_time = max_blocking_time.ToNative();

	return *qos;
};

void OpenDDSharp::DDS::ReliabilityQosPolicy::FromNative(::DDS::ReliabilityQosPolicy qos) {
	kind = (OpenDDSharp::DDS::ReliabilityQosPolicyKind)qos.kind;
	max_blocking_time.FromNative(qos.max_blocking_time);
};