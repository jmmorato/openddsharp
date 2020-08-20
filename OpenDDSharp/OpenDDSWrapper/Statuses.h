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
#pragma once

#include "Utils.h"
#include "marshal.h"

EXTERN_STRUCT_EXPORT RequestedIncompatibleQosStatusWrapper
{
    ::CORBA::Long total_count;
    ::CORBA::Long total_count_change;
    ::CORBA::Long last_policy_id;
    void* policies;

public:
    RequestedIncompatibleQosStatusWrapper() {
        total_count = 0;
        total_count_change = 0;
        last_policy_id = 0;
        policies = NULL;
    }

    RequestedIncompatibleQosStatusWrapper(const ::DDS::RequestedIncompatibleQosStatus native) {
        total_count = native.total_count;
        total_count_change = native.total_count_change;
        last_policy_id = native.last_policy_id;
        unbounded_sequence_to_ptr(native.policies, policies);
    }

    operator ::DDS::RequestedIncompatibleQosStatus() const {
        ::DDS::RequestedIncompatibleQosStatus native;
        native.total_count = total_count;
        native.total_count_change = total_count_change;
        native.last_policy_id = last_policy_id;
        if (policies != NULL) {
            ptr_to_unbounded_sequence(policies, native.policies);
        }
        return native;
    }
};

EXTERN_STRUCT_EXPORT OfferedIncompatibleQosStatusWrapper
{
    ::CORBA::Long total_count;
    ::CORBA::Long total_count_change;
    ::CORBA::Long last_policy_id;
    void* policies;

public:
    OfferedIncompatibleQosStatusWrapper() {
        total_count = 0;
        total_count_change = 0;
        last_policy_id = 0;
        policies = NULL;
    }

    OfferedIncompatibleQosStatusWrapper(const ::DDS::OfferedIncompatibleQosStatus native) {
        total_count = native.total_count;
        total_count_change = native.total_count_change;
        last_policy_id = native.last_policy_id;
        unbounded_sequence_to_ptr(native.policies, policies);
    }

    operator ::DDS::OfferedIncompatibleQosStatus() const {
        ::DDS::OfferedIncompatibleQosStatus native;
        native.total_count = total_count;
        native.total_count_change = total_count_change;
        native.last_policy_id = last_policy_id;
        if (policies != NULL) {
            ptr_to_unbounded_sequence(policies, native.policies);
        }
        return native;
    }
};
