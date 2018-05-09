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