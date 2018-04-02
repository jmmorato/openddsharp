#include "UserDataQosPolicy.h"

OpenDDSharp::DDS::UserDataQosPolicy::UserDataQosPolicy() {
	m_value = gcnew List<System::Byte>();
};

IEnumerable<System::Byte>^ OpenDDSharp::DDS::UserDataQosPolicy::Value::get() {
	return m_value;
};

void OpenDDSharp::DDS::UserDataQosPolicy::Value::set(IEnumerable<System::Byte>^ value) {
	m_value = value;
};

::DDS::UserDataQosPolicy OpenDDSharp::DDS::UserDataQosPolicy::ToNative() {
	if (m_value == nullptr) {
		m_value = gcnew List<System::Byte>();
	}

	::DDS::UserDataQosPolicy* qos = new ::DDS::UserDataQosPolicy();
	
	int count = System::Linq::Enumerable::Count(m_value);
	qos->value.length(count);
	
	int i = 0;
	while (i < count) {
		System::Byte byte = System::Linq::Enumerable::ElementAt(m_value, i);
		qos->value[i] = static_cast<CORBA::Octet>(byte);
		i++;
	}

	return *qos;
};

void OpenDDSharp::DDS::UserDataQosPolicy::FromNative(::DDS::UserDataQosPolicy qos) {
	List<System::Byte>^ list = gcnew List<System::Byte>();
	int length = qos.value.length();
	int i = 0;
	while (i < length) {
		list->Add(qos.value[i]);
		i++;
	}

	m_value = list;
};