#include "HistoryQosPolicy.h"

OpenDDSharp::DDS::HistoryQosPolicy::HistoryQosPolicy() {
	kind = OpenDDSharp::DDS::HistoryQosPolicyKind::KeepLastHistoryQos;
	depth = 1;
};

OpenDDSharp::DDS::HistoryQosPolicyKind OpenDDSharp::DDS::HistoryQosPolicy::Kind::get() {
	return kind;
};

void OpenDDSharp::DDS::HistoryQosPolicy::Kind::set(OpenDDSharp::DDS::HistoryQosPolicyKind value) {
	kind = value;
};

System::Int32 OpenDDSharp::DDS::HistoryQosPolicy::Depth::get() {
	return depth;
};

void OpenDDSharp::DDS::HistoryQosPolicy::Depth::set(System::Int32 value) {
	depth = value;
};

::DDS::HistoryQosPolicy OpenDDSharp::DDS::HistoryQosPolicy::ToNative() {
	::DDS::HistoryQosPolicy* qos = new ::DDS::HistoryQosPolicy();

	qos->kind = (::DDS::HistoryQosPolicyKind)kind;
	qos->depth = depth;

	return *qos;
};

void OpenDDSharp::DDS::HistoryQosPolicy::FromNative(::DDS::HistoryQosPolicy qos) {
	kind = (OpenDDSharp::DDS::HistoryQosPolicyKind)qos.kind;
	depth = qos.depth;
};