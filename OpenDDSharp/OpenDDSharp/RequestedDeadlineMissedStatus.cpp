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
#include "RequestedDeadlineMissedStatus.h"

OpenDDSharp::DDS::RequestedDeadlineMissedStatus::RequestedDeadlineMissedStatus(::DDS::RequestedDeadlineMissedStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
	last_instance_handle = status.last_instance_handle;
};

System::Int32 OpenDDSharp::DDS::RequestedDeadlineMissedStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::RequestedDeadlineMissedStatus::TotalCountChange::get() {
	return total_count_change;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::RequestedDeadlineMissedStatus::LastInstanceHandle::get() {
	return last_instance_handle;
};

::DDS::RequestedDeadlineMissedStatus OpenDDSharp::DDS::RequestedDeadlineMissedStatus::ToNative() {
	::DDS::RequestedDeadlineMissedStatus ret;

	ret.last_instance_handle = last_instance_handle;
	ret.total_count = total_count;
	ret.total_count_change = total_count_change;	

	return ret;
};

void OpenDDSharp::DDS::RequestedDeadlineMissedStatus::FromNative(::DDS::RequestedDeadlineMissedStatus native) {
	last_instance_handle = native.last_instance_handle;
	total_count = native.total_count;
	total_count_change = native.total_count_change;
};
