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
#include "LivelinessQosPolicy.h"

::OpenDDSharp::DDS::LivelinessQosPolicy::LivelinessQosPolicy() {
	kind = OpenDDSharp::DDS::LivelinessQosPolicyKind::AutomaticLivelinessQos;	
	lease_duration.Seconds = OpenDDSharp::DDS::Duration::InfiniteSeconds;
	lease_duration.NanoSeconds = OpenDDSharp::DDS::Duration::InfiniteNanoseconds;
};

::OpenDDSharp::DDS::LivelinessQosPolicyKind OpenDDSharp::DDS::LivelinessQosPolicy::Kind::get() {
	return kind;
};

void OpenDDSharp::DDS::LivelinessQosPolicy::Kind::set(OpenDDSharp::DDS::LivelinessQosPolicyKind value) {
	kind = value;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::LivelinessQosPolicy::LeaseDuration::get() {
	return lease_duration;
};

void OpenDDSharp::DDS::LivelinessQosPolicy::LeaseDuration::set(::OpenDDSharp::DDS::Duration value) {
	lease_duration = value;
};

::DDS::LivelinessQosPolicy OpenDDSharp::DDS::LivelinessQosPolicy::ToNative() {
	::DDS::LivelinessQosPolicy qos;

	qos.kind = (::DDS::LivelinessQosPolicyKind)kind;
	qos.lease_duration = lease_duration.ToNative();

	return qos;
};

void OpenDDSharp::DDS::LivelinessQosPolicy::FromNative(::DDS::LivelinessQosPolicy qos) {
	kind = (OpenDDSharp::DDS::LivelinessQosPolicyKind)qos.kind;
	lease_duration.FromNative(qos.lease_duration);
};