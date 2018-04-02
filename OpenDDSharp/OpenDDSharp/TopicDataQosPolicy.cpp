#include "TopicDataQosPolicy.h"

OpenDDSharp::DDS::TopicDataQosPolicy::TopicDataQosPolicy() {
	m_value = gcnew List<System::Byte>();
};

IEnumerable<System::Byte>^ OpenDDSharp::DDS::TopicDataQosPolicy::Value::get() {
	return m_value;
};

void OpenDDSharp::DDS::TopicDataQosPolicy::Value::set(IEnumerable<System::Byte>^ value) {
	m_value = value;
};

::DDS::TopicDataQosPolicy OpenDDSharp::DDS::TopicDataQosPolicy::ToNative() {
	if (m_value == nullptr) {
		m_value = gcnew List<System::Byte>();
	}

	::DDS::TopicDataQosPolicy* qos = new ::DDS::TopicDataQosPolicy();

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

void OpenDDSharp::DDS::TopicDataQosPolicy::FromNative(::DDS::TopicDataQosPolicy qos) {
	List<System::Byte>^ list = gcnew List<System::Byte>();
	int length = qos.value.length();
	int i = 0;
	while (i < length) {
		list->Add(qos.value[i]);
		i++;
	}

	m_value = list;
};