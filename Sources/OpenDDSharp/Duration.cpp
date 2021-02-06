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
#include "Duration.h"

System::Int32 OpenDDSharp::DDS::Duration::Seconds::get() {
	return sec;
};

void OpenDDSharp::DDS::Duration::Seconds::set(System::Int32 value) {
	sec = value;
};

System::UInt32 OpenDDSharp::DDS::Duration::NanoSeconds::get() {
	return nanosec;
};

void OpenDDSharp::DDS::Duration::NanoSeconds::set(System::UInt32 value) {
	nanosec = value;
};

::DDS::Duration_t OpenDDSharp::DDS::Duration::ToNative() {
	::DDS::Duration_t duration;

	duration.sec = sec;
	duration.nanosec = nanosec;

	return duration;
};

void OpenDDSharp::DDS::Duration::FromNative(::DDS::Duration_t duration) {
	sec = duration.sec;
	nanosec = duration.nanosec;
};