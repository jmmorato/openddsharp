#include "DurabilityQosPolicy.h"

OpenDDSharp::DDS::DurabilityQosPolicy::DurabilityQosPolicy() {
	kind = ::OpenDDSharp::DDS::DurabilityQosPolicyKind::VolatileDurabilityQos;
};

::OpenDDSharp::DDS::DurabilityQosPolicyKind OpenDDSharp::DDS::DurabilityQosPolicy::Kind::get() {
	return kind;
};

void OpenDDSharp::DDS::DurabilityQosPolicy::Kind::set(::OpenDDSharp::DDS::DurabilityQosPolicyKind value) {
	kind = value;
};

::DDS::DurabilityQosPolicy OpenDDSharp::DDS::DurabilityQosPolicy::ToNative() {
	::DDS::DurabilityQosPolicy* qos = new ::DDS::DurabilityQosPolicy();

	qos->kind = (::DDS::DurabilityQosPolicyKind)kind;

	return *qos;
};

void OpenDDSharp::DDS::DurabilityQosPolicy::FromNative(::DDS::DurabilityQosPolicy qos) {
	kind = (::OpenDDSharp::DDS::DurabilityQosPolicyKind)qos.kind;
};