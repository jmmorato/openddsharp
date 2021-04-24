/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

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
#include "LivelinessChangedStatus.h"

OpenDDSharp::DDS::LivelinessChangedStatus::LivelinessChangedStatus(::DDS::LivelinessChangedStatus status) {
	alive_count = status.alive_count;
	not_alive_count = status.not_alive_count;
	alive_count_change = status.alive_count_change;
	not_alive_count_change = status.not_alive_count_change;
	last_publication_handle = status.last_publication_handle;
};

System::Int32 OpenDDSharp::DDS::LivelinessChangedStatus::AliveCount::get() {
	return alive_count;
};

System::Int32 OpenDDSharp::DDS::LivelinessChangedStatus::NotAliveCount::get() {
	return not_alive_count;
};

System::Int32 OpenDDSharp::DDS::LivelinessChangedStatus::AliveCountChange::get() {
	return alive_count_change;
};

System::Int32 OpenDDSharp::DDS::LivelinessChangedStatus::NotAliveCountChange::get() {
	return not_alive_count_change;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::LivelinessChangedStatus::LastPublicationHandle::get() {
	return last_publication_handle;
};

void OpenDDSharp::DDS::LivelinessChangedStatus::FromNative(::DDS::LivelinessChangedStatus native) {
	alive_count = native.alive_count;
	alive_count_change = native.alive_count_change;
	last_publication_handle = native.last_publication_handle;
	not_alive_count = native.not_alive_count;
	not_alive_count_change = native.not_alive_count_change;
}