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
#include "OwnershipQosPolicy.h"

OpenDDSharp::DDS::OwnershipQosPolicy::OwnershipQosPolicy() {
	kind = ::OpenDDSharp::DDS::OwnershipQosPolicyKind::SharedOwnershipQos;
};

::OpenDDSharp::DDS::OwnershipQosPolicyKind OpenDDSharp::DDS::OwnershipQosPolicy::Kind::get() {
	return kind;
};

void OpenDDSharp::DDS::OwnershipQosPolicy::Kind::set(::OpenDDSharp::DDS::OwnershipQosPolicyKind value) {
	kind = value;
};

::DDS::OwnershipQosPolicy OpenDDSharp::DDS::OwnershipQosPolicy::ToNative() {
	::DDS::OwnershipQosPolicy* qos = new ::DDS::OwnershipQosPolicy();
	
	qos->kind = (::DDS::OwnershipQosPolicyKind)kind;

	return *qos;
};

void OpenDDSharp::DDS::OwnershipQosPolicy::FromNative(::DDS::OwnershipQosPolicy qos) {
	kind = (OpenDDSharp::DDS::OwnershipQosPolicyKind)qos.kind;
};