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
#include "ReliabilityQosPolicy.h"

OpenDDSharp::DDS::ReliabilityQosPolicy::ReliabilityQosPolicy() {
	kind = OpenDDSharp::DDS::ReliabilityQosPolicyKind::BestEffortReliabilityQos;
	max_blocking_time.Seconds = Duration::InfiniteSeconds;
	max_blocking_time.NanoSeconds = Duration::InfiniteNanoseconds;
};

OpenDDSharp::DDS::ReliabilityQosPolicyKind OpenDDSharp::DDS::ReliabilityQosPolicy::Kind::get() {
	return kind;
};

void OpenDDSharp::DDS::ReliabilityQosPolicy::Kind::set(OpenDDSharp::DDS::ReliabilityQosPolicyKind value) {
	kind = value;
};

OpenDDSharp::DDS::Duration OpenDDSharp::DDS::ReliabilityQosPolicy::MaxBlockingTime::get() {
	return max_blocking_time;
};

void OpenDDSharp::DDS::ReliabilityQosPolicy::MaxBlockingTime::set(OpenDDSharp::DDS::Duration value) {
	max_blocking_time = value;
};

::DDS::ReliabilityQosPolicy OpenDDSharp::DDS::ReliabilityQosPolicy::ToNative() {
	::DDS::ReliabilityQosPolicy qos;

	qos.kind = (::DDS::ReliabilityQosPolicyKind)kind;
	qos.max_blocking_time = max_blocking_time.ToNative();

	return qos;
};

void OpenDDSharp::DDS::ReliabilityQosPolicy::FromNative(::DDS::ReliabilityQosPolicy qos) {
	kind = (OpenDDSharp::DDS::ReliabilityQosPolicyKind)qos.kind;
	max_blocking_time.FromNative(qos.max_blocking_time);
};