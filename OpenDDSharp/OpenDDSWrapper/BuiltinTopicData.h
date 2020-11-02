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
#include "dds\DdsDcpsDomainC.h"
#include "marshal.h"
#include "QosPolicies.h"

EXTERN_STRUCT_EXPORT ParticipantBuiltinTopicDataWrapper
{
    ::DDS::BuiltinTopicKey_t key;
    UserDataQosPolicyWrapper user_data;

public:
    ParticipantBuiltinTopicDataWrapper();

    ParticipantBuiltinTopicDataWrapper(const ::DDS::ParticipantBuiltinTopicData native) {
        key = native.key;
        user_data = native.user_data;
    }

    operator ::DDS::ParticipantBuiltinTopicData() const {
        ::DDS::ParticipantBuiltinTopicData native;

        native.key = key;
        native.user_data = user_data;

        return native;
    }
};

EXTERN_STRUCT_EXPORT TopicBuiltinTopicDataWrapper
{
    ::DDS::BuiltinTopicKey_t key;
    CORBA::Char* name;
    CORBA::Char* type_name;
    DurabilityQosPolicyWrapper durability;
    DurabilityServiceQosPolicyWrapper durability_service;
    DeadlineQosPolicyWrapper deadline;
    LatencyBudgetQosPolicyWrapper latency_budget;
    LivelinessQosPolicyWrapper liveliness;
    ReliabilityQosPolicyWrapper reliability;
    TransportPriorityQosPolicyWrapper transport_priority;
    LifespanQosPolicyWrapper lifespan;
    DestinationOrderQosPolicyWrapper destination_order;
    HistoryQosPolicyWrapper history;
    ResourceLimitsQosPolicyWrapper resource_limits;
    OwnershipQosPolicyWrapper ownership;
    TopicDataQosPolicyWrapper topic_data;

public:
    TopicBuiltinTopicDataWrapper();

    TopicBuiltinTopicDataWrapper(const ::DDS::TopicBuiltinTopicData native) {
        key = native.key;
        topic_data = native.topic_data;
        deadline = native.deadline;
        destination_order = native.destination_order;
        durability = native.durability;
        durability_service = native.durability_service;
        history = native.history;
        latency_budget = native.latency_budget;
        lifespan = native.lifespan;
        liveliness = native.liveliness;
        if (native.name != NULL) {
            name = CORBA::string_dup(native.name);
        }
        if (native.type_name != NULL) {
            type_name = CORBA::string_dup(native.type_name);
        }
        ownership = native.ownership;
        reliability = native.reliability;
        resource_limits = native.resource_limits;
        transport_priority = native.transport_priority;
    }

    operator ::DDS::TopicBuiltinTopicData() const {
        ::DDS::TopicBuiltinTopicData native;

        native.key = key;
        native.deadline = deadline;
        native.destination_order = destination_order;
        native.durability = durability;
        native.durability_service = durability_service;
        native.history = history;
        native.latency_budget = latency_budget;
        native.lifespan = lifespan;
        native.liveliness = liveliness;
        if (name != NULL) {
            native.name = CORBA::string_dup(name);
        }
        if (type_name != NULL) {
            native.type_name = CORBA::string_dup(type_name);
        }
        native.ownership = ownership;
        native.reliability = reliability;
        native.resource_limits = resource_limits;
        native.transport_priority = transport_priority;
        native.topic_data = topic_data;

        return native;
    }
};
