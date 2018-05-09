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
#include "Timestamp.h"

System::Int32 OpenDDSharp::DDS::Timestamp::Seconds::get() {
	return sec;
};

void OpenDDSharp::DDS::Timestamp::Seconds::set(System::Int32 value) {
	sec = value;
};

System::UInt32 OpenDDSharp::DDS::Timestamp::NanoSeconds::get() {
	return nanosec;
};

void OpenDDSharp::DDS::Timestamp::NanoSeconds::set(System::UInt32 value) {
	nanosec = value;
};

::DDS::Time_t OpenDDSharp::DDS::Timestamp::ToNative() {
	::DDS::Time_t time;

	time.sec = sec;
	time.nanosec = nanosec;

	return time;
};

void OpenDDSharp::DDS::Timestamp::FromNative(::DDS::Time_t time) {
	sec = time.sec;
	nanosec = time.nanosec;
};