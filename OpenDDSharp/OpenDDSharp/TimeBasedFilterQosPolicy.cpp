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
#include "TimeBasedFilterQosPolicy.h"

::OpenDDSharp::DDS::TimeBasedFilterQosPolicy::TimeBasedFilterQosPolicy() {
	minimum_separation.Seconds = OpenDDSharp::DDS::Duration::ZeroSeconds;
	minimum_separation.NanoSeconds = OpenDDSharp::DDS::Duration::ZeroNanoseconds;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::TimeBasedFilterQosPolicy::MinimumSeparation::get() {
	return minimum_separation;
};

void OpenDDSharp::DDS::TimeBasedFilterQosPolicy::MinimumSeparation::set(::OpenDDSharp::DDS::Duration value) {
	minimum_separation = value;
};

::DDS::TimeBasedFilterQosPolicy OpenDDSharp::DDS::TimeBasedFilterQosPolicy::ToNative() {
	::DDS::TimeBasedFilterQosPolicy* qos = new ::DDS::TimeBasedFilterQosPolicy();

	qos->minimum_separation = minimum_separation.ToNative();

	return *qos;
};

void OpenDDSharp::DDS::TimeBasedFilterQosPolicy::FromNative(::DDS::TimeBasedFilterQosPolicy qos) {
	minimum_separation.FromNative(qos.minimum_separation);
};