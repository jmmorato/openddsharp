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
#include "QosPolicies.h"
#include "dds/DdsDcpsDomainC.h"

EXTERN_STRUCT_EXPORT ParticipantBuiltinTopicDataWrapper
{
    ::DDS::BuiltinTopicKey_t key;
    UserDataQosPolicyWrapper user_data;

public:
    ParticipantBuiltinTopicDataWrapper() = default;

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
    TopicBuiltinTopicDataWrapper() = default;

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
        else {
            name = NULL;
        }
        if (native.type_name != NULL) {
            type_name = CORBA::string_dup(native.type_name);
        }
        else {
            type_name = NULL;
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

EXTERN_STRUCT_EXPORT SubscriptionBuiltinTopicDataWrapper
{
    ::DDS::BuiltinTopicKey_t key;
    ::DDS::BuiltinTopicKey_t participant_key;
    CORBA::Char* topic_name;
    CORBA::Char* type_name;
    DurabilityQosPolicyWrapper durability;
    DeadlineQosPolicyWrapper deadline;
    LatencyBudgetQosPolicyWrapper latency_budget;
    LivelinessQosPolicyWrapper liveliness;
    ReliabilityQosPolicyWrapper reliability;
    OwnershipQosPolicyWrapper ownership;
    DestinationOrderQosPolicyWrapper destination_order;
    UserDataQosPolicyWrapper user_data;
    TimeBasedFilterQosPolicyWrapper time_based_filter;
    PresentationQosPolicyWrapper presentation;
    PartitionQosPolicyWrapper partition;
    TopicDataQosPolicyWrapper topic_data;
    GroupDataQosPolicyWrapper group_data;

public:
    SubscriptionBuiltinTopicDataWrapper() = default;

    SubscriptionBuiltinTopicDataWrapper(const ::DDS::SubscriptionBuiltinTopicData native) {
        key = native.key;
        participant_key = native.participant_key;
        if (native.topic_name != NULL) {
            topic_name = CORBA::string_dup(native.topic_name);
        }
        else {
            topic_name = NULL;
        }
        if (native.type_name != NULL) {
            type_name = CORBA::string_dup(native.type_name);
        }
        else {
            type_name = NULL;
        }
        durability = native.durability;
        deadline = native.deadline;
        latency_budget = native.latency_budget;
        liveliness = native.liveliness;
        reliability = native.reliability;
        ownership = native.ownership;
        destination_order = native.destination_order;
        user_data = native.user_data;
        time_based_filter = native.time_based_filter;
        presentation = native.presentation;
        partition = native.partition;        
        topic_data = native.topic_data;
        group_data = native.group_data;
    }

    operator ::DDS::SubscriptionBuiltinTopicData() const {
        ::DDS::SubscriptionBuiltinTopicData native;

        native.key = key;
        native.participant_key = participant_key;
        if (topic_name != NULL) {
            native.topic_name = CORBA::string_dup(topic_name);
        }
        if (type_name != NULL) {
            native.type_name = CORBA::string_dup(type_name);
        }
        native.durability = durability;
        native.deadline = deadline;
        native.latency_budget = latency_budget;
        native.liveliness = liveliness;
        native.reliability = reliability;
        native.ownership = ownership;
        native.destination_order = destination_order;
        native.user_data = user_data;
        native.time_based_filter = time_based_filter;
        native.presentation = presentation;
        native.partition = partition;
        native.topic_data = topic_data;
        native.group_data = group_data;

        return native;
    }
};

EXTERN_STRUCT_EXPORT PublicationBuiltinTopicDataWrapper
{
    ::DDS::BuiltinTopicKey_t key;
    ::DDS::BuiltinTopicKey_t participant_key;
    CORBA::Char* topic_name;
    CORBA::Char* type_name;
    DurabilityQosPolicyWrapper durability;
    DurabilityServiceQosPolicyWrapper durability_service;
    DeadlineQosPolicyWrapper deadline;
    LatencyBudgetQosPolicyWrapper latency_budget;
    LivelinessQosPolicyWrapper liveliness;
    ReliabilityQosPolicyWrapper reliability;
    LifespanQosPolicyWrapper lifespan;
    UserDataQosPolicyWrapper user_data;
    OwnershipQosPolicyWrapper ownership;
    OwnershipStrengthQosPolicyWrapper ownership_strength;
    DestinationOrderQosPolicyWrapper destination_order;
    PresentationQosPolicyWrapper presentation;
    PartitionQosPolicyWrapper partition;
    TopicDataQosPolicyWrapper topic_data;
    GroupDataQosPolicyWrapper group_data;

public:
    PublicationBuiltinTopicDataWrapper() = default;

    PublicationBuiltinTopicDataWrapper(const ::DDS::PublicationBuiltinTopicData native) {
        key = native.key;
        participant_key = native.participant_key;
        if (native.topic_name != NULL) {
            topic_name = CORBA::string_dup(native.topic_name);
        }
        else {
            topic_name = NULL;
        }
        if (native.type_name != NULL) {
            type_name = CORBA::string_dup(native.type_name);
        }
        else {
            type_name = NULL;
        }
        durability = native.durability;
        durability_service = native.durability_service;
        deadline = native.deadline;
        latency_budget = native.latency_budget;
        liveliness = native.liveliness;
        reliability = native.reliability;
        lifespan = native.lifespan;
        user_data = native.user_data;
        ownership = native.ownership;
        ownership_strength = native.ownership_strength;
        destination_order = native.destination_order;                
        presentation = native.presentation;
        partition = native.partition;
        topic_data = native.topic_data;
        group_data = native.group_data;
    }

    operator ::DDS::PublicationBuiltinTopicData() const {
        ::DDS::PublicationBuiltinTopicData native;

        native.key = key;
        native.participant_key = participant_key;
        if (topic_name != NULL) {
            native.topic_name = CORBA::string_dup(topic_name);
        }
        if (type_name != NULL) {
            native.type_name = CORBA::string_dup(type_name);
        }
        native.durability = durability;
        native.durability_service = durability_service;
        native.deadline = deadline;
        native.latency_budget = latency_budget;
        native.liveliness = liveliness;
        native.reliability = reliability;
        native.lifespan = lifespan;
        native.user_data = user_data;
        native.ownership = ownership;
        native.ownership_strength = ownership_strength;
        native.destination_order = destination_order;                
        native.presentation = presentation;
        native.partition = partition;
        native.topic_data = topic_data;
        native.group_data = group_data;

        return native;
    }
};
