/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
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