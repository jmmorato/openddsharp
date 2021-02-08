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
#include "LivelinessLostStatus.h"

OpenDDSharp::DDS::LivelinessLostStatus::LivelinessLostStatus(::DDS::LivelinessLostStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
};

System::Int32 OpenDDSharp::DDS::LivelinessLostStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::LivelinessLostStatus::TotalCountChange::get() {
	return total_count_change;
};

void OpenDDSharp::DDS::LivelinessLostStatus::FromNative(::DDS::LivelinessLostStatus native) {
	total_count = native.total_count;
	total_count_change = native.total_count_change;
}
