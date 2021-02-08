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
	::DDS::OwnershipStrengthQosPolicy qos;

	qos.value = m_value;

	return qos;
};

void OpenDDSharp::DDS::OwnershipStrengthQosPolicy::FromNative(::DDS::OwnershipStrengthQosPolicy qos) {
	m_value = qos.value;
};