#include "OwnershipStrengthQosPolicy.h"

OpenDDSharp::DDS::OwnershipStrengthQosPolicy::OwnershipStrengthQosPolicy() {
	m_value = 0;
};

System::Int32 OpenDDSharp::DDS::OwnershipStrengthQosPolicy::Value::get() {
	return m_value;
};

void OpenDDSharp::DDS::OwnershipStrengthQosPolicy::Value::set(System::Int32 value) {
	m_value = value;
};

::DDS::OwnershipStrengthQosPolicy OpenDDSharp::DDS::OwnershipStrengthQosPolicy::ToNative() {
	::DDS::OwnershipStrengthQosPolicy* qos = new ::DDS::OwnershipStrengthQosPolicy();

	qos->value = m_value;

	return *qos;
};

void OpenDDSharp::DDS::OwnershipStrengthQosPolicy::FromNative(::DDS::OwnershipStrengthQosPolicy qos) {
	m_value = qos.value;
};