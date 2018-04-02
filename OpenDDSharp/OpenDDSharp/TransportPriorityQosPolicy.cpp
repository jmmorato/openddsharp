#include "TransportPriorityQosPolicy.h"

OpenDDSharp::DDS::TransportPriorityQosPolicy::TransportPriorityQosPolicy() {
	m_value = 0;
};

System::Int32 OpenDDSharp::DDS::TransportPriorityQosPolicy::Value::get() {
	return m_value;
};

void OpenDDSharp::DDS::TransportPriorityQosPolicy::Value::set(System::Int32 value) {
	m_value = value;
};

::DDS::TransportPriorityQosPolicy OpenDDSharp::DDS::TransportPriorityQosPolicy::ToNative() {
	::DDS::TransportPriorityQosPolicy* qos = new ::DDS::TransportPriorityQosPolicy();

	qos->value = m_value;	

	return *qos;
};

void OpenDDSharp::DDS::TransportPriorityQosPolicy::FromNative(::DDS::TransportPriorityQosPolicy qos) {
	m_value = qos.value;
};