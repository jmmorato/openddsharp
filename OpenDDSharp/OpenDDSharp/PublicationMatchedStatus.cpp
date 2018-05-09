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
#include "PublicationMatchedStatus.h"

OpenDDSharp::DDS::PublicationMatchedStatus::PublicationMatchedStatus(::DDS::PublicationMatchedStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
	current_count = status.current_count;
	current_count_change = status.current_count_change;
	last_subscription_handle = status.last_subscription_handle;
};

System::Int32 OpenDDSharp::DDS::PublicationMatchedStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::PublicationMatchedStatus::TotalCountChange::get() {
	return total_count_change;
};

System::Int32 OpenDDSharp::DDS::PublicationMatchedStatus::CurrentCount::get() {
	return current_count;
};

System::Int32 OpenDDSharp::DDS::PublicationMatchedStatus::CurrentCountChange::get() {
	return current_count_change;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::PublicationMatchedStatus::LastSubscriptionHandle::get() {
	return last_subscription_handle;
};

::DDS::PublicationMatchedStatus OpenDDSharp::DDS::PublicationMatchedStatus::ToNative() {
	::DDS::PublicationMatchedStatus ret;

	ret.total_count = total_count;
	ret.total_count_change = total_count_change;
	ret.current_count = current_count;
	ret.current_count_change = current_count_change;
	ret.last_subscription_handle = last_subscription_handle;

	return ret;
}

void OpenDDSharp::DDS::PublicationMatchedStatus::FromNative(::DDS::PublicationMatchedStatus native) {
	total_count = native.total_count;
	total_count_change = native.total_count_change;
	current_count = native.current_count;
	current_count_change = native.current_count_change;
	last_subscription_handle = native.last_subscription_handle;
}
