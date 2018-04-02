#include "PresentationQosPolicy.h"

OpenDDSharp::DDS::PresentationQosPolicy::PresentationQosPolicy() {
	access_scope = ::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind::InstancePresentationQos;
	coherent_access = false;
	ordered_access = false;
};

::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind OpenDDSharp::DDS::PresentationQosPolicy::AccessScope::get() {
	return access_scope;
};

void OpenDDSharp::DDS::PresentationQosPolicy::AccessScope::set(::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind value) {
	access_scope = value;
};

System::Boolean OpenDDSharp::DDS::PresentationQosPolicy::CoherentAccess::get() {
	return coherent_access;
};

void OpenDDSharp::DDS::PresentationQosPolicy::CoherentAccess::set(System::Boolean value) {
	coherent_access = value;
};

System::Boolean OpenDDSharp::DDS::PresentationQosPolicy::OrderedAccess::get() {
	return ordered_access;
};

void OpenDDSharp::DDS::PresentationQosPolicy::OrderedAccess::set(System::Boolean value) {
	ordered_access = value;
};

::DDS::PresentationQosPolicy OpenDDSharp::DDS::PresentationQosPolicy::ToNative() {
	::DDS::PresentationQosPolicy* qos = new ::DDS::PresentationQosPolicy();

	qos->access_scope = (::DDS::PresentationQosPolicyAccessScopeKind)access_scope;

	if (coherent_access) {
		qos->coherent_access = TRUE;
	}
	else {
		qos->coherent_access = FALSE;
	}

	if (ordered_access) {
		qos->ordered_access = TRUE;
	}
	else {
		qos->ordered_access = FALSE;
	}

	return *qos;
};

void OpenDDSharp::DDS::PresentationQosPolicy::FromNative(::DDS::PresentationQosPolicy qos) {
	access_scope = (::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind)qos.access_scope;

	if (qos.coherent_access == FALSE) {
		coherent_access = false;
	}
	else {
		coherent_access = true;
	}

	if (qos.ordered_access == FALSE) {
		ordered_access = false;
	}
	else {
		ordered_access = true;
	}
};