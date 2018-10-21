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
#include "LifespanQosPolicy.h"

OpenDDSharp::DDS::LifespanQosPolicy::LifespanQosPolicy() {		
	duration.Seconds = OpenDDSharp::DDS::Duration::InfiniteSeconds;
	duration.NanoSeconds = OpenDDSharp::DDS::Duration::InfiniteNanoseconds;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::LifespanQosPolicy::Duration::get() {
	return duration;
};

void OpenDDSharp::DDS::LifespanQosPolicy::Duration::set(::OpenDDSharp::DDS::Duration value) {
	duration = value;
};

::DDS::LifespanQosPolicy OpenDDSharp::DDS::LifespanQosPolicy::ToNative() {
	::DDS::LifespanQosPolicy qos;

	qos.duration.sec = duration.Seconds;
	qos.duration.nanosec = duration.NanoSeconds;

	return qos;
};

void OpenDDSharp::DDS::LifespanQosPolicy::FromNative(::DDS::LifespanQosPolicy qos) {
	duration.Seconds = qos.duration.sec;
	duration.NanoSeconds = qos.duration.nanosec;
};