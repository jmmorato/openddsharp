/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

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
#include "BuiltinTopicKey.h"

array<System::Int32, 1>^ OpenDDSharp::DDS::BuiltinTopicKey::Value::get() {
	return m_value;
};

void OpenDDSharp::DDS::BuiltinTopicKey::FromNative(::DDS::BuiltinTopicKey_t native) {
	m_value = gcnew array<System::Int32, 1>(16);
	for (int i = 0; i < 16; i++) {
		m_value[i] = native.value[i];
	}
}

::DDS::BuiltinTopicKey_t OpenDDSharp::DDS::BuiltinTopicKey::ToNative() {
	::DDS::BuiltinTopicKey_t native{};

	if (m_value != nullptr && m_value->Length == 16) {
		for (int i = 0; i < 16; i++) {
			native.value[i] = m_value[i];
		}
	}

	return native;
}
