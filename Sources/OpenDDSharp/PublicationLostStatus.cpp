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
#include "PublicationLostStatus.h"

OpenDDSharp::OpenDDS::DCPS::PublicationLostStatus::PublicationLostStatus(::OpenDDS::DCPS::PublicationLostStatus status) {
	List<OpenDDSharp::DDS::InstanceHandle>^ list = gcnew List<OpenDDSharp::DDS::InstanceHandle>();
	int length = status.subscription_handles.length();
	int i = 0;
	while (i < length) {
		list->Add(status.subscription_handles[i]);
		i++;
	}

	subscription_handles = list;
};

IEnumerable<OpenDDSharp::DDS::InstanceHandle>^OpenDDSharp::OpenDDS::DCPS::PublicationLostStatus::SubscriptionHandles::get() {
	return subscription_handles;
};