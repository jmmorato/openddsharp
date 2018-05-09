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
#include "DeadlineQosPolicy.h"

::OpenDDSharp::DDS::DeadlineQosPolicy::DeadlineQosPolicy() {
	period.Seconds = OpenDDSharp::DDS::Duration::InfiniteSeconds;
	period.NanoSeconds = OpenDDSharp::DDS::Duration::InfiniteNanoseconds;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::DeadlineQosPolicy::Period::get() {
	return period;
};

void OpenDDSharp::DDS::DeadlineQosPolicy::Period::set(::OpenDDSharp::DDS::Duration value) {
	period = value;
};

::DDS::DeadlineQosPolicy OpenDDSharp::DDS::DeadlineQosPolicy::ToNative() {
	::DDS::DeadlineQosPolicy* qos = new ::DDS::DeadlineQosPolicy();

	qos->period = period.ToNative();

	return *qos;
};

void OpenDDSharp::DDS::DeadlineQosPolicy::FromNative(::DDS::DeadlineQosPolicy qos) {
	period.FromNative(qos.period);
};