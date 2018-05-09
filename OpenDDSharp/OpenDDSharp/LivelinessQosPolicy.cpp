#include "LivelinessQosPolicy.h"

::OpenDDSharp::DDS::LivelinessQosPolicy::LivelinessQosPolicy() {
	kind = OpenDDSharp::DDS::LivelinessQosPolicyKind::AutomaticLivelinessQos;	
	lease_duration.Seconds = OpenDDSharp::DDS::Duration::InfiniteSeconds;
	lease_duration.NanoSeconds = OpenDDSharp::DDS::Duration::InfiniteNanoseconds;
};

::OpenDDSharp::DDS::LivelinessQosPolicyKind OpenDDSharp::DDS::LivelinessQosPolicy::Kind::get() {
	return kind;
};

void OpenDDSharp::DDS::LivelinessQosPolicy::Kind::set(OpenDDSharp::DDS::LivelinessQosPolicyKind value) {
	kind = value;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::LivelinessQosPolicy::LeaseDuration::get() {
	return lease_duration;
};

void OpenDDSharp::DDS::LivelinessQosPolicy::LeaseDuration::set(::OpenDDSharp::DDS::Duration value) {
	lease_duration = value;
};

::DDS::LivelinessQosPolicy OpenDDSharp::DDS::LivelinessQosPolicy::ToNative() {
	::DDS::LivelinessQosPolicy* qos = new ::DDS::LivelinessQosPolicy();

	qos->kind = (::DDS::LivelinessQosPolicyKind)kind;
	qos->lease_duration = lease_duration.ToNative();

	return *qos;
};

void OpenDDSharp::DDS::LivelinessQosPolicy::FromNative(::DDS::LivelinessQosPolicy qos) {
	kind = (OpenDDSharp::DDS::LivelinessQosPolicyKind)qos.kind;
	lease_duration.FromNative(qos.lease_duration);
};