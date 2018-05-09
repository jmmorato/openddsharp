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
#include "LatencyBudgetQosPolicy.h"

::OpenDDSharp::DDS::LatencyBudgetQosPolicy::LatencyBudgetQosPolicy() {
	duration.Seconds = OpenDDSharp::DDS::Duration::ZeroSeconds;
	duration.NanoSeconds = OpenDDSharp::DDS::Duration::ZeroNanoseconds;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::LatencyBudgetQosPolicy::Duration::get() {
	return duration;
};

void OpenDDSharp::DDS::LatencyBudgetQosPolicy::Duration::set(::OpenDDSharp::DDS::Duration value) {
	duration = value;
};

::DDS::LatencyBudgetQosPolicy OpenDDSharp::DDS::LatencyBudgetQosPolicy::ToNative() {
	::DDS::LatencyBudgetQosPolicy* qos = new ::DDS::LatencyBudgetQosPolicy();

	qos->duration = duration.ToNative();

	return *qos;
};

void OpenDDSharp::DDS::LatencyBudgetQosPolicy::FromNative(::DDS::LatencyBudgetQosPolicy qos) {	
	duration.FromNative(qos.duration);
};