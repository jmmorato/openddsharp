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
#include "OfferedIncompatibleQosStatus.h"

OpenDDSharp::DDS::OfferedIncompatibleQosStatus::OfferedIncompatibleQosStatus(::DDS::OfferedIncompatibleQosStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
	last_policy_id = status.last_policy_id;

	List<QosPolicyCount^>^ list = gcnew List<QosPolicyCount^>();
	int length = status.policies.length();
	int i = 0;
	while (i < length) {
		list->Add(gcnew QosPolicyCount(status.policies[i]));
		i++;
	}

	policies = list;
};

System::Int32 OpenDDSharp::DDS::OfferedIncompatibleQosStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::OfferedIncompatibleQosStatus::TotalCountChange::get() {
	return total_count_change;
};

System::Int32 OpenDDSharp::DDS::OfferedIncompatibleQosStatus::LastPolicyId::get() {
	return last_policy_id;
};

IEnumerable<OpenDDSharp::DDS::QosPolicyCount^>^ OpenDDSharp::DDS::OfferedIncompatibleQosStatus::Policies::get() {
	return policies;
};

void OpenDDSharp::DDS::OfferedIncompatibleQosStatus::FromNative(::DDS::OfferedIncompatibleQosStatus native) {
	total_count = native.total_count;
	total_count_change = native.total_count_change;
	last_policy_id = native.last_policy_id;

	List<QosPolicyCount^>^ list = gcnew List<QosPolicyCount^>();
	int length = native.policies.length();
	int i = 0;
	while (i < length) {
		list->Add(gcnew QosPolicyCount(native.policies[i]));
		i++;
	}

	policies = list;
}
