#include "OwnershipQosPolicy.h"

OpenDDSharp::DDS::OwnershipQosPolicy::OwnershipQosPolicy() {
	kind = ::OpenDDSharp::DDS::OwnershipQosPolicyKind::SharedOwnershipQos;
};

::OpenDDSharp::DDS::OwnershipQosPolicyKind OpenDDSharp::DDS::OwnershipQosPolicy::Kind::get() {
	return kind;
};

void OpenDDSharp::DDS::OwnershipQosPolicy::Kind::set(::OpenDDSharp::DDS::OwnershipQosPolicyKind value) {
	kind = value;
};

::DDS::OwnershipQosPolicy OpenDDSharp::DDS::OwnershipQosPolicy::ToNative() {
	::DDS::OwnershipQosPolicy* qos = new ::DDS::OwnershipQosPolicy();
	
	qos->kind = (::DDS::OwnershipQosPolicyKind)kind;

	return *qos;
};

void OpenDDSharp::DDS::OwnershipQosPolicy::FromNative(::DDS::OwnershipQosPolicy qos) {
	kind = (OpenDDSharp::DDS::OwnershipQosPolicyKind)qos.kind;
};