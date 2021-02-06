/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
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

	::DDS::TopicDataQosPolicy qos;

	int count = System::Linq::Enumerable::Count(m_value);
	qos.value.length(count);

	int i = 0;
	while (i < count) {
		System::Byte byte = System::Linq::Enumerable::ElementAt(m_value, i);
		qos.value[i] = static_cast<CORBA::Octet>(byte);
		i++;
	}

	return qos;
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