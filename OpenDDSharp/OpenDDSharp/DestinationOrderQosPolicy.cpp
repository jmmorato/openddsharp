#include "DestinationOrderQosPolicy.h"

OpenDDSharp::DDS::DestinationOrderQosPolicy::DestinationOrderQosPolicy() {
	kind = OpenDDSharp::DDS::DestinationOrderQosPolicyKind::ByReceptionTimestampDestinationOrderQos;
};

OpenDDSharp::DDS::DestinationOrderQosPolicyKind OpenDDSharp::DDS::DestinationOrderQosPolicy::Kind::get() {
	return kind;
};

void OpenDDSharp::DDS::DestinationOrderQosPolicy::Kind::set(OpenDDSharp::DDS::DestinationOrderQosPolicyKind value) {
	kind = value;
};

::DDS::DestinationOrderQosPolicy OpenDDSharp::DDS::DestinationOrderQosPolicy::ToNative() {
	::DDS::DestinationOrderQosPolicy* qos = new ::DDS::DestinationOrderQosPolicy();

	qos->kind = (::DDS::DestinationOrderQosPolicyKind)kind;

	return *qos;
};

void OpenDDSharp::DDS::DestinationOrderQosPolicy::FromNative(::DDS::DestinationOrderQosPolicy qos) {
	kind = (OpenDDSharp::DDS::DestinationOrderQosPolicyKind)qos.kind;	
};